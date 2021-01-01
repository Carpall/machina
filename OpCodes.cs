using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public struct Instruction
    {
        public readonly OpCodes OpCode;
        public readonly Object[] Argument;
        public Instruction(OpCodes opcode, object[] arg)
        {
            OpCode = opcode;
            Argument = arg;
        }
    }
    public enum OpCodes
    {
        LoadArrayElem,
        UnsafeAsm,
        Call,
        Ret,
        UnsafeEmitGlobal,
        LoadString,
        Enter,
        LoadMem,
        StoreMem,
        LoadInstance,
        MemDeclare,
        LoadInt,
        StoreField,
        LoadField,
        StoreArrayElem,
        LoadArray,
        CallInstance,
        AddInt,
        LoadMemPointer,
        Push,
        Pop,
        DecrementRefCount,
        IncrementRefCount,
        LoadGlobal,
        StoreGlobal,
    }
}
