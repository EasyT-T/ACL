namespace ACL.Feature;

using ACL.Managed.ScriptObject;

public class Entity
{
    internal Entity(ManagedEntity baseEntity)
    {
        this.Base = baseEntity;
    }

    public ManagedEntity Base { get; }
}