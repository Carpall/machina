using Machina.TypeSystem;
using System.Collections.Generic;

namespace Machina.CModels
{
    struct CStructure : ICGlobalMember
    {
        public CStructPrototype Prototype { get; }
        public List<CVariableInfo> Body { get; }
        public int Size
        {
            get
            {
                var counter = 0;
                Body.ForEach(field => counter += field.Type.Size);

                return counter;
            }
        }

        public CStructure(CIdentifier name)
        {
            Prototype = new(name);
            Body = new();
        }
    }
}
