namespace ACL.Feature;

using ACL.Managed;
using ACL.Managed.ScriptObject;
using ManagedPlayer = ACL.Managed.ScriptObject.ManagedPlayer;

public static class GlobalFunctions
{
    public static ScriptFunction GetPlayerFunction { get; } =
        ScriptEngine.GetGlobalFunction("Player GetPlayer(int index)");

    public static ScriptFunction PrintFunction { get; } =
        ScriptEngine.GetGlobalFunction("void print(string &in message)");

    public static ScriptFunction CreateBankFunction { get; } =
        ScriptEngine.GetGlobalFunction("int CreateBank(int size)");

    public static ScriptFunction FreeBankFunction { get; } = ScriptEngine.GetGlobalFunction("void FreeBank(int bank)");

    public static ScriptFunction BankSizeFunction { get; } = ScriptEngine.GetGlobalFunction("int BankSize(int bank)");

    public static (ScriptErrorType, ManagedPlayer?) GetPlayer(int index)
    {
        using var context = ScriptEngine.CreateContext();

        context.Prepare(GetPlayerFunction);
        context.SetArgument(0, (uint)index);

        var result = context.Execute();

        return result == ScriptErrorType.AsSuccess
            ? (result, new ManagedPlayer(context.GetReturnPointer()))
            : (result, null);
    }

    public static ScriptErrorType Print(string message)
    {
        using var context = ScriptEngine.CreateContext();
        using var str = ManagedString.Create(message);

        context.Prepare(PrintFunction);
        context.SetArgument(0, str);

        return context.Execute();
    }

    public static ScriptErrorType Print(ManagedString message)
    {
        using var context = ScriptEngine.CreateContext();

        context.Prepare(PrintFunction);
        context.SetArgument(0, message);
        return context.Execute();
    }

    public static (ScriptErrorType, IntPtr) CreateBank(int size)
    {
        using var context = ScriptEngine.CreateContext();

        context.Prepare(CreateBankFunction);
        context.SetArgument(0, (uint)size);

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