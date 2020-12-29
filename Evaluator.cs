using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public class Evaluator
    {
        CodeEmitter Emitter;
        Bytecode Bytecode;
        void Generate()
        {

        }
        public Evaluator(string moduleName, Bytecode bytecode)
        {
            Emitter = new(moduleName);
            Bytecode = bytecode;
            Generate();
        }
        public override string ToString()
        {
            return Emitter.ToString();
        }
    }
}
