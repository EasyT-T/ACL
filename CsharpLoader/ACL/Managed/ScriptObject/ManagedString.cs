namespace ACL.Managed.ScriptObject;

using ACL.Managed;
using NativeBindings = ACL.Private.NativeBindings;

public class ManagedString : ScriptObjectBase, IDisposable
{
    public bool IsManagedCreated { get; }

    internal ManagedString(IntPtr handle, bool managedCreated = false)  : base(handle)
    {
        this.IsManagedCreated = managedCreated;
    }

    public static ManagedString Create(string str)
    {
        return new ManagedString(NativeBindings.TL_Tool_Get_String(str), true);
    }

    public override string ToString()
    {
        return NativeBindings.TL_Tool_String_Content(this.Handle);
    }

    public void Dispose()
    {
        if (this.IsManagedCreated)
        {
            NativeBindings.TL_Tool_Return_String(this.Handle);
        }

        GC.SuppressFinalize(this);
    }

    public static implicit operator string(ManagedString managedString)
    {
        return managedString.ToString();
    }
}