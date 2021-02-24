using Machina.TypeSystem;
using Machina.ValueSystem;

namespace Machina.CModels.CStatements
{
    struct CVariableDeclarationStatement : ICStatement
    {
        public IMachinaType Type { get; }
        public CIdentifier Name { get; }

        public CVariableDeclarationStatement(IMachinaType type, CIdentifier name)
        {
            Type = type;
            Name = name;
        }
    }
}
