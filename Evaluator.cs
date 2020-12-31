using System;
using System.Collections.Generic;

namespace Machina
{
    public class Evaluator
    {
        public const string EntryPointName = "main";
        CodeEmitter Emitter;
        Bytecode Bytecode;
        ushort GetMemoryIndexFromFieldName(List<Variable> fields, string fieldName)
        {
            ushort memIndex = 0;
            for (int h = 0; h < fields.Count; h++)
            {
                memIndex += fields[h].Size;
                if (fields[h].Name == fieldName)
                    return memIndex;
            }
            throw new Exception("Undeclared field '"+fieldName+'\'');
        }
        void GenerateFunction(Function function, bool isEntryPoint = false)
        {
            Dictionary<string, Variable> localVariables = new();
            int memIndexCount = 0;
            Emitter.EmitLabel(function.Name, isEntryPoint);
            foreach (var i in function.Body)
                switch (i.OpCode)
                {
                    case OpCodes.Call:
                        var name = i.Argument[0].ToString();
                        if (!Bytecode.GlobalMembers.ContainsKey(name))
                            throw new Exception($"Function {name} is not installed");
                        var f = (Function)Bytecode.GlobalMembers[name];
                        Emitter.EmitCall(f.Name, f.ReturnType == "void", f.Name == EntryPointName);
                        break;
                    case OpCodes.Enter:
                        Emitter.EmitSaveStackPointer(function.AllocationSize + function.ParametersAllocationSize);
                        if (isEntryPoint)
                        {
                            var argv = function.Parameters[0];
                            argv.SetMemoryIndex(8);
                            localVariables.Add(argv.Name, argv);
                            memIndexCount += argv.Size;
                            Emitter.EmitAllocARGV();
                        }
                        else
                            foreach (var arg in function.Parameters)
                            {
                                arg.SetMemoryIndex(memIndexCount += arg.Size);
                                localVariables.Add(arg.Name, arg);
                                Emitter.EmitStoreArgs(memIndexCount);
                            }
                        break;
                    case OpCodes.StoreArrayElem:
                        Emitter.EmitStoreElemArray((int)i.Argument[0]);
                        break;
                    case OpCodes.LoadArray:
                        Emitter.EmitLoad32BitValue(((int)i.Argument[0])*((int)i.Argument[1]));
                        Emitter.EmitCall("mem::alloc(u32)unknow*");
                        break;
                    case OpCodes.LoadMem:
                        Emitter.EmitLoadMem64Bit(localVariables[i.Argument[0].ToString()].MemoryIndex);
                        break;
                    case OpCodes.MemDeclare:
                        var size = Convert.ToUInt16(i.Argument[1]);
                        var loc = new Variable(i.Argument[0].ToString(), size);
                        loc.SetMemoryIndex(memIndexCount += size);
                        localVariables.Add(i.Argument[0].ToString(), loc);
                        break;
                    case OpCodes.LoadInt:
                        Emitter.EmitLoad64BitValue(Convert.ToInt64(i.Argument[0]));
                        break;
                    case OpCodes.StoreMem:
                        Emitter.EmitStoreMem64Bit(localVariables[i.Argument[0].ToString()].MemoryIndex);
                        break;
                    case OpCodes.LoadArrayElem:
                        Emitter.EmitLoadElemArray((int)i.Argument[0]);
                        break;
                    case OpCodes.UnsafeAsm:
                        Emitter.EmitAssembly(i.Argument[0].ToString());
                        break;
                    case OpCodes.LoadField:
                        Emitter.EmitLoadField(GetMemoryIndexFromFieldName(((StructType)Bytecode.GlobalMembers[i.Argument[0].ToString()]).Fields, i.Argument[1].ToString()));
                        break;
                    case OpCodes.StoreField:
                        Emitter.EmitStoreField(GetMemoryIndexFromFieldName(((StructType)Bytecode.GlobalMembers[i.Argument[0].ToString()]).Fields, i.Argument[1].ToString()));
                        break;
                    case OpCodes.LoadInstance:
                        var structType = (StructType)Bytecode.GlobalMembers[i.Argument[0].ToString()];
                        Emitter.EmitLoad32BitValue(structType.Size);
                        Emitter.EmitCall("mem::alloc(u32)unknow*");
                        break;
                    case OpCodes.UnsafeEmitGlobal:
                        var g = (Tuple<string, string, string>)i.Argument[0];
                        Emitter.EmitGlobal(g.Item1, g.Item2, g.Item3);
                        break;
                    case OpCodes.LoadString:
                        Emitter.EmitLoadString(i.Argument[0].ToString());
                        break;
                    case OpCodes.Ret:
                        Emitter.EmitRestoreStackPointer(function.AllocationSize + function.ParametersAllocationSize);
                        Emitter.EmitReturn();
                        break;
                }
        }
        void Generate()
        {
            foreach (var member in Bytecode.GlobalMembers.Values)
            {
                if (member is Function)
                    GenerateFunction((Function)member, ((Function)member).Name == EntryPointName);
                else if (member is StructType)
                    GenerateStruct((StructType)member);
            }
        }
        void GenerateField(Variable variable)
        {
            Emitter.EmitGlobal(variable.Name, variable.Type, variable.Value);
        }
        void GenerateStruct(StructType structType)
        {
            if (structType.Name == "void")
                throw new Exception("Cannot install a struct with a name of a built in type");
            for (int i = 0; i < structType.Methods.Count; i++)
            {
                structType.Methods[i].SetName(structType.Name + '.' + structType.Methods[i].Name);
                GenerateFunction(structType.Methods[i]);
            }
        }
        public Evaluator(string moduleName, Bytecode bytecode)
        {
            Emitter = new(moduleName);
            Bytecode = bytecode;
            Generate();
        }
        public override string ToString()
        {
            return Emitter.ToString();
        }
    }
}
