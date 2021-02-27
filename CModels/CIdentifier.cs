using System;
using System.Collections.Generic;

namespace Machina.CModels
{
    public struct CIdentifier
    {
        private const string EntryPointName = "main";
        public string Name { get; }

        private static void VerifyIdentifier(string identifier)
        {
            if (identifier == string.Empty || !(char.IsLetter(identifier[0]) || identifier[0] == '_'))
                throw new ArgumentException("invalid identifier");

            for (int i = 1; i < identifier.Length; i++)
                if (!(char.IsLetterOrDigit(identifier[i]) || identifier[i] == '_'))
                    throw new ArgumentException("invalid identifier");
        }

        public static CIdentifier FunctionPrototype(string name, List<CVariableInfo> parameters)
        {
            if (name == EntryPointName)
                return new CIdentifier(name);

            if (parameters.Count == 0)
                return new CIdentifier($"{name}v");

            var parameterlist = "";
            parameters.ForEach(parameter => parameterlist += parameter.Type.CType);

            return new CIdentifier($"{name}{parameterlist}");
        }

        public CIdentifier(string identifier)
        {
            VerifyIdentifier(identifier);
            Name = identifier;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
