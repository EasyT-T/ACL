namespace ACL.Managed.ScriptObject;

using ACL.Managed;
using ACL.Private;

public class ManagedEntity :ScriptObjectBase
{
    internal unsafe ManagedEntity(AngelObject* handle) : base((IntPtr)handle)
    {
    }
}