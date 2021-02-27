using Machina.Models.Function;
using System;
using System.Collections.Generic;

namespace Machina.Models.Module
{
    public class MachinaModule
    {
        private readonly CGenerator.CGenerator _generator = new();
        private readonly List<MachinaFunction> _functios = new();
        private readonly List<MachinaStructure> _structures = new();

        private readonly string _moduleName;

        public MachinaModule(string modulename)
        {
            _moduleName = modulename;
        }

        public void AddFunction(MachinaFunction function)
        {
            _functios.Add(function);
        }

        public void AddStructure(MachinaStructure structure)
        {
            _structures.Add(structure);
        }

        public void Generate()
        {
            _structures.ForEach(structure => _generator.GenerateStructure(structure.Structure));

            _functios.ForEach(function => _generator.GenerateFunction(function.Function));
        }

        public string DumpModule()
        {
            return _generator.GetCode();
        }
    }
}
