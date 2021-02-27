using Machina.CModels;
using Machina.TypeSystem;

namespace Machina.Models.Function
{
    public class MachinaStructure
    {
        internal CStructure Structure { get; }


        public MachinaStructure(string name)
        {
            Structure = new(new CIdentifier(name));
        }

        public void AddField(IMachinaType type, string name)
        {
            Structure.Body.Add(new(type, new(name)));
        }
    }
}
