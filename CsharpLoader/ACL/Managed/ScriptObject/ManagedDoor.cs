namespace ACL.Managed.ScriptObject;

using ACL.Private;
using ACL.SourceGenerators;

[ScriptClass("Door")]
public partial class ManagedDoor : ScriptObjectBase
{
    internal unsafe ManagedDoor(AngelObject* handle) : base((IntPtr)handle)
    {
    }

    #nullable disable

    [ScriptFunction("void Use()")]
    public partial void Use();

    [ScriptFunction("void SetOpen(bool)")]
    public partial void SetOpen(bool open);

    [ScriptFunction("bool IsOpened()")]
    public partial bool IsOpened();

    [ScriptFunction("void SetLockState(int)")]
    public partial void SetLockState(int state);

    [ScriptFunction("int GetLockState()")]
    public partial int GetLockState();

    [ScriptFunction("float GetOpenState()")]
    public partial float GetOpenState();

    [ScriptFunction("void Decompose()")]
    public partial void Decompose();

    [ScriptFunction("int GetDoorAccess()")]
    public partial int GetDoorAccess();

    [ScriptFunction("int GetDoorType()")]
    public partial int GetDoorType();

    [ScriptFunction("void SetKeycard(int)")]
    public partial void SetKeycard(int keycardType);

    [ScriptFunction("int GetKeycard()")]
    public partial int GetKeycard();

    [ScriptFunction("Entity GetEntity()")]
    public partial ManagedEntity GetEntity();
}