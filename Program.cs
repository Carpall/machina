using Machina.Models.Function;
using Machina.Models.Module;
using Machina.TypeSystem;
using System.Collections.Generic;
using System;

var function = new MachinaFunction(new MachinaTypeInt32(), "main", new List<Machina.CModels.CVariableInfo>());

function.LoadConstInt32(1);
function.Return();

var module = new MachinaModule("test");

module.AddFunction(function);

Console.WriteLine(module.DumpModule());