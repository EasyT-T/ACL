namespace ERF.Managed;

using System.Runtime.InteropServices;
using ERF.Private;

public class ScriptFunction
{
    internal ScriptFunction(IntPtr handle)
    {
        this.Handle = handle;
    }

    internal IntPtr Handle { get; }

    public string GetDeclaration(bool includeObjectName = true, bool includeNamespace = false,
        bool includeParamNames = false)
    {
        var strPtr = NativeBindings.TL_Function_GetDeclaration(this.Handle, includeObjectName, includeNamespace,
            includeParamNames);

        return Marshal.PtrToStringUTF8(strPtr) ?? string.Empty;
    }
}