namespace ERF.Private;

using System.Runtime.InteropServices;

internal class ScriptCallback : IDisposable
{
    internal ScriptCallback(IntPtr funcPtr)
    {
        this.Handle = funcPtr;
    }

    internal IntPtr Handle { get; }

    public static ScriptCallback Create(Delegate callback)
    {
        var funcPtr = NativeBindings.TL_Tool_Get_FuncPtr(Marshal.GetFunctionPointerForDelegate(callback));

        return new ScriptCallback(funcPtr);
    }

    public void Dispose()
    {
        NativeBindings.TL_Tool_Return_FuncPtr(this.Handle);

        GC.SuppressFinalize(this);
    }
}