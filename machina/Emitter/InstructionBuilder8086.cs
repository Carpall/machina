using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    public class InstructionBuilder8086
    {
        public string EntryPoint { private get; set; }
        public string FileName { private get; set; }

        public readonly List<Instruction8086> Builder = new();
        const string Indent = "   ";

        public void EmitInstruction(Instruction8086 instruction)
        {
            Builder.Add(instruction);
        }
        
        public string DumpAssembly(bool generateText = true)
        {
            StringBuilder assembly = new();
            if (generateText)
                assembly.AppendLine(@$".text
   .file ""{FileName}""
   .globl {EntryPoint}
   .intel_syntax
");
            for (int i = 0; i < Builder.Count; i++) {
                var label = Builder[i].Label;
                if (Builder[i].Kind == InstructionKind8086.Label)
                {
                    assembly.AppendLine($"{label}:");
                    continue;
                }
                var opcode = Builder[i].Kind;
                var arg0 = Builder[i].Arg0;
                var arg1 = Builder[i].Arg1;
                assembly.AppendLine(
                    $"{Indent}{(!string.IsNullOrEmpty(label) ? $"{label}: " : "")}{opcode} {arg0}{(!arg1.IsEmpty ? $", {arg1}" : "")}"
                );
            }
            return assembly.ToString();
        }
    }
}