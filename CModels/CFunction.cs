using Machina.TypeSystem;
using System.Collections.Generic;

namespace Machina.CModels
{
    struct CFunction : ICGlobalMember
    {
        public CFunctionPrototype Prototype { get; }
        public CStatementBlock Body { get; }

        public CFunction(IMachinaType type, CIdentifier name, List<CVariableInfo> parameters, CStatementBlock body)
        {
            Prototype = new(type, name, parameters);
            Body = body;
        }
    }
}
