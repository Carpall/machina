namespace Machina.CGenerator
{
    struct CInclude
    {
        public bool IsLocal { get; }
        public string Path { get; }

        public CInclude(bool isLocal, string path)
        {
            IsLocal = isLocal;
            Path = path;
        }
    }
}
