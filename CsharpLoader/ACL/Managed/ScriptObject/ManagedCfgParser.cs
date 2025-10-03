namespace ACL.Managed.ScriptObject;

using ACL.Private;

public class ManagedCfgParser : ScriptObjectBase
{
    internal unsafe ManagedCfgParser(AngelObject* handle) : base((IntPtr)handle)
    {
        this.unmanaged = (UnmanagedCfgParser*)handle->FieldsPointer;
    }

    public unsafe string Value
    {
        get => new ManagedString(this.unmanaged->Value);
        set
        {
            new ManagedString(this.unmanaged->Value, true).Dispose();
            this.unmanaged->Value = ManagedString.Create(value).Handle;
        }
    }

    private readonly unsafe UnmanagedCfgParser* unmanaged;
}