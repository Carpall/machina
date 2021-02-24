namespace Machina.TypeSystem
{
    struct MachinaTypeVoid : IMachinaType
    {
        public int Size => 0;
        public string CType => "void";
        public TypeKind Kind => TypeKind.Void;

        public string GetCType()
        {
            return CType;
        }
    }
}
