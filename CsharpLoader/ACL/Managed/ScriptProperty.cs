namespace ACL.Managed;

using System.Runtime.InteropServices;

public class ScriptProperty
{
    internal ScriptProperty(string name, string nameSpace, int typeId, bool isConst, string configGroup, IntPtr pointer,
        int accessMask)
    {
        this.Name = name;
        this.Namespace = nameSpace;
        this.TypeId = typeId;
        this.IsConst = isConst;
        this.ConfigGroup = configGroup;
        this.AccessMask = accessMask;

        this.Handle = pointer;
    }

    public string Name { get; }

    public string Namespace { get; }

    public int TypeId { get; }

    public bool IsConst { get; }

    public string ConfigGroup { get; }

    public int AccessMask { get; }

    internal IntPtr Handle { get; }

    public T? Get<T>()
    {
        return Marshal.PtrToStructure<T>(this.Handle);
    }
}