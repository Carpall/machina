using System;

namespace Machina.Emitter
{
    public struct RegisterValue
    {
        public Enum RegisterKind { get; set; }
        public bool HasConversion { get; set; }
        public RegisterConversion Conversion { get; set; }

        public bool SameOf(Enum register)
        {
            return Convert.ToInt32(register) == Convert.ToInt32(RegisterKind);
        } 
        public bool MatchRegisterKind<T>() where T : Enum
        {
            return RegisterKind is T;
        }
        public static RegisterValue RegisterConversion(RegisterConversion conversion, Enum registerKind) => new RegisterValue() { Conversion = conversion, HasConversion = true, RegisterKind = registerKind };
        public override string ToString()
        {
            return HasConversion ? Conversion.ToString(RegisterKind) : RegisterKind.ToString();
        }
    }
}