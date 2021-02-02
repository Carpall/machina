namespace Machina.Emitter
{
    struct RegisterConversion
    {
        public AssemblyType Type { get; set; }
        public Value Body { get; set; }
        public override string ToString()
        {
            return $"{Type.ToString().ToLower()} ptr [{Body}]";
        }
    }
}