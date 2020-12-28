using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Machina
{
    public struct Method
    {
        public OpCode[] Instructions;
        public Object[] InstructionArguments;
        public Data[] Arguments;
        public Method(OpCode[] instructions, object[] instructionArgs, Data[] locals, Data[] args)
        {
            Instructions = instructions;
            InstructionArguments = instructionArgs;
            Arguments = args;
            Locals = locals;
        }
    }
}
