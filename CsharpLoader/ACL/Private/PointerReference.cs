namespace ACL.Private;

using System.Runtime.InteropServices;

public class PointerReference : IDisposable
{
    private GCHandle handle;

    public IntPtr Handle => this.handle.AddrOfPinnedObject();

    internal PointerReference(object obj)
    {
        this.handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
    }

    public void Dispose()
    {
        this.handle.Free();

        GC.SuppressFinalize(this);
    }
}