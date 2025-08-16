namespace ERF.Feature;

using System.Runtime.InteropServices;
using ERF.Loader;
using ERF.Managed;
using ERF.Private;

public class EventManager(Plugin plugin)
{
    private static int globalFuncIndex;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OnServerUpdate();

    public event OnServerUpdate ServerUpdate
    {
        add => this.RegisterEvent(value, ServerUpdateCallbackDeclaration, ServerUpdateRegisterCallbackDeclaration);
        remove => throw new NotSupportedException("Remove event is not supported");
    }

    private static string ServerUpdateCallbackDeclaration => $"void ERFServerUpdate{globalFuncIndex}()";

    private static string ServerUpdateRegisterCallbackDeclaration => "bool RegisterCallback(int, _EVENT_CALLBACK1@)";

    private void RegisterEvent(Delegate callback, string callbackDeclaration, string registerCallbackDeclaration)
    {
        var scriptCallback = ScriptCallback.Create(callback);

        var moduleContext = plugin.ModuleContext;
        var context = moduleContext.Context;
        var module = moduleContext.Module;

        var callbackFunctionHandle = NativeBindings.TL_Tool_RegisterFunctionToModule(module.Handle,
            callbackDeclaration,
            scriptCallback.Handle,
            (int)CallConv.AsCallStdcall, IntPtr.Zero);

        var registerCallback = ScriptEngine.GetGlobalFunction(registerCallbackDeclaration);

        context.Prepare(registerCallback);
        context.SetArgument(0, (uint)EventType.ServerUpdateC);
        context.SetArgument(1, callbackFunctionHandle);
        context.Execute();

        globalFuncIndex++;
    }
}