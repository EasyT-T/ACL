namespace ACL.Private;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
internal struct AngelObject
{
    public IntPtr FieldsPointer;

    public PointerReference GetPointer()
    {
        return new PointerReference(this);
    }
}