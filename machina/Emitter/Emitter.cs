using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machina.Emitter
{
    class Emitter
    {
        readonly InstructionBuilder8086 Assembly = new();
        readonly List<Value> Stack = new();
        readonly RegistersBag8086 Registers = new();
        readonly string _fileName = "";

        const int PassArgumentsOffset = 2;
        const string EntryPointName = "main";

        public Emitter(string filename)
        {
            _fileName = filename;
        }

        public string GetAssembly(bool generateTextSection = true)
        {
            if (generateTextSection)
            {
                Assembly.EntryPoint = EntryPointName;
                Assembly.FileName = _fileName;
            }
            return Assembly.Assemble();
        }
        public void EmitInstruction(InstructionKind8086 opcode, Value dest, Value source, string label = "")
        {
            Assembly.EmitInstruction(new Instruction8086() { Label = label, Kind = opcode, Arg0 = dest, Arg1 = source });
        }
        public void EmitInstruction(InstructionKind8086 opcode, Value dest, string label = "")
        {
            EmitInstruction(opcode, dest, Value.Empty(), label);
        }
        public void EmitInstruction(InstructionKind8086 opcode, string label = "")
        {
            EmitInstruction(opcode, Value.Empty(), Value.Empty(), label);
        }
        public void EmitLabel(string name)
        {
            EmitInstruction(InstructionKind8086.Label, name);
        }
        void PushParams(int argCount)
        {
            if (argCount > 0)
            {
                for (; argCount > 0; argCount--)
                    Load(new Value() { Body = Registers.FetchNext32(),  });
            }
        }
        public void EmitFunctionLabel32(string name, int paramCount)
        {
            Registers.ResetCounter(PassArgumentsOffset);
            PushParams(paramCount);
            EmitLabel(name);
        }
        string AlignSize(int size)
        {
            for (; true; size++)
                if ((size % 16) == 0)
                    return size.ToString();
        }
        public void DeclareStackAllocation64(int size = 0)
        {
            if (size != 0)
                EmitInstruction(InstructionKind8086.sub, Value.Register(Register64Kind8086.rbp), Value.Constant(AlignSize(size)));
        }
        public void DeclareStackAllocation32(int size = 0)
        {
            if (size != 0)
                EmitInstruction(InstructionKind8086.sub, Value.Register(Register32Kind8086.ebp), Value.Constant(AlignSize(size)));
        }
        public void DeclareStackDellocation32(int size = 0)
        {
            if (size != 0)
                EmitInstruction(InstructionKind8086.add, Value.Register(Register32Kind8086.ebp), Value.Constant(AlignSize(size)));
        }
        public void Load(Value value)
        {
            Stack.Add(value);
        }
        public void Load(int value)
        {
            Stack.Add(Value.Constant(value));
        }
        Value Pop()
        {
            var p = Stack[^1];
            Stack.RemoveAt(Stack.Count - 1);
            if (p.IsInstruction) {
                var fetch = Value.Register(Registers.FetchNext64());
                EmitInstruction((InstructionKind8086)p.Body, fetch);
                Registers.AdvanceCounter(-1);
                return fetch;
            }
            return p;
        }
        void EmitMove(Value dest, Value source)
        {
            if (!dest.Equals(source))
            {
                if (source.MatchConstant(0))
                    EmitInstruction(InstructionKind8086.xor, dest, dest);
                else
                    EmitInstruction(InstructionKind8086.mov, dest, source);
            }
        }
        public void SavePreviousBP32()
        {
            EmitInstruction(InstructionKind8086.push, Value.Register(Register32Kind8086.ebp));
            EmitInstruction(InstructionKind8086.mov, Value.Register(Register32Kind8086.ebp), Value.Register(Register32Kind8086.esp));
        }
        public void RestorePreviousBP32()
        {
            EmitInstruction(InstructionKind8086.mov, Value.Register(Register32Kind8086.esp), Value.Register(Register32Kind8086.ebp));
            EmitInstruction(InstructionKind8086.pop, Value.Register(Register32Kind8086.ebp));
        }
        public void SavePreviousFrame64()
        {
            EmitInstruction(InstructionKind8086.push, Value.Register(Register64Kind8086.rbp));
            EmitInstruction(InstructionKind8086.mov, Value.Register(Register64Kind8086.rbp), Value.Register(Register64Kind8086.rsp));
        }
        public void RestorePreviousFrame64()
        {
            EmitInstruction(InstructionKind8086.leave);
        }
        public void EmitAddInt32()
        {
            var source = Pop();
            EmitMove(Value.Register(Registers.FetchNext32()), Pop());
            var dest = Value.Register(Registers.FetchPrevious32());
            EmitInstruction(InstructionKind8086.add, dest, source);
            Load(dest);
        }
        public void EmitSubInt32()
        {
            var source = Pop();
            EmitMove(Value.Register(Registers.FetchNext32()), Pop());
            var dest = Value.Register(Registers.FetchPrevious32());
            EmitInstruction(InstructionKind8086.sub, dest, source);
            Load(dest);
        }
        public void EmitMulInt32()
        {
            var source = Pop();
            EmitMove(Value.Register(Registers.FetchNext32()), Pop());
            var dest = Value.Register(Registers.FetchPrevious32());
            EmitInstruction(InstructionKind8086.imul, dest, source);
            Load(dest);
        }
        public void EmitDivInt32()
        {
            var source = Pop();
            EmitMove(Value.Register(Registers.FetchNext32()), Pop());
            var dest = Value.Register(Registers.FetchPrevious32());
            EmitInstruction(InstructionKind8086.idiv, dest, source);
            Load(dest);
        }
        public void EmitRetInt32()
        {
            EmitMove(Value.Register(RegistersBag8086.ReturnRegister32), Pop());
            EmitRetVoid();
        }
        public void EmitRetInt64()
        {
            EmitMove(Value.Register(RegistersBag8086.ReturnRegister64), Pop());
            EmitRetVoid();
        }
        public void EmitRetVoid()
        {
            EmitInstruction(InstructionKind8086.ret);
        }
        void SaveStackItems(int skipCount)
        {
            for (int i = Stack.Count - skipCount; i > 0; i--)
            {
                var p = Stack[i - 1];
                if (!p.IsConstant)
                {
                    EmitInstruction(InstructionKind8086.push, p);
                    Stack[i - 1] = Value.Instruction(InstructionKind8086.pop);
                }
            }
        }
        void PassArguments32(int argCount)
        {
            if (argCount > 0)
            {
                Registers.ResetCounter(PassArgumentsOffset + argCount);
                for (; argCount > 0; argCount--)
                    EmitMove(Value.Register(Registers.FetchPrevious32()), Pop());
            }
        }
        void PassArguments64(int argCount)
        {
            if (argCount > 0)
            {
                Registers.ResetCounter(PassArgumentsOffset + argCount);
                for (; argCount > 0; argCount--)
                    EmitMove(Value.Register(Registers.FetchPrevious64()), Pop());
            }
        }
        public void EmitCall32(string name, int argCount)
        {
            SaveStackItems(argCount);
            PassArguments32(argCount);
            EmitInstruction(InstructionKind8086.call, Value.Constant(name));
            Load(Value.Register(RegistersBag8086.ReturnRegister32));
        }
        public void EmitCall64(string name, int argCount)
        {
            SaveStackItems(argCount);
            PassArguments64(argCount);
            EmitInstruction(InstructionKind8086.call, Value.Constant(name));
            Load(Value.Register(RegistersBag8086.ReturnRegister64));
        }
    }
}
