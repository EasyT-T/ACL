namespace ERF.Private;

using System.Runtime.InteropServices;

public static partial class NativeBindings
{
    private const string DllName = "erf.dll";

    [LibraryImport(DllName, EntryPoint = "TL_Load_Plugins")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Load_Plugins();

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Call_Library")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Tool_Call_Library(
        [MarshalAs(UnmanagedType.LPStr)] string libraryName,
        [MarshalAs(UnmanagedType.LPStr)] string procName);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Get_String")]
    internal static partial IntPtr TL_Tool_Get_String(
        [MarshalAs(UnmanagedType.LPStr)] string str);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Return_String")]
    internal static partial void TL_Tool_Return_String(IntPtr stringPtr);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Get_FuncPtr")]
    internal static partial IntPtr TL_Tool_Get_FuncPtr(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Return_FuncPtr")]
    internal static partial void TL_Tool_Return_FuncPtr(IntPtr funcPtr);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Get_ModuleContext")]
    internal static partial IntPtr TL_Tool_Get_ModuleContext(
        IntPtr module,
        IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Return_ModuleContext")]
    internal static partial void TL_Tool_Return_ModuleContext(IntPtr moduleContext);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_RegisterFunctionToModule")]
    internal static partial IntPtr TL_Tool_RegisterFunctionToModule(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPStr)] string declaration,
        IntPtr funcPointer,
        uint callConv,
        IntPtr auxiliary = 0);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetEngine")]
    internal static partial IntPtr TL_Engine_GetEngine();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterGlobalFunction")]
    internal static partial int TL_Engine_RegisterGlobalFunction(
        [MarshalAs(UnmanagedType.LPStr)] string declaration,
        IntPtr funcPointer,
        uint callConv,
        IntPtr auxiliary = 0);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalFunctionCount")]
    internal static partial uint TL_Engine_GetGlobalFunctionCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalFunctionByIndex")]
    internal static partial IntPtr TL_Engine_GetGlobalFunctionByIndex(uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalFunctionByDecl")]
    internal static partial IntPtr TL_Engine_GetGlobalFunctionByDecl(
        [MarshalAs(UnmanagedType.LPStr)] string declaration);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetFuncdefCount")]
    internal static partial uint TL_Engine_GetFuncdefCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetFuncdefByIndex")]
    internal static partial IntPtr TL_Engine_GetFuncdefByIndex(uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypeDeclaration")]
    internal static partial IntPtr TL_Engine_GetTypeDeclaration(
        int typeId,
        [MarshalAs(UnmanagedType.Bool)] bool includeNamespace = false);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypeInfoByName")]
    internal static partial IntPtr TL_Engine_GetTypeInfoByName(
        [MarshalAs(UnmanagedType.LPStr)] string name);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypeInfoByDecl")]
    internal static partial IntPtr TL_Engine_GetTypeInfoByDecl(
        [MarshalAs(UnmanagedType.LPStr)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateContext")]
    internal static partial IntPtr TL_Engine_CreateContext();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateScriptObject")]
    internal static partial IntPtr TL_Engine_CreateScriptObject(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateDelegate")]
    internal static partial IntPtr TL_Engine_CreateDelegate(
        IntPtr func,
        IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetModule")]
    internal static partial IntPtr TL_Engine_GetModule(
        [MarshalAs(UnmanagedType.LPStr)] string module,
        int flag = 0);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_DiscardModule")]
    internal static partial int TL_Engine_DiscardModule(
        [MarshalAs(UnmanagedType.LPStr)] string module);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetTypeId")]
    internal static partial int TL_TypeInfo_GetTypeId(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetName")]
    internal static partial IntPtr TL_TypeInfo_GetName(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Release")]
    internal static partial int TL_Context_Release(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Prepare")]
    internal static partial int TL_Context_Prepare(
        IntPtr context,
        IntPtr function);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Unprepare")]
    internal static partial int TL_Context_Unprepare(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Execute")]
    internal static partial int TL_Context_Execute(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgByte")]
    internal static partial int TL_Context_SetArgByte(
        IntPtr context,
        uint arg,
        byte value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgWord")]
    internal static partial int TL_Context_SetArgWord(
        IntPtr context,
        uint arg,
        ushort value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgDWord")]
    internal static partial int TL_Context_SetArgDWord(
        IntPtr context,
        uint arg,
        uint value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgQWord")]
    internal static partial int TL_Context_SetArgQWord(
        IntPtr context,
        uint arg,
        ulong value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgFloat")]
    internal static partial int TL_Context_SetArgFloat(
        IntPtr context,
        uint arg,
        float value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgDouble")]
    internal static partial int TL_Context_SetArgDouble(
        IntPtr context,
        uint arg,
        double value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(
        IntPtr context,
        uint arg,
        IntPtr addr);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgObject")]
    internal static partial int TL_Context_SetArgObject(
        IntPtr context,
        uint arg,
        IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgVarType")]
    internal static partial int TL_Context_SetArgVarType(
        IntPtr context,
        uint arg,
        IntPtr ptr,
        int typeId);

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

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetUserData")]
    internal static partial IntPtr TL_Context_SetUserData(
        IntPtr context,
        IntPtr data,
        uint type = 0);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetUserData")]
    internal static partial IntPtr TL_Context_GetUserData(
        IntPtr context,
        uint type = 0);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetDeclaration")]
    internal static partial IntPtr TL_Function_GetDeclaration(
        IntPtr function,
        [MarshalAs(UnmanagedType.Bool)] bool includeObjectName = true,
        [MarshalAs(UnmanagedType.Bool)] bool includeNamespace = false,
        [MarshalAs(UnmanagedType.Bool)] bool includeParamNames = false);

    [LibraryImport(DllName, EntryPoint = "TL_Module_SetUserData")]
    internal static partial IntPtr TL_Module_SetUserData(IntPtr module, IntPtr data, uint type = 0);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetUserData")]
    internal static partial IntPtr TL_Module_GetUserData(IntPtr module, uint type = 0);

    [LibraryImport(DllName, EntryPoint = "TL_Module_AddScriptSection")]
    internal static partial int TL_Module_AddScriptSection(IntPtr module, [MarshalAs(UnmanagedType.LPStr)] string name,
        [MarshalAs(UnmanagedType.LPStr)] string code, uint codeLength = 0, int lineOffset = 0);

    [LibraryImport(DllName, EntryPoint = "TL_Module_Build")]
    internal static partial int TL_Module_Build(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetFunctionCount")]
    internal static partial uint TL_Module_GetFunctionCount(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetFunctionByDecl")]
    internal static partial IntPtr TL_Module_GetFunctionByDecl(IntPtr module,
        [MarshalAs(UnmanagedType.LPStr)] string decl);
}