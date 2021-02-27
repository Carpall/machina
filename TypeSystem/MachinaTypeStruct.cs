using Machina.Models.Function;

namespace Machina.TypeSystem
{
    struct MachinaTypeStruct : IMachinaType
    {
        public int Size { get; }
        public string CType { get; }
        public TypeKind Kind => TypeKind.Struct;

        public MachinaTypeStruct(MachinaStructure structure)
        {
            var counter = 0;
            structure.Structure.Body.ForEach(_ => counter++);
            Size = counter;

            CType = structure.Structure.Prototype.Name.Name;
        }

        public string GetCType()
        {
            return $"struct {CType}";
        }
    }
}
