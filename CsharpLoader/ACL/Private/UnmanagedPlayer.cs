namespace ACL.Private;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct UnmanagedPlayer
{
    public int IP;
    public int Port;
    public int Version;
    public int ItemCount;
    public IntPtr MP_PlayerServerLogic;
    public int ScreenWidth;
    public int ScreenHeight;
    public IntPtr Language;
    public IntPtr HardwareId;
    public int PingCollector;
    public int PingCollection;
    public bool IsBot;
    public bool RedirectedMove;
    public int Index;
    public IntPtr Name;
    public float X;
    public float Y;
    public float Z;
    public float Pitch;
    public float Yaw;
    public float SmoothedPitch;
    public float SmoothedYaw;
    public int AnimationIndex;
    public float AnimationInactive;
    public int ServerUpdateHash;
    public int UpdateHash;
    public int PlayerPivot;
    public int PlayerHead;
    public int PlayerWeaponHead;
    public int Hitbox;
    public bool Synced;
    public int PackedData;
    public int RoomIndex;
    public float Velocity;
    public float VolumeAdjacency;
    public float TargetVolumeAdjacency;
    public int Ping;
    public int NameColor;
    public int SelectedItem;
    public bool IsCrouch;
    public float SoundVolume;
    public bool IsBlinking;
    public bool IsInvisible;
    public bool IsLeaning;
    public int ShootsCount;
    public IntPtr PacketsCount;
    public IntPtr PacketFlood;
    public IntPtr Tags;
    public IntPtr PlayerModel;
    public IntPtr Voice;
    public IntPtr Weapon;
    public IntPtr Steam;
}