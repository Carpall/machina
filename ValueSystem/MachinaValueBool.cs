using Machina.TypeSystem;
using System;

namespace Machina.ValueSystem
{
    struct MachinaValueBool : IMachinaValue
    {
        public IMachinaType Type => new MachinaTypeBool();
        public int Value { get; }
        public bool IsConst => true;
        public bool CanBePointed => true;

        public MachinaValueBool(bool value)
        {
            Value = Convert.ToInt32(value);
        }

        public string GetCValue()
        {
            return $"(({Type.GetCType()}){Value})";
        }
    }
}
