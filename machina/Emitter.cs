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
        readonly List<string> Stack = new();
        readonly string[] Registers16 = { "ax", "bx", "cx", "dx", "si", "di" };
        readonly string[] Registers32 = { "eax", "ebx", "ecx", "edx", "esi", "edi" };
        readonly string[] Registers64 = { "rax", "rbx", "rcx", "rdx", "rsi", "rdi" };
        const string BPointer32 = "ebp";
        const string BPointer64 = "rbp";
        const string SPointer32 = "esp";
        const string SPointer64 = "rsp";
        const int PassArgumentsOffset = 2;
        int _stackCounter = 0;

        public string GetAssembly(bool generateText = true)
        {
            if (generateText)
                FrontEnd.Insert(0, @$".text
   .intel_syntax
   .globl main

");
            return FrontEnd.ToString();
        }
        void AdvanceCounter(int count = 1)
        {
            _stackCounter += count;
        }
        void ResetCounter(int to)
        {
            _stackCounter = to;
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
        public void EmitInstruction(string opcode, string dest = "", string source = "")
        {
            FrontEnd.AppendLine($"   {opcode} {dest}{(source != "" ? $", {source}" : "")}");
        }
        public void EmitLabel(string name)
        {
            FrontEnd.AppendLine($"{name}:");
        }
        void PushParams(int argCount)
        {
            if (argCount > 0)
            {
                for (; argCount > 0; argCount--)
                    Load(FetchNext32());
            }
        }
        public void EmitFunctionLabel32(string name, int paramCount)
        {
            ResetCounter(PassArgumentsOffset);
            PushParams(paramCount);
            EmitLabel(name);
        }
        string AlignSize(int size)
        {
            for (; true; size++)
                if ((size % 16) == 0)
                    return size.ToString();
        }
        public void DeclareStackAllocation64(int size = 0)
        {
            if (size != 0)
                EmitInstruction("sub", BPointer64, AlignSize(size));
        }
        public void DeclareStackAllocation32(int size = 0)
        {
            if (size != 0)
                EmitInstruction("add", BPointer32, AlignSize(size));
        }
        public void DeclareStackDellocation32(int size = 0)
        {
            if (size != 0)
                EmitInstruction("add", BPointer32, AlignSize(size));
        }
        public void Load(string value)
        {
            Stack.Add(value);
        }
        string Pop()
        {
            var p = Stack[^1];
            Stack.RemoveAt(Stack.Count - 1);
            if (p == "@pop") {
                var fetch = FetchNext64();
                EmitInstruction("pop", fetch);
                AdvanceCounter(-1);
                return fetch;
            }
            return p;
        }
        void EmitMove(string dest, string source)
        {
            if (dest != source)
            {
                if (source == "0")
                    EmitInstruction("xor", dest, dest);
                else
                    EmitInstruction("mov", dest, source);
            }
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
        public void SavePreviousFrame64()
        {
            EmitInstruction("push", BPointer64);
            EmitInstruction("mov", BPointer64, SPointer64);
        }
        public void RestorePreviousFrame64()
        {
            EmitInstruction("leave");
        }
        public void EmitAddInt32()
        {
            var source = Pop();
            EmitMove(FetchNext32(), Pop());
            var dest = FetchPrevious32();
            EmitInstruction("add", dest, source);
            Load(dest);
        }
        public void EmitSubInt32()
        {
            var source = Pop();
            EmitMove(FetchNext32(), Pop());
            var dest = FetchPrevious32();
            EmitInstruction("sub", dest, source);
            Load(dest);
        }
        public void EmitMulInt32()
        {
            var source = Pop();
            EmitMove(FetchNext32(), Pop());
            var dest = FetchPrevious32();
            EmitInstruction("imul", dest, source);
            Load(dest);
        }
        public void EmitDivInt32()
        {
            var source = Pop();
            EmitMove(FetchNext32(), Pop());
            var dest = FetchPrevious32();
            EmitInstruction("idiv", dest, source);
            Load(dest);
        }
        public void EmitRetInt32()
        {
            EmitMove(Registers32[0], Pop());
            EmitInstruction("ret");
        }
        public void EmitRetInt64()
        {
            EmitMove(Registers64[0], Pop());
            EmitInstruction("ret");
        }
        public void EmitRetVoid()
        {
            EmitInstruction("ret");
        }
        bool IsConstant(string value)
        {
            for (int i = 0; i < value.Length; i++)
                if (!char.IsDigit(value[i]))
                    return false;
            return true;
        }
        void SaveStackItems(int skipCount)
        {
            for (int i = Stack.Count - skipCount; i > 0; i--)
            {
                var p = Stack[i - 1];
                if (!IsConstant(p))
                {
                    EmitInstruction("push", p);
                    Stack[i - 1] = "@pop";
                }
            }
        }
        delegate string bitSizeFetch();
        void PassArguments(int argCount, bitSizeFetch fetchPrevious)
        {
            if (argCount > 0)
            {
                ResetCounter(PassArgumentsOffset + argCount);
                for (; argCount > 0; argCount--)
                    EmitMove(fetchPrevious(), Pop());
            }
        }
        public void EmitCall64(string name, int argCount)
        {
            SaveStackItems(argCount);
            PassArguments(argCount, FetchPrevious64);
            EmitInstruction("call", name);
            Load(Registers64[0]);
        }
        public void EmitCall32(string name, int argCount)
        {
            SaveStackItems(argCount);
            PassArguments(argCount, FetchPrevious32);
            EmitInstruction("call", name);
            Load(Registers32[0]);
        }
    }
}
