namespace ACL.Managed;

using ACL.Private;

public class ScriptModule : IDisposable
{
    internal IntPtr Handle { get; }

    internal ScriptModule(IntPtr handle)
    {
        this.Handle = handle;
    }

    public UserData SetUserData(UserData data, uint type)
    {
        return new UserData(NativeBindings.TL_Module_SetUserData(this.Handle, data.Handle, type));
    }

    public UserData GetUserData(uint type)
    {
        return new UserData(NativeBindings.TL_Module_GetUserData(this.Handle, type));
    }

    public static ScriptModule Create(string moduleName)
    {
        var handle = NativeBindings.TL_Engine_GetModule(moduleName, (int)GetModuleFlags.AsGmAlwaysCreate);

        return new ScriptModule(handle);
    }

    public void Dispose()
    {
        NativeBindings.TL_Module_Discard(this.Handle);

        GC.SuppressFinalize(this);
    }
}