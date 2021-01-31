using System;

var emitter = new Machina.Emitter("test");
emitter.EmitLabel("main");
emitter.SavePreviousBP64();
emitter.Load("9");
emitter.Load("2");
emitter.EmitAddInt32();
emitter.Load("3");
emitter.EmitAddInt32();
emitter.RestorePreviousBP64();
emitter.EmitRetInt32();
Console.WriteLine(emitter);