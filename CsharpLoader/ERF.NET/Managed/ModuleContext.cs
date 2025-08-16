namespace ERF.Managed;

using ERF.Private;

public class ModuleContext : UserData
{
    internal ModuleContext(IntPtr handle, ScriptModule module, ScriptContext context) : base(handle)
    {
        this.Module = module;
        this.Context = context;
    }

    public static ModuleContext Create(ScriptModule module, ScriptContext context)
    {
        return new ModuleContext(NativeBindings.TL_Tool_Get_ModuleContext(module.Handle, context.Handle), module, context);
    }

    public ScriptModule Module { get; }
    public ScriptContext Context { get; }
}