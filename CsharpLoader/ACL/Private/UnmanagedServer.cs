namespace ACL.Private;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct UnmanagedServer
{
    public IntPtr CFGValue;

    public int Port;

    public IntPtr HostName;

    public int CorpseAliveTime;

    public int Timeout;

    public int Chat;

    public int Console;

    public int VoiceBitrate;

    public int MaxPlayers;

    public IntPtr MapSeed;

    public IntPtr AdminPassword;

    public int Difficulty;

    public IntPtr GameMode;

    public int EmptyBehaviour;

    public IntPtr LogFile;

    public int ScriptAutoLoad;

    public int DisableNPCs;

    public float ProxPlayers;

    public float MapBounds;

    public int RespawnTime;

    public IntPtr ContentUrl;

    public IntPtr Password;

    public int ImprovedGates;

    public int MapSize;

    public int Advertise;

    public int AllowJump;

    public IntPtr Description;

    public int FastSlots;

    public float Gravity;
}