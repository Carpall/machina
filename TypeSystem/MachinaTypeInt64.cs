namespace Machina.TypeSystem
{
    struct MachinaTypeInt64 : IMachinaType
    {
        public int Size => 64;
        public string CType => "int64_t";
        public TypeKind Kind => TypeKind.Int;

        public string GetCType()
        {
            return CType;
        }
    }
}
