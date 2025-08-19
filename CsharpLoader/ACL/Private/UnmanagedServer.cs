namespace ACL.Private;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public struct UnmanagedServer
{
    [FieldOffset(0)] public IntPtr CFGValue;

    [FieldOffset(4)] public int Port;

    [FieldOffset(8)] public IntPtr HostName;

    [FieldOffset(12)] public int CorpseAliveTime;

    [FieldOffset(16)] public int Timeout;

    [FieldOffset(20)] [MarshalAs(UnmanagedType.Bool)]
    public bool Chat;

    [FieldOffset(24)] [MarshalAs(UnmanagedType.Bool)]
    public bool Console;

    [FieldOffset(28)] public int VoiceBitrate;

    [FieldOffset(32)] public int MaxPlayers;

    [FieldOffset(36)] public IntPtr MapSeed;

    [FieldOffset(40)] public IntPtr AdminPassword;

    [FieldOffset(44)] public int Difficulty;

    [FieldOffset(48)] public int GameMode;

    [FieldOffset(52)] public int EmptyBehaviour;

    [FieldOffset(56)] public IntPtr LogFile;

    [FieldOffset(60)] [MarshalAs(UnmanagedType.Bool)]
    public bool ScriptAutoLoad;

    [FieldOffset(64)] [MarshalAs(UnmanagedType.Bool)]
    public bool DisableNPCs;

    [FieldOffset(68)] public float ProxPlayers;

    [FieldOffset(72)] public float MapBounds;

    [FieldOffset(76)] public int RespawnTime;

    [FieldOffset(80)] public IntPtr ContentUrl;

    [FieldOffset(84)] public IntPtr Password;

    [FieldOffset(88)] [MarshalAs(UnmanagedType.Bool)]
    public bool ImprovedGates;

    [FieldOffset(92)] public int MapSize;

    [FieldOffset(96)] public int Advertise;

    [FieldOffset(100)] [MarshalAs(UnmanagedType.Bool)]
    public bool AllowJump;

    [FieldOffset(104)] public IntPtr Description;

    [FieldOffset(108)] [MarshalAs(UnmanagedType.Bool)]
    public bool FastSlots;

    [FieldOffset(112)] public float Gravity;
}