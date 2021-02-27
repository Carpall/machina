using Machina.TypeSystem;
using Machina.ValueSystem;

namespace Machina.CModels.CStatements
{
    struct CCallStatement : ICStatement
    {
        public IMachinaValue[] Parameters { get; }
        public CIdentifier Identifier { get; }

        public CCallStatement(CIdentifier identifier, IMachinaValue[] parameters)
        {
            Identifier = identifier;
            Parameters = parameters;
        }
    }
}
