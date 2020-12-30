using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct Variable
    {
        public readonly String Name;
        public readonly String Type;
        public readonly UInt16 Size;
        public readonly Boolean IsPointer;
        public UInt16 MemoryIndex;
        public void SetMemoryIndex(ushort index)
        {
            MemoryIndex = index;
        }
        public Variable(string name, string type, ushort size, bool isPointer)
        {
            Name = name;
            Type = type;
            Size = size;
            IsPointer = isPointer;
            MemoryIndex = 0;
        }
    }
    public struct Function
    {
        public readonly String Name;
        public readonly String ReturnType;
        public readonly Boolean IsPointer;
        public readonly List<Variable> Parameters;
        public readonly List<Instruction> Body;
        public readonly Boolean IsGeneric;
        public readonly String[] GenericNames;
        public String[] GenericTypes;
        public readonly UInt16 AllocationSize;
        public void AssignGenerics(params string[] types)
        {
            GenericTypes = types;
        }
        public Function(string name, string retType, ushort allocSize, bool isPointer = false)
        {
            Name = name;
            AllocationSize = allocSize;
            ReturnType = retType;
            IsPointer = isPointer;
            Parameters = new();
            Body = new();
            IsGeneric = false;
            GenericNames = null;
            GenericTypes = null;
        }
        public Function(string name, string retType, ushort allocSize, bool isPointer = false, params string[] genericNames)
        {
            Name = name;
            AllocationSize = allocSize;
            ReturnType = retType;
            IsPointer = isPointer;
            Parameters = new();
            Body = new();
            IsGeneric = true;
            GenericNames = genericNames;
            GenericTypes = null;
        }
        public void AddParameter(string name, string type, ushort size, bool isPointer)
        {
            Parameters.Add(new(name, type, size, isPointer));
        }
        public void PopInstruction()
        {
            Body.RemoveAt(Body.Count-1);
        }
        public void AddInstruction(OpCodes opcode, object arg)
        {
            Body.Add(new(opcode, arg));
        }
        public void AddInstruction(OpCodes opcode)
        {
            Body.Add(new(opcode, null));
        }
    }
}
