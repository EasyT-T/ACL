namespace ERF.Loader.Managed;

using ERF.Loader.Binding;

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

    public ScriptErrorType Execute()
    {
        return (ScriptErrorType)NativeBindings.TL_Context_Execute(this.Handle);
    }

    public ScriptErrorType SetArgument(uint index, byte value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgByte(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, ushort value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgWord(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, uint value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgDWord(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, ulong value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgQWord(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, float value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgFloat(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, double value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgDouble(this.Handle, index, value);
    }

    public ScriptErrorType SetArgument(uint index, IObjectClass value)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgObject(this.Handle, index, value.Handle);
    }

    public ScriptErrorType SetArgument(uint index, IntPtr address)
    {
        return (ScriptErrorType)NativeBindings.TL_Context_SetArgAddress(this.Handle, index, address);
    }

    public byte GetReturnByte()
    {
        return NativeBindings.TL_Context_GetReturnByte(this.Handle);
    }

    public ushort GetReturnUShort()
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

    public void Dispose()
    {
        _ = NativeBindings.TL_Context_Release(this.Handle);

        GC.SuppressFinalize(this);
    }
}