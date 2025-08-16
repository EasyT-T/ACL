#ifndef UERMTESTLIB_LIBRARY_H
#define UERMTESTLIB_LIBRARY_H
#include <string>

#include "angelscript.h"
#include "as_module.h"
#include "as_scriptfunction.h"

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

    ModuleContext(asIScriptModule *module, asIScriptContext *context) : module(module), context(context) {
    }
};

extern "C" _declspec(dllexport) bool _stdcall TL_Load_Plugins();

extern "C" _declspec(dllexport) bool _stdcall TL_Tool_Call_Library(const char *libraryName, const char *procName);

extern "C" _declspec(dllexport) BBStr * _stdcall TL_Tool_Get_String(const char *str);

extern "C" _declspec(dllexport) void _stdcall TL_Tool_Return_String(const BBStr *string);

extern "C" _declspec(dllexport) asSFuncPtr * _stdcall TL_Tool_Get_FuncPtr(void *func);

extern "C" _declspec(dllexport) void _stdcall TL_Tool_Return_FuncPtr(const asSFuncPtr *funcPtr);

extern "C" _declspec(dllexport) ModuleContext * _stdcall TL_Tool_Get_ModuleContext(
    asIScriptModule *module, asIScriptContext *context);

extern "C" _declspec(dllexport) void _stdcall TL_Tool_Return_ModuleContext(const ModuleContext *moduleContext);

extern "C" _declspec(dllexport) asCScriptFunction * _stdcall TL_Tool_RegisterFunctionToModule(
    asCModule *module, const char *declaration,
    const asSFuncPtr *funcPointer, asDWORD callConv,
    void *auxiliary);

extern "C" _declspec(dllexport) asIScriptEngine * _stdcall TL_Engine_GetEngine();

extern "C" _declspec(dllexport) int _stdcall TL_Engine_RegisterGlobalFunction(
    const char *declaration, const asSFuncPtr *funcPointer, asDWORD callConv, void *auxiliary = nullptr);

extern "C" _declspec(dllexport) asUINT _stdcall TL_Engine_GetGlobalFunctionCount();

extern "C" _declspec(dllexport) asIScriptFunction * _stdcall TL_Engine_GetGlobalFunctionByIndex(asUINT index);

extern "C" _declspec(dllexport) asIScriptFunction * _stdcall TL_Engine_GetGlobalFunctionByDecl(
    const char *declaration);

extern "C" _declspec(dllexport) asUINT _stdcall TL_Engine_GetFuncdefCount();

extern "C" _declspec(dllexport) asITypeInfo * _stdcall TL_Engine_GetFuncdefByIndex(asUINT index);

extern "C" _declspec(dllexport) const char *TL_Engine_GetTypeDeclaration(int typeId, bool includeNamespace = false);

extern "C" _declspec(dllexport) asITypeInfo * _stdcall TL_Engine_GetTypeInfoByName(const char *name);

extern "C" _declspec(dllexport) asITypeInfo * _stdcall TL_Engine_GetTypeInfoByDecl(const char *decl);

extern "C" _declspec(dllexport) asIScriptContext * _stdcall TL_Engine_CreateContext();

extern "C" _declspec(dllexport) void * _stdcall TL_Engine_CreateScriptObject(const asITypeInfo *type);

extern "C" _declspec(dllexport) asIScriptFunction * _stdcall TL_Engine_CreateDelegate(
    asIScriptFunction *func, void *obj);

extern "C" _declspec(dllexport) asIScriptModule * _stdcall TL_Engine_GetModule(
    const char *module, asEGMFlags flag = asGM_ONLY_IF_EXISTS);

extern "C" _declspec(dllexport) int _stdcall TL_Engine_DiscardModule(const char *module);

extern "C" _declspec(dllexport) int TL_TypeInfo_GetTypeId(const asITypeInfo *type);

extern "C" _declspec(dllexport) const char *TL_TypeInfo_GetName(const asITypeInfo *type);

extern "C" _declspec(dllexport) int _stdcall TL_Context_Release(const asIScriptContext *context);

extern "C" _declspec(dllexport) int _stdcall TL_Context_Prepare(
    asIScriptContext *context,
    asIScriptFunction *function
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_Unprepare(asIScriptContext *context);

extern "C" _declspec(dllexport) int _stdcall TL_Context_Execute(asIScriptContext *context);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgByte(
    asIScriptContext *context,
    asUINT arg,
    asBYTE value
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgWord(
    asIScriptContext *context,
    asUINT arg,
    asWORD value
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgDWord(
    asIScriptContext *context,
    asUINT arg,
    asDWORD value
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgQWord(
    asIScriptContext *context,
    asUINT arg,
    asQWORD value
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgFloat(
    asIScriptContext *context,
    asUINT arg,
    float value
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgDouble(
    asIScriptContext *context,
    asUINT arg,
    double value
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgAddress(
    asIScriptContext *context,
    asUINT arg,
    void *addr
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgObject(
    asIScriptContext *context,
    asUINT arg,
    void *obj
);

extern "C" _declspec(dllexport) int _stdcall TL_Context_SetArgVarType(
    asIScriptContext *context,
    asUINT arg,
    void *ptr,
    int typeId
);

extern "C" _declspec(dllexport) asBYTE _stdcall TL_Context_GetReturnByte(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) asWORD _stdcall TL_Context_GetReturnWord(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) asDWORD _stdcall TL_Context_GetReturnDWord(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) asQWORD _stdcall TL_Context_GetReturnQWord(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) float _stdcall TL_Context_GetReturnFloat(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) double _stdcall TL_Context_GetReturnDouble(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) void * _stdcall TL_Context_GetReturnAddress(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) void * _stdcall TL_Context_GetReturnObject(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) void * _stdcall TL_Context_GetAddressOfReturnValue(
    asIScriptContext *context
);

extern "C" _declspec(dllexport) void * _stdcall TL_Context_SetUserData(asIScriptContext *context, void *data,
                                                                       asPWORD type = 0);

extern "C" _declspec(dllexport) void * _stdcall TL_Context_GetUserData(const asIScriptContext *context,
                                                                       asPWORD type = 0);

extern "C" _declspec(dllexport) const char * _stdcall TL_Function_GetDeclaration(
    const asIScriptFunction *function, bool includeObjectName = true, bool includeNamespace = false,
    bool includeParamNames = false);

extern "C" _declspec(dllexport) void * _stdcall TL_Module_SetUserData(asIScriptModule *module, void *data,
                                                                      asPWORD type = 0);

extern "C" _declspec(dllexport) void * _stdcall TL_Module_GetUserData(const asIScriptModule *module, asPWORD type = 0);

extern "C" _declspec(dllexport) int _stdcall TL_Module_AddScriptSection(asIScriptModule *module, const char *name,
                                                                        const char *code,
                                                                        size_t codeLength = 0, int lineOffset = 0);

extern "C" _declspec(dllexport) int _stdcall TL_Module_Build(asIScriptModule *module);

extern "C" _declspec(dllexport) asUINT _stdcall TL_Module_GetFunctionCount(const asIScriptModule *module);

extern "C" _declspec(dllexport) asIScriptFunction * _stdcall TL_Module_GetFunctionByDecl(
    const asIScriptModule *module, const char *decl);

#endif //UERMTESTLIB_LIBRARY_H
