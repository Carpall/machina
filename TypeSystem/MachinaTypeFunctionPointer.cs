using Machina.CModels;
using System;

namespace Machina.TypeSystem
{
    struct MachinaTypeFunctionPointer : IMachinaType
    {
        public int Size => Environment.Is64BitOperatingSystem ? 64 : 32;
        public string CType => "";
        public CFunctionPrototype Prototype { get; }
        public TypeKind Kind => TypeKind.Pointer;

        public MachinaTypeFunctionPointer(CFunctionPrototype prototype)
        {
            Prototype = prototype;
        }

        public string GetCType()
        {
            return $"{Prototype.ReturnType.GetCType()} ({string.Join(", ", Prototype.Parameters)})";
        }
    }
}
