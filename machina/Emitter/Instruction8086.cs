using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    public struct Instruction8086
    {
        public string Label { get; set; }
        public InstructionKind8086 Kind { get; set; }
        public Value Arg0 { get; set; }
        public Value Arg1 { get; set; }
    }
}
