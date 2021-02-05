using Machina.Emitter;

namespace Machina.AssemblyBuilder
{
    interface AssemblyBuilder
    {
        public abstract InstructionBuilder8086 GetInstructions(); 
        public abstract string DumpAssembly();
    }
}