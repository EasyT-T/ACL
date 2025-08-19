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

    public static UserData Create(object data)
    {
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(data));
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

    public void Dispose()
    {
        Marshal.FreeHGlobal(this.Handle);

        GC.SuppressFinalize(this);
    }
}