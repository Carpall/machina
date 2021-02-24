using System;

namespace Machina.CModels
{
    public struct CIdentifier
    {
        public string Name { get; }

        private static void VerifyIdentifier(string identifier)
        {
            if (identifier == string.Empty || !(char.IsLetter(identifier[0]) || identifier[0] == '_'))
                throw new ArgumentException("invalid identifier");

            for (int i = 1; i < identifier.Length; i++)
                if (!(char.IsLetterOrDigit(identifier[i]) || identifier[i] == '_'))
                    throw new ArgumentException("invalid identifier");
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
