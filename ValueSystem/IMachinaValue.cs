using Machina.TypeSystem;
using System;

namespace Machina.ValueSystem
{
    public interface IMachinaValue
    {
        public IMachinaType Type { get; }
        public bool IsConst { get; }
        public bool CanBePointed { get; }

        public abstract string GetCValue();
    }
}