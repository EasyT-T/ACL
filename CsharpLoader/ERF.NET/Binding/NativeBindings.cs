namespace ERF.Binding;

using System.Runtime.InteropServices;

internal static partial class NativeBindings
{
    private const string DllName = "erf.dll";

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Call_Library")]
    internal static partial void TL_Tool_Call_Library([MarshalAs(UnmanagedType.LPStr)] string libraryName, [MarshalAs(UnmanagedType.LPStr)] string procName);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Get_String")]
    internal static partial IntPtr TL_Tool_Get_String([MarshalAs(UnmanagedType.LPStr)] string str);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Return_String")]
    internal static partial void TL_Tool_Return_String(IntPtr @string);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateContext")]
    internal static partial IntPtr TL_Engine_CreateContext();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalFunctionByDecl")]
    internal static partial IntPtr TL_Engine_GetGlobalFunctionByDecl(
        [MarshalAs(UnmanagedType.LPStr)] string declaration);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Release")]
    internal static partial int TL_Context_Release(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Prepare")]
    internal static partial int TL_Context_Prepare(IntPtr context, IntPtr function);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Unprepare")]
    internal static partial int TL_Context_Unprepare(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Execute")]
    internal static partial int TL_Context_Execute(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgByte")]
    internal static partial int TL_Context_SetArgByte(IntPtr context, uint arg, byte value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgWord")]
    internal static partial int TL_Context_SetArgWord(IntPtr context, uint arg, ushort value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgDWord")]
    internal static partial int TL_Context_SetArgDWord(IntPtr context, uint arg, uint value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgQWord")]
    internal static partial int TL_Context_SetArgQWord(IntPtr context, uint arg, ulong value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgFloat")]
    internal static partial int TL_Context_SetArgFloat(IntPtr context, uint arg, float value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgDouble")]
    internal static partial int TL_Context_SetArgDouble(IntPtr context, uint arg, double value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(IntPtr context, uint arg, IntPtr addr);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgObject")]
    internal static partial int TL_Context_SetArgObject(IntPtr context, uint arg, IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgVarType")]
    internal static partial int TL_Context_SetArgVarType(IntPtr context, uint arg, IntPtr ptr, int typeId);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetReturnByte")]
    internal static partial byte TL_Context_GetReturnByte(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetReturnWord")]
    internal static partial ushort TL_Context_GetReturnWord(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetReturnDWord")]
    internal static partial uint TL_Context_GetReturnDWord(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetReturnQWord")]
    internal static partial ulong TL_Context_GetReturnQWord(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetReturnFloat")]
    internal static partial float TL_Context_GetReturnFloat(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetReturnDouble")]
    internal static partial double TL_Context_GetReturnDouble(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetReturnAddress")]
    internal static partial IntPtr TL_Context_GetReturnAddress(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetReturnObject")]
    internal static partial IntPtr TL_Context_GetReturnObject(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetAddressOfReturnValue")]
    internal static partial IntPtr TL_Context_GetAddressOfReturnValue(IntPtr context);
}