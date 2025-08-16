#include "library.h"

#include <ostream>
#include <vector>
#include <Windows.h>

#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <iostream>
#include <nethost.h>

#include "as_memory.h"
#include "as_module.h"
#include "as_scriptengine.h"

static bool initialized = false;

namespace {
    asIScriptEngine *engine;
    HMODULE mainModule;

    using CreateLoader = void(*)();

    hostfxr_initialize_for_runtime_config_fn init_fptr;
    hostfxr_get_runtime_delegate_fn get_delegate_fptr;
    hostfxr_close_fn close_fptr;

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

bool _stdcall DllMain(HINSTANCE, DWORD, LPVOID) {
    if (initialized) {
        return true;
    }

    mainModule = GetModuleHandle("uemph.dll");

    if (!mainModule) {
        return false;
    }

    engine = **reinterpret_cast<asIScriptEngine ***>(reinterpret_cast<char *>(mainModule) + 0x167410);

    initialized = true;

    return initialized;
}

bool _stdcall TL_Load_Plugins() {
    if (!load_hostfxr()) {
        return false;
    }

    load_assembly_and_get_function_pointer_fn getFuncPtr = get_dotnet_load_assembly(L"ERF.NET.runtimeconfig.json");

    if (getFuncPtr == nullptr) {
        return false;
    }

    CreateLoader createLoader;

    const int rc = getFuncPtr(L"ERF.NET.dll",
                              L"ERF.Loader.PluginLoader, ERF.NET",
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

bool _stdcall TL_Tool_Call_Library(const char *libraryName, const char *procName) {
    const HMODULE module = LoadLibraryA(libraryName);

    if (!module) {
        return false;
    }

    const auto createLibrary = reinterpret_cast<void (*)()>(GetProcAddress(module, procName));
    createLibrary();

    return true;
}

BBStr * _stdcall TL_Tool_Get_String(const char *str) {
    return new BBStr(str);
}

void _stdcall TL_Tool_Return_String(const BBStr *string) {
    delete string;
}

asSFuncPtr * _stdcall TL_Tool_Get_FuncPtr(void *func) {
    const auto funcPtr = new asSFuncPtr(2);
    funcPtr->ptr.f.func = reinterpret_cast<asFUNCTION_t>(func);

    return funcPtr;
}

void _stdcall TL_Tool_Return_FuncPtr(const asSFuncPtr *funcPtr) {
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

    func->nameSpace = module->m_defaultNamespace;
    func->module = module;
    module->m_globalFunctions.Put(func);

    return func;
}

asIScriptEngine * _stdcall TL_Engine_GetEngine() {
    return engine;
}

int __stdcall TL_Engine_RegisterGlobalFunction(const char *declaration, const asSFuncPtr *funcPointer,
                                               const asDWORD callConv,
                                               void *auxiliary) {
    return engine->RegisterGlobalFunction(declaration, *funcPointer, callConv, auxiliary);
}

asUINT _stdcall TL_Engine_GetGlobalFunctionCount() {
    return engine->GetGlobalFunctionCount();
}

asIScriptFunction * __stdcall TL_Engine_GetGlobalFunctionByIndex(const asUINT index) {
    return engine->GetGlobalFunctionByIndex(index);
}

asIScriptFunction * _stdcall TL_Engine_GetGlobalFunctionByDecl(const char *declaration) {
    return engine->GetGlobalFunctionByDecl(declaration);
}

asUINT __stdcall TL_Engine_GetFuncdefCount() {
    return engine->GetFuncdefCount();
}

asITypeInfo * __stdcall TL_Engine_GetFuncdefByIndex(const asUINT index) {
    return engine->GetFuncdefByIndex(index);
}

const char *TL_Engine_GetTypeDeclaration(const int typeId, const bool includeNamespace) {
    return engine->GetTypeDeclaration(typeId, includeNamespace);
}

asITypeInfo * __stdcall TL_Engine_GetTypeInfoByName(const char *name) {
    return engine->GetTypeInfoByName(name);
}

asITypeInfo * __stdcall TL_Engine_GetTypeInfoByDecl(const char *decl) {
    return engine->GetTypeInfoByDecl(decl);
}

asIScriptContext * _stdcall TL_Engine_CreateContext() {
    return engine->CreateContext();
}

void * _stdcall TL_Engine_CreateScriptObject(const asITypeInfo *type) {
    return engine->CreateScriptObject(type);
}

asIScriptFunction * _stdcall TL_Engine_CreateDelegate(asIScriptFunction *func, void *obj) {
    return engine->CreateDelegate(func, obj);
}

asIScriptModule * __stdcall TL_Engine_GetModule(const char *module, const asEGMFlags flag) {
    return engine->GetModule(module, flag);
}

int __stdcall TL_Engine_DiscardModule(const char *module) {
    return engine->DiscardModule(module);
}

int TL_TypeInfo_GetTypeId(const asITypeInfo *type) {
    return type->GetTypeId();
}

const char *TL_TypeInfo_GetName(const asITypeInfo *type) {
    return type->GetName();
}

int _stdcall TL_Context_Release(const asIScriptContext *context) {
    return context->Release();
}

int _stdcall TL_Context_Prepare(asIScriptContext *context, asIScriptFunction *function) {
    return context->Prepare(function);
}

int _stdcall TL_Context_Unprepare(asIScriptContext *context) {
    return context->Unprepare();
}

int _stdcall TL_Context_Execute(asIScriptContext *context) {
    return context->Execute();
}

int _stdcall TL_Context_SetArgByte(asIScriptContext *context, const asUINT arg, const asBYTE value) {
    return context->SetArgByte(arg, value);
}

int _stdcall TL_Context_SetArgWord(asIScriptContext *context, const asUINT arg, const asWORD value) {
    return context->SetArgWord(arg, value);
}

int _stdcall TL_Context_SetArgDWord(asIScriptContext *context, const asUINT arg, const asDWORD value) {
    return context->SetArgDWord(arg, value);
}

int _stdcall TL_Context_SetArgQWord(asIScriptContext *context, const asUINT arg, const asQWORD value) {
    return context->SetArgQWord(arg, value);
}

int _stdcall TL_Context_SetArgFloat(asIScriptContext *context, const asUINT arg, const float value) {
    return context->SetArgFloat(arg, value);
}

int _stdcall TL_Context_SetArgDouble(asIScriptContext *context, const asUINT arg, const double value) {
    return context->SetArgDouble(arg, value);
}

int _stdcall TL_Context_SetArgAddress(asIScriptContext *context, const asUINT arg, void *addr) {
    return context->SetArgAddress(arg, addr);
}

int _stdcall TL_Context_SetArgObject(asIScriptContext *context, const asUINT arg, void *obj) {
    return context->SetArgObject(arg, obj);
}

int _stdcall TL_Context_SetArgVarType(asIScriptContext *context, const asUINT arg, void *ptr, const int typeId) {
    return context->SetArgVarType(arg, ptr, typeId);
}

asBYTE _stdcall TL_Context_GetReturnByte(asIScriptContext *context) {
    return context->GetReturnByte();
}

asWORD _stdcall TL_Context_GetReturnWord(asIScriptContext *context) {
    return context->GetReturnWord();
}

asDWORD _stdcall TL_Context_GetReturnDWord(asIScriptContext *context) {
    return context->GetReturnDWord();
}

asQWORD _stdcall TL_Context_GetReturnQWord(asIScriptContext *context) {
    return context->GetReturnQWord();
}

float _stdcall TL_Context_GetReturnFloat(asIScriptContext *context) {
    return context->GetReturnFloat();
}

double _stdcall TL_Context_GetReturnDouble(asIScriptContext *context) {
    return context->GetReturnDouble();
}

void * _stdcall TL_Context_GetReturnAddress(asIScriptContext *context) {
    return context->GetReturnAddress();
}

void * _stdcall TL_Context_GetReturnObject(asIScriptContext *context) {
    return context->GetReturnObject();
}

void * _stdcall TL_Context_GetAddressOfReturnValue(asIScriptContext *context) {
    return context->GetAddressOfReturnValue();
}

void * __stdcall TL_Context_SetUserData(asIScriptContext *context, void *data, const asPWORD type) {
    return context->SetUserData(data, type);
}

void * __stdcall TL_Context_GetUserData(const asIScriptContext *context, const asPWORD type) {
    return context->GetUserData(type);
}

const char * __stdcall TL_Function_GetDeclaration(const asIScriptFunction *function, const bool includeObjectName,
                                                  const bool includeNamespace, const bool includeParamNames) {
    return function->GetDeclaration(includeObjectName, includeNamespace, includeParamNames);
}

void * __stdcall TL_Module_SetUserData(asIScriptModule *module, void *data, const asPWORD type) {
    return module->SetUserData(data, type);
}

void * __stdcall TL_Module_GetUserData(const asIScriptModule *module, const asPWORD type) {
    return module->GetUserData(type);
}

int __stdcall TL_Module_AddScriptSection(asIScriptModule *module, const char *name, const char *code,
                                         const size_t codeLength,
                                         const int lineOffset) {
    return module->AddScriptSection(name, code, codeLength, lineOffset);
}

int __stdcall TL_Module_Build(asIScriptModule *module) {
    return module->Build();
}

asUINT __stdcall TL_Module_GetFunctionCount(const asIScriptModule *module) {
    return module->GetFunctionCount();
}

asIScriptFunction * __stdcall TL_Module_GetFunctionByDecl(const asIScriptModule *module, const char *decl) {
    return module->GetFunctionByDecl(decl);
}
