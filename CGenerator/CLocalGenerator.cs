using Machina.CModels;
using Machina.CModels.CStatements;
using System;

namespace Machina.CGenerator
{
    class CLocalGenerator
    {
        private readonly TextGenerator _local = new();

        private void GenerateReturnStatement(CReturnStatement statement)
        {
            _local.WriteReturn();

            if (!statement.IsVoid)
                _local.WriteValue(statement.Body);
        }

        private void GenerateVariableDeclarationStatement(CVariableDeclarationStatement statement)
        {
            _local.WriteType(statement.Type);
            _local.WriteIdentifier(statement.Name);
        }

        private void GenerateAssignmentStatement(CAssignmentStatement statement)
        {
            _local.WriteIdentifier(statement.Identifier);
            _local.WriteEqual();
            _local.WriteValue(statement.Value);
        }

        private void RecognizeStatement(ICStatement statement)
        {
            _local.Indent();

            switch (statement)
            {
                case CReturnStatement returnstatement:
                    GenerateReturnStatement(returnstatement);
                    break;
                case CVariableDeclarationStatement variabledeclarationstatement:
                    GenerateVariableDeclarationStatement(variabledeclarationstatement);
                    break;
                case CAssignmentStatement assignmentstatement:
                    GenerateAssignmentStatement(assignmentstatement);
                    break;
                default:
                    throw new ArgumentException("invalid statement");
            }

            _local.WriteSemicolon();
            _local.NewLine();
        }

        public void WriteStatementBlock(CStatementBlock block)
        {
            _local.NewLine();

            foreach (var statement in block.Statements)
                RecognizeStatement(statement);
        }

        public string GetCode()
        {
            return _local.GetCode();
        }
    }
}