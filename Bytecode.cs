﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Console;

namespace Machina
{
    public class Bytecode
    {
        string ModuleName;
        public List<Method> ModuleFunctions;
        public List<Data> ModuleGlobals;
        public List<CustomType> ModuleTypes;

        public void InstallMethod(Method method)
        {
            ModuleFunctions.Add(method);
        }
        public void InstallType(CustomType type)
        {
            ModuleTypes.Add(type);
        }
        public void InstallGlobal(Data global)
        {
            ModuleGlobals.Add(global);
        }
        public Bytecode(string moduleName)
        {
            ModuleName = moduleName;
        }
        public string CompileAOT()
        {
            return new Evaluator(this).ToString();
        }
        public void Save(string path) {
            var temp = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Temp\b2c\" + ModuleName + ".c";
            var dest = path + "/" + ModuleName + ".exe";
            if (File.Exists(dest))
                File.Delete(dest);
            if (File.Exists(temp))
                File.Delete(temp);
            WriteLine("[...] Generating");
            File.WriteAllText(temp, CompileAOT());
            while (!File.Exists(temp)) ;
            WriteLine("[ x ] Generated");
            WriteLine("[...] Writing");
            WriteLine("[ x ] Temp: {0}", temp);
            WriteLine("[...] Compiling & Linking");
            Process.Start("gcc", temp + " -o " + dest);
            while (!File.Exists(dest)) ;
            WriteLine("[ x ] Compiled: {0}", Path.GetFullPath(dest));
            File.Delete(temp);
        }
    }
}