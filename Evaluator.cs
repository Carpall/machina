using System;
using System.Collections.Generic;

namespace Machina
{
    public class Evaluator
    {
        public const string EntryPointName = "main";
        CodeEmitter Emitter;
        Bytecode Bytecode;
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
                        Emitter.EmitSaveStackPointer(function.AllocationSize);
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
                    case OpCodes.LoadMem:
                        Emitter.EmitLoadMem64Bit(localVariables[i.Argument[0].ToString()].MemoryIndex);
                        break;
                    case OpCodes.StoreMem:
                        name = i.Argument[0].ToString();
                        if (!localVariables.ContainsKey(name))
                        {
                            var size = Convert.ToUInt16(i.Argument[1]);
                            var loc = new Variable(name, size);
                            loc.SetMemoryIndex(memIndexCount += size);
                            localVariables.Add(name, loc);
                            break;
                        }
                        Emitter.EmitStoreMem64Bit(memIndexCount += localVariables[name].Size);
                        break;
                    case OpCodes.LoadArrayElem:
                        Emitter.EmitLoadElemArray((int)i.Argument[0], (int)i.Argument[1]);
                        break;
                    case OpCodes.UnsafeAsm:
                        Emitter.EmitAssembly(i.Argument[0].ToString());
                        break;
                    case OpCodes.UnsafeEmitGlobal:
                        var g = (Tuple<string, string, string>)i.Argument[0];
                        Emitter.EmitGlobal(g.Item1, g.Item2, g.Item3);
                        break;
                    case OpCodes.LoadString:
                        Emitter.EmitLoadString(i.Argument[0].ToString());
                        break;
                    case OpCodes.Ret:
                        Emitter.EmitRestoreStackPointer(function.AllocationSize);
                        Emitter.EmitReturn();
                        break;
                }
        }
        void Generate()
        {
            foreach (var member in Bytecode.GlobalMembers.Values)
                if (member is Function)
                    GenerateFunction((Function)member, ((Function)member).Name == EntryPointName);
        }
        void GenerateStruct()
        {

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
