namespace ACL.Managed;

using System.Runtime.InteropServices;
using NativeBindings = ACL.Private.NativeBindings;

public static class ScriptEngine
{
    internal static IntPtr Handle { get; } = NativeBindings.TL_Engine_GetEngine();

    public static ScriptContext CreateContext()
    {
        return new ScriptContext(NativeBindings.TL_Engine_CreateContext());
    }

    public static uint GetGlobalFunctionCount()
    {
        return NativeBindings.TL_Engine_GetGlobalFunctionCount();
    }

    public static ScriptFunction GetGlobalFunction(uint index)
    {
        return new ScriptFunction(NativeBindings.TL_Engine_GetGlobalFunctionByIndex(index));
    }

    public static ScriptFunction GetGlobalFunction(string declaration)
    {
        return new ScriptFunction(NativeBindings.TL_Engine_GetGlobalFunctionByDecl(declaration));
    }

    public static uint GetGlobalPropertyCount()
    {
        return NativeBindings.TL_Engine_GetGlobalPropertyCount();
    }

    public static ScriptProperty GetGlobalProperty(uint index)
    {
        NativeBindings.TL_Engine_GetGlobalPropertyByIndex(index, out var namePtr, out var nameSpacePtr, out var typeId,
            out var isConst, out var configGroupPtr, out var pointer, out var accessMask);

        var name = namePtr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringUTF8(namePtr) ?? string.Empty;
        var nameSpace = nameSpacePtr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringUTF8(nameSpacePtr) ?? string.Empty;
        var configGroup = configGroupPtr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringUTF8(configGroupPtr) ?? string.Empty;

        return new ScriptProperty(name, nameSpace, typeId, isConst, configGroup, pointer, accessMask);
    }

    public static ScriptProperty GetGlobalProperty(string declaration)
    {
        var index = NativeBindings.TL_Engine_GetGlobalPropertyIndexByDecl(declaration);

        return GetGlobalProperty((uint)index);
    }
}