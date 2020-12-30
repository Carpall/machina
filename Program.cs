using Machina;
using System;
using static System.Console;

var bt = new Bytecode("file.s");                              // instance of a bytecode builder <moduleName>

Function main = new("main", "void", 4);                       // instance of a function builder <name> <type> <allocationSize1>
main.AddParameter("args", "[str]", 8, true);                  // add a parameter declaration <name> <type> <size> <isPointer>

// function body
main.AddInstruction(OpCodes.Enter);                           // init the function in a safe way
main.AddInstruction(OpCodes.LoadString, "Hello World");       // load a string
main.AddInstruction(OpCodes.Call, "io::println(str)void");    // call std method (hand written)
main.AddInstruction(OpCodes.Ret);                             // break the function executing and restore the stack pointer

bt.InstallFunction(main);                                     // install the function model to the bytecode image

bt.Save($"C:/Users/{Environment.UserName}/Desktop");          // saving the file
//WriteLine(bt.CompileAOT());