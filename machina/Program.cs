using System;

var emitter = new Machina.Emitter.Emitter("test");

emitter.EmitFunctionLabel32(name: "main", paramCount: 0);

emitter.SavePreviousFrame64();
emitter.DeclareStackAllocation64(size: 0);

emitter.Load(2);
emitter.Load(3);
emitter.EmitCall32(name: "add", argCount: 2);

emitter.RestorePreviousFrame64();
emitter.EmitRetInt32();

emitter.EmitFunctionLabel32("add", 2);

emitter.EmitAddInt32();
emitter.EmitRetInt32();

Console.WriteLine(emitter.GetAssembly());