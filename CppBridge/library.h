#pragma once
#include <string>

#include "as_module.h"
#include "as_scriptengine.h"

struct BBStr : std::string {
    BBStr *next;
    BBStr *prev;
};

struct ModuleContext {
    asIScriptModule *module;
    asIScriptContext *context;

    std::string str{};
    bool flag1{};
    bool flag2{};
    bool flag3{};

    ModuleContext(asIScriptModule *module, asIScriptContext *context);
};

extern "C" {
_declspec(dllexport) bool _stdcall TL_Load_Plugins();

_declspec(dllexport) bool _stdcall TL_Tool_Call_Library(const char *libraryName, const char *procName);

_declspec(dllexport) BBStr * _stdcall TL_Tool_Get_String(const char *str);

_declspec(dllexport) const char * _stdcall TL_Tool_String_Content(const BBStr * string);

_declspec(dllexport) void _stdcall TL_Tool_Return_String(const BBStr *string);

_declspec(dllexport) asSFuncPtr * _stdcall TL_Tool_Get_FuncPtr(void *func);

_declspec(dllexport) void _stdcall TL_Tool_Return_FuncPtr(const asSFuncPtr *funcPtr);

_declspec(dllexport) ModuleContext * __stdcall TL_Tool_Get_ModuleContext(asIScriptModule *module, asIScriptContext *context);

_declspec(dllexport) void __stdcall TL_Tool_Return_ModuleContext(const ModuleContext *moduleContext);

_declspec(dllexport) asCScriptFunction * __stdcall TL_Tool_RegisterFunctionToModule(asCModule *module, const char *declaration,
                                                               const asSFuncPtr *funcPointer, asDWORD callConv,
                                                               void *auxiliary);

_declspec(dllexport) asIScriptEngine * _stdcall TL_Engine_GetEngine();

_declspec(dllexport) int __stdcall TL_Engine_AddRef();

_declspec(dllexport) int __stdcall TL_Engine_Release();

_declspec(dllexport) int __stdcall TL_Engine_ShutDownAndRelease();

_declspec(dllexport) int __stdcall TL_Engine_SetEngineProperty(asEEngineProp property, asPWORD value);

_declspec(dllexport) int __stdcall TL_Engine_SetMessageCallback(const asSFuncPtr *callback, void *obj, asDWORD callConv);

_declspec(dllexport) int __stdcall TL_Engine_GetMessageCallback(asSFuncPtr *callback, void **obj, asDWORD *callConv);

_declspec(dllexport) int __stdcall TL_Engine_ClearMessageCallback();

_declspec(dllexport) int __stdcall TL_Engine_WriteMessage(const char *section, int row, int col, asEMsgType type, const char *message);

_declspec(dllexport) int __stdcall TL_Engine_SetJITCompiler(asIJITCompilerAbstract *compiler);

_declspec(dllexport) asIJITCompilerAbstract * __stdcall TL_Engine_GetJITCompiler();

_declspec(dllexport) int __stdcall TL_Engine_RegisterGlobalFunction(const char *declaration, const asSFuncPtr *funcPointer, asDWORD callConv,
                                               void *auxiliary);

_declspec(dllexport) asUINT __stdcall TL_Engine_GetGlobalFunctionCount();

_declspec(dllexport) asIScriptFunction * __stdcall TL_Engine_GetGlobalFunctionByIndex(asUINT index);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Engine_GetGlobalFunctionByDecl(const char *declaration);

_declspec(dllexport) int __stdcall TL_Engine_RegisterGlobalProperty(const char *declaration, void *pointer);

_declspec(dllexport) asUINT __stdcall TL_Engine_GetGlobalPropertyCount();

_declspec(dllexport) int __stdcall TL_Engine_GetGlobalPropertyByIndex(asUINT index, const char **name, const char **nameSpace, int *typeId,
                                                 bool *isConst, const char **configGroup, void **pointer,
                                                 asDWORD *accessMask);

_declspec(dllexport) int __stdcall TL_Engine_GetGlobalPropertyIndexByName(const char *name);

_declspec(dllexport) int __stdcall TL_Engine_GetGlobalPropertyIndexByDecl(const char *decl);

_declspec(dllexport) int __stdcall TL_Engine_RegisterObjectType(const char *obj, int byteSize, asQWORD flags);

_declspec(dllexport) int __stdcall TL_Engine_RegisterObjectProperty(const char *obj, const char *declaration, int byteOffset,
                                               int compositeOffset, bool isCompositeIndirect);

_declspec(dllexport) int __stdcall TL_Engine_RegisterObjectMethod(const char *obj, const char *declaration, const asSFuncPtr *funcPointer,
                                             asDWORD callConv, void *auxiliary, int compositeOffset,
                                             bool isCompositeIndirect);

_declspec(dllexport) int __stdcall TL_Engine_RegisterObjectBehaviour(const char *obj, asEBehaviours behaviour, const char *declaration,
                                                const asSFuncPtr *funcPointer, asDWORD callConv, void *auxiliary,
                                                int compositeOffset, bool isCompositeIndirect);

_declspec(dllexport) int __stdcall TL_Engine_RegisterInterface(const char *name);

_declspec(dllexport) int __stdcall TL_Engine_RegisterInterfaceMethod(const char *intf, const char *declaration);

_declspec(dllexport) asUINT __stdcall TL_Engine_GetObjectTypeCount();

_declspec(dllexport) asITypeInfo * __stdcall TL_Engine_GetObjectTypeByIndex(asUINT index);

_declspec(dllexport) int __stdcall TL_Engine_RegisterStringFactory(const char *datatype, asIStringFactory *factory);

_declspec(dllexport) int __stdcall TL_Engine_GetStringFactory(asDWORD *typeModifiers, asIStringFactory **factory);

_declspec(dllexport) int __stdcall TL_Engine_RegisterDefaultArrayType(const char *type);

_declspec(dllexport) int __stdcall TL_Engine_GetDefaultArrayTypeId();

_declspec(dllexport) int __stdcall TL_Engine_RegisterEnum(const char *type);

_declspec(dllexport) int __stdcall TL_Engine_RegisterEnumValue(const char *type, const char *name, int value);

_declspec(dllexport) asUINT __stdcall TL_Engine_GetEnumCount();

_declspec(dllexport) asITypeInfo * __stdcall TL_Engine_GetEnumByIndex(asUINT index);

_declspec(dllexport) int __stdcall TL_Engine_RegisterFuncdef(const char *decl);

_declspec(dllexport) asUINT __stdcall TL_Engine_GetFuncdefCount();

_declspec(dllexport) asITypeInfo * __stdcall TL_Engine_GetFuncdefByIndex(asUINT index);

_declspec(dllexport) int __stdcall TL_Engine_RegisterTypedef(const char *type, const char *decl);

_declspec(dllexport) asUINT __stdcall TL_Engine_GetTypedefCount();

_declspec(dllexport) asITypeInfo * __stdcall TL_Engine_GetTypedefByIndex(asUINT index);

_declspec(dllexport) int __stdcall TL_Engine_BeginConfigGroup(const char *groupName);

_declspec(dllexport) int __stdcall TL_Engine_EndConfigGroup();

_declspec(dllexport) int __stdcall TL_Engine_RemoveConfigGroup(const char *groupName);

_declspec(dllexport) asDWORD __stdcall TL_Engine_SetDefaultAccessMask(asDWORD defaultMask);

_declspec(dllexport) int __stdcall TL_Engine_SetDefaultNamespace(const char *nameSpace);

_declspec(dllexport) const char * __stdcall TL_Engine_GetDefaultNamespace();

_declspec(dllexport) asIScriptModule * __stdcall TL_Engine_GetModule(const char *module, asEGMFlags flag);

_declspec(dllexport) int __stdcall TL_Engine_DiscardModule(const char *module);

_declspec(dllexport) asUINT __stdcall TL_Engine_GetModuleCount();

_declspec(dllexport) asIScriptModule * __stdcall TL_Engine_GetModuleByIndex(asUINT index);

_declspec(dllexport) int __stdcall TL_Engine_GetLastFunctionId();

_declspec(dllexport) asIScriptFunction * __stdcall TL_Engine_GetFunctionById(int funcId);

_declspec(dllexport) int __stdcall TL_Engine_GetTypeIdByDecl(const char *decl);

_declspec(dllexport) const char * __stdcall TL_Engine_GetTypeDeclaration(int typeId, bool includeNamespace);

_declspec(dllexport) int __stdcall TL_Engine_GetSizeOfPrimitiveType(int typeId);

_declspec(dllexport) asITypeInfo * __stdcall TL_Engine_GetTypeInfoById(int typeId);

_declspec(dllexport) asITypeInfo * __stdcall TL_Engine_GetTypeInfoByName(const char *name);

_declspec(dllexport) asITypeInfo * __stdcall TL_Engine_GetTypeInfoByDecl(const char *decl);

_declspec(dllexport) asIScriptContext * __stdcall TL_Engine_CreateContext();

_declspec(dllexport) void * __stdcall TL_Engine_CreateScriptObject(const asITypeInfo *type);

_declspec(dllexport) void * __stdcall TL_Engine_CreateScriptObjectCopy(void *obj, const asITypeInfo *type);

_declspec(dllexport) void * __stdcall TL_Engine_CreateUninitializedScriptObject(const asITypeInfo *type);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Engine_CreateDelegate(asIScriptFunction *func, void *obj);

_declspec(dllexport) int __stdcall TL_Engine_AssignScriptObject(void *dstObj, void *srcObj, const asITypeInfo *type);

_declspec(dllexport) void __stdcall TL_Engine_ReleaseScriptObject(void *obj, const asITypeInfo *type);

_declspec(dllexport) void __stdcall TL_Engine_AddRefScriptObject(void *obj, const asITypeInfo *type);

_declspec(dllexport) int __stdcall TL_Engine_RefCastObject(void *obj, asITypeInfo *fromType, asITypeInfo *toType, void **newPtr,
                                      bool useOnlyImplicitCast);

_declspec(dllexport) asILockableSharedBool * __stdcall TL_Engine_GetWeakRefFlagOfScriptObject(void *obj, const asITypeInfo *type);

_declspec(dllexport) asIScriptContext * __stdcall TL_Engine_RequestContext();

_declspec(dllexport) void __stdcall TL_Engine_ReturnContext(asIScriptContext *ctx);

_declspec(dllexport) int __stdcall TL_Engine_SetContextCallbacks(asREQUESTCONTEXTFUNC_t requestCtx, asRETURNCONTEXTFUNC_t returnCtx,
                                            void *param);

_declspec(dllexport) asETokenClass __stdcall TL_Engine_ParseToken(const char *string, size_t stringLength, asUINT *tokenLength);

_declspec(dllexport) int __stdcall TL_Engine_GarbageCollect(asDWORD flags, asUINT numIterations);

_declspec(dllexport) void __stdcall TL_Engine_GetGCStatistics(asUINT *currentSize, asUINT *totalDestroyed, asUINT *totalDetected,
                                         asUINT *newObjects, asUINT *totalNewDestroyed);

_declspec(dllexport) int __stdcall TL_Engine_NotifyGarbageCollectorOfNewObject(void *obj, asITypeInfo *type);

_declspec(dllexport) int __stdcall TL_Engine_GetObjectInGC(asUINT idx, asUINT *seqNbr, void **obj, asITypeInfo **type);

_declspec(dllexport) void __stdcall TL_Engine_GCEnumCallback(void *reference);

_declspec(dllexport) void __stdcall TL_Engine_ForwardGCEnumReferences(void *ref, asITypeInfo *type);

_declspec(dllexport) void __stdcall TL_Engine_ForwardGCReleaseReferences(void *ref, asITypeInfo *type);

_declspec(dllexport) void __stdcall TL_Engine_SetCircularRefDetectedCallback(asCIRCULARREFFUNC_t callback, void *param);

_declspec(dllexport) void * __stdcall TL_Engine_SetUserData(void *data, asPWORD type);

_declspec(dllexport) void * __stdcall TL_Engine_GetUserData(asPWORD type);

_declspec(dllexport) void __stdcall TL_Engine_SetEngineUserDataCleanupCallback(asCLEANENGINEFUNC_t callback, asPWORD type);

_declspec(dllexport) void __stdcall TL_Engine_SetModuleUserDataCleanupCallback(asCLEANMODULEFUNC_t callback, asPWORD type);

_declspec(dllexport) void __stdcall TL_Engine_SetContextUserDataCleanupCallback(asCLEANCONTEXTFUNC_t callback, asPWORD type);

_declspec(dllexport) void __stdcall TL_Engine_SetFunctionUserDataCleanupCallback(asCLEANFUNCTIONFUNC_t callback, asPWORD type);

_declspec(dllexport) void __stdcall TL_Engine_SetTypeInfoUserDataCleanupCallback(asCLEANTYPEINFOFUNC_t callback, asPWORD type);

_declspec(dllexport) void __stdcall TL_Engine_SetScriptObjectUserDataCleanupCallback(asCLEANSCRIPTOBJECTFUNC_t callback, asPWORD type);

_declspec(dllexport) int __stdcall TL_Engine_SetTranslateAppExceptionCallback(const asSFuncPtr *callback, void *param, int callConv);

_declspec(dllexport) asIScriptEngine * __stdcall TL_Module_GetEngine(const asIScriptModule *module);

_declspec(dllexport) void __stdcall TL_Module_SetName(asIScriptModule *module, const char *name);

_declspec(dllexport) const char * __stdcall TL_Module_GetName(const asIScriptModule *module);

_declspec(dllexport) void __stdcall TL_Module_Discard(asIScriptModule *module);

_declspec(dllexport) int __stdcall TL_Module_AddScriptSection(asIScriptModule *module, const char *name, const char *code, size_t codeLength,
                                         int lineOffset);

_declspec(dllexport) int __stdcall TL_Module_Build(asIScriptModule *module);

_declspec(dllexport) int __stdcall TL_Module_CompileFunction(asIScriptModule *module, const char *sectionName, const char *code,
                                        int lineOffset, asDWORD compileFlags, asIScriptFunction **outFunc);

_declspec(dllexport) int __stdcall TL_Module_CompileGlobalVar(asIScriptModule *module, const char *sectionName, const char *code,
                                         int lineOffset);

_declspec(dllexport) asDWORD __stdcall TL_Module_SetAccessMask(asIScriptModule *module, asDWORD accessMask);

_declspec(dllexport) int __stdcall TL_Module_SetDefaultNamespace(asIScriptModule *module, const char *nameSpace);

_declspec(dllexport) const char * __stdcall TL_Module_GetDefaultNamespace(const asIScriptModule *module);

_declspec(dllexport) asUINT __stdcall TL_Module_GetFunctionCount(const asIScriptModule *module);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Module_GetFunctionByIndex(const asIScriptModule *module, asUINT index);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Module_GetFunctionByDecl(const asIScriptModule *module, const char *decl);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Module_GetFunctionByName(const asIScriptModule *module, const char *name);

_declspec(dllexport) int __stdcall TL_Module_RemoveFunction(asIScriptModule *module, asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Module_ResetGlobalVars(asIScriptModule *module, asIScriptContext *ctx);

_declspec(dllexport) asUINT __stdcall TL_Module_GetGlobalVarCount(const asIScriptModule *module);

_declspec(dllexport) int __stdcall TL_Module_GetGlobalVarIndexByName(const asIScriptModule *module, const char *name);

_declspec(dllexport) int __stdcall TL_Module_GetGlobalVarIndexByDecl(const asIScriptModule *module, const char *decl);

_declspec(dllexport) const char * __stdcall TL_Module_GetGlobalVarDeclaration(const asIScriptModule *module, asUINT index,
                                                         bool includeNamespace);

_declspec(dllexport) int __stdcall TL_Module_GetGlobalVar(const asIScriptModule *module, asUINT index, const char **name,
                                     const char **nameSpace,
                                     int *typeId, bool *isConst);

_declspec(dllexport) void * __stdcall TL_Module_GetAddressOfGlobalVar(asIScriptModule *module, asUINT index);

_declspec(dllexport) int __stdcall TL_Module_RemoveGlobalVar(asIScriptModule *module, asUINT index);

_declspec(dllexport) asUINT __stdcall TL_Module_GetObjectTypeCount(const asIScriptModule *module);

_declspec(dllexport) asITypeInfo * __stdcall TL_Module_GetObjectTypeByIndex(const asIScriptModule *module, asUINT index);

_declspec(dllexport) int __stdcall TL_Module_GetTypeIdByDecl(const asIScriptModule *module, const char *decl);

_declspec(dllexport) asITypeInfo * __stdcall TL_Module_GetTypeInfoByName(const asIScriptModule *module, const char *name);

_declspec(dllexport) asITypeInfo * __stdcall TL_Module_GetTypeInfoByDecl(const asIScriptModule *module, const char *decl);

_declspec(dllexport) asUINT __stdcall TL_Module_GetEnumCount(const asIScriptModule *module);

_declspec(dllexport) asITypeInfo * __stdcall TL_Module_GetEnumByIndex(const asIScriptModule *module, asUINT index);

_declspec(dllexport) asUINT __stdcall TL_Module_GetTypedefCount(const asIScriptModule *module);

_declspec(dllexport) asITypeInfo * __stdcall TL_Module_GetTypedefByIndex(const asIScriptModule *module, asUINT index);

_declspec(dllexport) asUINT __stdcall TL_Module_GetImportedFunctionCount(const asIScriptModule *module);

_declspec(dllexport) int __stdcall TL_Module_GetImportedFunctionIndexByDecl(const asIScriptModule *module, const char *decl);

_declspec(dllexport) const char * __stdcall TL_Module_GetImportedFunctionDeclaration(const asIScriptModule *module, asUINT importIndex);

_declspec(dllexport) const char * __stdcall TL_Module_GetImportedFunctionSourceModule(const asIScriptModule *module, asUINT importIndex);

_declspec(dllexport) int __stdcall TL_Module_BindImportedFunction(asIScriptModule *module, asUINT importIndex, asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Module_UnbindImportedFunction(asIScriptModule *module, asUINT importIndex);

_declspec(dllexport) int __stdcall TL_Module_BindAllImportedFunctions(asIScriptModule *module);

_declspec(dllexport) int __stdcall TL_Module_UnbindAllImportedFunctions(asIScriptModule *module);

_declspec(dllexport) int __stdcall TL_Module_SaveByteCode(const asIScriptModule *module, asIBinaryStream *out, bool stripDebugInfo);

_declspec(dllexport) int __stdcall TL_Module_LoadByteCode(asIScriptModule *module, asIBinaryStream *in, bool *wasDebugInfoStripped);

_declspec(dllexport) void * __stdcall TL_Module_SetUserData(asIScriptModule *module, void *data, asPWORD type);

_declspec(dllexport) void * __stdcall TL_Module_GetUserData(const asIScriptModule *module, asPWORD type);

_declspec(dllexport) int __stdcall TL_Context_AddRef(const asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_Release(const asIScriptContext *context);

_declspec(dllexport) asIScriptEngine * __stdcall TL_Context_GetEngine(const asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_Prepare(asIScriptContext *context, asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Context_Unprepare(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_Execute(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_Abort(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_Suspend(asIScriptContext *context);

_declspec(dllexport) asEContextState __stdcall TL_Context_GetState(const asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_PushState(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_PopState(asIScriptContext *context);

_declspec(dllexport) bool __stdcall TL_Context_IsNested(const asIScriptContext *context, asUINT *nestCount);

_declspec(dllexport) int __stdcall TL_Context_SetObject(asIScriptContext *context, void *obj);

_declspec(dllexport) int __stdcall TL_Context_SetArgByte(asIScriptContext *context, asUINT arg, asBYTE value);

_declspec(dllexport) int __stdcall TL_Context_SetArgWord(asIScriptContext *context, asUINT arg, asWORD value);

_declspec(dllexport) int __stdcall TL_Context_SetArgDWord(asIScriptContext *context, asUINT arg, asDWORD value);

_declspec(dllexport) int __stdcall TL_Context_SetArgQWord(asIScriptContext *context, asUINT arg, asQWORD value);

_declspec(dllexport) int __stdcall TL_Context_SetArgFloat(asIScriptContext *context, asUINT arg, float value);

_declspec(dllexport) int __stdcall TL_Context_SetArgDouble(asIScriptContext *context, asUINT arg, double value);

_declspec(dllexport) int __stdcall TL_Context_SetArgAddress(asIScriptContext *context, asUINT arg, void *addr);

_declspec(dllexport) int __stdcall TL_Context_SetArgObject(asIScriptContext *context, asUINT arg, void *obj);

_declspec(dllexport) int __stdcall TL_Context_SetArgVarType(asIScriptContext *context, asUINT arg, void *ptr, int typeId);

_declspec(dllexport) void * __stdcall TL_Context_GetAddressOfArg(asIScriptContext *context, asUINT arg);

_declspec(dllexport) asBYTE __stdcall TL_Context_GetReturnByte(asIScriptContext *context);

_declspec(dllexport) asWORD __stdcall TL_Context_GetReturnWord(asIScriptContext *context);

_declspec(dllexport) asDWORD __stdcall TL_Context_GetReturnDWord(asIScriptContext *context);

_declspec(dllexport) asQWORD __stdcall TL_Context_GetReturnQWord(asIScriptContext *context);

_declspec(dllexport) float __stdcall TL_Context_GetReturnFloat(asIScriptContext *context);

_declspec(dllexport) double __stdcall TL_Context_GetReturnDouble(asIScriptContext *context);

_declspec(dllexport) void * __stdcall TL_Context_GetReturnAddress(asIScriptContext *context);

_declspec(dllexport) void * __stdcall TL_Context_GetReturnObject(asIScriptContext *context);

_declspec(dllexport) void * __stdcall TL_Context_GetAddressOfReturnValue(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_SetException(asIScriptContext *context, const char *info, bool allowCatch);

_declspec(dllexport) int __stdcall TL_Context_GetExceptionLineNumber(asIScriptContext *context, int *column, const char **sectionName);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Context_GetExceptionFunction(asIScriptContext *context);

_declspec(dllexport) const char * __stdcall TL_Context_GetExceptionString(asIScriptContext *context);

_declspec(dllexport) bool __stdcall TL_Context_WillExceptionBeCaught(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_SetExceptionCallback(asIScriptContext *context, const asSFuncPtr *callback, void *obj,
                                              int callConv);

_declspec(dllexport) void __stdcall TL_Context_ClearExceptionCallback(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_SetLineCallback(asIScriptContext *context, const asSFuncPtr *callback, void *obj,
                                         int callConv);

_declspec(dllexport) void __stdcall TL_Context_ClearLineCallback(asIScriptContext *context);

_declspec(dllexport) asUINT __stdcall TL_Context_GetCallstackSize(const asIScriptContext *context);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Context_GetFunction(asIScriptContext *context, asUINT stackLevel);

_declspec(dllexport) int __stdcall TL_Context_GetLineNumber(asIScriptContext *context, asUINT stackLevel, int *column,
                                       const char **sectionName);

_declspec(dllexport) int __stdcall TL_Context_GetVarCount(asIScriptContext *context, asUINT stackLevel);

_declspec(dllexport) int __stdcall TL_Context_GetVar(asIScriptContext *context, asUINT varIndex, asUINT stackLevel, const char **name,
                                int *typeId, asETypeModifiers *typeModifiers, bool *isVarOnHeap, int *stackOffset);

_declspec(dllexport) const char * __stdcall TL_Context_GetVarDeclaration(asIScriptContext *context, asUINT varIndex, asUINT stackLevel,
                                                    bool includeNamespace);

_declspec(dllexport) void * __stdcall TL_Context_GetAddressOfVar(asIScriptContext *context, asUINT varIndex, asUINT stackLevel,
                                            bool dontDereference, bool returnAddressOfUnitializedObjects);

_declspec(dllexport) bool __stdcall TL_Context_IsVarInScope(asIScriptContext *context, asUINT varIndex, asUINT stackLevel);

_declspec(dllexport) int __stdcall TL_Context_GetThisTypeId(asIScriptContext *context, asUINT stackLevel);

_declspec(dllexport) void * __stdcall TL_Context_GetThisPointer(asIScriptContext *context, asUINT stackLevel);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Context_GetSystemFunction(asIScriptContext *context);

_declspec(dllexport) void * __stdcall TL_Context_SetUserData(asIScriptContext *context, void *data, asPWORD type);

_declspec(dllexport) void * __stdcall TL_Context_GetUserData(const asIScriptContext *context, asPWORD type);

_declspec(dllexport) int __stdcall TL_Context_StartDeserialization(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_FinishDeserialization(asIScriptContext *context);

_declspec(dllexport) int __stdcall TL_Context_PushFunction(asIScriptContext *context, asIScriptFunction *func, void *object);

_declspec(dllexport) int __stdcall TL_Context_GetStateRegisters(asIScriptContext *context, asUINT stackLevel,
                                           asIScriptFunction **callingSystemFunction,
                                           asIScriptFunction **initialFunction, asDWORD *origStackPointer,
                                           asDWORD *argumentsSize, asQWORD *valueRegister, void **objectRegister,
                                           asITypeInfo **objectTypeRegister);

_declspec(dllexport) int __stdcall TL_Context_GetCallStateRegisters(asIScriptContext *context, asUINT stackLevel, asDWORD *stackFramePointer,
                                               asIScriptFunction **currentFunction, asDWORD *programPointer,
                                               asDWORD *stackPointer, asDWORD *stackIndex);

_declspec(dllexport) int __stdcall TL_Context_SetStateRegisters(asIScriptContext *context, asUINT stackLevel,
                                           asIScriptFunction *callingSystemFunction, asIScriptFunction *initialFunction,
                                           asDWORD origStackPointer, asDWORD argumentsSize, asQWORD valueRegister,
                                           void *objectRegister, asITypeInfo *objectTypeRegister);

_declspec(dllexport) int __stdcall TL_Context_SetCallStateRegisters(asIScriptContext *context, asUINT stackLevel, asDWORD stackFramePointer,
                                               asIScriptFunction *currentFunction, asDWORD programPointer,
                                               asDWORD stackPointer, asDWORD stackIndex);

_declspec(dllexport) int __stdcall TL_Context_GetArgsOnStackCount(asIScriptContext *context, asUINT stackLevel);

_declspec(dllexport) int __stdcall TL_Context_GetArgOnStack(asIScriptContext *context, asUINT stackLevel, asUINT arg, int *typeId,
                                       asUINT *flags, void **address);

_declspec(dllexport) int __stdcall TL_Object_AddRef(const asIScriptObject *obj);

_declspec(dllexport) int __stdcall TL_Object_Release(const asIScriptObject *obj);

_declspec(dllexport) asILockableSharedBool * __stdcall TL_Object_GetWeakRefFlag(const asIScriptObject *obj);

_declspec(dllexport) int __stdcall TL_Object_GetTypeId(const asIScriptObject *obj);

_declspec(dllexport) asITypeInfo * __stdcall TL_Object_GetObjectType(const asIScriptObject *obj);

_declspec(dllexport) asUINT __stdcall TL_Object_GetPropertyCount(const asIScriptObject *obj);

_declspec(dllexport) int __stdcall TL_Object_GetPropertyTypeId(const asIScriptObject *obj, asUINT prop);

_declspec(dllexport) const char * __stdcall TL_Object_GetPropertyName(const asIScriptObject *obj, asUINT prop);

_declspec(dllexport) void * __stdcall TL_Object_GetAddressOfProperty(asIScriptObject *obj, asUINT prop);

_declspec(dllexport) asIScriptEngine * __stdcall TL_Object_GetEngine(const asIScriptObject *obj);

_declspec(dllexport) int __stdcall TL_Object_CopyFrom(asIScriptObject *obj, const asIScriptObject *other);

_declspec(dllexport) void * __stdcall TL_Object_SetUserData(asIScriptObject *obj, void *data, asPWORD type);

_declspec(dllexport) void * __stdcall TL_Object_GetUserData(const asIScriptObject *obj, asPWORD type);

_declspec(dllexport) asIScriptEngine * __stdcall TL_TypeInfo_GetEngine(const asITypeInfo *type);

_declspec(dllexport) const char * __stdcall TL_TypeInfo_GetConfigGroup(const asITypeInfo *type);

_declspec(dllexport) asDWORD __stdcall TL_TypeInfo_GetAccessMask(const asITypeInfo *type);

_declspec(dllexport) asIScriptModule * __stdcall TL_TypeInfo_GetModule(const asITypeInfo *type);

_declspec(dllexport) int __stdcall TL_TypeInfo_AddRef(const asITypeInfo *type);

_declspec(dllexport) int __stdcall TL_TypeInfo_Release(const asITypeInfo *type);

_declspec(dllexport) const char * __stdcall TL_TypeInfo_GetName(const asITypeInfo *type);

_declspec(dllexport) const char * __stdcall TL_TypeInfo_GetNamespace(const asITypeInfo *type);

_declspec(dllexport) asITypeInfo * __stdcall TL_TypeInfo_GetBaseType(const asITypeInfo *type);

_declspec(dllexport) bool __stdcall TL_TypeInfo_DerivesFrom(const asITypeInfo *type, const asITypeInfo *objType);

_declspec(dllexport) asQWORD __stdcall TL_TypeInfo_GetFlags(const asITypeInfo *type);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetSize(const asITypeInfo *type);

_declspec(dllexport) int __stdcall TL_TypeInfo_GetTypeId(const asITypeInfo *type);

_declspec(dllexport) int __stdcall TL_TypeInfo_GetSubTypeId(const asITypeInfo *type, asUINT subTypeIndex);

_declspec(dllexport) asITypeInfo * __stdcall TL_TypeInfo_GetSubType(const asITypeInfo *type, asUINT subTypeIndex);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetSubTypeCount(const asITypeInfo *type);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetInterfaceCount(const asITypeInfo *type);

_declspec(dllexport) asITypeInfo * __stdcall TL_TypeInfo_GetInterface(const asITypeInfo *type, asUINT index);

_declspec(dllexport) bool __stdcall TL_TypeInfo_Implements(const asITypeInfo *type, const asITypeInfo *objType);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetFactoryCount(const asITypeInfo *type);

_declspec(dllexport) asIScriptFunction * __stdcall TL_TypeInfo_GetFactoryByIndex(const asITypeInfo *type, asUINT index);

_declspec(dllexport) asIScriptFunction * __stdcall TL_TypeInfo_GetFactoryByDecl(const asITypeInfo *type, const char *decl);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetMethodCount(const asITypeInfo *type);

_declspec(dllexport) asIScriptFunction * __stdcall TL_TypeInfo_GetMethodByIndex(const asITypeInfo *type, asUINT index, bool getVirtual);

_declspec(dllexport) asIScriptFunction * __stdcall TL_TypeInfo_GetMethodByName(const asITypeInfo *type, const char *name, bool getVirtual);

_declspec(dllexport) asIScriptFunction * __stdcall TL_TypeInfo_GetMethodByDecl(const asITypeInfo *type, const char *decl, bool getVirtual);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetPropertyCount(const asITypeInfo *type);

_declspec(dllexport) int __stdcall TL_TypeInfo_GetProperty(const asITypeInfo *type, asUINT index, const char **name, int *typeId,
                                      bool *isPrivate,
                                      bool *isProtected, int *offset, bool *isReference, asDWORD *accessMask,
                                      int *compositeOffset, bool *isCompositeIndirect, bool *isConst);

_declspec(dllexport) const char * __stdcall
TL_TypeInfo_GetPropertyDeclaration(const asITypeInfo *type, asUINT index, bool includeNamespace);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetBehaviourCount(const asITypeInfo *type);

_declspec(dllexport) asIScriptFunction * __stdcall TL_TypeInfo_GetBehaviourByIndex(const asITypeInfo *type, asUINT index,
                                                              asEBehaviours *outBehaviour);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetChildFuncdefCount(const asITypeInfo *type);

_declspec(dllexport) asITypeInfo * __stdcall TL_TypeInfo_GetChildFuncdef(const asITypeInfo *type, asUINT index);

_declspec(dllexport) asITypeInfo * __stdcall TL_TypeInfo_GetParentType(const asITypeInfo *type);

_declspec(dllexport) asUINT __stdcall TL_TypeInfo_GetEnumValueCount(const asITypeInfo *type);

_declspec(dllexport) const char * __stdcall TL_TypeInfo_GetEnumValueByIndex(const asITypeInfo *type, asUINT index, int *outValue);

_declspec(dllexport) int __stdcall TL_TypeInfo_GetTypedefTypeId(const asITypeInfo *type);

_declspec(dllexport) asIScriptFunction * __stdcall TL_TypeInfo_GetFuncdefSignature(const asITypeInfo *type);

_declspec(dllexport) void * __stdcall TL_TypeInfo_SetUserData(asITypeInfo *type, void *data, asPWORD userType);

_declspec(dllexport) void * __stdcall TL_TypeInfo_GetUserData(const asITypeInfo *type, asPWORD userType);

_declspec(dllexport) asIScriptEngine * __stdcall TL_Function_GetEngine(const asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Function_AddRef(const asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Function_Release(const asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Function_GetId(const asIScriptFunction *func);

_declspec(dllexport) asEFuncType __stdcall TL_Function_GetFuncType(const asIScriptFunction *func);

_declspec(dllexport) const char * __stdcall TL_Function_GetModuleName(const asIScriptFunction *func);

_declspec(dllexport) asIScriptModule * __stdcall TL_Function_GetModule(const asIScriptFunction *func);

_declspec(dllexport) const char * __stdcall TL_Function_GetConfigGroup(const asIScriptFunction *func);

_declspec(dllexport) asDWORD __stdcall TL_Function_GetAccessMask(const asIScriptFunction *func);

_declspec(dllexport) void * __stdcall TL_Function_GetAuxiliary(const asIScriptFunction *func);

_declspec(dllexport) asITypeInfo * __stdcall TL_Function_GetObjectType(const asIScriptFunction *func);

_declspec(dllexport) const char * __stdcall TL_Function_GetObjectName(const asIScriptFunction *func);

_declspec(dllexport) const char * __stdcall TL_Function_GetName(const asIScriptFunction *func);

_declspec(dllexport) const char * __stdcall TL_Function_GetNamespace(const asIScriptFunction *func);

_declspec(dllexport) const char * __stdcall TL_Function_GetDeclaration(const asIScriptFunction *func, bool includeObjectName,
                                                  bool includeNamespace, bool includeParamNames);

_declspec(dllexport) bool __stdcall TL_Function_IsReadOnly(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsPrivate(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsProtected(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsFinal(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsOverride(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsShared(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsExplicit(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsProperty(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsVariadic(const asIScriptFunction *func);

_declspec(dllexport) asUINT __stdcall TL_Function_GetParamCount(const asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Function_GetParam(const asIScriptFunction *func, asUINT index, int *typeId, asDWORD *flags,
                                   const char **name, const char **defaultArg);

_declspec(dllexport) int __stdcall TL_Function_GetReturnTypeId(const asIScriptFunction *func, asDWORD *flags);

_declspec(dllexport) asUINT __stdcall TL_Function_GetSubTypeCount(const asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Function_GetSubTypeId(const asIScriptFunction *func, asUINT subTypeIndex);

_declspec(dllexport) asITypeInfo * __stdcall TL_Function_GetSubType(const asIScriptFunction *func, asUINT subTypeIndex);

_declspec(dllexport) int __stdcall TL_Function_GetTypeId(const asIScriptFunction *func);

_declspec(dllexport) bool __stdcall TL_Function_IsCompatibleWithTypeId(const asIScriptFunction *func, int typeId);

_declspec(dllexport) void * __stdcall TL_Function_GetDelegateObject(const asIScriptFunction *func);

_declspec(dllexport) asITypeInfo * __stdcall TL_Function_GetDelegateObjectType(const asIScriptFunction *func);

_declspec(dllexport) asIScriptFunction * __stdcall TL_Function_GetDelegateFunction(const asIScriptFunction *func);

_declspec(dllexport) asUINT __stdcall TL_Function_GetVarCount(const asIScriptFunction *func);

_declspec(dllexport) int __stdcall TL_Function_GetVar(const asIScriptFunction *func, asUINT index, const char **name, int *typeId);

_declspec(dllexport) const char * __stdcall TL_Function_GetVarDecl(const asIScriptFunction *func, asUINT index, bool includeNamespace);

_declspec(dllexport) int __stdcall TL_Function_FindNextLineWithCode(const asIScriptFunction *func, int line);

_declspec(dllexport) int __stdcall TL_Function_GetDeclaredAt(const asIScriptFunction *func, const char **scriptSection, int *row, int *col);

_declspec(dllexport) asDWORD * __stdcall TL_Function_GetByteCode(asIScriptFunction *func, asUINT *length);

_declspec(dllexport) int __stdcall TL_Function_SetJITFunction(asIScriptFunction *func, asJITFunction jitFunc);

_declspec(dllexport) asJITFunction __stdcall TL_Function_GetJITFunction(const asIScriptFunction *func);

_declspec(dllexport) void * __stdcall TL_Function_SetUserData(asIScriptFunction *func, void *userData, asPWORD type);

_declspec(dllexport) void * __stdcall TL_Function_GetUserData(const asIScriptFunction *func, asPWORD type);
}