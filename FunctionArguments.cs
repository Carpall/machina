using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct Data
    {
        public ValueType Type;
        public string Name;
        public ConstValue DefaultValue;
        public Data(string name, ValueType type, ConstValue value)
        {
            Name = name;
            Type = type;
            DefaultValue = value;
        }
    }
}
