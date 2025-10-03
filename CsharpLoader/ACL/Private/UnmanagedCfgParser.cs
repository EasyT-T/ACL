namespace ACL.Private;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct UnmanagedCfgParser
{
    public IntPtr Value;
}