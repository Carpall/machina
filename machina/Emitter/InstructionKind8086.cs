using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    enum InstructionKind8086
    {
        Label,
        mov,
        add,
        sub,
        imul,
        idiv,
        push,
        pop,
        xor,
        leave,
        ret,
        call,
        cmp,
        sete,
        setne,
        setg,
        setl,
        movzx,
        jmp,
        je,
        jne,
        jg,
        jl
    }
}
