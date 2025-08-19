namespace ACL.Managed;

public abstract class ScriptObjectBase(IntPtr handle)
{
    protected internal IntPtr Handle { get; } = handle;
}