using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    struct Value
    {
        public object Body { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsConstant { get; set; }
        public bool IsRegister { get; set; }
        public bool IsInstruction { get; set; }
        public static Value Empty() => new Value() { Body = "", IsEmpty = true };
        public static Value Constant(object constant)  => new Value() { Body = constant, IsConstant = true };
        public static Value Register(Register16Kind8086 register) => new Value() { Body = register, IsRegister = true };
        public static Value Register(Register32Kind8086 register) => new Value() { Body = register, IsRegister = true };
        public static Value Register(Register64Kind8086 register) => new Value() { Body = register, IsRegister = true };
        public static Value Instruction(InstructionKind8086 instruction) => new Value() { Body = instruction, IsInstruction = true };
        public bool MatchConstant(object constant)
        {
            return IsConstant && Body.Equals(constant);
        }
        public override string ToString()
        {
            return Body.ToString();
        }
    }
}
