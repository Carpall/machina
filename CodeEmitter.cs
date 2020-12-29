﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public class CodeEmitter
    {
        StringBuilder Functions = new();
        StringBuilder TextSection = new();
        StringBuilder Builder = new();
        Dictionary<String, String> Symbols = new();
        UInt16 StackCount = 0;
        UInt16 GlobalCount = 0;
        string[] FunctionArgRegs8Bit =  { "al",  "bl",  "cl",  "dl",  "sil", "dil" };
        string[] FunctionArgRegs32Bit = { "eax", "ebx", "ecx", "edx", "esi", "edi" };
        string[] FunctionArgRegs64Bit = { "rax", "rbx", "rcx", "rdx", "rsi", "rdi" };
        public CodeEmitter(string moduleName)
        {
            TextSection.AppendLine(".text");
            TextSection.AppendLine("   .file \""+moduleName+'"');
            TextSection.AppendLine("   .intel_syntax");
            TextSection.AppendLine("   .globl main");
        }
        int RoundToStackAllignment(int size)
        {
            for (int i = 0; ; i++)
                if ((size+i) % 16 == 0)
                    return size + i;
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
        string FetchNextRegister8Bit()
        {
            var x = FunctionArgRegs8Bit[StackCount];
            StackCount++;
            return x;
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
        string FetchPreviousRegister8Bit()
        {
            var x = FunctionArgRegs8Bit[StackCount-1];
            StackCount--;
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
        string FetchCurrentRegister8Bit()
        {
            return FunctionArgRegs8Bit[StackCount];
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
        public void EmitLoadVoid()
        {
            var top = FetchNextRegister32Bit();
            EmitInstruction("xor", top, top);
        }
        public void EmitLoadString(string value)
        {
            var name = EmitGlobal("asciz", '"'+value+'"');
            EmitLoadAdress("[rip+\"" + name + "\"]");
        }
        public void EmitLoadTrue()
        {
            EmitInstruction("mov", FetchNextRegister8Bit(), "1");
        }
        public void EmitLoadFalse()
        {
            EmitInstruction("mov", FetchNextRegister8Bit(), "0");
        }
        public void EmitLoadAdress(string value)
        {
            EmitInstruction("lea", FetchNextRegister64Bit(), value);
        }
        public void EmitSaveStackPointer(int size = 0)
        {
            EmitInstruction("push", "rbp");
            EmitInstruction("mov", "rbp", "rsp");
            if (size != 0)
                EmitInstruction("sub", "rsp", RoundToStackAllignment(size).ToString());
        }
        public void EmitRestoreStackPointer(int size = 0)
        {
            if (size != 0)
                EmitInstruction("add", "rsp", RoundToStackAllignment(size).ToString());
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
            StackCount++;
            EmitInstruction("add", op2, op1);
        }
        public void EmitSub32Bit()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            StackCount++;
            EmitInstruction("sub", op2, op1);
        }
        public void EmitSub64Bit()
        {
            var op1 = FetchPreviousRegister64Bit();
            var op2 = FetchPreviousRegister64Bit();
            StackCount++;
            EmitInstruction("sub", op2, op1);
        }
        public void EmitMul32Bit()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            StackCount++;
            EmitInstruction("imul", op2, op1);
        }
        public void EmitMul64Bit()
        {
            var op1 = FetchPreviousRegister64Bit();
            var op2 = FetchPreviousRegister64Bit();
            StackCount++;
            EmitInstruction("imul", op2, op1);
        }
        public void EmitDiv32Bit()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            StackCount++;
            EmitInstruction("idiv", op2, op1);
        }
        public void EmitDiv64Bit()
        {
            var op1 = FetchPreviousRegister64Bit();
            var op2 = FetchPreviousRegister64Bit();
            StackCount++;
            EmitInstruction("idiv", op2, op1);
        }
        public void EmitLoadMem32Bit(int index)
        {
            EmitInstruction("mov", FetchNextRegister32Bit(), "[rbp-" + index + "]");
        }
        public void EmitLoadMem64Bit(int index)
        {
            EmitInstruction("mov", FetchNextRegister64Bit(), "[rbp-" + index + "]");
        }
        public void EmitStoreMem32Bit(int index)
        {
            EmitInstruction("mov", "[rbp-" + index + "]", FetchPreviousRegister32Bit());
        }
        public void EmitStoreMem64Bit(int index)
        {
            EmitInstruction("mov", "[rbp-" + index + "]", FetchPreviousRegister64Bit());
        }
        public void EmitCompareEQ()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            EmitInstruction("cmp", op2, op1);
            EmitInstruction("sete", FetchNextRegister8Bit());
        }
        public void EmitCompareNEQ()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            EmitInstruction("cmp", op2, op1);
            EmitInstruction("setne", FetchNextRegister8Bit());
        }
        public void EmitCompareL()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            EmitInstruction("cmp", op2, op1);
            EmitInstruction("setl", FetchNextRegister8Bit());
        }
        public void EmitCompareG()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            EmitInstruction("cmp", op2, op1);
            EmitInstruction("setg", FetchNextRegister8Bit());
        }
        public void EmitJumpTrue()
        {
            var op1 = FetchPreviousRegister32Bit();
            var op2 = FetchPreviousRegister32Bit();
            EmitInstruction("cmp", op2, op1);
            EmitInstruction("setg", FetchNextRegister8Bit());
        }
        public void EmitReturn()
        {
            EmitInstruction("ret");
        }
        public override string ToString()
        {
            TextSection.Append(Builder.ToString());
            TextSection.Append(Functions.ToString());
            return TextSection.ToString();
        }
    }
}
