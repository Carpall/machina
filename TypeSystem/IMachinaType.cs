
using System;

namespace Machina.TypeSystem
{
    public interface IMachinaType
    {
        public int Size { get; }
        public string CType { get; }
        public TypeKind Kind { get; }

        public abstract string GetCType();

        public IMachinaType GetRealType()
        {
            if (Kind == TypeKind.Expression)
                return ((MachinaTypeExpression)this).ElementType.GetRealType();
            else
                return this;
        }

        public void ExpectType(IMachinaType rightType)
        {
            if (!Equals(GetRealType(), rightType.GetRealType()))
                throw new ArgumentException("expected type " + CType);
        }

        public void ExpectIntType()
        {
            if (GetRealType().Kind != TypeKind.Int)
                throw new ArgumentException("expected int type");
        }

        public void ExpectBoolType()
        {
            if (GetRealType().Kind != TypeKind.Bool)
                throw new ArgumentException("expected bool type");
        }

        public void ExpectVoidType()
        {
            if (GetRealType().Kind != TypeKind.Void)
                throw new ArgumentException("expected void type");
        }
    }
}
