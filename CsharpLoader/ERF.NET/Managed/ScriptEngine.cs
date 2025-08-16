namespace ERF.Managed;

using NativeBindings = ERF.Private.NativeBindings;

public static class ScriptEngine
{
    internal static IntPtr Handle { get; } = NativeBindings.TL_Engine_GetEngine();

    public static ScriptContext CreateContext()
    {
        return new ScriptContext(NativeBindings.TL_Engine_CreateContext());
    }

    public static uint GetGlobalFunctionCount()
    {
        return NativeBindings.TL_Engine_GetGlobalFunctionCount();
    }

    public static ScriptFunction GetGlobalFunction(uint index)
    {
        return new ScriptFunction(NativeBindings.TL_Engine_GetGlobalFunctionByIndex(index));
    }

    public static ScriptFunction GetGlobalFunction(string declaration)
    {
        return new ScriptFunction(NativeBindings.TL_Engine_GetGlobalFunctionByDecl(declaration));
    }
}