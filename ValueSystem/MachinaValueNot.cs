using Machina.TypeSystem;

namespace Machina.ValueSystem
{
    struct MachinaValueNot : IMachinaValue
    {
        public IMachinaType Type => new MachinaTypeBool();
        public IMachinaValue Value { get; }
        public bool IsConst { get; }
        public bool CanBePointed => false;

        public MachinaValueNot(IMachinaValue value)
        {
            Value = value;
            IsConst = value.IsConst;
        }

        public string GetCValue()
        {
            return $"!{Value.GetCValue()}";
        }
    }
}
