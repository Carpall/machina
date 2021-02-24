using Machina.TypeSystem;
using System;

namespace Machina.ValueSystem
{
    struct MachinaValueInt : IMachinaValue
    {
        public IMachinaType Type { get; }
        public object Value { get; }
        public bool IsConst { get; }

        public MachinaValueInt(IMachinaType type, ulong value)
        {
            if (type.Kind != TypeKind.Int)
                throw new ArgumentException("non int type");

            Type = type;
            Value = value;
            IsConst = true;
        }

        public MachinaValueInt(IMachinaType type, string name)
        {
            if (type.Kind != TypeKind.Int)
                throw new ArgumentException("non int type");

            Type = type;
            Value = name;
            IsConst = false;
        }
    }
}
