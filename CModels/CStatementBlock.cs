using Machina.CModels.CStatements;
using Machina.TypeSystem;
using Machina.ValueSystem;
using System;
using System.Collections.Generic;

namespace Machina.CModels
{
    class CStatementBlock
    {
        public List<ICStatement> Statements { get; } = new();

        internal void Return(IMachinaValue value)
        {
            Statements.Add(new CReturnStatement(value));
        }

        internal void DeclareVariable(IMachinaType type, string name)
        {
            Statements.Add(new CVariableDeclarationStatement(type, new CIdentifier(name)));
        }

        internal void AssignVariable(string name, IMachinaValue value)
        {
            Statements.Add(new CAssignmentStatement(new CIdentifier(name), value));
        }
    }
}
