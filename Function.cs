using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct Variable
    {
        public String Name;
        public readonly UInt16 Size;
        public UInt16 MemoryIndex;
        public readonly string Type;
        public readonly string Value;
        public void SetMemoryIndex(int index)
        {
            MemoryIndex = (ushort)index;
        }
        public Variable(string name, ushort size, string type = "", string value = "")
        {
            Name = name;
            Size = size;
            Type = type;
            Value = value;
            MemoryIndex = 0;
        }
        public void SetName(string name)
        {
            Name = name;
        }
    }
    public struct Function
    {
        public String Name;
        public readonly String ReturnType;
        public readonly List<Variable> Parameters;
        public readonly List<Instruction> Body;
        public readonly Boolean IsGeneric;
        public readonly String[] GenericNames;
        public String[] GenericTypes;
        public readonly UInt16 AllocationSize;

        public ushort ParametersAllocationSize
        {
            get
            {
                ushort size = 0;
                for (int i = 0; i < Parameters.Count; i++)
                    size += Parameters[i].Size;
                return size;
            }
        }

        public void AssignGenerics(params string[] types)
        {
            GenericTypes = types;
        }
        public Function(string name, string retType, ushort allocSize)
        {
            Name = name;
            AllocationSize = allocSize;
            ReturnType = retType;
            Parameters = new();
            Body = new();
            IsGeneric = false;
            GenericNames = null;
            GenericTypes = null;
        }
        public Function(string name, string retType, ushort allocSize, params string[] genericNames)
        {
            Name = name;
            AllocationSize = allocSize;
            ReturnType = retType;
            Parameters = new();
            Body = new();
            IsGeneric = true;
            GenericNames = genericNames;
            GenericTypes = null;
        }
        public void AddParameter(string name, ushort size)
        {
            Parameters.Add(new(name, size));
        }
        public void PopInstruction()
        {
            Body.RemoveAt(Body.Count-1);
        }
        public void AddInstruction(OpCodes opcode, params object[] arg)
        {
            Body.Add(new(opcode, arg));
        }
        public void SetName(string name)
        {
            Name = name;
        }
    }
}
