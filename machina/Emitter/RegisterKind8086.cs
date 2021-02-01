using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    enum Register8Kind8086
    {
        al,
        bl,
        cl,
        dl,
        sil,
        dil,
        r8b,
        r9b,
        r10b,
        r11b,
        r12b,
        r13b,
        r14b,
        r15b,
        bpl,
        spl,
    }
    enum Register32Kind8086
    {
        eax,
        ebx,
        ecx,
        edx,
        esi,
        edi,
        r8d,
        r9d,
        r10d,
        r11d,
        r12d,
        r13d,
        r14d,
        r15d,
        ebp,
        esp,
    }
    enum Register64Kind8086
    {
        rax,
        rbx,
        rcx,
        rdx,
        rsi,
        rdi,
        r8,
        r9,
        r10,
        r11,
        r12,
        r13,
        r14,
        r15,
        rbp,
        rsp,
    }
}
