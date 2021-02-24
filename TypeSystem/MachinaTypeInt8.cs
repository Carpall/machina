namespace Machina.TypeSystem
{
    struct MachinaTypeInt8 : IMachinaType
    {
        public int Size => 8;
        public string CType => "int8_t";
        public TypeKind Kind => TypeKind.Int;

        public string GetCType()
        {
            return CType;
        }
    }
}
