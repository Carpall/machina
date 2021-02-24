namespace Machina.CModels
{
    struct CStructPrototype : ICPrototype
    {
        public CIdentifier Name { get; }

        public CStructPrototype(CIdentifier identifier)
        {
            Name = identifier;
        }
    }
}
