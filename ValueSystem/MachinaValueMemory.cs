using Machina.CModels;
using Machina.TypeSystem;

namespace Machina.ValueSystem
{
    public struct MachinaValueMemory : IMachinaValue
    {
        public IMachinaType Type { get; }
        public CIdentifier Identifier { get; }
        public bool IsConst => false;
        public bool CanBePointed => true;

        public MachinaValueMemory(IMachinaType type, string name)
        {
            Type = type;
            Identifier = new CIdentifier(name);
        }

        public string GetCValue()
        {
            return $"({Type.GetCType()}){Identifier.Name}";
        }
    }
}
