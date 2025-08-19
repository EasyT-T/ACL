namespace ACL.Managed.ScriptObject;

using System.Runtime.InteropServices;
using ACL.Managed;
using ACL.Private;
using ACL.SourceGenerators;

[ScriptClass("Server")]
public partial class ManagedServer : ScriptObjectBase
{
    public static ManagedServer Instance { get; } =
        new ManagedServer(ScriptEngine.GetGlobalProperty("Server server").Handle);

    internal ManagedServer(IntPtr handle) : base(handle)
    {
    }

    public IntPtr CfgValue => this.Unmanaged.CFGValue;

    public int Port => this.Unmanaged.Port;
    public string HostName => new ManagedString(this.Unmanaged.HostName).ToString();

    public int CorpseAliveTime => this.Unmanaged.CorpseAliveTime;

    public int Timeout => this.Unmanaged.Timeout;

    public bool EnabledChat => this.Unmanaged.Chat;

    public bool EnabledConsole => this.Unmanaged.Console;

    public int VoiceBitrate => this.Unmanaged.VoiceBitrate;

    public int MaxPlayers => this.Unmanaged.MaxPlayers;

    public string MapSeed => new ManagedString(this.Unmanaged.MapSeed).ToString();

    public string AdminPassword => new ManagedString(this.Unmanaged.AdminPassword).ToString();

    public int Difficulty => this.Unmanaged.Difficulty;

    public int GameMode => this.Unmanaged.GameMode;

    public int EmptyBehaviour => this.Unmanaged.EmptyBehaviour;

    public string LogFile => new ManagedString(this.Unmanaged.LogFile).ToString();

    public bool ScriptAutoLoad => this.Unmanaged.ScriptAutoLoad;

    public bool DisableNpc => this.Unmanaged.DisableNPCs;

    public float ProxPlayers => this.Unmanaged.ProxPlayers;

    public float MapBounds => this.Unmanaged.MapBounds;

    public int RespawnTime => this.Unmanaged.RespawnTime;

    public string ContentUrl => new ManagedString(this.Unmanaged.ContentUrl).ToString();

    public string Password => new ManagedString(this.Unmanaged.Password).ToString();

    public bool ImprovedGates => this.Unmanaged.ImprovedGates;

    public int MapSize => this.Unmanaged.MapSize;

    public int Advertise => this.Unmanaged.Advertise;

    public bool AllowJump => this.Unmanaged.AllowJump;

    public string Description => new ManagedString(this.Unmanaged.Description).ToString();

    public bool FastSlots => this.Unmanaged.FastSlots;

    public float Gravity => this.Unmanaged.Gravity;

    private UnmanagedServer Unmanaged => Marshal.PtrToStructure<UnmanagedServer>(Marshal.ReadIntPtr(Marshal.ReadIntPtr(this.Handle)));

    [ScriptFunction("void Restart()")]
    public partial void Restart();

    [ScriptFunction("void Console()")]
    public partial void Console(string command);

    [ScriptFunction("string& GetVersion()")]
    public partial string GetVersion();

    [ScriptFunction("int GetUPS()")]
    public partial int GetUps();
}