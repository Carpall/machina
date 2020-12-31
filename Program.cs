using Machina;
using System;
using System.IO;
using static System.Console;

// bt -> BytecodeImageBuilder Instance
var bt = new Bytecode(moduleName: "file.s");

// name -> FunctionID
// retType -> Function Return Type
// allocSize -> Total sum of all local variables
// main -> FunctionBuilder Instance
Function main = new(name: "main", retType: "void", allocSize: 8);

// name -> 
// Adding Function Parameter
main.AddParameter(name: "args", size: 8);

// function body
main.AddInstruction(OpCodes.Enter);
main.AddInstruction(OpCodes.MemDeclare, "person", 8);
main.AddInstruction(OpCodes.LoadInstance, "Person");
main.AddInstruction(OpCodes.StoreMem, "person");
main.AddInstruction(OpCodes.LoadMem, "person");
main.AddInstruction(OpCodes.LoadMem, "args");
main.AddInstruction(OpCodes.LoadInt, 1);
main.AddInstruction(OpCodes.LoadArrayElem, 8);
main.AddInstruction(OpCodes.StoreField, "Person", "name");
main.AddInstruction(OpCodes.LoadMem, "person");
main.AddInstruction(OpCodes.LoadField, "Person", "name");
main.AddInstruction(OpCodes.Call, "io::println(str)void");
main.AddInstruction(OpCodes.Ret);

// struct Person {
//   name: str,
//   age: u32
// }
// void main(args: [str]) {
//   var person: Person = new Person;
//   person.name = args[1];
//   io::println(person.name);
// }

// struct body
StructType person = new("Person");
person.InstallField(new("name", 8));
person.InstallField(new("age", 4));

bt.InstallFunction(main);
bt.InstallStruct(person);

bt.Save($"C:/Users/{Environment.UserName}/Desktop");
//WriteLine(bt.CompileAOT());