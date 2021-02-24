using Machina.ValueSystem;

namespace Machina.CModels.CStatements
{
    struct CReturnStatement : ICStatement
    {
        public IMachinaValue Body { get; }
        public bool IsVoid
        {
            get
            {
                return Body is MachinaValueVoid;
            }
        }

        public CReturnStatement(IMachinaValue body)
        {
            Body = body;
        }
    }
}
