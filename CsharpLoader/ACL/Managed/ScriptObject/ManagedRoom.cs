namespace ACL.Managed.ScriptObject;

using ACL.Private;
using ACL.SourceGenerators;

[ScriptClass("Room")]
public partial class ManagedRoom : ScriptObjectBase
{
    internal unsafe ManagedRoom(AngelObject* handle) : base((IntPtr)handle)
    {
    }

    #nullable disable

    [ScriptFunction("string& GetName()")]
    public partial string GetName();

    [ScriptFunction("int GetIndex()")]
    public partial int GetIndex();

    [ScriptFunction("int GetIdentifier()")]
    public partial int GetIdentifier();

    [ScriptFunction("Entity GetEntity()")]
    public partial ManagedEntity GetEntity();

    [ScriptFunction("bool IsAdjacent(Room)")]
    public partial bool IsAdjacent(ManagedRoom room);

    [ScriptFunction("Room GetAdjacentRoom(int)")]
    public partial ManagedRoom GetAdjacentRoom(int index);

    [ScriptFunction("Door GetAdjacentDoor(int)")]
    public partial ManagedDoor GetAdjacentDoor(int index);

    [ScriptFunction("Door GetDoor(int)")]
    public partial ManagedDoor GetDoor(int index);

    [ScriptFunction("bool IsInside(Entity)")]
    public partial bool IsInside(ManagedEntity entity);
}