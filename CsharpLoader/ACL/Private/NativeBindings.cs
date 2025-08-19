namespace ACL.Private;

using System.Runtime.InteropServices;

internal static partial class NativeBindings
{
    private const string DllName = "ACL.dll";

    #region Tool Functions

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Get_String")]
    internal static partial IntPtr TL_Tool_Get_String(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string str);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_String_Content")]
    [return: MarshalAs(UnmanagedType.LPUTF8Str)]
    internal static partial string TL_Tool_String_Content(IntPtr stringPtr);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Return_String")]
    internal static partial void TL_Tool_Return_String(IntPtr stringPtr);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Get_FuncPtr")]
    internal static partial IntPtr TL_Tool_Get_FuncPtr(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Return_FuncPtr")]
    internal static partial void TL_Tool_Return_FuncPtr(IntPtr funcPtr);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Get_ModuleContext")]
    internal static partial IntPtr TL_Tool_Get_ModuleContext(IntPtr module, IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_Return_ModuleContext")]
    internal static partial void TL_Tool_Return_ModuleContext(IntPtr moduleContext);

    [LibraryImport(DllName, EntryPoint = "TL_Tool_RegisterFunctionToModule")]
    internal static partial IntPtr TL_Tool_RegisterFunctionToModule(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string declaration,
        IntPtr funcPointer,
        uint callConv,
        IntPtr auxiliary);

    #endregion

    #region Engine Functions

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetEngine")]
    internal static partial IntPtr TL_Engine_GetEngine();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_AddRef")]
    internal static partial int TL_Engine_AddRef();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_Release")]
    internal static partial int TL_Engine_Release();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_ShutDownAndRelease")]
    internal static partial int TL_Engine_ShutDownAndRelease();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetEngineProperty")]
    internal static partial int TL_Engine_SetEngineProperty(int property, IntPtr value);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetMessageCallback")]
    internal static partial int TL_Engine_SetMessageCallback(
        IntPtr callback,
        IntPtr obj,
        uint callConv);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetMessageCallback")]
    internal static partial int TL_Engine_GetMessageCallback(
        IntPtr callback,
        IntPtr obj,
        IntPtr callConv);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_ClearMessageCallback")]
    internal static partial int TL_Engine_ClearMessageCallback();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_WriteMessage")]
    internal static partial int TL_Engine_WriteMessage(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string section,
        int row,
        int col,
        int type,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string message);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetJITCompiler")]
    internal static partial int TL_Engine_SetJITCompiler(IntPtr compiler);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetJITCompiler")]
    internal static partial IntPtr TL_Engine_GetJITCompiler();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterGlobalFunction")]
    internal static partial int TL_Engine_RegisterGlobalFunction(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string declaration,
        IntPtr funcPointer,
        uint callConv,
        IntPtr auxiliary);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalFunctionCount")]
    internal static partial uint TL_Engine_GetGlobalFunctionCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalFunctionByIndex")]
    internal static partial IntPtr TL_Engine_GetGlobalFunctionByIndex(uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalFunctionByDecl")]
    internal static partial IntPtr TL_Engine_GetGlobalFunctionByDecl(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string declaration);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterGlobalProperty")]
    internal static partial int TL_Engine_RegisterGlobalProperty(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string declaration,
        IntPtr pointer);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalPropertyCount")]
    internal static partial uint TL_Engine_GetGlobalPropertyCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalPropertyByIndex")]
    internal static partial int TL_Engine_GetGlobalPropertyByIndex(
        uint index,
        out IntPtr name,
        out IntPtr nameSpace,
        out int typeId,
        [MarshalAs(UnmanagedType.Bool)] out bool isConst,
        out IntPtr configGroup,
        out IntPtr pointer,
        out int accessMask);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalPropertyIndexByName")]
    internal static partial int TL_Engine_GetGlobalPropertyIndexByName(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGlobalPropertyIndexByDecl")]
    internal static partial int TL_Engine_GetGlobalPropertyIndexByDecl(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterObjectType")]
    internal static partial int TL_Engine_RegisterObjectType(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string obj,
        int byteSize,
        ulong flags);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterObjectProperty")]
    internal static partial int TL_Engine_RegisterObjectProperty(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string obj,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string declaration,
        int byteOffset,
        int compositeOffset,
        [MarshalAs(UnmanagedType.Bool)] bool isCompositeIndirect);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterObjectMethod")]
    internal static partial int TL_Engine_RegisterObjectMethod(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string obj,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string declaration,
        IntPtr funcPointer,
        uint callConv,
        IntPtr auxiliary,
        int compositeOffset,
        [MarshalAs(UnmanagedType.Bool)] bool isCompositeIndirect);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterObjectBehaviour")]
    internal static partial int TL_Engine_RegisterObjectBehaviour(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string obj,
        int behaviour,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string declaration,
        IntPtr funcPointer,
        uint callConv,
        IntPtr auxiliary,
        int compositeOffset,
        [MarshalAs(UnmanagedType.Bool)] bool isCompositeIndirect);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterInterface")]
    internal static partial int TL_Engine_RegisterInterface(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterInterfaceMethod")]
    internal static partial int TL_Engine_RegisterInterfaceMethod(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string intf,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string declaration);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetObjectTypeCount")]
    internal static partial uint TL_Engine_GetObjectTypeCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetObjectTypeByIndex")]
    internal static partial IntPtr TL_Engine_GetObjectTypeByIndex(uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterStringFactory")]
    internal static partial int TL_Engine_RegisterStringFactory(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string datatype,
        IntPtr factory);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetStringFactory")]
    internal static partial int TL_Engine_GetStringFactory(
        IntPtr typeModifiers,
        IntPtr factory);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterDefaultArrayType")]
    internal static partial int TL_Engine_RegisterDefaultArrayType(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetDefaultArrayTypeId")]
    internal static partial int TL_Engine_GetDefaultArrayTypeId();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterEnum")]
    internal static partial int TL_Engine_RegisterEnum(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterEnumValue")]
    internal static partial int TL_Engine_RegisterEnumValue(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string type,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name,
        int value);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetEnumCount")]
    internal static partial uint TL_Engine_GetEnumCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetEnumByIndex")]
    internal static partial IntPtr TL_Engine_GetEnumByIndex(uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterFuncdef")]
    internal static partial int TL_Engine_RegisterFuncdef(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetFuncdefCount")]
    internal static partial uint TL_Engine_GetFuncdefCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetFuncdefByIndex")]
    internal static partial IntPtr TL_Engine_GetFuncdefByIndex(uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RegisterTypedef")]
    internal static partial int TL_Engine_RegisterTypedef(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string type,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypedefCount")]
    internal static partial uint TL_Engine_GetTypedefCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypedefByIndex")]
    internal static partial IntPtr TL_Engine_GetTypedefByIndex(uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_BeginConfigGroup")]
    internal static partial int TL_Engine_BeginConfigGroup(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string groupName);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_EndConfigGroup")]
    internal static partial int TL_Engine_EndConfigGroup();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RemoveConfigGroup")]
    internal static partial int TL_Engine_RemoveConfigGroup(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string groupName);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetDefaultAccessMask")]
    internal static partial uint TL_Engine_SetDefaultAccessMask(uint defaultMask);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetDefaultNamespace")]
    internal static partial int TL_Engine_SetDefaultNamespace(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string nameSpace);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetDefaultNamespace")]
    internal static partial IntPtr TL_Engine_GetDefaultNamespace();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetModule")]
    internal static partial IntPtr TL_Engine_GetModule(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string module,
        int flag);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_DiscardModule")]
    internal static partial int TL_Engine_DiscardModule(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string module);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetModuleCount")]
    internal static partial uint TL_Engine_GetModuleCount();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetModuleByIndex")]
    internal static partial IntPtr TL_Engine_GetModuleByIndex(uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetLastFunctionId")]
    internal static partial int TL_Engine_GetLastFunctionId();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetFunctionById")]
    internal static partial IntPtr TL_Engine_GetFunctionById(int funcId);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypeIdByDecl")]
    internal static partial int TL_Engine_GetTypeIdByDecl(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypeDeclaration")]
    internal static partial IntPtr TL_Engine_GetTypeDeclaration(
        int typeId,
        [MarshalAs(UnmanagedType.Bool)] bool includeNamespace);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetSizeOfPrimitiveType")]
    internal static partial int TL_Engine_GetSizeOfPrimitiveType(int typeId);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypeInfoById")]
    internal static partial IntPtr TL_Engine_GetTypeInfoById(int typeId);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypeInfoByName")]
    internal static partial IntPtr TL_Engine_GetTypeInfoByName(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetTypeInfoByDecl")]
    internal static partial IntPtr TL_Engine_GetTypeInfoByDecl(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateContext")]
    internal static partial IntPtr TL_Engine_CreateContext();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateScriptObject")]
    internal static partial IntPtr TL_Engine_CreateScriptObject(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateScriptObjectCopy")]
    internal static partial IntPtr TL_Engine_CreateScriptObjectCopy(
        IntPtr obj,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateUninitializedScriptObject")]
    internal static partial IntPtr TL_Engine_CreateUninitializedScriptObject(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_CreateDelegate")]
    internal static partial IntPtr TL_Engine_CreateDelegate(
        IntPtr func,
        IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_AssignScriptObject")]
    internal static partial int TL_Engine_AssignScriptObject(
        IntPtr dstObj,
        IntPtr srcObj,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_ReleaseScriptObject")]
    internal static partial void TL_Engine_ReleaseScriptObject(
        IntPtr obj,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_AddRefScriptObject")]
    internal static partial void TL_Engine_AddRefScriptObject(
        IntPtr obj,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RefCastObject")]
    internal static partial int TL_Engine_RefCastObject(
        IntPtr obj,
        IntPtr fromType,
        IntPtr toType,
        IntPtr newPtr,
        [MarshalAs(UnmanagedType.Bool)] bool useOnlyImplicitCast);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetWeakRefFlagOfScriptObject")]
    internal static partial IntPtr TL_Engine_GetWeakRefFlagOfScriptObject(
        IntPtr obj,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_RequestContext")]
    internal static partial IntPtr TL_Engine_RequestContext();

    [LibraryImport(DllName, EntryPoint = "TL_Engine_ReturnContext")]
    internal static partial void TL_Engine_ReturnContext(IntPtr ctx);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetContextCallbacks")]
    internal static partial int TL_Engine_SetContextCallbacks(
        IntPtr requestCtx,
        IntPtr returnCtx,
        IntPtr param);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_ParseToken")]
    internal static partial int TL_Engine_ParseToken(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string str,
        nuint stringLength,
        IntPtr tokenLength);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GarbageCollect")]
    internal static partial int TL_Engine_GarbageCollect(uint flags, uint numIterations);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetGCStatistics")]
    internal static partial void TL_Engine_GetGCStatistics(
        IntPtr currentSize,
        IntPtr totalDestroyed,
        IntPtr totalDetected,
        IntPtr newObjects,
        IntPtr totalNewDestroyed);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_NotifyGarbageCollectorOfNewObject")]
    internal static partial int TL_Engine_NotifyGarbageCollectorOfNewObject(
        IntPtr obj,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetObjectInGC")]
    internal static partial int TL_Engine_GetObjectInGC(
        uint idx,
        IntPtr seqNbr,
        IntPtr obj,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GCEnumCallback")]
    internal static partial void TL_Engine_GCEnumCallback(IntPtr reference);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_ForwardGCEnumReferences")]
    internal static partial void TL_Engine_ForwardGCEnumReferences(
        IntPtr refPtr,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_ForwardGCReleaseReferences")]
    internal static partial void TL_Engine_ForwardGCReleaseReferences(
        IntPtr refPtr,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetCircularRefDetectedCallback")]
    internal static partial void TL_Engine_SetCircularRefDetectedCallback(
        IntPtr callback,
        IntPtr param);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetUserData")]
    internal static partial IntPtr TL_Engine_SetUserData(
        IntPtr data,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_GetUserData")]
    internal static partial IntPtr TL_Engine_GetUserData(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetEngineUserDataCleanupCallback")]
    internal static partial void TL_Engine_SetEngineUserDataCleanupCallback(
        IntPtr callback,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetModuleUserDataCleanupCallback")]
    internal static partial void TL_Engine_SetModuleUserDataCleanupCallback(
        IntPtr callback,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetContextUserDataCleanupCallback")]
    internal static partial void TL_Engine_SetContextUserDataCleanupCallback(
        IntPtr callback,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetFunctionUserDataCleanupCallback")]
    internal static partial void TL_Engine_SetFunctionUserDataCleanupCallback(
        IntPtr callback,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetTypeInfoUserDataCleanupCallback")]
    internal static partial void TL_Engine_SetTypeInfoUserDataCleanupCallback(
        IntPtr callback,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetScriptObjectUserDataCleanupCallback")]
    internal static partial void TL_Engine_SetScriptObjectUserDataCleanupCallback(
        IntPtr callback,
        IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_Engine_SetTranslateAppExceptionCallback")]
    internal static partial int TL_Engine_SetTranslateAppExceptionCallback(
        IntPtr callback,
        IntPtr param,
        int callConv);

    #endregion

    #region Module Functions

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetEngine")]
    internal static partial IntPtr TL_Module_GetEngine(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_SetName")]
    internal static partial void TL_Module_SetName(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetName")]
    internal static partial IntPtr TL_Module_GetName(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_Discard")]
    internal static partial void TL_Module_Discard(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_AddScriptSection")]
    internal static partial int TL_Module_AddScriptSection(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string code,
        nuint codeLength,
        int lineOffset);

    [LibraryImport(DllName, EntryPoint = "TL_Module_Build")]
    internal static partial int TL_Module_Build(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_CompileFunction")]
    internal static partial int TL_Module_CompileFunction(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string sectionName,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string code,
        int lineOffset,
        uint compileFlags,
        IntPtr outFunc);

    [LibraryImport(DllName, EntryPoint = "TL_Module_CompileGlobalVar")]
    internal static partial int TL_Module_CompileGlobalVar(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string sectionName,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string code,
        int lineOffset);

    [LibraryImport(DllName, EntryPoint = "TL_Module_SetAccessMask")]
    internal static partial uint TL_Module_SetAccessMask(IntPtr module, uint accessMask);

    [LibraryImport(DllName, EntryPoint = "TL_Module_SetDefaultNamespace")]
    internal static partial int TL_Module_SetDefaultNamespace(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string nameSpace);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetDefaultNamespace")]
    internal static partial IntPtr TL_Module_GetDefaultNamespace(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetFunctionCount")]
    internal static partial uint TL_Module_GetFunctionCount(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetFunctionByIndex")]
    internal static partial IntPtr TL_Module_GetFunctionByIndex(IntPtr module, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetFunctionByDecl")]
    internal static partial IntPtr TL_Module_GetFunctionByDecl(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetFunctionByName")]
    internal static partial IntPtr TL_Module_GetFunctionByName(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(DllName, EntryPoint = "TL_Module_RemoveFunction")]
    internal static partial int TL_Module_RemoveFunction(IntPtr module, IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Module_ResetGlobalVars")]
    internal static partial int TL_Module_ResetGlobalVars(IntPtr module, IntPtr ctx);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetGlobalVarCount")]
    internal static partial uint TL_Module_GetGlobalVarCount(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetGlobalVarIndexByName")]
    internal static partial int TL_Module_GetGlobalVarIndexByName(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetGlobalVarIndexByDecl")]
    internal static partial int TL_Module_GetGlobalVarIndexByDecl(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetGlobalVarDeclaration")]
    internal static partial IntPtr TL_Module_GetGlobalVarDeclaration(
        IntPtr module,
        uint index,
        [MarshalAs(UnmanagedType.Bool)] bool includeNamespace);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetGlobalVar")]
    internal static partial int TL_Module_GetGlobalVar(
        IntPtr module,
        uint index,
        IntPtr name,
        IntPtr nameSpace,
        IntPtr typeId,
        IntPtr isConst);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetAddressOfGlobalVar")]
    internal static partial IntPtr TL_Module_GetAddressOfGlobalVar(IntPtr module, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Module_RemoveGlobalVar")]
    internal static partial int TL_Module_RemoveGlobalVar(IntPtr module, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetObjectTypeCount")]
    internal static partial uint TL_Module_GetObjectTypeCount(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetObjectTypeByIndex")]
    internal static partial IntPtr TL_Module_GetObjectTypeByIndex(IntPtr module, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetTypeIdByDecl")]
    internal static partial int TL_Module_GetTypeIdByDecl(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetTypeInfoByName")]
    internal static partial IntPtr TL_Module_GetTypeInfoByName(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetTypeInfoByDecl")]
    internal static partial IntPtr TL_Module_GetTypeInfoByDecl(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetEnumCount")]
    internal static partial uint TL_Module_GetEnumCount(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetEnumByIndex")]
    internal static partial IntPtr TL_Module_GetEnumByIndex(IntPtr module, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetTypedefCount")]
    internal static partial uint TL_Module_GetTypedefCount(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetTypedefByIndex")]
    internal static partial IntPtr TL_Module_GetTypedefByIndex(IntPtr module, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetImportedFunctionCount")]
    internal static partial uint TL_Module_GetImportedFunctionCount(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetImportedFunctionIndexByDecl")]
    internal static partial int TL_Module_GetImportedFunctionIndexByDecl(
        IntPtr module,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetImportedFunctionDeclaration")]
    internal static partial IntPtr TL_Module_GetImportedFunctionDeclaration(IntPtr module, uint importIndex);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetImportedFunctionSourceModule")]
    internal static partial IntPtr TL_Module_GetImportedFunctionSourceModule(IntPtr module, uint importIndex);

    [LibraryImport(DllName, EntryPoint = "TL_Module_BindImportedFunction")]
    internal static partial int TL_Module_BindImportedFunction(
        IntPtr module,
        uint importIndex,
        IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Module_UnbindImportedFunction")]
    internal static partial int TL_Module_UnbindImportedFunction(IntPtr module, uint importIndex);

    [LibraryImport(DllName, EntryPoint = "TL_Module_BindAllImportedFunctions")]
    internal static partial int TL_Module_BindAllImportedFunctions(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_UnbindAllImportedFunctions")]
    internal static partial int TL_Module_UnbindAllImportedFunctions(IntPtr module);

    [LibraryImport(DllName, EntryPoint = "TL_Module_SaveByteCode")]
    internal static partial int TL_Module_SaveByteCode(
        IntPtr module,
        IntPtr outStream,
        [MarshalAs(UnmanagedType.Bool)] bool stripDebugInfo);

    [LibraryImport(DllName, EntryPoint = "TL_Module_LoadByteCode")]
    internal static partial int TL_Module_LoadByteCode(
        IntPtr module,
        IntPtr inStream,
        IntPtr wasDebugInfoStripped);

    [LibraryImport(DllName, EntryPoint = "TL_Module_SetUserData")]
    internal static partial IntPtr TL_Module_SetUserData(IntPtr module, IntPtr data, uint type);

    [LibraryImport(DllName, EntryPoint = "TL_Module_GetUserData")]
    internal static partial IntPtr TL_Module_GetUserData(IntPtr module, uint type);

    #endregion

    #region Context Functions

    [LibraryImport(DllName, EntryPoint = "TL_Context_AddRef")]
    internal static partial int TL_Context_AddRef(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Release")]
    internal static partial int TL_Context_Release(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetEngine")]
    internal static partial IntPtr TL_Context_GetEngine(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Prepare")]
    internal static partial int TL_Context_Prepare(IntPtr context, IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Unprepare")]
    internal static partial int TL_Context_Unprepare(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Execute")]
    internal static partial int TL_Context_Execute(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Abort")]
    internal static partial int TL_Context_Abort(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_Suspend")]
    internal static partial int TL_Context_Suspend(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetState")]
    internal static partial int TL_Context_GetState(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_PushState")]
    internal static partial int TL_Context_PushState(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_PopState")]
    internal static partial int TL_Context_PopState(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_IsNested")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Context_IsNested(IntPtr context, IntPtr nestCount);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetObject")]
    internal static partial int TL_Context_SetObject(IntPtr context, IntPtr obj);

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

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(IntPtr context, uint arg, out byte value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(IntPtr context, uint arg, out ushort value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(IntPtr context, uint arg, out uint value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(IntPtr context, uint arg, out ulong value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(IntPtr context, uint arg, out float value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(IntPtr context, uint arg, out double value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgAddress")]
    internal static partial int TL_Context_SetArgAddress(IntPtr context, uint arg, out IntPtr value);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgObject")]
    internal static partial int TL_Context_SetArgObject(IntPtr context, uint arg, IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetArgVarType")]
    internal static partial int TL_Context_SetArgVarType(
        IntPtr context,
        uint arg,
        IntPtr ptr,
        int typeId);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetAddressOfArg")]
    internal static partial IntPtr TL_Context_GetAddressOfArg(IntPtr context, uint arg);

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

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetException")]
    internal static partial int TL_Context_SetException(
        IntPtr context,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string info,
        [MarshalAs(UnmanagedType.Bool)] bool allowCatch);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetExceptionLineNumber")]
    internal static partial int TL_Context_GetExceptionLineNumber(
        IntPtr context,
        IntPtr column,
        IntPtr sectionName);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetExceptionFunction")]
    internal static partial IntPtr TL_Context_GetExceptionFunction(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetExceptionString")]
    internal static partial IntPtr TL_Context_GetExceptionString(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_WillExceptionBeCaught")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Context_WillExceptionBeCaught(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetExceptionCallback")]
    internal static partial int TL_Context_SetExceptionCallback(
        IntPtr context,
        IntPtr callback,
        IntPtr obj,
        int callConv);

    [LibraryImport(DllName, EntryPoint = "TL_Context_ClearExceptionCallback")]
    internal static partial void TL_Context_ClearExceptionCallback(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetLineCallback")]
    internal static partial int TL_Context_SetLineCallback(
        IntPtr context,
        IntPtr callback,
        IntPtr obj,
        int callConv);

    [LibraryImport(DllName, EntryPoint = "TL_Context_ClearLineCallback")]
    internal static partial void TL_Context_ClearLineCallback(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetCallstackSize")]
    internal static partial uint TL_Context_GetCallstackSize(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetFunction")]
    internal static partial IntPtr TL_Context_GetFunction(IntPtr context, uint stackLevel);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetLineNumber")]
    internal static partial int TL_Context_GetLineNumber(
        IntPtr context,
        uint stackLevel,
        IntPtr column,
        IntPtr sectionName);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetVarCount")]
    internal static partial int TL_Context_GetVarCount(IntPtr context, uint stackLevel);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetVar")]
    internal static partial int TL_Context_GetVar(
        IntPtr context,
        uint varIndex,
        uint stackLevel,
        IntPtr name,
        IntPtr typeId,
        IntPtr typeModifiers,
        IntPtr isVarOnHeap,
        IntPtr stackOffset);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetVarDeclaration")]
    internal static partial IntPtr TL_Context_GetVarDeclaration(
        IntPtr context,
        uint varIndex,
        uint stackLevel,
        [MarshalAs(UnmanagedType.Bool)] bool includeNamespace);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetAddressOfVar")]
    internal static partial IntPtr TL_Context_GetAddressOfVar(
        IntPtr context,
        uint varIndex,
        uint stackLevel,
        [MarshalAs(UnmanagedType.Bool)] bool dontDereference,
        [MarshalAs(UnmanagedType.Bool)] bool returnAddressOfUnitializedObjects);

    [LibraryImport(DllName, EntryPoint = "TL_Context_IsVarInScope")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Context_IsVarInScope(
        IntPtr context,
        uint varIndex,
        uint stackLevel);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetThisTypeId")]
    internal static partial int TL_Context_GetThisTypeId(IntPtr context, uint stackLevel);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetThisPointer")]
    internal static partial IntPtr TL_Context_GetThisPointer(IntPtr context, uint stackLevel);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetSystemFunction")]
    internal static partial IntPtr TL_Context_GetSystemFunction(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetUserData")]
    internal static partial IntPtr TL_Context_SetUserData(IntPtr context, IntPtr data, uint type);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetUserData")]
    internal static partial IntPtr TL_Context_GetUserData(IntPtr context, uint type);

    [LibraryImport(DllName, EntryPoint = "TL_Context_StartDeserialization")]
    internal static partial int TL_Context_StartDeserialization(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_FinishDeserialization")]
    internal static partial int TL_Context_FinishDeserialization(IntPtr context);

    [LibraryImport(DllName, EntryPoint = "TL_Context_PushFunction")]
    internal static partial int TL_Context_PushFunction(
        IntPtr context,
        IntPtr func,
        IntPtr objectPtr);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetStateRegisters")]
    internal static partial int TL_Context_GetStateRegisters(
        IntPtr context,
        uint stackLevel,
        IntPtr callingSystemFunction,
        IntPtr initialFunction,
        IntPtr origStackPointer,
        IntPtr argumentsSize,
        IntPtr valueRegister,
        IntPtr objectRegister,
        IntPtr objectTypeRegister);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetCallStateRegisters")]
    internal static partial int TL_Context_GetCallStateRegisters(
        IntPtr context,
        uint stackLevel,
        IntPtr stackFramePointer,
        IntPtr currentFunction,
        IntPtr programPointer,
        IntPtr stackPointer,
        IntPtr stackIndex);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetStateRegisters")]
    internal static partial int TL_Context_SetStateRegisters(
        IntPtr context,
        uint stackLevel,
        IntPtr callingSystemFunction,
        IntPtr initialFunction,
        IntPtr origStackPointer,
        IntPtr argumentsSize,
        long valueRegister,
        IntPtr objectRegister,
        IntPtr objectTypeRegister);

    [LibraryImport(DllName, EntryPoint = "TL_Context_SetCallStateRegisters")]
    internal static partial int TL_Context_SetCallStateRegisters(
        IntPtr context,
        uint stackLevel,
        uint stackFramePointer,
        IntPtr currentFunction,
        uint programPointer,
        uint stackPointer,
        uint stackIndex);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetArgsOnStackCount")]
    internal static partial int TL_Context_GetArgsOnStackCount(IntPtr context, uint stackLevel);

    [LibraryImport(DllName, EntryPoint = "TL_Context_GetArgOnStack")]
    internal static partial int TL_Context_GetArgOnStack(
        IntPtr context,
        uint stackLevel,
        uint arg,
        IntPtr typeId,
        IntPtr flags,
        IntPtr address);

    #endregion

    #region Object Functions

    [LibraryImport(DllName, EntryPoint = "TL_Object_AddRef")]
    internal static partial int TL_Object_AddRef(IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Object_Release")]
    internal static partial int TL_Object_Release(IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetWeakRefFlag")]
    internal static partial IntPtr TL_Object_GetWeakRefFlag(IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetTypeId")]
    internal static partial int TL_Object_GetTypeId(IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetObjectType")]
    internal static partial IntPtr TL_Object_GetObjectType(IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetPropertyCount")]
    internal static partial uint TL_Object_GetPropertyCount(IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetPropertyTypeId")]
    internal static partial int TL_Object_GetPropertyTypeId(IntPtr obj, uint prop);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetPropertyName")]
    internal static partial IntPtr TL_Object_GetPropertyName(IntPtr obj, uint prop);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetAddressOfProperty")]
    internal static partial IntPtr TL_Object_GetAddressOfProperty(IntPtr obj, uint prop);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetEngine")]
    internal static partial IntPtr TL_Object_GetEngine(IntPtr obj);

    [LibraryImport(DllName, EntryPoint = "TL_Object_CopyFrom")]
    internal static partial int TL_Object_CopyFrom(IntPtr obj, IntPtr other);

    [LibraryImport(DllName, EntryPoint = "TL_Object_SetUserData")]
    internal static partial IntPtr TL_Object_SetUserData(IntPtr obj, IntPtr data, uint type);

    [LibraryImport(DllName, EntryPoint = "TL_Object_GetUserData")]
    internal static partial IntPtr TL_Object_GetUserData(IntPtr obj, uint type);

    #endregion

    #region TypeInfo Functions

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetEngine")]
    internal static partial IntPtr TL_TypeInfo_GetEngine(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetConfigGroup")]
    internal static partial IntPtr TL_TypeInfo_GetConfigGroup(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetAccessMask")]
    internal static partial uint TL_TypeInfo_GetAccessMask(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetModule")]
    internal static partial IntPtr TL_TypeInfo_GetModule(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_AddRef")]
    internal static partial int TL_TypeInfo_AddRef(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_Release")]
    internal static partial int TL_TypeInfo_Release(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetName")]
    internal static partial IntPtr TL_TypeInfo_GetName(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetNamespace")]
    internal static partial IntPtr TL_TypeInfo_GetNamespace(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetBaseType")]
    internal static partial IntPtr TL_TypeInfo_GetBaseType(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_DerivesFrom")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_TypeInfo_DerivesFrom(IntPtr type, IntPtr objType);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetFlags")]
    internal static partial ulong TL_TypeInfo_GetFlags(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetSize")]
    internal static partial uint TL_TypeInfo_GetSize(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetTypeId")]
    internal static partial int TL_TypeInfo_GetTypeId(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetSubTypeId")]
    internal static partial int TL_TypeInfo_GetSubTypeId(IntPtr type, uint subTypeIndex);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetSubType")]
    internal static partial IntPtr TL_TypeInfo_GetSubType(IntPtr type, uint subTypeIndex);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetSubTypeCount")]
    internal static partial uint TL_TypeInfo_GetSubTypeCount(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetInterfaceCount")]
    internal static partial uint TL_TypeInfo_GetInterfaceCount(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetInterface")]
    internal static partial IntPtr TL_TypeInfo_GetInterface(IntPtr type, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_Implements")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_TypeInfo_Implements(IntPtr type, IntPtr objType);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetFactoryCount")]
    internal static partial uint TL_TypeInfo_GetFactoryCount(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetFactoryByIndex")]
    internal static partial IntPtr TL_TypeInfo_GetFactoryByIndex(IntPtr type, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetFactoryByDecl")]
    internal static partial IntPtr TL_TypeInfo_GetFactoryByDecl(
        IntPtr type,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetMethodCount")]
    internal static partial uint TL_TypeInfo_GetMethodCount(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetMethodByIndex")]
    internal static partial IntPtr TL_TypeInfo_GetMethodByIndex(
        IntPtr type,
        uint index,
        [MarshalAs(UnmanagedType.Bool)] bool getVirtual);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetMethodByName")]
    internal static partial IntPtr TL_TypeInfo_GetMethodByName(
        IntPtr type,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string name,
        [MarshalAs(UnmanagedType.Bool)] bool getVirtual);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetMethodByDecl")]
    internal static partial IntPtr TL_TypeInfo_GetMethodByDecl(
        IntPtr type,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string decl,
        [MarshalAs(UnmanagedType.Bool)] bool getVirtual);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetPropertyCount")]
    internal static partial uint TL_TypeInfo_GetPropertyCount(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetProperty")]
    internal static partial int TL_TypeInfo_GetProperty(
        IntPtr type,
        uint index,
        IntPtr name,
        IntPtr typeId,
        IntPtr isPrivate,
        IntPtr isProtected,
        IntPtr offset,
        IntPtr isReference,
        IntPtr accessMask,
        IntPtr compositeOffset,
        IntPtr isCompositeIndirect,
        IntPtr isConst);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetPropertyDeclaration")]
    internal static partial IntPtr TL_TypeInfo_GetPropertyDeclaration(
        IntPtr type,
        uint index,
        [MarshalAs(UnmanagedType.Bool)] bool includeNamespace);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetBehaviourCount")]
    internal static partial uint TL_TypeInfo_GetBehaviourCount(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetBehaviourByIndex")]
    internal static partial IntPtr TL_TypeInfo_GetBehaviourByIndex(
        IntPtr type,
        uint index,
        IntPtr outBehaviour);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetChildFuncdefCount")]
    internal static partial uint TL_TypeInfo_GetChildFuncdefCount(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetChildFuncdef")]
    internal static partial IntPtr TL_TypeInfo_GetChildFuncdef(IntPtr type, uint index);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetParentType")]
    internal static partial IntPtr TL_TypeInfo_GetParentType(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetEnumValueCount")]
    internal static partial uint TL_TypeInfo_GetEnumValueCount(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetEnumValueByIndex")]
    internal static partial IntPtr TL_TypeInfo_GetEnumValueByIndex(
        IntPtr type,
        uint index,
        IntPtr outValue);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetTypedefTypeId")]
    internal static partial int TL_TypeInfo_GetTypedefTypeId(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetFuncdefSignature")]
    internal static partial IntPtr TL_TypeInfo_GetFuncdefSignature(IntPtr type);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_SetUserData")]
    internal static partial IntPtr TL_TypeInfo_SetUserData(IntPtr type, IntPtr data, uint userType);

    [LibraryImport(DllName, EntryPoint = "TL_TypeInfo_GetUserData")]
    internal static partial IntPtr TL_TypeInfo_GetUserData(IntPtr type, uint userType);

    #endregion

    #region Function Functions

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetEngine")]
    internal static partial IntPtr TL_Function_GetEngine(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_AddRef")]
    internal static partial int TL_Function_AddRef(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_Release")]
    internal static partial int TL_Function_Release(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetId")]
    internal static partial int TL_Function_GetId(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetFuncType")]
    internal static partial int TL_Function_GetFuncType(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetModuleName")]
    internal static partial IntPtr TL_Function_GetModuleName(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetModule")]
    internal static partial IntPtr TL_Function_GetModule(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetConfigGroup")]
    internal static partial IntPtr TL_Function_GetConfigGroup(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetAccessMask")]
    internal static partial uint TL_Function_GetAccessMask(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetAuxiliary")]
    internal static partial IntPtr TL_Function_GetAuxiliary(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetObjectType")]
    internal static partial IntPtr TL_Function_GetObjectType(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetObjectName")]
    internal static partial IntPtr TL_Function_GetObjectName(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetName")]
    internal static partial IntPtr TL_Function_GetName(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetNamespace")]
    internal static partial IntPtr TL_Function_GetNamespace(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetDeclaration")]
    internal static partial IntPtr TL_Function_GetDeclaration(
        IntPtr function,
        [MarshalAs(UnmanagedType.Bool)] bool includeObjectName,
        [MarshalAs(UnmanagedType.Bool)] bool includeNamespace,
        [MarshalAs(UnmanagedType.Bool)] bool includeParamNames);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsReadOnly")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsReadOnly(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsPrivate")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsPrivate(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsProtected")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsProtected(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsFinal")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsFinal(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsOverride")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsOverride(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsShared")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsShared(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsExplicit")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsExplicit(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsProperty")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsProperty(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsVariadic")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsVariadic(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetParamCount")]
    internal static partial uint TL_Function_GetParamCount(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetParam")]
    internal static partial int TL_Function_GetParam(
        IntPtr func,
        uint index,
        IntPtr typeId,
        IntPtr flags,
        IntPtr name,
        IntPtr defaultArg);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetReturnTypeId")]
    internal static partial int TL_Function_GetReturnTypeId(IntPtr func, IntPtr flags);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetSubTypeCount")]
    internal static partial uint TL_Function_GetSubTypeCount(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetSubTypeId")]
    internal static partial int TL_Function_GetSubTypeId(IntPtr func, uint subTypeIndex);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetSubType")]
    internal static partial IntPtr TL_Function_GetSubType(IntPtr func, uint subTypeIndex);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetTypeId")]
    internal static partial int TL_Function_GetTypeId(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_IsCompatibleWithTypeId")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool TL_Function_IsCompatibleWithTypeId(IntPtr func, int typeId);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetDelegateObject")]
    internal static partial IntPtr TL_Function_GetDelegateObject(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetDelegateObjectType")]
    internal static partial IntPtr TL_Function_GetDelegateObjectType(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetDelegateFunction")]
    internal static partial IntPtr TL_Function_GetDelegateFunction(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetVarCount")]
    internal static partial uint TL_Function_GetVarCount(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetVar")]
    internal static partial int TL_Function_GetVar(
        IntPtr func,
        uint index,
        IntPtr name,
        IntPtr typeId);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetVarDecl")]
    internal static partial IntPtr TL_Function_GetVarDecl(
        IntPtr func,
        uint index,
        [MarshalAs(UnmanagedType.Bool)] bool includeNamespace);

    [LibraryImport(DllName, EntryPoint = "TL_Function_FindNextLineWithCode")]
    internal static partial int TL_Function_FindNextLineWithCode(IntPtr func, int line);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetDeclaredAt")]
    internal static partial int TL_Function_GetDeclaredAt(
        IntPtr func,
        IntPtr scriptSection,
        IntPtr row,
        IntPtr col);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetByteCode")]
    internal static partial IntPtr TL_Function_GetByteCode(
        IntPtr func,
        IntPtr length);

    [LibraryImport(DllName, EntryPoint = "TL_Function_SetJITFunction")]
    internal static partial int TL_Function_SetJITFunction(
        IntPtr func,
        IntPtr jitFunc);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetJITFunction")]
    internal static partial IntPtr TL_Function_GetJITFunction(IntPtr func);

    [LibraryImport(DllName, EntryPoint = "TL_Function_SetUserData")]
    internal static partial IntPtr TL_Function_SetUserData(
        IntPtr func,
        IntPtr userData,
        uint type);

    [LibraryImport(DllName, EntryPoint = "TL_Function_GetUserData")]
    internal static partial IntPtr TL_Function_GetUserData(IntPtr func, uint type);

    #endregion
}