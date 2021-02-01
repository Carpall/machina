using System;
using System.Collections.Generic;
using System.Text;

namespace Machina.Emitter
{
    class RegistersBag8086
    {
        public const Register8Kind8086 ReturnRegister16 = Register8Kind8086.al;
        public const Register32Kind8086 ReturnRegister32 = Register32Kind8086.eax;
        public const Register64Kind8086 ReturnRegister64 = Register64Kind8086.rax;
        int _registerCounter = 0;
        public void AdvanceCounter(int count = 1)
        {
            _registerCounter += count;
        }
        public void ResetCounter(int to)
        {
            _registerCounter = to;
        }
        public Register8Kind8086 FetchNext8()
        {
            return (Register8Kind8086)_registerCounter++;
        }
        public Register8Kind8086 FetchPrevious8()
        {
            return (Register8Kind8086)(--_registerCounter);
        }
        public Register32Kind8086 FetchNext32()
        {
            return (Register32Kind8086)_registerCounter++;
        }
        public Register32Kind8086 FetchPrevious32()
        {
            return (Register32Kind8086)(--_registerCounter);
        }
        public Register64Kind8086 FetchNext64()
        {
            return (Register64Kind8086)_registerCounter++;
        }
        public Register64Kind8086 FetchPrevious64()
        {
            return (Register64Kind8086)(--_registerCounter);
        }

        public override string ToString()
        {
            return _registerCounter.ToString();
        }
    }
}
