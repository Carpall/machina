using Machina.TypeSystem;
using System.Collections.Generic;

namespace Machina.CModels
{
    struct CFunctionPrototype : ICPrototype
    {
        public IMachinaType ReturnType { get; }
        public CIdentifier Name { get; }
        public List<CVariableInfo> Parameters { get; }

        public CFunctionPrototype(IMachinaType type, CIdentifier identifier, List<CVariableInfo> parameters)
        {
            ReturnType = type;
            Name = identifier;
            Parameters = parameters;
        }
    }
}
