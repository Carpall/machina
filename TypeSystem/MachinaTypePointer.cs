using System;

namespace Machina.TypeSystem
{
    struct MachinaTypePointer : IMachinaType
    {
        public int Size => Environment.Is64BitOperatingSystem ? 64 : 32;
        public string CType => "";
        public IMachinaType ElementType { get; }
        public TypeKind Kind => TypeKind.Pointer;

        public MachinaTypePointer(IMachinaType elementType)
        {
            ElementType = elementType;
        }

        public string GetCType()
        {
            return $"{ElementType.GetCType()}*";
        }
    }
}
