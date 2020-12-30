using Machina;
using System;
using System.IO;
using static System.Console;

var bt = new Bytecode("file.s");

Function main = new("main", "void", 16);
main.AddParameter("args", 8);

// function body
main.AddInstruction(OpCodes.Enter);
main.AddInstruction(OpCodes.LoadMem, "args");
main.AddInstruction(OpCodes.LoadArrayElem, 8, 1);
main.AddInstruction(OpCodes.StoreMem, "arg", 8);
main.AddInstruction(OpCodes.LoadMem, "arg");
main.AddInstruction(OpCodes.Call, "io::print(str)void");
main.AddInstruction(OpCodes.Ret);

// func main(args: [str]): ? {
//   var arg: str = args[1];
//   io::print(arg);
// }

bt.InstallFunction(main);

bt.Save($"C:/Users/{Environment.UserName}/Desktop");
//WriteLine(bt.CompileAOT());