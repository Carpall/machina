using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machina.Emitter
{
    class Emitter
    {
        readonly InstructionBuilder8086 _assembly = new();
        readonly List<Value> _stack = new();
        readonly RegistersBag8086 _registers = new();

        const int PassArgumentsOffset = 2;
        const string EntryPointName = "main";
        private const Register64Kind8086 StackAllocationPointer64 = Register64Kind8086.rbp;

        public Emitter(string filename = null)
        {
            _assembly.FileName = filename;
            _assembly.EntryPoint = EntryPointName;
        }

        public string DumpAssembly(bool generateTextSection = true)
        {
            return _assembly.DumpAssembly(generateTextSection);
        }

        public void EmitInstruction(Instruction8086 instruction)
        {
            _assembly.EmitInstruction(instruction);
        }
        public void EmitInstruction(InstructionKind8086 opcode, Value dest, Value source, string label = "")
        {
            EmitInstruction(new Instruction8086() { Label = label, Kind = opcode, Arg0 = dest, Arg1 = source });
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
                    Load(new Value() { Body = _registers.FetchNext32(),  });
            }
        }
        public void EmitFunctionLabel32(string name, int paramCount)
        {
            _registers.ResetCounter(paramCount);
            PushParams(paramCount);
            EmitLabel(name);
        }
        string AlignSize(int size)
        {
            for (; ; size++)
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
            _stack.Add(value);
        }
        public void Load(int value)
        {
            _stack.Add(Value.Constant(value));
        }
        Value Pop()
        {
            var p = _stack[^1];
            _stack.RemoveAt(_stack.Count - 1);
            if (p.IsInstruction) {
                var fetch = Value.Register(_registers.FetchNext64());
                EmitInstruction((InstructionKind8086)p.Body, fetch);
                _registers.AdvanceCounter(-1);
                return fetch;
            }
            return p;
        }

        void EmitMove(Value dest, Value source)
        {
            if (dest.SameRegisterOf(source)) return;

            if (dest.IsMemoryReference)
                LookAtMemoryReference(ref source);
            if (source.MatchConstant(Value.Constant(0)))
                EmitInstruction(InstructionKind8086.xor, dest, dest);
            else
                MakeAutomaticConversion(source, dest);
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
        public void LookAtMemoryReference(ref Value value)
        {
            if (!value.IsMemoryReference) return;
            
            var old = value;
            EmitMove(value = Value.Register(_registers.Current64()), old);
        }
        public void EmitStoreInStack64(int index, AssemblyType type)
        {
            var addr = Value.MemoryReference(new MemoryReference() { Index = index, MemoryPointer = StackAllocationPointer64, AssemblyType = type });
            var value = Pop();
            LookAtMemoryReference(ref value);
            EmitMove(addr, value);
        }
        public void EmitLoadFromStack64(int index, AssemblyType type)
        {
            Load(Value.MemoryReference(new MemoryReference()
                { MemoryPointer = StackAllocationPointer64, Index = index, AssemblyType = type }));
        }
        void MakeAutomaticConversion(Value source, Value dest)
        {
            var instruction = new Instruction8086() { Kind = InstructionKind8086.mov, Arg0 = dest, Arg1 = source };
            if (source.LowerBitSizedThan(dest))
                instruction.Kind = InstructionKind8086.movzx;
            else if (source.BiggerBitSizedThan(dest))
                instruction.Arg1 = Value.Register(RegisterValue.RegisterConversion(new RegisterConversion() { Type = dest.GetAssemblyTypeFromRegSize() }, ((RegisterValue)source.Body).RegisterKind));
            EmitInstruction(instruction);
        }
        void EmitOpInt32(InstructionKind8086 instruction)
        {
            var second = Pop();
            var first = Pop();
            var dest = Value.Register(_registers.FetchNext32());
            if (!first.IsRegister)
                EmitMove(dest, first);
            else
            {
                dest = first;
                if (second.IsRegister)
                    MakeAutomaticConversion(second, first);
            }

            EmitInstruction(instruction, dest, second);
            Load(dest);
        }
        public void EmitAddInt32()
        {
            EmitOpInt32(InstructionKind8086.add);
        }
        public void EmitSubInt32()
        {
            EmitOpInt32(InstructionKind8086.sub);
        }
        public void EmitMulInt32()
        {
            EmitOpInt32(InstructionKind8086.imul);
        }
        public void EmitDivInt32()
        {
            EmitOpInt32(InstructionKind8086.idiv);
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
            RestorePreviousFrame64();
            EmitInstruction(InstructionKind8086.ret);
        }
        void SaveStackItems(int skipCount)
        {
            for (int i = _stack.Count - skipCount; i > 0; i--)
            {
                var p = _stack[i - 1];
                if (!p.IsConstant)
                {
                    EmitInstruction(InstructionKind8086.push, p);
                    _stack[i - 1] = Value.Instruction(InstructionKind8086.pop);
                }
            }
        }
        void PassArguments32(int argCount)
        {
            if (argCount > 0)
            {
                _registers.ResetCounter(PassArgumentsOffset + argCount);
                for (; argCount > 0; argCount--)
                    EmitMove(Value.Register(_registers.FetchPrevious32()), Pop());
            }
        }
        void PassArguments64(int argCount)
        {
            if (argCount > 0)
            {
                _registers.ResetCounter(PassArgumentsOffset + argCount);
                for (; argCount > 0; argCount--)
                    EmitMove(Value.Register(_registers.FetchPrevious64()), Pop());
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
        void Compare(InstructionKind8086 setInstruction)
        {
            var second = Pop();
            var first = Pop();
            if (second.IsConstant && first.IsConstant)
                throw new ArgumentException("Impossible to compare two constants");
            if (first.IsMemoryReference)
                LookAtMemoryReference(ref second);
            EmitInstruction(InstructionKind8086.cmp, first, second);
            var fetch = _registers.FetchNext8();
            EmitInstruction(setInstruction, Value.Register(fetch));
            Load(Value.Register(fetch));
        }
        public void EmitCompareEQ()
        {
            Compare(InstructionKind8086.sete);
        }
        public void EmitCompareNEQ()
        {
            Compare(InstructionKind8086.setne);
        }
        public void EmitCompareGT()
        {
            Compare(InstructionKind8086.setg);
        }
        public void EmitCompareLS()
        {
            Compare(InstructionKind8086.setl);
        }
    }
}
