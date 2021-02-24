using Machina.TypeSystem;
using System;

namespace Machina.ValueSystem
{
    public interface IMachinaValue
    {
        public IMachinaType Type { get; }
        public bool IsConst { get; }

        public string GetCValue()
        {
            string value = this switch
            {
                MachinaValueInt mv => mv.Value.ToString(),
                MachinaValueBinary mv => mv.ToString(),
                MachinaValueBool mv => mv.Value.ToString(),
                MachinaValueNot mv => mv.ToString(),
                _ => throw new ArgumentException("invalid type"),
            };

            return $"({Type.GetCType()}){value}";
        }
    }
}