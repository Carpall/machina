namespace Machina.TypeSystem
{
    struct MachinaTypeExpression : IMachinaType
    {
        public int Size { get; }
        public string CType { get; }
        public IMachinaType ElementType { get; }
        public TypeKind Kind => TypeKind.Expression;

        public MachinaTypeExpression(IMachinaType elementType)
        {
            ElementType = elementType;
            Size = 0;
            CType = "";
        }

        public string GetCType()
        {
            return ElementType.GetCType();
        }
    }
}
