using System;
using Machina.Emitter;

var emitter = new Machina.Emitter.Emitter("test");

emitter.EmitFunctionLabel32("main",  0);

emitter.SavePreviousFrame64();
emitter.DeclareStackAllocation64(0);

emitter.Load(1);
emitter.Load(2);
emitter.EmitAddInt32();

emitter.RestorePreviousFrame64();
emitter.EmitRetInt32();

Console.WriteLine(emitter.GetAssembly());

Console.ReadKey();