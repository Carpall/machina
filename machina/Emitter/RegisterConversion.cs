using System;

namespace Machina.Emitter
{
    public struct RegisterConversion
    {
        public AssemblyType Type { get; set; }
        public string ToString(Enum registerKind)
        {
            return $"{Type.ToString().ToLower()} ptr [{registerKind}]";
        }
    }
}