namespace ERF.Feature;

using ERF.Managed;

public static class GlobalFunctions
{
    public static ScriptFunction PrintFunction { get; }

    public static ScriptFunction CreateBankFunction { get; }

    public static ScriptFunction FreeBankFunction { get; }

    public static ScriptFunction BankSizeFunction { get; }

    static GlobalFunctions()
    {
        PrintFunction = ScriptEngine.GetGlobalFunctionByDecl("void print(string &in message)");
        CreateBankFunction = ScriptEngine.GetGlobalFunctionByDecl("int CreateBank(int size)");
        FreeBankFunction = ScriptEngine.GetGlobalFunctionByDecl("void FreeBank(int bank)");
        BankSizeFunction = ScriptEngine.GetGlobalFunctionByDecl("int BankSize(int bank)");
    }

    public static ScriptErrorType Print(string message)
    {
        using var context = ScriptEngine.CreateContext();
        using var str = EString.Create(message);

        context.Prepare(PrintFunction);
        context.SetArgument(0, str);

        return context.Execute();
    }

    public static (ScriptErrorType, IntPtr) CreateBank(int size)
    {
        using var context = ScriptEngine.CreateContext();

        context.Prepare(CreateBankFunction);
        context.SetArgument(0, size);

        var result = context.Execute();

        return result != ScriptErrorType.AsSuccess ? (result, IntPtr.Zero) : (result, context.GetReturnPointer());
    }

    public static ScriptErrorType FreeBank(IntPtr bank)
    {
        using var context = ScriptEngine.CreateContext();

        context.Prepare(FreeBankFunction);
        context.SetArgument(0, bank);

        return context.Execute();
    }

    public static (ScriptErrorType, uint) BankSize(IntPtr bank)
    {
        using var context = ScriptEngine.CreateContext();

        context.Prepare(BankSizeFunction);
        context.SetArgument(0, bank);

        var result = context.Execute();

        return result != ScriptErrorType.AsSuccess ? (result, 0) : (result, context.GetReturnUInt32());
    }
}