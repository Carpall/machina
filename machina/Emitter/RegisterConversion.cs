using System;

namespace Machina.Emitter
{
    struct RegisterConversion
    {
        public AssemblyType Type { get; set; }
        public string ToString(Enum registerKind)
        {
            return $"{Type.ToString().ToLower()} ptr [{registerKind}]";
        }
    }
}