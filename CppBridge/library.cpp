#include "library.h"
#include <ostream>
#include <Windows.h>

#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <iostream>
#include <nethost.h>

static bool initialized = false;

namespace {
    inline asIScriptEngine *engine;
    inline HMODULE mainModule;

    using CreateLoader = void(*)();

    inline hostfxr_initialize_for_runtime_config_fn init_fptr;
    inline hostfxr_get_runtime_delegate_fn get_delegate_fptr;
    inline hostfxr_close_fn close_fptr;

    bool load_hostfxr() {
        // Pre-allocate a large buffer for the path to hostfxr
        char_t buffer[MAX_PATH];
        size_t buffer_size = sizeof(buffer) / sizeof(char_t);
        int rc = get_hostfxr_path(buffer, &buffer_size, nullptr);
        if (rc != 0)
            return false;

        // Load hostfxr and get desired exports
        HMODULE lib = LoadLibraryW(buffer);
        init_fptr = reinterpret_cast<hostfxr_initialize_for_runtime_config_fn>(GetProcAddress(
            lib, "hostfxr_initialize_for_runtime_config"));
        get_delegate_fptr = reinterpret_cast<hostfxr_get_runtime_delegate_fn>(GetProcAddress(
            lib, "hostfxr_get_runtime_delegate"));
        close_fptr = reinterpret_cast<hostfxr_close_fn>(GetProcAddress(lib, "hostfxr_close"));

        return init_fptr && get_delegate_fptr && close_fptr;
    }

    load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t *config_path) {
        // Load .NET Core
        void *load_assembly_and_get_function_pointer = nullptr;
        hostfxr_handle cxt = nullptr;
        int rc = init_fptr(config_path, nullptr, &cxt);
        if (rc != 0 || cxt == nullptr) {
            close_fptr(cxt);
            return nullptr;
        }

        // Get the load assembly function pointer
        rc = get_delegate_fptr(
            cxt,
            hdt_load_assembly_and_get_function_pointer,
            &load_assembly_and_get_function_pointer);

        close_fptr(cxt);

        if (rc != 0 || load_assembly_and_get_function_pointer == nullptr) {
            return nullptr;
        }

        return reinterpret_cast<load_assembly_and_get_function_pointer_fn>(load_assembly_and_get_function_pointer);
    }
}

ModuleContext::ModuleContext(asIScriptModule *module, asIScriptContext *context) : module(module), context(context) {
}

bool __stdcall DllMain(HINSTANCE, DWORD, LPVOID) {
    if (initialized) {
        return true;
    }

    mainModule = GetModuleHandle("uemph.dll");

    if (!mainModule) {
        return false;
    }

    engine = **reinterpret_cast<asIScriptEngine ***>(reinterpret_cast<char *>(mainModule) + 0x167410);

    _putenv_s("DOTNET_EnableDiagnostics", "1");

    initialized = true;

    return initialized;
}

bool __stdcall TL_Load_Plugins() {
    if (!load_hostfxr()) {
        return false;
    }

    load_assembly_and_get_function_pointer_fn getFuncPtr = get_dotnet_load_assembly(L"ACL.runtimeconfig.json");

    if (getFuncPtr == nullptr) {
        return false;
    }

    CreateLoader createLoader;

    const int rc = getFuncPtr(L"ACL.NET.dll",
                              L"ACL.Loader.PluginLoader, ACL.NET",
                              L"CreateLoader",
                              UNMANAGEDCALLERSONLY_METHOD,
                              nullptr,
                              reinterpret_cast<void **>(&createLoader));

    if (rc != 0) {
        return false;
    }

    createLoader();

    return true;
}

bool __stdcall TL_Tool_Call_Library(const char *libraryName, const char *procName) {
    const HMODULE module = LoadLibraryA(libraryName);

    if (!module) {
        return false;
    }

    const auto createLibrary = reinterpret_cast<void (*)()>(GetProcAddress(module, procName));
    createLibrary();

    return true;
}

BBStr * __stdcall TL_Tool_Get_String(const char *str) {
    return new BBStr(str);
}

void __stdcall TL_Tool_Return_String(const BBStr *string) {
    delete string;
}

asSFuncPtr * __stdcall TL_Tool_Get_FuncPtr(void *func) {
    const auto funcPtr = new asSFuncPtr(2);
    funcPtr->ptr.f.func = reinterpret_cast<asFUNCTION_t>(func);

    return funcPtr;
}

void __stdcall TL_Tool_Return_FuncPtr(const asSFuncPtr *funcPtr) {
    delete funcPtr;
}

ModuleContext * __stdcall TL_Tool_Get_ModuleContext(asIScriptModule *module, asIScriptContext *context) {
    return new ModuleContext(module, context);
}

void __stdcall TL_Tool_Return_ModuleContext(const ModuleContext *moduleContext) {
    delete moduleContext;
}

asCScriptFunction * __stdcall TL_Tool_RegisterFunctionToModule(asCModule *module, const char *declaration,
                                                               const asSFuncPtr *funcPointer, const asDWORD callConv,
                                                               void *auxiliary) {
    engine->RegisterGlobalFunction(declaration, *funcPointer, callConv, auxiliary);
    const auto c_engine = reinterpret_cast<asCScriptEngine *>(engine);

    const auto func = reinterpret_cast<asCScriptFunction *>(c_engine->GetGlobalFunctionByDecl(declaration));

    func->funcType = asFUNC_SCRIPT;
    func->nameSpace = module->m_defaultNamespace;
    func->module = module;
    module->m_globalFunctions.Put(func);

    return func;
}

asIScriptEngine * __stdcall TL_Engine_GetEngine() {
    return engine;
}

int __stdcall TL_Engine_AddRef() {
    return engine->AddRef();
}

int __stdcall TL_Engine_Release() {
    return engine->Release();
}

int __stdcall TL_Engine_ShutDownAndRelease() {
    return engine->ShutDownAndRelease();
}

int __stdcall TL_Engine_SetEngineProperty(asEEngineProp property, asPWORD value) {
    return engine->SetEngineProperty(property, value);
}

int __stdcall TL_Engine_SetMessageCallback(const asSFuncPtr *callback, void *obj, asDWORD callConv) {
    return engine->SetMessageCallback(*callback, obj, callConv);
}

int __stdcall TL_Engine_GetMessageCallback(asSFuncPtr *callback, void **obj, asDWORD *callConv) {
    return engine->GetMessageCallback(callback, obj, callConv);
}

int __stdcall TL_Engine_ClearMessageCallback() {
    return engine->ClearMessageCallback();
}

int __stdcall TL_Engine_WriteMessage(const char *section, int row, int col, asEMsgType type, const char *message) {
    return engine->WriteMessage(section, row, col, type, message);
}

int __stdcall TL_Engine_SetJITCompiler(asIJITCompilerAbstract *compiler) {
    return engine->SetJITCompiler(compiler);
}

asIJITCompilerAbstract * __stdcall TL_Engine_GetJITCompiler() {
    return engine->GetJITCompiler();
}

int __stdcall TL_Engine_RegisterGlobalFunction(const char *declaration, const asSFuncPtr *funcPointer, asDWORD callConv,
                                               void *auxiliary) {
    return engine->RegisterGlobalFunction(declaration, *funcPointer, callConv, auxiliary);
}

asUINT __stdcall TL_Engine_GetGlobalFunctionCount() {
    return engine->GetGlobalFunctionCount();
}

asIScriptFunction * __stdcall TL_Engine_GetGlobalFunctionByIndex(asUINT index) {
    return engine->GetGlobalFunctionByIndex(index);
}

asIScriptFunction * __stdcall TL_Engine_GetGlobalFunctionByDecl(const char *declaration) {
    return engine->GetGlobalFunctionByDecl(declaration);
}

int __stdcall TL_Engine_RegisterGlobalProperty(const char *declaration, void *pointer) {
    return engine->RegisterGlobalProperty(declaration, pointer);
}

asUINT __stdcall TL_Engine_GetGlobalPropertyCount() {
    return engine->GetGlobalPropertyCount();
}

int __stdcall TL_Engine_GetGlobalPropertyByIndex(asUINT index, const char **name, const char **nameSpace, int *typeId,
                                                 bool *isConst, const char **configGroup, void **pointer,
                                                 asDWORD *accessMask) {
    return engine->GetGlobalPropertyByIndex(index, name, nameSpace, typeId, isConst, configGroup, pointer, accessMask);
}

int __stdcall TL_Engine_GetGlobalPropertyIndexByName(const char *name) {
    return engine->GetGlobalPropertyIndexByName(name);
}

int __stdcall TL_Engine_GetGlobalPropertyIndexByDecl(const char *decl) {
    return engine->GetGlobalPropertyIndexByDecl(decl);
}

int __stdcall TL_Engine_RegisterObjectType(const char *obj, int byteSize, asQWORD flags) {
    return engine->RegisterObjectType(obj, byteSize, flags);
}

int __stdcall TL_Engine_RegisterObjectProperty(const char *obj, const char *declaration, int byteOffset,
                                               int compositeOffset, bool isCompositeIndirect) {
    return engine->RegisterObjectProperty(obj, declaration, byteOffset, compositeOffset, isCompositeIndirect);
}

int __stdcall TL_Engine_RegisterObjectMethod(const char *obj, const char *declaration, const asSFuncPtr *funcPointer,
                                             asDWORD callConv, void *auxiliary, int compositeOffset,
                                             bool isCompositeIndirect) {
    return engine->RegisterObjectMethod(obj, declaration, *funcPointer, callConv, auxiliary, compositeOffset,
                                        isCompositeIndirect);
}

int __stdcall TL_Engine_RegisterObjectBehaviour(const char *obj, asEBehaviours behaviour, const char *declaration,
                                                const asSFuncPtr *funcPointer, asDWORD callConv, void *auxiliary,
                                                int compositeOffset, bool isCompositeIndirect) {
    return engine->RegisterObjectBehaviour(obj, behaviour, declaration, *funcPointer, callConv, auxiliary,
                                           compositeOffset, isCompositeIndirect);
}

int __stdcall TL_Engine_RegisterInterface(const char *name) {
    return engine->RegisterInterface(name);
}

int __stdcall TL_Engine_RegisterInterfaceMethod(const char *intf, const char *declaration) {
    return engine->RegisterInterfaceMethod(intf, declaration);
}

asUINT __stdcall TL_Engine_GetObjectTypeCount() {
    return engine->GetObjectTypeCount();
}

asITypeInfo * __stdcall TL_Engine_GetObjectTypeByIndex(asUINT index) {
    return engine->GetObjectTypeByIndex(index);
}

int __stdcall TL_Engine_RegisterStringFactory(const char *datatype, asIStringFactory *factory) {
    return engine->RegisterStringFactory(datatype, factory);
}

int __stdcall TL_Engine_GetStringFactory(asDWORD *typeModifiers, asIStringFactory **factory) {
    return engine->GetStringFactory(typeModifiers, factory);
}

int __stdcall TL_Engine_RegisterDefaultArrayType(const char *type) {
    return engine->RegisterDefaultArrayType(type);
}

int __stdcall TL_Engine_GetDefaultArrayTypeId() {
    return engine->GetDefaultArrayTypeId();
}

int __stdcall TL_Engine_RegisterEnum(const char *type) {
    return engine->RegisterEnum(type);
}

int __stdcall TL_Engine_RegisterEnumValue(const char *type, const char *name, int value) {
    return engine->RegisterEnumValue(type, name, value);
}

asUINT __stdcall TL_Engine_GetEnumCount() {
    return engine->GetEnumCount();
}

asITypeInfo * __stdcall TL_Engine_GetEnumByIndex(asUINT index) {
    return engine->GetEnumByIndex(index);
}

int __stdcall TL_Engine_RegisterFuncdef(const char *decl) {
    return engine->RegisterFuncdef(decl);
}

asUINT __stdcall TL_Engine_GetFuncdefCount() {
    return engine->GetFuncdefCount();
}

asITypeInfo * __stdcall TL_Engine_GetFuncdefByIndex(asUINT index) {
    return engine->GetFuncdefByIndex(index);
}

int __stdcall TL_Engine_RegisterTypedef(const char *type, const char *decl) {
    return engine->RegisterTypedef(type, decl);
}

asUINT __stdcall TL_Engine_GetTypedefCount() {
    return engine->GetTypedefCount();
}

asITypeInfo * __stdcall TL_Engine_GetTypedefByIndex(asUINT index) {
    return engine->GetTypedefByIndex(index);
}

int __stdcall TL_Engine_BeginConfigGroup(const char *groupName) {
    return engine->BeginConfigGroup(groupName);
}

int __stdcall TL_Engine_EndConfigGroup() {
    return engine->EndConfigGroup();
}

int __stdcall TL_Engine_RemoveConfigGroup(const char *groupName) {
    return engine->RemoveConfigGroup(groupName);
}

asDWORD __stdcall TL_Engine_SetDefaultAccessMask(asDWORD defaultMask) {
    return engine->SetDefaultAccessMask(defaultMask);
}

int __stdcall TL_Engine_SetDefaultNamespace(const char *nameSpace) {
    return engine->SetDefaultNamespace(nameSpace);
}

const char * __stdcall TL_Engine_GetDefaultNamespace() {
    return engine->GetDefaultNamespace();
}

asIScriptModule * __stdcall TL_Engine_GetModule(const char *module, asEGMFlags flag) {
    return engine->GetModule(module, flag);
}

int __stdcall TL_Engine_DiscardModule(const char *module) {
    return engine->DiscardModule(module);
}

asUINT __stdcall TL_Engine_GetModuleCount() {
    return engine->GetModuleCount();
}

asIScriptModule * __stdcall TL_Engine_GetModuleByIndex(asUINT index) {
    return engine->GetModuleByIndex(index);
}

int __stdcall TL_Engine_GetLastFunctionId() {
    return engine->GetLastFunctionId();
}

asIScriptFunction * __stdcall TL_Engine_GetFunctionById(int funcId) {
    return engine->GetFunctionById(funcId);
}

int __stdcall TL_Engine_GetTypeIdByDecl(const char *decl) {
    return engine->GetTypeIdByDecl(decl);
}

const char * __stdcall TL_Engine_GetTypeDeclaration(int typeId, bool includeNamespace) {
    return engine->GetTypeDeclaration(typeId, includeNamespace);
}

int __stdcall TL_Engine_GetSizeOfPrimitiveType(int typeId) {
    return engine->GetSizeOfPrimitiveType(typeId);
}

asITypeInfo * __stdcall TL_Engine_GetTypeInfoById(int typeId) {
    return engine->GetTypeInfoById(typeId);
}

asITypeInfo * __stdcall TL_Engine_GetTypeInfoByName(const char *name) {
    return engine->GetTypeInfoByName(name);
}

asITypeInfo * __stdcall TL_Engine_GetTypeInfoByDecl(const char *decl) {
    return engine->GetTypeInfoByDecl(decl);
}

asIScriptContext * __stdcall TL_Engine_CreateContext() {
    return engine->CreateContext();
}

void * __stdcall TL_Engine_CreateScriptObject(const asITypeInfo *type) {
    return engine->CreateScriptObject(type);
}

void * __stdcall TL_Engine_CreateScriptObjectCopy(void *obj, const asITypeInfo *type) {
    return engine->CreateScriptObjectCopy(obj, type);
}

void * __stdcall TL_Engine_CreateUninitializedScriptObject(const asITypeInfo *type) {
    return engine->CreateUninitializedScriptObject(type);
}

asIScriptFunction * __stdcall TL_Engine_CreateDelegate(asIScriptFunction *func, void *obj) {
    return engine->CreateDelegate(func, obj);
}

int __stdcall TL_Engine_AssignScriptObject(void *dstObj, void *srcObj, const asITypeInfo *type) {
    return engine->AssignScriptObject(dstObj, srcObj, type);
}

void __stdcall TL_Engine_ReleaseScriptObject(void *obj, const asITypeInfo *type) {
    engine->ReleaseScriptObject(obj, type);
}

void __stdcall TL_Engine_AddRefScriptObject(void *obj, const asITypeInfo *type) {
    engine->AddRefScriptObject(obj, type);
}

int __stdcall TL_Engine_RefCastObject(void *obj, asITypeInfo *fromType, asITypeInfo *toType, void **newPtr,
                                      bool useOnlyImplicitCast) {
    return engine->RefCastObject(obj, fromType, toType, newPtr, useOnlyImplicitCast);
}

asILockableSharedBool * __stdcall TL_Engine_GetWeakRefFlagOfScriptObject(void *obj, const asITypeInfo *type) {
    return engine->GetWeakRefFlagOfScriptObject(obj, type);
}

asIScriptContext * __stdcall TL_Engine_RequestContext() {
    return engine->RequestContext();
}

void __stdcall TL_Engine_ReturnContext(asIScriptContext *ctx) {
    engine->ReturnContext(ctx);
}

int __stdcall TL_Engine_SetContextCallbacks(asREQUESTCONTEXTFUNC_t requestCtx, asRETURNCONTEXTFUNC_t returnCtx,
                                            void *param) {
    return engine->SetContextCallbacks(requestCtx, returnCtx, param);
}

asETokenClass __stdcall TL_Engine_ParseToken(const char *string, size_t stringLength, asUINT *tokenLength) {
    return engine->ParseToken(string, stringLength, tokenLength);
}

int __stdcall TL_Engine_GarbageCollect(asDWORD flags, asUINT numIterations) {
    return engine->GarbageCollect(flags, numIterations);
}

void __stdcall TL_Engine_GetGCStatistics(asUINT *currentSize, asUINT *totalDestroyed, asUINT *totalDetected,
                                         asUINT *newObjects, asUINT *totalNewDestroyed) {
    engine->GetGCStatistics(currentSize, totalDestroyed, totalDetected, newObjects, totalNewDestroyed);
}

int __stdcall TL_Engine_NotifyGarbageCollectorOfNewObject(void *obj, asITypeInfo *type) {
    return engine->NotifyGarbageCollectorOfNewObject(obj, type);
}

int __stdcall TL_Engine_GetObjectInGC(asUINT idx, asUINT *seqNbr, void **obj, asITypeInfo **type) {
    return engine->GetObjectInGC(idx, seqNbr, obj, type);
}

void __stdcall TL_Engine_GCEnumCallback(void *reference) {
    engine->GCEnumCallback(reference);
}

void __stdcall TL_Engine_ForwardGCEnumReferences(void *ref, asITypeInfo *type) {
    engine->ForwardGCEnumReferences(ref, type);
}

void __stdcall TL_Engine_ForwardGCReleaseReferences(void *ref, asITypeInfo *type) {
    engine->ForwardGCReleaseReferences(ref, type);
}

void __stdcall TL_Engine_SetCircularRefDetectedCallback(asCIRCULARREFFUNC_t callback, void *param) {
    engine->SetCircularRefDetectedCallback(callback, param);
}

void * __stdcall TL_Engine_SetUserData(void *data, asPWORD type) {
    return engine->SetUserData(data, type);
}

void * __stdcall TL_Engine_GetUserData(asPWORD type) {
    return engine->GetUserData(type);
}

void __stdcall TL_Engine_SetEngineUserDataCleanupCallback(asCLEANENGINEFUNC_t callback, asPWORD type) {
    engine->SetEngineUserDataCleanupCallback(callback, type);
}

void __stdcall TL_Engine_SetModuleUserDataCleanupCallback(asCLEANMODULEFUNC_t callback, asPWORD type) {
    engine->SetModuleUserDataCleanupCallback(callback, type);
}

void __stdcall TL_Engine_SetContextUserDataCleanupCallback(asCLEANCONTEXTFUNC_t callback, asPWORD type) {
    engine->SetContextUserDataCleanupCallback(callback, type);
}

void __stdcall TL_Engine_SetFunctionUserDataCleanupCallback(asCLEANFUNCTIONFUNC_t callback, asPWORD type) {
    engine->SetFunctionUserDataCleanupCallback(callback, type);
}

void __stdcall TL_Engine_SetTypeInfoUserDataCleanupCallback(asCLEANTYPEINFOFUNC_t callback, asPWORD type) {
    engine->SetTypeInfoUserDataCleanupCallback(callback, type);
}

void __stdcall TL_Engine_SetScriptObjectUserDataCleanupCallback(asCLEANSCRIPTOBJECTFUNC_t callback, asPWORD type) {
    engine->SetScriptObjectUserDataCleanupCallback(callback, type);
}

int __stdcall TL_Engine_SetTranslateAppExceptionCallback(const asSFuncPtr *callback, void *param, int callConv) {
    return engine->SetTranslateAppExceptionCallback(*callback, param, callConv);
}

asIScriptEngine * __stdcall TL_Module_GetEngine(const asIScriptModule *module) {
    return module->GetEngine();
}

void __stdcall TL_Module_SetName(asIScriptModule *module, const char *name) {
    module->SetName(name);
}

const char * __stdcall TL_Module_GetName(const asIScriptModule *module) {
    return module->GetName();
}

void __stdcall TL_Module_Discard(asIScriptModule *module) {
    module->Discard();
}

int __stdcall TL_Module_AddScriptSection(asIScriptModule *module, const char *name, const char *code, size_t codeLength,
                                         int lineOffset) {
    return module->AddScriptSection(name, code, codeLength, lineOffset);
}

int __stdcall TL_Module_Build(asIScriptModule *module) {
    return module->Build();
}

int __stdcall TL_Module_CompileFunction(asIScriptModule *module, const char *sectionName, const char *code,
                                        int lineOffset, asDWORD compileFlags, asIScriptFunction **outFunc) {
    return module->CompileFunction(sectionName, code, lineOffset, compileFlags, outFunc);
}

int __stdcall TL_Module_CompileGlobalVar(asIScriptModule *module, const char *sectionName, const char *code,
                                         int lineOffset) {
    return module->CompileGlobalVar(sectionName, code, lineOffset);
}

asDWORD __stdcall TL_Module_SetAccessMask(asIScriptModule *module, asDWORD accessMask) {
    return module->SetAccessMask(accessMask);
}

int __stdcall TL_Module_SetDefaultNamespace(asIScriptModule *module, const char *nameSpace) {
    return module->SetDefaultNamespace(nameSpace);
}

const char * __stdcall TL_Module_GetDefaultNamespace(const asIScriptModule *module) {
    return module->GetDefaultNamespace();
}

asUINT __stdcall TL_Module_GetFunctionCount(const asIScriptModule *module) {
    return module->GetFunctionCount();
}

asIScriptFunction * __stdcall TL_Module_GetFunctionByIndex(const asIScriptModule *module, asUINT index) {
    return module->GetFunctionByIndex(index);
}

asIScriptFunction * __stdcall TL_Module_GetFunctionByDecl(const asIScriptModule *module, const char *decl) {
    return module->GetFunctionByDecl(decl);
}

asIScriptFunction * __stdcall TL_Module_GetFunctionByName(const asIScriptModule *module, const char *name) {
    return module->GetFunctionByName(name);
}

int __stdcall TL_Module_RemoveFunction(asIScriptModule *module, asIScriptFunction *func) {
    return module->RemoveFunction(func);
}

int __stdcall TL_Module_ResetGlobalVars(asIScriptModule *module, asIScriptContext *ctx) {
    return module->ResetGlobalVars(ctx);
}

asUINT __stdcall TL_Module_GetGlobalVarCount(const asIScriptModule *module) {
    return module->GetGlobalVarCount();
}

int __stdcall TL_Module_GetGlobalVarIndexByName(const asIScriptModule *module, const char *name) {
    return module->GetGlobalVarIndexByName(name);
}

int __stdcall TL_Module_GetGlobalVarIndexByDecl(const asIScriptModule *module, const char *decl) {
    return module->GetGlobalVarIndexByDecl(decl);
}

const char * __stdcall TL_Module_GetGlobalVarDeclaration(const asIScriptModule *module, asUINT index,
                                                         bool includeNamespace) {
    return module->GetGlobalVarDeclaration(index, includeNamespace);
}

int __stdcall TL_Module_GetGlobalVar(const asIScriptModule *module, asUINT index, const char **name,
                                     const char **nameSpace, int *typeId, bool *isConst) {
    return module->GetGlobalVar(index, name, nameSpace, typeId, isConst);
}

void * __stdcall TL_Module_GetAddressOfGlobalVar(asIScriptModule *module, asUINT index) {
    return module->GetAddressOfGlobalVar(index);
}

int __stdcall TL_Module_RemoveGlobalVar(asIScriptModule *module, asUINT index) {
    return module->RemoveGlobalVar(index);
}

asUINT __stdcall TL_Module_GetObjectTypeCount(const asIScriptModule *module) {
    return module->GetObjectTypeCount();
}

asITypeInfo * __stdcall TL_Module_GetObjectTypeByIndex(const asIScriptModule *module, asUINT index) {
    return module->GetObjectTypeByIndex(index);
}

int __stdcall TL_Module_GetTypeIdByDecl(const asIScriptModule *module, const char *decl) {
    return module->GetTypeIdByDecl(decl);
}

asITypeInfo * __stdcall TL_Module_GetTypeInfoByName(const asIScriptModule *module, const char *name) {
    return module->GetTypeInfoByName(name);
}

asITypeInfo * __stdcall TL_Module_GetTypeInfoByDecl(const asIScriptModule *module, const char *decl) {
    return module->GetTypeInfoByDecl(decl);
}

asUINT __stdcall TL_Module_GetEnumCount(const asIScriptModule *module) {
    return module->GetEnumCount();
}

asITypeInfo * __stdcall TL_Module_GetEnumByIndex(const asIScriptModule *module, asUINT index) {
    return module->GetEnumByIndex(index);
}

asUINT __stdcall TL_Module_GetTypedefCount(const asIScriptModule *module) {
    return module->GetTypedefCount();
}

asITypeInfo * __stdcall TL_Module_GetTypedefByIndex(const asIScriptModule *module, asUINT index) {
    return module->GetTypedefByIndex(index);
}

asUINT __stdcall TL_Module_GetImportedFunctionCount(const asIScriptModule *module) {
    return module->GetImportedFunctionCount();
}

int __stdcall TL_Module_GetImportedFunctionIndexByDecl(const asIScriptModule *module, const char *decl) {
    return module->GetImportedFunctionIndexByDecl(decl);
}

const char * __stdcall TL_Module_GetImportedFunctionDeclaration(const asIScriptModule *module, asUINT importIndex) {
    return module->GetImportedFunctionDeclaration(importIndex);
}

const char * __stdcall TL_Module_GetImportedFunctionSourceModule(const asIScriptModule *module, asUINT importIndex) {
    return module->GetImportedFunctionSourceModule(importIndex);
}

int __stdcall TL_Module_BindImportedFunction(asIScriptModule *module, asUINT importIndex, asIScriptFunction *func) {
    return module->BindImportedFunction(importIndex, func);
}

int __stdcall TL_Module_UnbindImportedFunction(asIScriptModule *module, asUINT importIndex) {
    return module->UnbindImportedFunction(importIndex);
}

int __stdcall TL_Module_BindAllImportedFunctions(asIScriptModule *module) {
    return module->BindAllImportedFunctions();
}

int __stdcall TL_Module_UnbindAllImportedFunctions(asIScriptModule *module) {
    return module->UnbindAllImportedFunctions();
}

int __stdcall TL_Module_SaveByteCode(const asIScriptModule *module, asIBinaryStream *out, bool stripDebugInfo) {
    return module->SaveByteCode(out, stripDebugInfo);
}

int __stdcall TL_Module_LoadByteCode(asIScriptModule *module, asIBinaryStream *in, bool *wasDebugInfoStripped) {
    return module->LoadByteCode(in, wasDebugInfoStripped);
}

void * __stdcall TL_Module_SetUserData(asIScriptModule *module, void *data, asPWORD type) {
    return module->SetUserData(data, type);
}

void * __stdcall TL_Module_GetUserData(const asIScriptModule *module, asPWORD type) {
    return module->GetUserData(type);
}

int __stdcall TL_Context_AddRef(const asIScriptContext *context) {
    return context->AddRef();
}

int __stdcall TL_Context_Release(const asIScriptContext *context) {
    return context->Release();
}

asIScriptEngine * __stdcall TL_Context_GetEngine(const asIScriptContext *context) {
    return context->GetEngine();
}

int __stdcall TL_Context_Prepare(asIScriptContext *context, asIScriptFunction *func) {
    return context->Prepare(func);
}

int __stdcall TL_Context_Unprepare(asIScriptContext *context) {
    return context->Unprepare();
}

int __stdcall TL_Context_Execute(asIScriptContext *context) {
    return context->Execute();
}

int __stdcall TL_Context_Abort(asIScriptContext *context) {
    return context->Abort();
}

int __stdcall TL_Context_Suspend(asIScriptContext *context) {
    return context->Suspend();
}

asEContextState __stdcall TL_Context_GetState(const asIScriptContext *context) {
    return context->GetState();
}

int __stdcall TL_Context_PushState(asIScriptContext *context) {
    return context->PushState();
}

int __stdcall TL_Context_PopState(asIScriptContext *context) {
    return context->PopState();
}

bool __stdcall TL_Context_IsNested(const asIScriptContext *context, asUINT *nestCount) {
    return context->IsNested(nestCount);
}

int __stdcall TL_Context_SetObject(asIScriptContext *context, void *obj) {
    return context->SetObject(obj);
}

int __stdcall TL_Context_SetArgByte(asIScriptContext *context, asUINT arg, asBYTE value) {
    return context->SetArgByte(arg, value);
}

int __stdcall TL_Context_SetArgWord(asIScriptContext *context, asUINT arg, asWORD value) {
    return context->SetArgWord(arg, value);
}

int __stdcall TL_Context_SetArgDWord(asIScriptContext *context, asUINT arg, asDWORD value) {
    return context->SetArgDWord(arg, value);
}

int __stdcall TL_Context_SetArgQWord(asIScriptContext *context, asUINT arg, asQWORD value) {
    return context->SetArgQWord(arg, value);
}

int __stdcall TL_Context_SetArgFloat(asIScriptContext *context, asUINT arg, float value) {
    return context->SetArgFloat(arg, value);
}

int __stdcall TL_Context_SetArgDouble(asIScriptContext *context, asUINT arg, double value) {
    return context->SetArgDouble(arg, value);
}

int __stdcall TL_Context_SetArgAddress(asIScriptContext *context, asUINT arg, void *addr) {
    return context->SetArgAddress(arg, addr);
}

int __stdcall TL_Context_SetArgObject(asIScriptContext *context, asUINT arg, void *obj) {
    return context->SetArgObject(arg, obj);
}

int __stdcall TL_Context_SetArgVarType(asIScriptContext *context, asUINT arg, void *ptr, int typeId) {
    return context->SetArgVarType(arg, ptr, typeId);
}

void * __stdcall TL_Context_GetAddressOfArg(asIScriptContext *context, asUINT arg) {
    return context->GetAddressOfArg(arg);
}

asBYTE __stdcall TL_Context_GetReturnByte(asIScriptContext *context) {
    return context->GetReturnByte();
}

asWORD __stdcall TL_Context_GetReturnWord(asIScriptContext *context) {
    return context->GetReturnWord();
}

asDWORD __stdcall TL_Context_GetReturnDWord(asIScriptContext *context) {
    return context->GetReturnDWord();
}

asQWORD __stdcall TL_Context_GetReturnQWord(asIScriptContext *context) {
    return context->GetReturnQWord();
}

float __stdcall TL_Context_GetReturnFloat(asIScriptContext *context) {
    return context->GetReturnFloat();
}

double __stdcall TL_Context_GetReturnDouble(asIScriptContext *context) {
    return context->GetReturnDouble();
}

void * __stdcall TL_Context_GetReturnAddress(asIScriptContext *context) {
    return context->GetReturnAddress();
}

void * __stdcall TL_Context_GetReturnObject(asIScriptContext *context) {
    return context->GetReturnObject();
}

void * __stdcall TL_Context_GetAddressOfReturnValue(asIScriptContext *context) {
    return context->GetAddressOfReturnValue();
}

int __stdcall TL_Context_SetException(asIScriptContext *context, const char *info, bool allowCatch) {
    return context->SetException(info, allowCatch);
}

int __stdcall TL_Context_GetExceptionLineNumber(asIScriptContext *context, int *column, const char **sectionName) {
    return context->GetExceptionLineNumber(column, sectionName);
}

asIScriptFunction * __stdcall TL_Context_GetExceptionFunction(asIScriptContext *context) {
    return context->GetExceptionFunction();
}

const char * __stdcall TL_Context_GetExceptionString(asIScriptContext *context) {
    return context->GetExceptionString();
}

bool __stdcall TL_Context_WillExceptionBeCaught(asIScriptContext *context) {
    return context->WillExceptionBeCaught();
}

int __stdcall TL_Context_SetExceptionCallback(asIScriptContext *context, const asSFuncPtr *callback, void *obj,
                                              int callConv) {
    return context->SetExceptionCallback(*callback, obj, callConv);
}

void __stdcall TL_Context_ClearExceptionCallback(asIScriptContext *context) {
    context->ClearExceptionCallback();
}

int __stdcall TL_Context_SetLineCallback(asIScriptContext *context, const asSFuncPtr *callback, void *obj,
                                         int callConv) {
    return context->SetLineCallback(*callback, obj, callConv);
}

void __stdcall TL_Context_ClearLineCallback(asIScriptContext *context) {
    context->ClearLineCallback();
}

asUINT __stdcall TL_Context_GetCallstackSize(const asIScriptContext *context) {
    return context->GetCallstackSize();
}

asIScriptFunction * __stdcall TL_Context_GetFunction(asIScriptContext *context, asUINT stackLevel) {
    return context->GetFunction(stackLevel);
}

int __stdcall TL_Context_GetLineNumber(asIScriptContext *context, asUINT stackLevel, int *column,
                                       const char **sectionName) {
    return context->GetLineNumber(stackLevel, column, sectionName);
}

int __stdcall TL_Context_GetVarCount(asIScriptContext *context, asUINT stackLevel) {
    return context->GetVarCount(stackLevel);
}

int __stdcall TL_Context_GetVar(asIScriptContext *context, asUINT varIndex, asUINT stackLevel, const char **name,
                                int *typeId, asETypeModifiers *typeModifiers, bool *isVarOnHeap, int *stackOffset) {
    return context->GetVar(varIndex, stackLevel, name, typeId, typeModifiers, isVarOnHeap, stackOffset);
}

const char * __stdcall TL_Context_GetVarDeclaration(asIScriptContext *context, asUINT varIndex, asUINT stackLevel,
                                                    bool includeNamespace) {
    return context->GetVarDeclaration(varIndex, stackLevel, includeNamespace);
}

void * __stdcall TL_Context_GetAddressOfVar(asIScriptContext *context, asUINT varIndex, asUINT stackLevel,
                                            bool dontDereference, bool returnAddressOfUnitializedObjects) {
    return context->GetAddressOfVar(varIndex, stackLevel, dontDereference, returnAddressOfUnitializedObjects);
}

bool __stdcall TL_Context_IsVarInScope(asIScriptContext *context, asUINT varIndex, asUINT stackLevel) {
    return context->IsVarInScope(varIndex, stackLevel);
}

int __stdcall TL_Context_GetThisTypeId(asIScriptContext *context, asUINT stackLevel) {
    return context->GetThisTypeId(stackLevel);
}

void * __stdcall TL_Context_GetThisPointer(asIScriptContext *context, asUINT stackLevel) {
    return context->GetThisPointer(stackLevel);
}

asIScriptFunction * __stdcall TL_Context_GetSystemFunction(asIScriptContext *context) {
    return context->GetSystemFunction();
}

void * __stdcall TL_Context_SetUserData(asIScriptContext *context, void *data, asPWORD type) {
    return context->SetUserData(data, type);
}

void * __stdcall TL_Context_GetUserData(const asIScriptContext *context, asPWORD type) {
    return context->GetUserData(type);
}

int __stdcall TL_Context_StartDeserialization(asIScriptContext *context) {
    return context->StartDeserialization();
}

int __stdcall TL_Context_FinishDeserialization(asIScriptContext *context) {
    return context->FinishDeserialization();
}

int __stdcall TL_Context_PushFunction(asIScriptContext *context, asIScriptFunction *func, void *object) {
    return context->PushFunction(func, object);
}

int __stdcall TL_Context_GetStateRegisters(asIScriptContext *context, asUINT stackLevel,
                                           asIScriptFunction **callingSystemFunction,
                                           asIScriptFunction **initialFunction, asDWORD *origStackPointer,
                                           asDWORD *argumentsSize, asQWORD *valueRegister, void **objectRegister,
                                           asITypeInfo **objectTypeRegister) {
    return context->GetStateRegisters(stackLevel, callingSystemFunction, initialFunction, origStackPointer,
                                      argumentsSize, valueRegister, objectRegister, objectTypeRegister);
}

int __stdcall TL_Context_GetCallStateRegisters(asIScriptContext *context, asUINT stackLevel, asDWORD *stackFramePointer,
                                               asIScriptFunction **currentFunction, asDWORD *programPointer,
                                               asDWORD *stackPointer, asDWORD *stackIndex) {
    return context->GetCallStateRegisters(stackLevel, stackFramePointer, currentFunction, programPointer, stackPointer,
                                          stackIndex);
}

int __stdcall TL_Context_SetStateRegisters(asIScriptContext *context, asUINT stackLevel,
                                           asIScriptFunction *callingSystemFunction, asIScriptFunction *initialFunction,
                                           asDWORD origStackPointer,
                                           asDWORD argumentsSize, asQWORD valueRegister, void *objectRegister,
                                           asITypeInfo *objectTypeRegister) {
    return context->SetStateRegisters(stackLevel, callingSystemFunction, initialFunction, origStackPointer,
                                      argumentsSize, valueRegister, objectRegister, objectTypeRegister);
}

int __stdcall TL_Context_SetCallStateRegisters(asIScriptContext *context, asUINT stackLevel, asDWORD stackFramePointer,
                                               asIScriptFunction *currentFunction, asDWORD programPointer,
                                               asDWORD stackPointer, asDWORD stackIndex) {
    return context->SetCallStateRegisters(stackLevel, stackFramePointer, currentFunction, programPointer, stackPointer,
                                          stackIndex);
}

int __stdcall TL_Context_GetArgsOnStackCount(asIScriptContext *context, asUINT stackLevel) {
    return context->GetArgsOnStackCount(stackLevel);
}

int __stdcall TL_Context_GetArgOnStack(asIScriptContext *context, asUINT stackLevel, asUINT arg, int *typeId,
                                       asUINT *flags, void **address) {
    return context->GetArgOnStack(stackLevel, arg, typeId, flags, address);
}

int __stdcall TL_Object_AddRef(const asIScriptObject *obj) {
    return obj->AddRef();
}

int __stdcall TL_Object_Release(const asIScriptObject *obj) {
    return obj->Release();
}

asILockableSharedBool * __stdcall TL_Object_GetWeakRefFlag(const asIScriptObject *obj) {
    return obj->GetWeakRefFlag();
}

int __stdcall TL_Object_GetTypeId(const asIScriptObject *obj) {
    return obj->GetTypeId();
}

asITypeInfo * __stdcall TL_Object_GetObjectType(const asIScriptObject *obj) {
    return obj->GetObjectType();
}

asUINT __stdcall TL_Object_GetPropertyCount(const asIScriptObject *obj) {
    return obj->GetPropertyCount();
}

int __stdcall TL_Object_GetPropertyTypeId(const asIScriptObject *obj, asUINT prop) {
    return obj->GetPropertyTypeId(prop);
}

const char * __stdcall TL_Object_GetPropertyName(const asIScriptObject *obj, asUINT prop) {
    return obj->GetPropertyName(prop);
}

void * __stdcall TL_Object_GetAddressOfProperty(asIScriptObject *obj, asUINT prop) {
    return obj->GetAddressOfProperty(prop);
}

asIScriptEngine * __stdcall TL_Object_GetEngine(const asIScriptObject *obj) {
    return obj->GetEngine();
}

int __stdcall TL_Object_CopyFrom(asIScriptObject *obj, const asIScriptObject *other) {
    return obj->CopyFrom(other);
}

void * __stdcall TL_Object_SetUserData(asIScriptObject *obj, void *data, asPWORD type) {
    return obj->SetUserData(data, type);
}

void * __stdcall TL_Object_GetUserData(const asIScriptObject *obj, asPWORD type) {
    return obj->GetUserData(type);
}

asIScriptEngine * __stdcall TL_TypeInfo_GetEngine(const asITypeInfo *type) {
    return type->GetEngine();
}

const char * __stdcall TL_TypeInfo_GetConfigGroup(const asITypeInfo *type) {
    return type->GetConfigGroup();
}

asDWORD __stdcall TL_TypeInfo_GetAccessMask(const asITypeInfo *type) {
    return type->GetAccessMask();
}

asIScriptModule * __stdcall TL_TypeInfo_GetModule(const asITypeInfo *type) {
    return type->GetModule();
}

int __stdcall TL_TypeInfo_AddRef(const asITypeInfo *type) {
    return type->AddRef();
}

int __stdcall TL_TypeInfo_Release(const asITypeInfo *type) {
    return type->Release();
}

const char * __stdcall TL_TypeInfo_GetName(const asITypeInfo *type) {
    return type->GetName();
}

const char * __stdcall TL_TypeInfo_GetNamespace(const asITypeInfo *type) {
    return type->GetNamespace();
}

asITypeInfo * __stdcall TL_TypeInfo_GetBaseType(const asITypeInfo *type) {
    return type->GetBaseType();
}

bool __stdcall TL_TypeInfo_DerivesFrom(const asITypeInfo *type, const asITypeInfo *objType) {
    return type->DerivesFrom(objType);
}

asQWORD __stdcall TL_TypeInfo_GetFlags(const asITypeInfo *type) {
    return type->GetFlags();
}

asUINT __stdcall TL_TypeInfo_GetSize(const asITypeInfo *type) {
    return type->GetSize();
}

int __stdcall TL_TypeInfo_GetTypeId(const asITypeInfo *type) {
    return type->GetTypeId();
}

int __stdcall TL_TypeInfo_GetSubTypeId(const asITypeInfo *type, asUINT subTypeIndex) {
    return type->GetSubTypeId(subTypeIndex);
}

asITypeInfo * __stdcall TL_TypeInfo_GetSubType(const asITypeInfo *type, asUINT subTypeIndex) {
    return type->GetSubType(subTypeIndex);
}

asUINT __stdcall TL_TypeInfo_GetSubTypeCount(const asITypeInfo *type) {
    return type->GetSubTypeCount();
}

asUINT __stdcall TL_TypeInfo_GetInterfaceCount(const asITypeInfo *type) {
    return type->GetInterfaceCount();
}

asITypeInfo * __stdcall TL_TypeInfo_GetInterface(const asITypeInfo *type, asUINT index) {
    return type->GetInterface(index);
}

bool __stdcall TL_TypeInfo_Implements(const asITypeInfo *type, const asITypeInfo *objType) {
    return type->Implements(objType);
}

asUINT __stdcall TL_TypeInfo_GetFactoryCount(const asITypeInfo *type) {
    return type->GetFactoryCount();
}

asIScriptFunction * __stdcall TL_TypeInfo_GetFactoryByIndex(const asITypeInfo *type, asUINT index) {
    return type->GetFactoryByIndex(index);
}

asIScriptFunction * __stdcall TL_TypeInfo_GetFactoryByDecl(const asITypeInfo *type, const char *decl) {
    return type->GetFactoryByDecl(decl);
}

asUINT __stdcall TL_TypeInfo_GetMethodCount(const asITypeInfo *type) {
    return type->GetMethodCount();
}

asIScriptFunction * __stdcall TL_TypeInfo_GetMethodByIndex(const asITypeInfo *type, asUINT index, bool getVirtual) {
    return type->GetMethodByIndex(index, getVirtual);
}

asIScriptFunction * __stdcall TL_TypeInfo_GetMethodByName(const asITypeInfo *type, const char *name, bool getVirtual) {
    return type->GetMethodByName(name, getVirtual);
}

asIScriptFunction * __stdcall TL_TypeInfo_GetMethodByDecl(const asITypeInfo *type, const char *decl, bool getVirtual) {
    return type->GetMethodByDecl(decl, getVirtual);
}

asUINT __stdcall TL_TypeInfo_GetPropertyCount(const asITypeInfo *type) {
    return type->GetPropertyCount();
}

int __stdcall TL_TypeInfo_GetProperty(const asITypeInfo *type, asUINT index, const char **name, int *typeId,
                                      bool *isPrivate, bool *isProtected, int *offset, bool *isReference,
                                      asDWORD *accessMask, int *compositeOffset,
                                      bool *isCompositeIndirect, bool *isConst) {
    return type->GetProperty(index, name, typeId, isPrivate, isProtected, offset, isReference, accessMask,
                             compositeOffset, isCompositeIndirect, isConst);
}

const char * __stdcall
TL_TypeInfo_GetPropertyDeclaration(const asITypeInfo *type, asUINT index, bool includeNamespace) {
    return type->GetPropertyDeclaration(index, includeNamespace);
}

asUINT __stdcall TL_TypeInfo_GetBehaviourCount(const asITypeInfo *type) {
    return type->GetBehaviourCount();
}

asIScriptFunction * __stdcall TL_TypeInfo_GetBehaviourByIndex(const asITypeInfo *type, asUINT index,
                                                              asEBehaviours *outBehaviour) {
    return type->GetBehaviourByIndex(index, outBehaviour);
}

asUINT __stdcall TL_TypeInfo_GetChildFuncdefCount(const asITypeInfo *type) {
    return type->GetChildFuncdefCount();
}

asITypeInfo * __stdcall TL_TypeInfo_GetChildFuncdef(const asITypeInfo *type, asUINT index) {
    return type->GetChildFuncdef(index);
}

asITypeInfo * __stdcall TL_TypeInfo_GetParentType(const asITypeInfo *type) {
    return type->GetParentType();
}

asUINT __stdcall TL_TypeInfo_GetEnumValueCount(const asITypeInfo *type) {
    return type->GetEnumValueCount();
}

const char * __stdcall TL_TypeInfo_GetEnumValueByIndex(const asITypeInfo *type, asUINT index, int *outValue) {
    return type->GetEnumValueByIndex(index, outValue);
}

int __stdcall TL_TypeInfo_GetTypedefTypeId(const asITypeInfo *type) {
    return type->GetTypedefTypeId();
}

asIScriptFunction * __stdcall TL_TypeInfo_GetFuncdefSignature(const asITypeInfo *type) {
    return type->GetFuncdefSignature();
}

void * __stdcall TL_TypeInfo_SetUserData(asITypeInfo *type, void *data, asPWORD userType) {
    return type->SetUserData(data, userType);
}

void * __stdcall TL_TypeInfo_GetUserData(const asITypeInfo *type, asPWORD userType) {
    return type->GetUserData(userType);
}

asIScriptEngine * __stdcall TL_Function_GetEngine(const asIScriptFunction *func) {
    return func->GetEngine();
}

int __stdcall TL_Function_AddRef(const asIScriptFunction *func) {
    return func->AddRef();
}

int __stdcall TL_Function_Release(const asIScriptFunction *func) {
    return func->Release();
}

int __stdcall TL_Function_GetId(const asIScriptFunction *func) {
    return func->GetId();
}

asEFuncType __stdcall TL_Function_GetFuncType(const asIScriptFunction *func) {
    return func->GetFuncType();
}

const char * __stdcall TL_Function_GetModuleName(const asIScriptFunction *func) {
    return func->GetModuleName();
}

asIScriptModule * __stdcall TL_Function_GetModule(const asIScriptFunction *func) {
    return func->GetModule();
}

const char * __stdcall TL_Function_GetConfigGroup(const asIScriptFunction *func) {
    return func->GetConfigGroup();
}

asDWORD __stdcall TL_Function_GetAccessMask(const asIScriptFunction *func) {
    return func->GetAccessMask();
}

void * __stdcall TL_Function_GetAuxiliary(const asIScriptFunction *func) {
    return func->GetAuxiliary();
}

asITypeInfo * __stdcall TL_Function_GetObjectType(const asIScriptFunction *func) {
    return func->GetObjectType();
}

const char * __stdcall TL_Function_GetObjectName(const asIScriptFunction *func) {
    return func->GetObjectName();
}

const char * __stdcall TL_Function_GetName(const asIScriptFunction *func) {
    return func->GetName();
}

const char * __stdcall TL_Function_GetNamespace(const asIScriptFunction *func) {
    return func->GetNamespace();
}

const char * __stdcall TL_Function_GetDeclaration(const asIScriptFunction *func, bool includeObjectName,
                                                  bool includeNamespace, bool includeParamNames) {
    return func->GetDeclaration(includeObjectName, includeNamespace, includeParamNames);
}

bool __stdcall TL_Function_IsReadOnly(const asIScriptFunction *func) {
    return func->IsReadOnly();
}

bool __stdcall TL_Function_IsPrivate(const asIScriptFunction *func) {
    return func->IsPrivate();
}

bool __stdcall TL_Function_IsProtected(const asIScriptFunction *func) {
    return func->IsProtected();
}

bool __stdcall TL_Function_IsFinal(const asIScriptFunction *func) {
    return func->IsFinal();
}

bool __stdcall TL_Function_IsOverride(const asIScriptFunction *func) {
    return func->IsOverride();
}

bool __stdcall TL_Function_IsShared(const asIScriptFunction *func) {
    return func->IsShared();
}

bool __stdcall TL_Function_IsExplicit(const asIScriptFunction *func) {
    return func->IsExplicit();
}

bool __stdcall TL_Function_IsProperty(const asIScriptFunction *func) {
    return func->IsProperty();
}

bool __stdcall TL_Function_IsVariadic(const asIScriptFunction *func) {
    return func->IsVariadic();
}

asUINT __stdcall TL_Function_GetParamCount(const asIScriptFunction *func) {
    return func->GetParamCount();
}

int __stdcall TL_Function_GetParam(const asIScriptFunction *func, asUINT index, int *typeId, asDWORD *flags,
                                   const char **name, const char **defaultArg) {
    return func->GetParam(index, typeId, flags, name, defaultArg);
}

int __stdcall TL_Function_GetReturnTypeId(const asIScriptFunction *func, asDWORD *flags) {
    return func->GetReturnTypeId(flags);
}

asUINT __stdcall TL_Function_GetSubTypeCount(const asIScriptFunction *func) {
    return func->GetSubTypeCount();
}

int __stdcall TL_Function_GetSubTypeId(const asIScriptFunction *func, asUINT subTypeIndex) {
    return func->GetSubTypeId(subTypeIndex);
}

asITypeInfo * __stdcall TL_Function_GetSubType(const asIScriptFunction *func, asUINT subTypeIndex) {
    return func->GetSubType(subTypeIndex);
}

int __stdcall TL_Function_GetTypeId(const asIScriptFunction *func) {
    return func->GetTypeId();
}

bool __stdcall TL_Function_IsCompatibleWithTypeId(const asIScriptFunction *func, int typeId) {
    return func->IsCompatibleWithTypeId(typeId);
}

void * __stdcall TL_Function_GetDelegateObject(const asIScriptFunction *func) {
    return func->GetDelegateObject();
}

asITypeInfo * __stdcall TL_Function_GetDelegateObjectType(const asIScriptFunction *func) {
    return func->GetDelegateObjectType();
}

asIScriptFunction * __stdcall TL_Function_GetDelegateFunction(const asIScriptFunction *func) {
    return func->GetDelegateFunction();
}

asUINT __stdcall TL_Function_GetVarCount(const asIScriptFunction *func) {
    return func->GetVarCount();
}

int __stdcall TL_Function_GetVar(const asIScriptFunction *func, asUINT index, const char **name, int *typeId) {
    return func->GetVar(index, name, typeId);
}

const char * __stdcall TL_Function_GetVarDecl(const asIScriptFunction *func, asUINT index, bool includeNamespace) {
    return func->GetVarDecl(index, includeNamespace);
}

int __stdcall TL_Function_FindNextLineWithCode(const asIScriptFunction *func, int line) {
    return func->FindNextLineWithCode(line);
}

int __stdcall TL_Function_GetDeclaredAt(const asIScriptFunction *func, const char **scriptSection, int *row, int *col) {
    return func->GetDeclaredAt(scriptSection, row, col);
}

asDWORD * __stdcall TL_Function_GetByteCode(asIScriptFunction *func, asUINT *length) {
    return func->GetByteCode(length);
}

int __stdcall TL_Function_SetJITFunction(asIScriptFunction *func, asJITFunction jitFunc) {
    return func->SetJITFunction(jitFunc);
}

asJITFunction __stdcall TL_Function_GetJITFunction(const asIScriptFunction *func) {
    return func->GetJITFunction();
}

void * __stdcall TL_Function_SetUserData(asIScriptFunction *func, void *userData, asPWORD type) {
    return func->SetUserData(userData, type);
}

void * __stdcall TL_Function_GetUserData(const asIScriptFunction *func, const asPWORD type) {
    return func->GetUserData(type);
}

const char * __stdcall TL_Tool_String_Content(const BBStr *string) {
    const unsigned int length = string->size() + 1;
    const auto str = new char[length];
    memcpy_s(str, length, string->c_str(), length);

    return str;
}