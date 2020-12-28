using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct ConstValue
    {
        public ValueType Type;
        public object Value;
        public ConstValue(ValueType type, object value)
        {
            Type = type;
            Value = value;
        }
    }
}
