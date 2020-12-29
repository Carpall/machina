using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public class CodeEmitter
    {
        StringBuilder TextSection = new();
        StringBuilder Builder = new();
        Dictionary<String, String> Symbols = new();
        UInt16 StackCount = 0;
        UInt16 GlobalCount = 0;
        string[] FunctionArgRegs32Bit = { "eax", "ebx", "ecx", "edx", "esi", "edi", "r8d", "r9d", "r10d" };
        string[] FunctionArgRegs64Bit = { "rax", "rbx", "rcx", "rdx", "rsi", "rdi", "r8", "r9", "r10" };
        public CodeEmitter(string moduleName)
        {
            TextSection.AppendLine(".text");
            TextSection.AppendLine("   .file \""+moduleName+'"');
            TextSection.AppendLine("   .intel_syntax");
            TextSection.AppendLine("   .globl main");
            // initialize built in
            EmitGlobal("fmt_s", "asciz", "\"%s\"");
            Symbols.Add("io::print(str)void", null);
            EmitAssembly(
@"
""io::print(str)void"":
   push rbp
   mov  rbp, rsp
   sub  rsp, 24
   mov  rdx, rax
   lea  rcx, [rip+""fmt_s""]
   call printf
   xor  eax, eax
   add  rsp, 24
   pop  rbp
   ret
");
        }
        public void EmitAssembly(string asm)
        {
            Builder.Append(asm);
        }
        public string EmitGlobal(string type, string value)
        {
            var symbol = "%c" + type + "=" + value;
            if (Symbols.ContainsKey(symbol))
                return Symbols[symbol];
            var name = "%c" + type + (++GlobalCount);
            Symbols.Add(symbol, name);
            TextSection.AppendLine("   \""+name+"\": ."+type+" "+value);
            TextSection.AppendLine("   .globl \""+name+'"');
            return name;
        }
        public void EmitGlobal(string name, string type, string value)
        {
            if (Symbols.ContainsKey(name))
                return;
            Symbols.Add(name, null);
            TextSection.AppendLine("   \"" + name + "\": ." + type + " " + value);
            TextSection.AppendLine("   .globl \"" + name + '"');
        }
        public void EmitLabel(string name)
        {
            if (Symbols.ContainsKey(name.Trim()))
                throw new Exception($"Label {name} is already declared, or is the name of a function");
            Symbols.Add(name, null);
            Builder.AppendLine(name + ":");
        }
        public void EmitInstruction(string instruction, string op1, string op2, string op3)
        {
            Builder.AppendLine("   " + instruction + " " + op1 + ", " + op2 + ", " + op3);
        }
        public void EmitInstruction(string instruction, string op1, string op2)
        {
            Builder.AppendLine("   " + instruction + " " + op1 + ", " + op2);
        }
        public void EmitInstruction(string instruction, string op1)
        {
            Builder.AppendLine("   " + instruction + " " + op1);
        }
        public void EmitInstruction(string instruction)
        {
            Builder.AppendLine("   " + instruction);
        }
        public void EmitCall(string name, bool isVoid)
        {
            if (!Symbols.ContainsKey(name))
                throw new Exception($"Function {name} is not declared");
            EmitInstruction("call", '"'+name+'"');
            StackCount = Convert.ToUInt16(!isVoid);
        }
        public void EmitLabelIndented(string name)
        {
            EmitLabel("   "+name);
        }
        string FetchNextRegister32Bit()
        {
            var x = FunctionArgRegs32Bit[StackCount];
            StackCount++;
            return x;
        }
        string FetchNextRegister64Bit()
        {
            var x = FunctionArgRegs64Bit[StackCount];
            StackCount++;
            return x;
        }
        string FetchPreviousRegister32Bit()
        {
            var x = FunctionArgRegs32Bit[StackCount-1];
            StackCount--;
            return x;
        }
        string FetchPreviousRegister64Bit()
        {
            var x = FunctionArgRegs64Bit[StackCount-1];
            StackCount--;
            return x;
        }
        string FetchCurrentRegister32Bit()
        {
            return FunctionArgRegs32Bit[StackCount];
        }
        string FetchCurrentRegister64Bit()
        {
            return FunctionArgRegs64Bit[StackCount];
        }
        public void EmitLoad32BitValue(int value)
        {
            EmitInstruction("mov", FetchNextRegister32Bit(), value.ToString());
        }
        public void EmitLoad64BitValue(long value)
        {
            EmitInstruction("mov", FetchNextRegister64Bit(), value.ToString());
        }
        public void EmitLoadString(string value)
        {
            var name = EmitGlobal("asciz", '"'+value+'"');
            EmitInstruction("lea", FetchNextRegister64Bit(), "[rip+\""+ name + "\"]");
        }
        public void EmitLoadAdress(string value)
        {
            EmitInstruction("lea", FetchNextRegister64Bit(), value);
        }
        public void EmitSaveRSP(int size)
        {
            EmitInstruction("push", "rbp");
            EmitInstruction("mov", "rbp", "rsp");
            EmitInstruction("sub", "rsp", size.ToString());
        }
        public void EmitRestoreRSP(int size)
        {
            EmitInstruction("add", "rsp", size.ToString());
            EmitInstruction("pop", "rbp");
        }
        public void EmitAdd32Bit()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            StackCount++;
            EmitInstruction("add", op2, op1);
        }
        public void EmitAdd64Bit()
        {
            var op1 = FetchPreviousRegister64Bit();
            var op2 = FetchPreviousRegister64Bit();
            EmitInstruction("add", op2, op1);
        }
        public void EmitReturn()
        {
            EmitInstruction("ret");
        }
        public override string ToString()
        {
            TextSection.Append(Builder.ToString());
            return TextSection.ToString();
        }
    }
}
