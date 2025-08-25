namespace ACL.Managed;

using System.Runtime.InteropServices;
using NativeBindings = ACL.Private.NativeBindings;

public class ScriptContext : IDisposable
{
    internal ScriptContext(IntPtr handle)
    {
        this.Handle = handle;
    }

    internal IntPtr Handle { get; }

    public ScriptErrorType Prepare(ScriptFunction function)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_Prepare(this.Handle, function.Handle);
    }

    public ScriptErrorType Unprepare()
    {
        return (ScriptErrorType)NativeBindings.TL_Context_Unprepare(this.Handle);
    }

    public ScriptErrorType Execute()
    {
        return (ScriptErrorType)NativeBindings.TL_Context_Execute(this.Handle);
    }

    public ScriptErrorType SetObject(ScriptObjectBase scriptObject)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetObject(this.Handle, scriptObject.Handle);
    }

    public ScriptErrorType SetArgument(uint index, byte value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgByte(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, out byte value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, out value);
    }

    public ScriptErrorType SetArgument(uint index, ushort value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgWord(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, out ushort value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, out value);
    }

    public ScriptErrorType SetArgument(uint index, uint value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgDWord(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, out uint value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, out value);
    }

    public ScriptErrorType SetArgument(uint index, ulong value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgQWord(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, out ulong value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, out value);
    }

    public ScriptErrorType SetArgument(uint index, float value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgFloat(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, out float value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, out value);
    }

    public ScriptErrorType SetArgument(uint index, double value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgDouble(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, out double value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, out value);
    }

    public ScriptErrorType SetArgument(uint index, ScriptObjectBase value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgObject(this.Handle, index, value.Handle);
    }

    public ScriptErrorType SetArgument(uint index, IntPtr address)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, address);
    }

    public ScriptErrorType SetArgument(uint index, Delegate @delegate)
    {
        GCHandle.Alloc(@delegate);
        var ptr = Marshal.GetFunctionPointerForDelegate(@delegate);
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, ptr);
    }

    public ScriptErrorType SetArgument<T>(uint index, Func<IntPtr, T> constructDelegate, out T? value)
        where T : ScriptObjectBase
    {
        var result = (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, out IntPtr handle);

        value = result == ScriptErrorType.AsSuccess ? constructDelegate(handle) : null;

        return result;
    }

    public byte GetReturnByte()
    {
        return NativeBindings.TL_Context_GetReturnByte(this.Handle);
    }

    public ushort GetReturnUInt16()
    {
        return NativeBindings.TL_Context_GetReturnWord(this.Handle);
    }

    public uint GetReturnUInt32()
    {
        return NativeBindings.TL_Context_GetReturnDWord(this.Handle);
    }

    public ulong GetReturnUInt64()
    {
        return NativeBindings.TL_Context_GetReturnQWord(this.Handle);
    }

    public float GetReturnSingle()
    {
        return NativeBindings.TL_Context_GetReturnFloat(this.Handle);
    }

    public double GetReturnDouble()
    {
        return NativeBindings.TL_Context_GetReturnDouble(this.Handle);
    }

    public IntPtr GetReturnPointer()
    {
        return NativeBindings.TL_Context_GetReturnAddress(this.Handle);
    }

    public IntPtr GetReturnObject()
    {
        return NativeBindings.TL_Context_GetReturnObject(this.Handle);
    }

    public T GetReturnObject<T>(Func<IntPtr, T> constructFunc) where T : ScriptObjectBase
    {
        var handle = NativeBindings.TL_Context_GetReturnObject(this.Handle);
        return constructFunc(handle);
    }

    public UserData SetUserData(UserData data, uint type)
    {
        return new UserData(NativeBindings.TL_Context_SetUserData(this.Handle, data.Handle, type));
    }

    public UserData GetUserData(uint type)
    {
        return new UserData(NativeBindings.TL_Context_GetUserData(this.Handle, type));
    }

    public void Dispose()
    {
        _ = NativeBindings.TL_Context_Release(this.Handle);

        GC.SuppressFinalize(this);
    }
}