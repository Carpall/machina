using Machina;
using System;
using static System.Console;

var emitter = new CodeEmitter("file.s");
emitter.EmitLabel("main");
emitter.EmitSaveRSP(16);
emitter.EmitLoad32BitValue(10);
emitter.EmitLoad32BitValue(10);
emitter.EmitAdd32Bit();
emitter.EmitLoad32BitValue(20);
emitter.EmitLoad32BitValue(20);
emitter.EmitAdd32Bit();
emitter.EmitAdd32Bit();
emitter.EmitRestoreRSP(16);
emitter.EmitReturn();

WriteLine(emitter.ToString());

// Bytecode bt = new("test");
// WriteLine(bt.CompileAOT());