using Machina.TypeSystem;

namespace Machina.ValueSystem
{
    struct MachinaValueNot : IMachinaValue
    {
        public IMachinaType Type => new MachinaTypeBool();
        public IMachinaValue Value { get; }
        public bool IsConst { get; }

        public MachinaValueNot(IMachinaValue value)
        {
            Value = value;
            IsConst = false;
        }

        public override string ToString()
        {
            return $"!{Value.GetCValue()}";
        }
    }
}
