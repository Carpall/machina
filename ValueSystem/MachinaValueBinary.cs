using Machina.TypeSystem;

namespace Machina.ValueSystem
{
    internal struct MachinaValueBinary : IMachinaValue
    {
        public BinaryOperator BinaryOperator { get; }
        public IMachinaValue Left { get; }
        public IMachinaValue Right { get; }
        public IMachinaType Type => new MachinaTypeExpression(Left.Type);
        public bool IsConst => false;

        public MachinaValueBinary(IMachinaValue value1, IMachinaValue value2, BinaryOperator binaryoperator)
        {
            value1.Type.ExpectType(value2.Type);

            Left = value1;
            Right = value2;
            BinaryOperator = binaryoperator;
        }

        private char GetBinaryOperator(BinaryOperator binaryoperator)
        {
            return binaryoperator switch
            {
                BinaryOperator.Add => '+',
                BinaryOperator.Sub => '-',
                BinaryOperator.Mul => '*',
                BinaryOperator.Div => '/',
            };
        }

        public override string ToString()
        {
            return /*$"(({Type.GetCType()})*/$"({Left.GetCValue()} {GetBinaryOperator(BinaryOperator)} {Right.GetCValue()})";
        }
    }
}
