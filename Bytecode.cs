using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Console;

namespace Machina
{
    public class Bytecode
    {
        String ModuleName;
        public readonly Dictionary<String, Object> GlobalMembers = new();
        public void InstallFunction(Function function)
        {
            GlobalMembers.Add(function.Name, function);
        }
        public void InstallStruct(StructType structure)
        {
            GlobalMembers.Add(structure.Name, structure);
        }
        public void InstallGlobal(Variable variable)
        {
            GlobalMembers.Add(variable.Name, variable);
        }
        public void InizializeBuiltIns()
        {
            const string printstrvoid = @"    push  rbp
   mov   rbp, rsp
   sub   rsp, 32
   mov   rdx, rax
   lea   rcx, [rip+""@fmt_s""]
   call  printf
   xor   eax, eax
   add   rsp, 32
   pop   rbp
   ret
";

            const string printlnstrvoid = @"   push  rbp
   mov   rbp, rsp
   sub   rsp, 32
   mov   rcx, rax
   call	 puts
   xor   eax, eax
   add   rsp, 32
   pop   rbp
   ret
";
            const string printi32void = @"   push  rbp
   mov   rbp, rsp
   sub   rsp, 32
   mov   edx, eax
   lea   rcx, [rip+""@fmt_d""]
   call  printf
   xor   eax, eax
   add   rsp, 32
   pop   rbp
   ret
";
            const string printchrvoid = @"   push  rbp
   mov   rbp, rsp
   sub   rsp, 32
   mov   ecx, eax
   call	 putchar
   xor   eax, eax
   add   rsp, 32
   pop   rbp
   ret
";
            const string shellstri32 = @"   push  rbp
   mov   rbp, rsp
   sub	 rsp, 48
   mov   rcx, rax
   call	 system
   add   rsp, 48
   pop   rbp
   ret
";
            const string readlnstr = @"   push  rbp
   mov   rbp, rsp
   sub   rsp, 48
   mov   ecx, 80
   call  malloc
   mov   [rbp-8], rax
   mov   rcx, rax
   call	 gets
   mov   rax, [rbp-8]
   add   rsp, 48
   pop   rbp
   ret
";
            const string readchr = @"   push  rbp
   mov   rbp, rsp
   sub   rsp, 32
   call  getchar
   add   rsp, 32
   pop   rbp
   ret
";
            const string getsizeunknowptru32 = @"   push  rbp
   mov   rbp, rsp
   sub   rsp, 32
   mov   rcx, rax
   call  _msize
   add   rsp, 32
   pop   rbp
   ret
";
            const string deallocunknowptrvoid = @"   push  rbp
   mov   rbp, rsp
   sub   rsp, 32
   mov   rcx, rax
   call  free
   add   rsp, 32
   pop   rbp
   ret
";
            const string allocu32unknowptr = @"   push  rbp
   mov   rbp, rsp
   mov   ecx, eax
   call  malloc
   pop   rbp
   ret
";
            var model = new Function("io::print(str)void", "void", 0);
            // io
            // void print(str)
            model.AddParameter("text", "str", 8, true);
            model.AddInstruction(OpCodes.UnsafeAsm, printstrvoid);
            model.AddInstruction(OpCodes.UnsafeEmitGlobal, new Tuple<string, string, string>("@fmt_s", "asciz","\"%s\""));
            GlobalMembers.Add("io::print(str)void", model);
            // void println(str)
            model = new Function("io::println(str)void", "void", 0);
            model.AddParameter("text", "str", 8, true);
            model.AddInstruction(OpCodes.UnsafeAsm, printlnstrvoid);
            GlobalMembers.Add("io::println(str)void", model);
            // void print(i32)
            model = new Function("io::print(i32)void", "void", 0);
            model.AddParameter("num", "i32", 8, true);
            model.AddInstruction(OpCodes.UnsafeAsm, printi32void);
            model.AddInstruction(OpCodes.UnsafeEmitGlobal, new Tuple<string, string, string>("@fmt_d", "asciz", "\"%d\""));
            GlobalMembers.Add("io::print(i32)void", model);
            // void print(chr)
            model = new Function("io::print(chr)void", "void", 0);
            model.AddParameter("ascii", "chr", 8, true);
            model.AddInstruction(OpCodes.UnsafeAsm, printchrvoid);
            GlobalMembers.Add("io::print(chr)void", model);
            // i32 shell(str)
            model = new Function("io::shell(str)i32", "i32", 0);
            model.AddParameter("prompt", "str", 8, true);
            model.AddInstruction(OpCodes.UnsafeAsm, shellstri32);
            GlobalMembers.Add("io::shell(str)i32", model);
            // str readln()
            model = new Function("io::readln()str", "str", 0);
            model.AddInstruction(OpCodes.UnsafeAsm, readlnstr);
            GlobalMembers.Add("io::readln()str", model);
            // chr read()
            model = new Function("io::read()chr", "chr", 0);
            model.AddInstruction(OpCodes.UnsafeAsm, readchr);
            GlobalMembers.Add("io::read()chr", model);
            // mem
            // u32 getsize(unknow*)
            model = new Function("mem::getsize(unknow*)u32", "u32", 0);
            model.AddParameter("block", "unknow", 8, true);
            model.AddInstruction(OpCodes.UnsafeAsm, getsizeunknowptru32);
            GlobalMembers.Add("mem::getsize(unknow*)u32", model);
            // void dealloc(unknow*)
            model = new Function("mem::dealloc(unknow*)void", "void", 0);
            model.AddParameter("block", "unknow", 8, true);
            model.AddInstruction(OpCodes.UnsafeAsm, deallocunknowptrvoid);
            GlobalMembers.Add("mem::dealloc(unknow*)void", model);
            // unknow* alloc(u32)
            model = new Function("mem::alloc(u32)unknow*", "unknow", 0, true);
            model.AddParameter("size", "u32", 4, false);
            model.AddInstruction(OpCodes.UnsafeAsm, allocu32unknowptr);
            GlobalMembers.Add("mem::alloc(u32)unknow*", model);
        }
        public Bytecode(string moduleName)
        {
            InizializeBuiltIns();
            ModuleName = moduleName;
        }
        public string CompileAOT()
        {
            return new Evaluator(ModuleName, this).ToString();
        }
        public void Save(string dir) {
            var temp = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Temp\b2c\" + ModuleName + ".s";
            var dest = dir + "/" + Path.ChangeExtension(ModuleName, "exe");
            if (File.Exists(dest))
                File.Delete(dest);
            if (File.Exists(temp))
                File.Delete(temp);
            WriteLine("[...] Generating");
            var code = CompileAOT();
            WriteLine("[ x ] Generated");
            WriteLine("[...] Writing");
            File.WriteAllText(temp, code);
            while (!File.Exists(temp)) ;
            WriteLine("[ x ] Temp: {0}", temp);
            WriteLine("[...] Compiling & Linking");
            Process.Start("clang", string.Format("-o {0} {1}", dest, temp));
            while (!File.Exists(dest)) ;
            WriteLine("[ x ] Compiled: {0}", Path.GetFullPath(dest));
            WriteLine("[...] Cleaning Temp");
            File.Delete(temp);
            while (File.Exists(temp)) ;
            WriteLine("[ x ] Done");
        }
    }
}