using Machina.TypeSystem;

namespace Machina.CModels
{
    public struct CVariableInfo
    {
        public IMachinaType Type { get; }
        public CIdentifier Name { get; }
        
        public CVariableInfo(IMachinaType type, CIdentifier identifier)
        {
            Type = type;
            Name = identifier;
        }

        public override string ToString()
        {
            return $"{Type.GetCType()} {Name.Name}";
        }
    }
}
