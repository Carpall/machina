using System;
using System.Collections.Generic;
using Machina.Emitter;

namespace Machina.AssemblyBuilder
{
    public class AssemblyImage
    {
        readonly List<AssemblyBuilder> _image = new();
        readonly string _filename;
        
        public AssemblyImage(string filename)
        {
            _filename = filename;
        }
        
        Emitter.Emitter Build()
        {
            var result = new Emitter.Emitter(_filename);
            foreach (var builder in _image)
            {
                var instructions = builder.GetInstructions();
                foreach (var instruction in instructions.Builder)
                    result.EmitInstruction(instruction);
            }
            return result;
        }
        public string DumpAssembly() => Build().DumpAssembly();

        public void DefineFunction(FunctionBuilder64 function)
        {
            _image.Add(function);
        }
    }
}