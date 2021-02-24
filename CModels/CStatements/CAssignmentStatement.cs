using Machina.ValueSystem;

namespace Machina.CModels.CStatements
{
    struct CAssignmentStatement : ICStatement
    {
        public CIdentifier Identifier { get; }
        public IMachinaValue Value { get; }

        public CAssignmentStatement(CIdentifier identifier, IMachinaValue value)
        {
            Identifier = identifier;
            Value = value;
        }
    }
}
