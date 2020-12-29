using Machina;
using System;
using static System.Console;

var emitter = new CodeEmitter("file.s");
emitter.EmitLabel("main");
emitter.EmitSaveRSP(16);
emitter.EmitLoadString("ciao");
emitter.EmitCall("io::print(str)void");
emitter.EmitRestoreRSP(16);
emitter.EmitReturn();

WriteLine(emitter.ToString());

// Bytecode bt = new("test");
// WriteLine(bt.CompileAOT());