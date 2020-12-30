using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct StructType
    {
        public readonly String Name;
        public readonly Function Constructor;
        public readonly List<Variable> Fields;
        public readonly List<Function> InstanceMembers;
        public readonly Boolean IsGeneric;
        public readonly String[] GenericNames;
        public String[] GenericTypes;
        public void AssignGenerics(params string[] types)
        {
            GenericTypes = types;
        }
        public StructType(string name, ref Function ctor)
        {
            Name = name;
            Constructor = ctor;
            Fields = new();
            InstanceMembers = new();
            IsGeneric = false;
            GenericNames = null;
            GenericTypes = null;
        }
        public StructType(string name, ref Function ctor, params string[] genericNames)
        {
            Name = name;
            Constructor = ctor;
            Fields = new();
            InstanceMembers = new();
            IsGeneric = true;
            GenericNames = genericNames;
            GenericTypes = null;
        }
        public void UnistallFunction()
        {
            InstanceMembers.RemoveAt(InstanceMembers.Count-1);
        }
        public void InstallFunction(Function function)
        {
            InstanceMembers.Add(function);
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
