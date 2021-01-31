using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machina
{
    class Emitter
    {
        readonly StringBuilder FrontEnd = new();
        readonly Stack<string> Stack = new();
        readonly string[] Registers16 = { "ax", "bx", "cx", "dx", "si", "di" };
        readonly string[] Registers32 = { "eax", "ebx", "ecx", "edx", "esi", "edi" };
        readonly string[] Registers64 = { "rax", "rbx", "rcx", "rdx", "rsi", "rdi" };
        const string BPointer32 = "ebp";
        const string BPointer64 = "rbp";
        const string SPointer32 = "esp";
        const string SPointer64 = "rsp";
        int _stackCounter = 0;
        public Emitter(string filename)
        {
            FrontEnd.AppendLine(@$"
.text
   .file ""{filename}""
   .intel_syntax
   .globl main
");
        }

        void AdvanceCounter()
        {
            _stackCounter++;
        }
        string FetchNext16()
        {
            return Registers16[_stackCounter++];
        }
        string FetchPrevious16()
        {
            return Registers16[--_stackCounter];
        }
        string FetchNext32()
        {
            return Registers32[_stackCounter++];
        }
        string FetchPrevious32()
        {
            return Registers32[--_stackCounter];
        }
        string FetchNext64()
        {
            return Registers64[_stackCounter++];
        }
        string FetchPrevious64()
        {
            return Registers64[--_stackCounter];
        }
        public override string ToString()
        {
            return FrontEnd.ToString();
        }
        public void EmitInstruction(string opcode, string dest = "", string source = "")
        {
            FrontEnd.AppendLine($"   {opcode} {dest}{(source != "" ? $", {source}" : "")}");
        }
        public void EmitLabel(string name)
        {
            FrontEnd.AppendLine($"{name}:");
        }
        public void Load(string value)
        {
            Stack.Push(value);
        }
        string Pop()
        {
            return Stack.Pop();
        }
        void EmitMove32(string dest, string source)
        {
            if (dest != source)
                EmitInstruction("mov", dest, source);
        }
        public void SavePreviousBP32()
        {
            EmitInstruction("push", BPointer32);
            EmitInstruction("mov", BPointer32, SPointer32);
        }
        public void RestorePreviousBP32()
        {
            EmitInstruction("mov", SPointer32, BPointer32);
            EmitInstruction("pop", BPointer32);
        }
        public void SavePreviousBP64()
        {
            EmitInstruction("push", BPointer64);
            EmitInstruction("mov", BPointer64, SPointer64);
        }
        public void RestorePreviousBP64()
        {
            EmitInstruction("leave");
        }
        public void EmitAddInt32()
        {
            var source = Pop();
            EmitMove32(FetchNext32(), Pop());
            var dest = FetchPrevious32();
            EmitInstruction("add", dest, source);
            Load(dest);
        }
        public void EmitSubInt32()
        {
            var source = Pop();
            EmitMove32(FetchNext32(), Pop());
            var dest = FetchPrevious32();
            EmitInstruction("sub", dest, source);
            Load(dest);
        }
        public void EmitMulInt32()
        {
            var source = Pop();
            EmitMove32(FetchNext32(), Pop());
            var dest = FetchPrevious32();
            EmitInstruction("imul", dest, source);
            Load(dest);
        }
        public void EmitDivInt32()
        {
            var source = Pop();
            EmitMove32(FetchNext32(), Pop());
            var dest = FetchPrevious32();
            EmitInstruction("idiv", dest, source);
            Load(dest);
        }
        public void EmitRetInt32()
        {
            EmitMove32(Registers32[0], Pop());
            EmitInstruction("ret");
        }
    }
}
