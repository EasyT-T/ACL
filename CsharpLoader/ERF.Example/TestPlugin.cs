namespace ERF.Example;

using System.Diagnostics;
using System.Runtime.InteropServices;
using ERF.Feature;
using ERF.Loader;
using ERF.Managed;
using ERF.Private;

public class TestPlugin : Plugin
{
    public override string Name => "TestPlugin";

    private long tickCount;

    public override void EnablePlugin()
    {
        if (File.Exists("debug"))
        {
            while (!Debugger.IsAttached)
            {
                Thread.Sleep(100);
            }

            Debugger.Break();
        }

        GlobalFunctions.Print("Hello World!");

        this.EventManager.ServerUpdate += this.OnServerUpdate;

        /*var functionCount = ScriptEngine.GetGlobalFunctionCount();

        for (uint i = 0; i < functionCount; i++)
        {
            var function = ScriptEngine.GetGlobalFunction(i);

            GlobalFunctions.Print($"Function #{i}: {function.GetDeclaration(true, true, true)}");
        }

        var funcdefCount = NativeBindings.TL_Engine_GetFuncdefCount();

        for (uint i = 0; i < funcdefCount; i++)
        {
            var funcdef = NativeBindings.TL_Engine_GetFuncdefByIndex(i);

            var typeId = NativeBindings.TL_TypeInfo_GetTypeId(funcdef);
            var declaration = NativeBindings.TL_Engine_GetTypeDeclaration(typeId, true);
            var declarationStr = Marshal.PtrToStringUTF8(declaration);

            GlobalFunctions.Print($"Funcdef #{i}: {declarationStr}({typeId})");
        }*/
    }

    public void OnServerUpdate()
    {
        this.tickCount++;
    }
}