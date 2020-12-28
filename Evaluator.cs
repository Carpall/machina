using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public class Evaluator
    {
        CodeEmitter Emitter = new();
        Bytecode Bytecode;
        void Generate()
        {

        }
        public Evaluator(Bytecode bytecode)
        {
            Bytecode = bytecode;
            Generate();
        }
        public override string ToString()
        {
            return Emitter.ToString();
        }
    }
}
