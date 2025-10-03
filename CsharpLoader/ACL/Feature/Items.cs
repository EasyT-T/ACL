namespace ACL.Feature;

using ACL.Managed.ScriptObject;

public class Items
{
    internal Items(ManagedItems baseItems)
    {
        this.Base = baseItems;
    }

    public ManagedItems Base { get; }

    public bool Picked => this.Base.IsPicked();

    public Player Picker
    {
        get => Player.Get(this.Base.GetPicker());
        set => this.Base.SetPicker(value.Base);
    }

    public Entity Entity => new Entity(this.Base.GetEntity());

    public int Index => this.Base.GetIndex();

    public string Name => this.Base.GetName();

    public string TemplateName => this.Base.GetTemplateName();

    public int TemplateIndex => this.Base.GetTemplateIndex();

    public bool IsWeapon => this.Base.IsWeapon();

    public float State
    {
        get => this.Base.GetState();
        set => this.Base.SetState(value);
    }

    public float State2
    {
        get => this.Base.GetState2();
        set => this.Base.SetState2(value);
    }

    public float State3
    {
        get => this.Base.GetState3();
        set => this.Base.SetState3(value);
    }

    public int Slots => this.Base.GetSlots();

    public Items Parent => new Items(this.Base.GetParentItem());

    public Items Fine(int level)
    {
        return new Items(this.Base.Fine(level));
    }

    public Items GetSlot(int slot)
    {
        return new Items(this.Base.GetSlotItem(slot));
    }

    public bool PushItem(Items item)
    {
        return this.Base.PushItem(item.Base);
    }

    public bool RemoveSlot(int slot)
    {
        return this.Base.RemoveSlotItem(slot);
    }

    public void Remove()
    {
        this.Base.Remove();
    }
}