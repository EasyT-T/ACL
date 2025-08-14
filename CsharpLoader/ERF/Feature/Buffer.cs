namespace ERF.Loader.Feature;

using ERF.Loader.Managed;

public class Buffer : IDisposable
{
    internal IntPtr Handle { get; }

    internal Buffer(IntPtr handle)
    {
        this.Handle = handle;
    }

    public static Buffer Create(int length)
    {
        using var context = ScriptEngine.CreateContext();
        context.Prepare(GlobalFunctions.CreateBank);
        context.SetArgument(0, length);

        var result = context.Execute();

        if (result != ScriptErrorType.AsSuccess)
        {
            throw new Exception("Unable to call 'CreateBank' function: " + result);
        }

        var ptr = context.GetReturnPointer();

        return new Buffer(ptr);
    }

    public static bool TryCreate(int length, out Buffer? buffer)
    {
        var context = ScriptEngine.CreateContext();
        context.Prepare(GlobalFunctions.CreateBank);
        context.SetArgument(0, length);

        if (context.Execute() != ScriptErrorType.AsSuccess)
        {
            buffer = null;
            return false;
        }

        var ptr = context.GetReturnPointer();

        buffer = new Buffer(ptr);
        return true;
    }

    public int GetLength()
    {
        using var context = ScriptEngine.CreateContext();
        context.Prepare(GlobalFunctions.BankSize);
        context.SetArgument(0, this.Handle);

        var result = context.Execute();

        if (result != ScriptErrorType.AsSuccess)
        {
            throw new Exception("Unable to call 'GetLength' function: " + result);
        }

        return (int)context.GetReturnUInt32();
    }

    public void Dispose()
    {
        using var context = ScriptEngine.CreateContext();
        context.Prepare(GlobalFunctions.FreeBank);
        context.SetArgument(0, this.Handle);
        context.Execute();

        GC.SuppressFinalize(this);
    }
}