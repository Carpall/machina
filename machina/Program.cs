using System;
using Machina.AssemblyBuilder;
using Machina.Emitter;

#if DEBUG
try
{
#endif
    var builder = new AssemblyImage("test");

    var main = new FunctionBuilder64("main", Array.Empty<MachinaType>(), MachinaType.Int);
    
    main.LoadInt(10); // stack: [10]
    main.LoadInt(1); // [10, 1]
    main.AddInt();
    main.Ret();

    builder.DefineFunction(main); 

    Console.WriteLine(builder.DumpAssembly());
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