#include "library.h"

#include <ostream>
#include <vector>
#include <Windows.h>

#include <coreclr_delegates.h>
#include <hostfxr.h>
#include <iostream>
#include <nethost.h>

static bool initialized = false;

namespace {
    HMODULE mainModule;
    asIScriptEngine *engine;

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

BBStr * __stdcall TL_Tool_Get_String(const char *str) {
    return new BBStr(str);
}

void __stdcall TL_Tool_Return_String(const BBStr *string) {
    delete string;
}

asIScriptContext * _stdcall TL_Engine_CreateContext() {
    return engine->CreateContext();
}

asIScriptFunction * __stdcall TL_Engine_GetGlobalFunctionByDecl(const char *declaration) {
    return engine->GetGlobalFunctionByDecl(declaration);
}

int __stdcall TL_Context_Release(const asIScriptContext *context) {
    return context->Release();
}

int _stdcall TL_Context_Prepare(asIScriptContext *context, asIScriptFunction *function) {
    return context->Prepare(function);
}

int __stdcall TL_Context_Unprepare(asIScriptContext *context) {
    return context->Unprepare();
}

int __stdcall TL_Context_Execute(asIScriptContext *context) {
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
