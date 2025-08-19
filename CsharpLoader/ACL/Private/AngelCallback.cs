namespace ACL.Private;

using System.Runtime.InteropServices;

internal class AngelCallback : IDisposable
{
    internal AngelCallback(IntPtr funcPtr)
    {
        this.Handle = funcPtr;
    }

    internal IntPtr Handle { get; }

    public static AngelCallback Create(Delegate callback)
    {
        var funcPtr = NativeBindings.TL_Tool_Get_FuncPtr(Marshal.GetFunctionPointerForDelegate(callback));

        return new AngelCallback(funcPtr);
    }

    public void Dispose()
    {
        NativeBindings.TL_Tool_Return_FuncPtr(this.Handle);

        GC.SuppressFinalize(this);
    }
}