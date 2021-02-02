using System;
using Machina.Emitter;

var emitter = new Machina.Emitter.Emitter("test");

emitter.EmitFunctionLabel32("main",  0);

emitter.SavePreviousFrame64();
emitter.DeclareStackAllocation64(0);

emitter.Load(Value.Constant(7));
emitter.EmitStoreInStack64(-4, AssemblyType.QWORD);
emitter.EmitLoadFromStack64(-4, AssemblyType.QWORD);
emitter.EmitLoadFromStack64(-4, AssemblyType.QWORD);
emitter.EmitCompareEQ();

emitter.EmitRetInt32();

Console.WriteLine(emitter.DumpAssembly());

Console.ReadKey();