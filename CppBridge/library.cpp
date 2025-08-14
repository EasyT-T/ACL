#include "library.h"

#include <ostream>
#include <vector>
#include <Windows.h>

static bool initialized = false;

namespace {
    HMODULE mainModule;
    asIScriptEngine *engine;
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

    const bool result = TL_Tool_Call_Library("erf.net.dll", "CreateLoader");

    initialized = result;

    return initialized;
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
