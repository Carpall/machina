using Machina.Models.Function;
using Machina.Models.Module;
using Machina.TypeSystem;
using System.Collections.Generic;
using System;
using Machina.ValueSystem;
using Machina.CModels;

var structure = new MachinaStructure("MyStruct");
structure.AddField(new MachinaTypeInt32(), "field");
structure.AddField(new MachinaTypeInt32(), "field2");

var function = new MachinaFunction(
    new MachinaTypeInt32(),
    "add",
    new List<CVariableInfo>()
    {
        new CVariableInfo(new MachinaTypeInt32(), new CIdentifier("a")),
        new CVariableInfo(new MachinaTypeInt32(), new CIdentifier("b"))
    }
);

function.LoadConstInt32(1);
function.LoadConstInt32(2);
function.LoadFunction(function);
function.CallFunction();
function.PopTop();

function.LoadConstInt32(2);
function.Return();

var module = new MachinaModule("test");

module.AddFunction(function);

module.AddStructure(structure);

module.Generate();

Console.WriteLine(module.DumpModule());