using System;

namespace Machina.TypeSystem
{
    struct MachinaTypeIntPlatform : IMachinaType
    {
        public int Size => Environment.Is64BitOperatingSystem ? 64 : 32;
        public string CType => "int";
        public TypeKind Kind => TypeKind.Int;

        public string GetCType()
        {
            return CType;
        }
    }
}
