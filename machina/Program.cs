using System;

var emitter = new Machina.Emitter.Emitter("test");

emitter.EmitFunctionLabel32("main",  0);

emitter.SavePreviousFrame64();
emitter.DeclareStackAllocation64(size: 0);

emitter.Load(1);
emitter.Load(2);
emitter.EmitCompareEQ();
emitter.Load(3);
emitter.Load(4);
emitter.EmitCompareNEQ();
emitter.EmitCompareEQ();

emitter.RestorePreviousFrame64();
emitter.EmitRetInt32();

Console.WriteLine(emitter.GetAssembly());

Console.Write("Press any key...");
Console.ReadKey();