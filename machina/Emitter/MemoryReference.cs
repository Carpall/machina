using System;

namespace Machina.Emitter
{
    struct MemoryReference
    {
        public int Index { get; set; }
        public Enum MemoryPointer { get; set; }

        public override string ToString()
        {
            return $"[{MemoryPointer}{(Index > 0 ? $"+{Index}" : Index)}]";
        }
    }
}