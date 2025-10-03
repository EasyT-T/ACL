namespace ACL.Private;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
internal ref struct AngelObject
{
    public IntPtr FieldsPointer;

    public unsafe IntPtr GetPointer()
    {
        return (IntPtr)Unsafe.AsPointer(ref this);
    }
}