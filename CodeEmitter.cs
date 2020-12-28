using System;
using System.Collections.Generic;
using System.Text;

namespace Machina
{
    public class CodeEmitter
    {
        StringBuilder Builder = new();
        Stack<Data> VirtualStack = new();
        public void EmitCall(string name)
        {
            for (int i = 0; i < VirtualStack.Count; i++)
            {
                var p = VirtualStack.Pop();
                if (p.Name == null)

            }
        }
        public void EmitLoad(Data data)
        {
            VirtualStack.Push(data);
        }
        public void EmitUnload()
        {
            VirtualStack.Pop();
        }
    }
}
