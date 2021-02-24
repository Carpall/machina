using Machina.TypeSystem;

namespace Machina.ValueSystem
{
    struct MachinaValueVoid : IMachinaValue
    {
        public IMachinaType Type => new MachinaTypeVoid();
        public bool IsConst => true;
    }
}
