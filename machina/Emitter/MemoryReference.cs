using System;

namespace Machina.Emitter
{
    public struct MemoryReference
    {
        public int Index { get; set; }
        public Enum MemoryPointer { get; set; }
        public AssemblyType AssemblyType { get; set; }

        public override string ToString()
        {
            return $"{AssemblyType.ToString().ToLower()} ptr [{MemoryPointer}{(Index > 0 ? $"+{Index}" : Index)}]";
        }
    }
}