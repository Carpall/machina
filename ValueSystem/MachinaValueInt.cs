using Machina.TypeSystem;
using System;

namespace Machina.ValueSystem
{
    struct MachinaValueInt : IMachinaValue
    {
        public IMachinaType Type { get; }
        public ulong Value { get; }
        public bool IsConst => true;
        public bool CanBePointed => false;

        public MachinaValueInt(IMachinaType type, ulong value)
        {
            if (type.Kind != TypeKind.Int)
                throw new ArgumentException("non int type");

            Type = type;
            Value = value;
        }

        public string GetCValue()
        {
            return $"({Type.GetCType()}){Value}";
        }
    }
}
