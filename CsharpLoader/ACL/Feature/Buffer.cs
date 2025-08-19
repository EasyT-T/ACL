namespace ACL.Feature;

using ACL.Managed;

public class Buffer : IDisposable
{
    internal IntPtr Handle { get; }

    internal Buffer(IntPtr handle)
    {
        this.Handle = handle;
    }

    public static Buffer Create(int length)
    {
        var result = GlobalFunctions.CreateBank(length);

        if (result.Item1 != ScriptErrorType.AsSuccess || result.Item2 == IntPtr.Zero)
        {
            throw new Exception("Unable to call 'CreateBank' function: " + result.Item1);
        }

        return new Buffer(result.Item2);
    }

    public static bool TryCreate(int length, out Buffer? buffer)
    {
        var result = GlobalFunctions.CreateBank(length);

        if (result.Item1 != ScriptErrorType.AsSuccess || result.Item2 == IntPtr.Zero)
        {
            buffer = null;
            return false;
        }

        buffer = new Buffer(result.Item2);
        return true;
    }

    public int GetLength()
    {
        var result = GlobalFunctions.BankSize(this.Handle);

        if (result.Item1 != ScriptErrorType.AsSuccess)
        {
            throw new Exception("Unable to call 'GetLength' function: " + result.Item1);
        }

        return (int)result.Item2;
    }

    public void Dispose()
    {
        GlobalFunctions.FreeBank(this.Handle);

        GC.SuppressFinalize(this);
    }
}