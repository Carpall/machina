using Machina.CModels;
using Machina.TypeSystem;

namespace Machina.ValueSystem
{
    struct MachinaValueMemory : IMachinaValue
    {
        public IMachinaType Type { get; }
        public CIdentifier Identifier { get; }
        public bool IsConst => false;

        public MachinaValueMemory(IMachinaType type, string name)
        {
            Type = type;
            Identifier = new CIdentifier(name);
        }
    }
}
