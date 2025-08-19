namespace ACL.Managed.ScriptObject;

using ACL.SourceGenerators;

[ScriptClass("Items")]
public partial class ManagedItems : ScriptObjectBase
{
    internal ManagedItems(IntPtr handle) : base(handle)
    {
    }

    #nullable disable

    [ScriptFunction("bool IsPicked()")]
    public partial bool IsPicked();

    [ScriptFunction("Player GetPicker()")]
    public partial ManagedPlayer GetPicker();

    [ScriptFunction("bool SetPicker(Player)")]
    public partial bool SetPicker(ManagedPlayer player);

    [ScriptFunction("Entity GetEntity()")]
    public partial ManagedEntity GetEntity();

    [ScriptFunction("int GetIndex()")]
    public partial int GetIndex();

    [ScriptFunction("string& GetName()")]
    public partial string GetName();

    [ScriptFunction("string& GetTemplateName()")]
    public partial string GetTemplateName();

    [ScriptFunction("int GetTemplateIndex()")]
    public partial int GetTemplateIndex();

    [ScriptFunction("bool IsWeapon()")]
    public partial bool IsWeapon();

    [ScriptFunction("void SetState(float)")]
    public partial void SetState(float state);

    [ScriptFunction("void SetState2(float)")]
    public partial void SetState2(float state);

    [ScriptFunction("void SetState3(float)")]
    public partial void SetState3(float state);

    [ScriptFunction("float GetState()")]
    public partial float GetState();

    [ScriptFunction("float GetState2()")]
    public partial float GetState2();

    [ScriptFunction("float GetState3()")]
    public partial float GetState3();

    [ScriptFunction("Items Fine(int)")]
    public partial ManagedItems Fine(int level);

    [ScriptFunction("int GetSlots()")]
    public partial int GetSlots();

    [ScriptFunction("Items GetParentItem()")]
    public partial ManagedItems GetParentItem();

    [ScriptFunction("Items GetSlotItem(int)")]
    public partial ManagedItems GetSlotItem(int index);

    [ScriptFunction("bool PushItem(Items)")]
    public partial bool PushItem(ManagedItems item);

    [ScriptFunction("bool RemoveSlotItem(int)")]
    public partial bool RemoveSlotItem(int index);

    [ScriptFunction("void Remove()")]
    public partial void Remove();
}