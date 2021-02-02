using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    class InstructionBuilder8086
    {
        public string EntryPoint { private get; set; }
        public string FileName { private get; set; }

        readonly List<Instruction8086> _builder = new();
        const string Indent = "   ";

        public void EmitInstruction(Instruction8086 instruction)
        {
            _builder.Add(instruction);
        }
        public string Assemble(bool generateText = true)
        {
            StringBuilder assembly = new();
            if (generateText)
                assembly.AppendLine(@$".text
   .file ""{FileName}""
   .globl {EntryPoint}
   .intel_syntax
");
            for (int i = 0; i < _builder.Count; i++) {
                var label = _builder[i].Label;
                if (_builder[i].Kind == InstructionKind8086.Label)
                {
                    assembly.AppendLine($"{label}:");
                    continue;
                }
                var opcode = _builder[i].Kind;
                var arg0 = _builder[i].Arg0;
                var arg1 = _builder[i].Arg1;
                assembly.AppendLine(
                    $"{Indent}{(!string.IsNullOrEmpty(label) ? $"{label}: " : "")}{opcode} {arg0}{(!arg1.IsEmpty ? $", {arg1}" : "")}"
                );
            }
            return assembly.ToString();
        }
    }
}