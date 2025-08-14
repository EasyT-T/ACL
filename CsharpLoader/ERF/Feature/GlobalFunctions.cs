namespace ERF.Loader.Feature;

using ERF.Loader.Managed;

public static class GlobalFunctions
{
    public static ScriptFunction CreateBank { get; }

    public static ScriptFunction FreeBank { get; }

    public static ScriptFunction BankSize { get; }

    static GlobalFunctions()
    {
        CreateBank = ScriptEngine.GetGlobalFunctionByDecl("int CreateBank(int size)");
        FreeBank = ScriptEngine.GetGlobalFunctionByDecl("void FreeBank(int bank)");
        BankSize = ScriptEngine.GetGlobalFunctionByDecl("int BankSize(int bank)");
    }
}