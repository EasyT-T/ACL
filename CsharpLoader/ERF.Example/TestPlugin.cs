namespace ERF.Example;

using ERF.Feature;
using ERF.Loader;
using ERF.Managed;

public class TestPlugin : IPlugin
{
    public void EnablePlugin()
    {
        var function = ScriptEngine.GetGlobalFunctionByDecl("void print(string &in message)");
        using var context = ScriptEngine.CreateContext();
        using var message = EString.Create("Hello from .NET C#! 你好，世界！");

        context.Prepare(function);
        context.SetArgument(0, message);
        context.Execute();
    }
}