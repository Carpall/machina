using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct StructType
    {
        public readonly String Name;
        public readonly List<Variable> Fields;
        public readonly List<Function> Methods;

        public int Size
        {
            get
            {
                int size = 0;
                for (int i = 0; i < Fields.Count; i++)
                    size += Fields[i].Size;
                return size;
            }
        }

        public StructType(string name)
        {
            Name = name;
            Fields = new();
            Methods = new();
        }
        public void UnistallFunction()
        {
            Methods.RemoveAt(Methods.Count-1);
        }
        public void InstallFunction(Function function)
        {
            Methods.Add(function);
        }
        public void UnistallField()
        {
            Fields.RemoveAt(Fields.Count - 1);
        }
        public void InstallField(Variable variable)
        {
            Fields.Add(variable);
        }
    }
}
