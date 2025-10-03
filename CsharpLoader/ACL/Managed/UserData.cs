namespace ACL.Managed;

using System.Runtime.InteropServices;

public class UserData : IDisposable, IEquatable<UserData>
{
    internal IntPtr Handle { get; }

    internal UserData(IntPtr handle)
    {
        this.Handle = handle;
    }

    public T? Get<T>()
    {
        return Marshal.PtrToStructure<T>(this.Handle);
    }

    public static unsafe UserData Create(object data)
    {
        var ptr = (IntPtr)NativeMemory.Alloc((UIntPtr)Marshal.SizeOf(data));
        Marshal.StructureToPtr(data, ptr, false);

        return new UserData(ptr);
    }

    public override bool Equals(object? obj)
    {
        return obj is UserData data && this.Handle == data.Handle;
    }

    public bool Equals(UserData? other)
    {
        return this.Handle == other?.Handle;
    }

    public override int GetHashCode()
    {
        return this.Handle.GetHashCode();
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public unsafe void Dispose()
    {
        this.Dispose(true);

        NativeMemory.Free((void*)this.Handle);

        GC.SuppressFinalize(this);
    }
}