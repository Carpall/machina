using Machina.TypeSystem;
using System;

namespace Machina.ValueSystem
{
    struct MachinaValuePointer : IMachinaValue
    {
        public IMachinaValue ElementValue { get; }
        public IMachinaType Type { get; }
        public bool IsConst { get; }
        public bool CanBePointed => false;

        public MachinaValuePointer(IMachinaValue elementValue)
        {
            if (!elementValue.CanBePointed)
                throw new ArgumentException("value cannot be pointed");

            ElementValue = elementValue;
            Type = new MachinaTypePointer(elementValue.Type);
            IsConst = elementValue.IsConst;
        }

        public string GetCValue()
        {
            return $"&({ElementValue.GetCValue()})";
        }
    }
}
