namespace ERF.Example;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ERF.Loader;
using ERF.Loader.Binding;
using ERF.Loader.Feature;
using ERF.Loader.Managed;

public class TestPlugin
{
    [UnmanagedCallersOnly(EntryPoint = nameof(EnablePlugin), CallConvs = [typeof(CallConvCdecl)])]
    public static void EnablePlugin()
    {
        var function = ScriptEngine.GetGlobalFunctionByDecl("void print(string &in message)");
        using var context = ScriptEngine.CreateContext();
        using var message = EString.Create("Hello from .NET C#! 你好，世界！");

        context.Prepare(function);
        context.SetArgument(0, message);
        context.Execute();
    }
}