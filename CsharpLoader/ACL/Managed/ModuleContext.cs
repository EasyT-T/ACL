namespace ACL.Managed;

using ACL.Private;

public class ModuleContext : UserData
{
    internal ModuleContext(IntPtr handle, ScriptModule module, ScriptContext context) : base(handle)
    {
        this.Module = module;
        this.Context = context;
    }

    public ScriptModule Module { get; }
    public ScriptContext Context { get; }

    public new static UserData Create(object _)
    {
        throw new PlatformNotSupportedException("Use ModuleContext::Create(ScriptModule, ScriptContext) instead");
    }

    public static ModuleContext Create(ScriptModule module, ScriptContext context)
    {
        return new ModuleContext(NativeBindings.TL_Tool_Get_ModuleContext(module.Handle, context.Handle), module, context);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            NativeBindings.TL_Tool_Return_ModuleContext(this.Handle);
        }
    }
}