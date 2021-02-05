using System;
using Machina.Emitter;

#if DEBUG
try
{
#endif
    var emitter = new Machina.Emitter.Emitter("test");

    emitter.EmitFunctionLabel32("main", 0);

    // emitter.SavePreviousFrame64();
    // emitter.DeclareStackAllocation64(0);
    
    emitter.Load(10);
    emitter.Load(1);
    emitter.EmitAddInt32();
    emitter.Duplicate32();
    emitter.EmitCompareJumpNEQ("exit");

    emitter.Load(0);
    
    emitter.EmitRetInt32(false);
    
    emitter.EmitLabel("exit");
    
    emitter.Load(1);
    
    emitter.EmitRetInt32(false);

    Console.WriteLine(emitter.DumpAssembly());
#if DEBUG
}
catch (Exception e)
{
    Console.WriteLine(e);
}
finally
{
    Console.ReadKey();
}
#endif