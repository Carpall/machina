using System;
using System.Collections.Generic;

namespace Machina
{
    public class Evaluator
    {
        CodeEmitter Emitter;
        Bytecode Bytecode;
        List<string> FunctionReferences = new();
        void GenerateFunction(Function function, bool isEntryPoint = false)
        {
            Dictionary<string, Variable> localVariables = new();
            int memIndexCount = 0;
            Emitter.EmitLabel(function.Name);
            foreach (var i in function.Body)
                switch (i.OpCode)
                {
                    case OpCodes.Call:
                        var f = (Function)Bytecode.GlobalMembers[i.Argument.ToString()];
                        if (!FunctionReferences.Contains(i.Argument.ToString()))
                            FunctionReferences.Add(i.Argument.ToString());
                        Emitter.EmitCall(f.Name, f.ReturnType == "void");
                        break;
                    case OpCodes.Enter:
                        Emitter.EmitSaveStackPointer(function.AllocationSize);
                        if (isEntryPoint)
                        {
                            var argv = function.Parameters[0];
                            localVariables.Add(argv.Name, argv);
                            memIndexCount += argv.Size;
                            function.Parameters[0].SetMemoryIndex(8);
                            Emitter.EmitAllocARGV();
                        }
                        break;
                    case OpCodes.UnsafeAsm:
                        Emitter.EmitAssembly(i.Argument.ToString());
                        break;
                    case OpCodes.UnsafeEmitGlobal:
                        var g = (Tuple<string, string, string>)i.Argument;
                        Emitter.EmitGlobal(g.Item1, g.Item2, g.Item3);
                        break;
                    case OpCodes.LoadString:
                        Emitter.EmitLoadString(i.Argument.ToString());
                        break;
                    case OpCodes.Ret:
                        Emitter.EmitRestoreStackPointer(function.AllocationSize);
                        Emitter.EmitReturn();
                        break;
                }
        }
        void Generate()
        {
            if (Bytecode.GlobalMembers.TryGetValue("main", out object entryPoint))
                GenerateFunction((Function)entryPoint, true);
            else
                throw new Exception("Missing main function in the bytecode instance");
            foreach (var reference in FunctionReferences)
                GenerateFunction((Function)Bytecode.GlobalMembers[reference]);
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
