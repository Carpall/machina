using Machina.CModels;
using Machina.Models.Function;
using System;
using System.Collections.Generic;

namespace Machina.CGenerator
{
    class CGenerator
    {
        private readonly List<CInclude> _includes = new();
        private readonly List<ICPrototype> _prototypes = new();
        private readonly TextGenerator _module = new();

        public CGenerator()
        {
            Include(new CInclude(false, "stdint.h"));
        }

        public void GenerateFunction(CFunction function)
        {
            var scope = new CLocalGenerator();

            scope.WriteStatementBlock(function.Body);

            _module.WriteFunctionPrototype(function.Prototype.ReturnType, function.Prototype.Name, function.Prototype.Parameters, true);

            _module.WriteBlock(scope.GetCode());
            _module.NewLine();

            _prototypes.Add(function.Prototype);
        }

        public void GenerateStructure(CStructure structure)
        {
            var scope = new TextGenerator();

            structure.Body.ForEach(field =>
            {
                scope.NewLine();
                scope.Indent();
                scope.Write(field.ToString());
                scope.WriteSemicolon();
                scope.NewLine();
            });

            _module.WriteStructurePrototype(structure.Prototype.Name.Name, true);

            _module.WriteBlock(scope.GetCode());
            _module.WriteSemicolon();
            _module.NewLine();
        }

        public void Include(CInclude include)
        {
            _includes.Add(include);
        }

        private void WriteIncludes(ref TextGenerator result)
        {
            foreach (var include in _includes)
                result.WriteInclude(include);
        }

        private void WritePrototypes(ref TextGenerator result)
        {
            foreach (var prototype in _prototypes)
            {
                if (prototype is CFunctionPrototype function)
                    result.WriteFunctionPrototype(function.ReturnType, function.Name, function.Parameters, false);
                else if (prototype is CStructPrototype structp)
                    result.WriteStructurePrototype(structp.Name.Name, false);
                else
                    throw new ArgumentException("invalid prototype");

                result.NewLine();
            }
        }

        public string GetCode()
        {
            var result = new TextGenerator();

            WriteIncludes(ref result);

            result.NewLine();
            result.NewLine();
            
            WritePrototypes(ref result);

            result.NewLine();
            result.NewLine();

            result.LinkTextgenerator(_module);

            return result.GetCode();
        }
    }
}
