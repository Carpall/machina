using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct Data
    {
        TypeCode Type;
        String Name;
        public Data(TypeCode type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
