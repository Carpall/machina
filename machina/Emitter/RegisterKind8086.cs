using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    enum Register16Kind8086
    {
        ax,
        bx,
        cx,
        dx,
        si,
        di,
        bp,
        sp,
        r8w,
        r9w,
        r10w,
        r11w,
        r12w,
        r13w,
        r14w,
        r15w
    }
    enum Register32Kind8086
    {
        eax,
        ebx,
        ecx,
        edx,
        esi,
        edi,
        ebp,
        esp,
        r8d,
        r9d,
        r10d,
        r11d,
        r12d,
        r13d,
        r14d,
        r15d
    }
    enum Register64Kind8086
    {
        rax,
        rbx,
        rcx,
        rdx,
        rsi,
        rdi,
        rbp,
        rsp,
        r8,
        r9,
        r10,
        r11,
        r12,
        r13,
        r14,
        r15
    }
}
