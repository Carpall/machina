using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    enum ValueKind
    {
        Empty,
        Constant,
        Register,
        Instruction,
        MemoryReference
    }
    struct Value
    {
        public object Body { get; set; }
        public ValueKind Kind { get; set; }
        public bool IsEmpty => Kind == ValueKind.Empty;
        public bool IsConstant => Kind == ValueKind.Constant;
        public bool IsRegister => Kind == ValueKind.Register;
        public bool IsInstruction => Kind == ValueKind.Instruction;
        public bool IsMemoryReference => Kind == ValueKind.MemoryReference;

        public static Value Empty() => new Value() { Body = "", Kind = ValueKind.Empty };
        public static Value Constant(object constant)  => new Value() { Body = constant, Kind = ValueKind.Constant };
        public static Value Register(RegisterValue register) => new Value() { Body = register, Kind = ValueKind.Register };
        public static Value Register (Enum register) => Register(new RegisterValue() { RegisterKind = register });
        public static Value Instruction(InstructionKind8086 instruction) => new Value() { Body = instruction, Kind = ValueKind.Instruction };
        public static Value MemoryReference(MemoryReference reference) => new Value() { Body = reference, Kind = ValueKind.MemoryReference };
        public AssemblyType GetAssemblyTypeFromRegSize()
        {
            // transfer to register struct 
            return ((RegisterValue)Body).RegisterKind switch
            {
                Register8Kind8086 => AssemblyType.BYTE,
                Register32Kind8086 => AssemblyType.DWORD,
                Register64Kind8086 => AssemblyType.QWORD,
                _ => throw new ArgumentException("Impossible recognize assembly type from a value which is not a register")
            };
        }
        public bool LowerBitSizedThan(Value register)
        {
            if (!IsRegister) return false;

            var reg = (RegisterValue)Body;
            var reg2 = (RegisterValue)register.Body;
            return (reg.MatchRegisterKind<Register8Kind8086>() &&
                    (reg2.MatchRegisterKind<Register32Kind8086>() || reg2.MatchRegisterKind<Register64Kind8086>())) ||
                   (reg.MatchRegisterKind<Register32Kind8086>() && reg2.MatchRegisterKind<Register64Kind8086>());
        }
        public bool BiggerBitSizedThan(Value register)
        {
            if (!IsRegister) return false;

            var reg = (RegisterValue)Body;
            var reg2 = (RegisterValue)register.Body;
            return (reg.MatchRegisterKind<Register64Kind8086>() &&
                    (reg2.MatchRegisterKind<Register8Kind8086>() || reg2.MatchRegisterKind<Register32Kind8086>())) ||
                   (reg.MatchRegisterKind<Register32Kind8086>() && reg2.MatchRegisterKind<Register8Kind8086>());
        }
        public bool MatchRegister(Value register)
        {
            return IsRegister && register.IsRegister && ((RegisterValue)Body).RegisterKind.Equals(((RegisterValue)register.Body).RegisterKind);
        }
        public bool MatchConstant(Value constant)
        {
            return IsConstant && Body.Equals(constant.Body);
        }
        public override string ToString()
        {
            return Body.ToString();
        }
    }
}