using Machina;
using System;
using System.IO;
using static System.Console;


// High Level Rappresentation {
//   struct Person {
//     name: str,
//     age: u32
//   }
//   
//   void main(args: [str]) {
//     var person: Person = new Person;
//     person.name = args[1];
//     io::println(person.name);
//   }
// } End High Level Rappresentation


// bt -> BytecodeImageBuilder Instance
var bt = new Bytecode(moduleName: "file.s");

// name -> FunctionID
// retType -> Function Return Type
// allocSize -> Total sum of all local variables
// main -> FunctionBuilder Instance
Function main = new(name: "main", retType: "void", allocSize: 8);

// name -> ParameterAlias
// size -> Parameter Type Size
// Adding Function Parameter
main.AddParameter(name: "args", size: 8);

main.AddInstruction(OpCodes.Enter);
main.AddInstruction(OpCodes.MemDeclare, "arrInt", 8);
main.AddInstruction(OpCodes.LoadArray, 4, 4);
main.AddInstruction(OpCodes.StoreMem, "arrInt");
main.AddInstruction(OpCodes.Ret);

StructType person = new("Person");
person.InstallField(new("name", 8));
person.InstallField(new("age", 4));

bt.InstallFunction(main);
bt.InstallStruct(person);
//bt.Save("C:/Users/Mondelli/Desktop");
WriteLine(bt.CompileAOT());