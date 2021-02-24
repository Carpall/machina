using Machina.TypeSystem;
using System;

namespace Machina.ValueSystem
{
    struct MachinaValueBool : IMachinaValue
    {
        public IMachinaType Type => new MachinaTypeBool();
        public object Value { get; }
        public bool IsConst { get; }

        public MachinaValueBool(bool value)
        {
            Value = Convert.ToInt32(value);
            IsConst = true;
        }

        public MachinaValueBool(string name)
        {
            Value = name;
            IsConst = false;
        }
    }
}
