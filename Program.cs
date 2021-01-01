using Machina;
using System;
using System.IO;
using static System.Console;


var bt = new Bytecode(moduleName: "file.s");

Function main = new("main", "void", 8);

main.AddParameter("args", 8);

main.AddInstruction(OpCodes.Enter);
main.AddInstruction(OpCodes.LoadString, "Carpal");
main.AddInstruction(OpCodes.StoreGlobal, "name");
main.AddInstruction(OpCodes.LoadGlobal, "name");
main.AddInstruction(OpCodes.Call, "io::print(str)void");
main.AddInstruction(OpCodes.Ret);

bt.InstallFunction(main);
//bt.InstallGlobal(new("#name", 8, "asciz", "Carpal"));
bt.InstallGlobal(new("#name", 8, "asciz", "default"));
bt.InstallGlobal(new("name", 8, "quad", "\"#name\""));

bt.Save("C:/Users/Mondelli/Desktop");
//WriteLine(bt.CompileAOT());