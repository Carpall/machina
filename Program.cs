using Machina;
using System;
using System.Diagnostics;
using System.IO;
using static System.Console;

var emitter = new CodeEmitter("file.s");
emitter.EmitLabel("main");
emitter.EmitSaveStackPointer();
emitter.EmitRestoreStackPointer();
emitter.EmitReturn();

WriteLine(emitter.ToString());

// Bytecode bt = new("test");
// WriteLine(bt.CompileAOT());