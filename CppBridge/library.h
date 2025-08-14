#ifndef UERMTESTLIB_LIBRARY_H
#define UERMTESTLIB_LIBRARY_H
#include <string>

#include "angelscript.h"

struct BBStr : std::string {
    BBStr *next;
    BBStr *prev;
};

extern "C" __declspec(dllexport) bool _stdcall TL_Tool_Call_Library(const char *libraryName, const char *procName);

extern "C" __declspec(dllexport) BBStr * _stdcall TL_Tool_Get_String(const char *str);

extern "C" __declspec(dllexport) void _stdcall TL_Tool_Return_String(const BBStr *string);

extern "C" __declspec(dllexport) asIScriptContext * _stdcall TL_Engine_CreateContext();

extern "C" __declspec(dllexport) asIScriptFunction * __stdcall TL_Engine_GetGlobalFunctionByDecl(
    const char *declaration);

extern "C" __declspec(dllexport) int __stdcall TL_Context_Release(const asIScriptContext *context);

extern "C" __declspec(dllexport) int __stdcall TL_Context_Prepare(
    asIScriptContext *context,
    asIScriptFunction *function
);

extern "C" __declspec(dllexport) int _stdcall TL_Context_Unprepare(asIScriptContext *context);

extern "C" __declspec(dllexport) int _stdcall TL_Context_Execute(asIScriptContext *context);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgByte(
    asIScriptContext *context,
    asUINT arg,
    asBYTE value
);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgWord(
    asIScriptContext *context,
    asUINT arg,
    asWORD value
);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgDWord(
    asIScriptContext *context,
    asUINT arg,
    asDWORD value
);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgQWord(
    asIScriptContext *context,
    asUINT arg,
    asQWORD value
);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgFloat(
    asIScriptContext *context,
    asUINT arg,
    float value
);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgDouble(
    asIScriptContext *context,
    asUINT arg,
    double value
);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgAddress(
    asIScriptContext *context,
    asUINT arg,
    void *addr
);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgObject(
    asIScriptContext *context,
    asUINT arg,
    void *obj
);

extern "C" __declspec(dllexport) int __stdcall TL_Context_SetArgVarType(
    asIScriptContext *context,
    asUINT arg,
    void *ptr,
    int typeId
);

extern "C" __declspec(dllexport) asBYTE __stdcall TL_Context_GetReturnByte(
    asIScriptContext *context
);

extern "C" __declspec(dllexport) asWORD __stdcall TL_Context_GetReturnWord(
    asIScriptContext *context
);

extern "C" __declspec(dllexport) asDWORD __stdcall TL_Context_GetReturnDWord(
    asIScriptContext *context
);

extern "C" __declspec(dllexport) asQWORD __stdcall TL_Context_GetReturnQWord(
    asIScriptContext *context
);

extern "C" __declspec(dllexport) float __stdcall TL_Context_GetReturnFloat(
    asIScriptContext *context
);

extern "C" __declspec(dllexport) double __stdcall TL_Context_GetReturnDouble(
    asIScriptContext *context
);

extern "C" __declspec(dllexport) void * __stdcall TL_Context_GetReturnAddress(
    asIScriptContext *context
);

extern "C" __declspec(dllexport) void * __stdcall TL_Context_GetReturnObject(
    asIScriptContext *context
);

extern "C" __declspec(dllexport) void * __stdcall TL_Context_GetAddressOfReturnValue(
    asIScriptContext *context
);

#endif //UERMTESTLIB_LIBRARY_H
