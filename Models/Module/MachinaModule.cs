using Machina.Models.Function;

namespace Machina.Models.Module
{
    public class MachinaModule
    {
        private readonly CGenerator.CGenerator _generator = new();
        private readonly string _moduleName;

        public MachinaModule(string modulename)
        {
            _moduleName = modulename;
        }

        public void AddFunction(MachinaFunction function)
        {
            _generator.GenerateFunction(function.Function);
        }

        public string DumpModule()
        {
            return _generator.GetCode();
        }
    }
}
