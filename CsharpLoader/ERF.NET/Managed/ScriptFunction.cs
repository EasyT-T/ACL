namespace ERF.Managed;

public class ScriptFunction
{
    internal ScriptFunction(IntPtr handle)
    {
        this.Handle = handle;
    }

    internal IntPtr Handle { get; }
}