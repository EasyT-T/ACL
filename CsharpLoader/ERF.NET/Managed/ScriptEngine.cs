namespace ERF.Managed;

using ERF.Binding;

public class ScriptEngine
{
    public static ScriptContext CreateContext()
    {
        return new ScriptContext(NativeBindings.TL_Engine_CreateContext());
    }

    public static ScriptFunction GetGlobalFunctionByDecl(string declaration)
    {
        return new ScriptFunction(NativeBindings.TL_Engine_GetGlobalFunctionByDecl(declaration));
    }
}