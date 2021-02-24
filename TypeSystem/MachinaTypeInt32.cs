namespace Machina.TypeSystem
{
    struct MachinaTypeInt32 : IMachinaType
    {
        public int Size => 32;
        public string CType => "int32_t";
        public TypeKind Kind => TypeKind.Int;

        public string GetCType()
        {
            return CType;
        }
    }
}
