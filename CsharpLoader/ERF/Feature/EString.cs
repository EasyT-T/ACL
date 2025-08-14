namespace ERF.Loader.Feature;

using System.Runtime.InteropServices;
using ERF.Loader.Binding;
using ERF.Loader.Managed;

public class EString : IObjectClass, IDisposable
{
    private readonly IntPtr handle;

    internal EString(IntPtr handle)
    {
        this.handle = handle;
    }

    IntPtr IObjectClass.Handle => this.handle;

    public static EString Create(string str)
    {
        return new EString(NativeBindings.TL_Tool_Get_String(str));
    }

    public void Dispose()
    {
        NativeBindings.TL_Tool_Return_String(this.handle);

        GC.SuppressFinalize(this);
    }
}