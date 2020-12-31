﻿using Machina;
using System;
using System.IO;
using static System.Console;


var bt = new Bytecode(moduleName: "file.s");

Function main = new("main", "void", 8);

main.AddParameter("args", 8);

main.AddInstruction(OpCodes.Enter);
main.AddInstruction(OpCodes.LoadString, "Ciao Mondo");
main.AddInstruction(OpCodes.CallPointer);
main.AddInstruction(OpCodes.Ret);

bt.InstallFunction(main);

//bt.Save("C:/Users/Mondelli/Desktop");
WriteLine(bt.CompileAOT());