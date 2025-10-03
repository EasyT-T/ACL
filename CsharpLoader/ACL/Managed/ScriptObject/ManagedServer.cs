namespace ACL.Managed.ScriptObject;

using ACL.Managed;
using ACL.Private;
using ACL.SourceGenerators;

[ScriptClass("Server")]
public partial class ManagedServer : ScriptObjectBase
{
    public static unsafe ManagedServer Instance { get; } =
        new ManagedServer((AngelObject*)ScriptEngine.GetGlobalProperty("Server server").Handle);

    internal unsafe ManagedServer(AngelObject* handle) : base((IntPtr)handle)
    {
        var handle2 = (AngelObject**)handle;
        this.unmanaged = (UnmanagedServer*)(*handle2)->FieldsPointer;
    }

    public unsafe ManagedCfgParser CfgValue
    {
        get => new ManagedCfgParser((AngelObject*)this.unmanaged->CFGValue);
        set => this.unmanaged->CFGValue = value.Handle;
    }

    public unsafe int Port
    {
        get => this.unmanaged->Port;
        set => this.unmanaged->Port = value;
    }

    public unsafe string HostName
    {
        get => new ManagedString(this.unmanaged->HostName);
        set
        {
            new ManagedString(this.unmanaged->HostName, true).Dispose();
            this.unmanaged->HostName = ManagedString.Create(value).Handle;
        }
    }

    public unsafe int CorpseAliveTime
    {
        get => this.unmanaged->CorpseAliveTime;
        set => this.unmanaged->CorpseAliveTime = value;
    }

    public unsafe int Timeout
    {
        get => this.unmanaged->Timeout;
        set => this.unmanaged->Timeout = value;
    }

    public unsafe bool EnabledChat
    {
        get => this.unmanaged->Chat == 1;
        set => this.unmanaged->Chat = value ? 1 : 0;
    }

    public unsafe bool EnabledConsole
    {
        get => this.unmanaged->Console == 1;
        set => this.unmanaged->Console = value ? 1 : 0;
    }

    public unsafe int VoiceBitrate
    {
        get => this.unmanaged->VoiceBitrate;
        set => this.unmanaged->VoiceBitrate = value;
    }

    public unsafe int MaxPlayers
    {
        get => this.unmanaged->MaxPlayers;
        set => this.unmanaged->MaxPlayers = value;
    }

    public unsafe string MapSeed
    {
        get => new ManagedString(this.unmanaged->MapSeed);
        set
        {
            new ManagedString(this.unmanaged->MapSeed, true).Dispose();
            this.unmanaged->MapSeed = ManagedString.Create(value).Handle;
        }
    }

    public unsafe string AdminPassword
    {
        get => new ManagedString(this.unmanaged->AdminPassword);
        set
        {
            new ManagedString(this.unmanaged->AdminPassword, true).Dispose();
            this.unmanaged->AdminPassword = ManagedString.Create(value).Handle;
        }
    }

    public unsafe int Difficulty
    {
        get => this.unmanaged->Difficulty;
        set => this.unmanaged->Difficulty = value;
    }

    public unsafe string GameMode
    {
        get => new ManagedString(this.unmanaged->GameMode);
        set
        {
            new ManagedString(this.unmanaged->GameMode, true).Dispose();
            this.unmanaged->GameMode = ManagedString.Create(value).Handle;
        }
    }

    public unsafe int EmptyBehaviour
    {
        get => this.unmanaged->EmptyBehaviour;
        set => this.unmanaged->EmptyBehaviour = value;
    }

    public unsafe string LogFile
    {
        get => new ManagedString(this.unmanaged->LogFile);
        set
        {
            new ManagedString(this.unmanaged->LogFile, true).Dispose();
            this.unmanaged->LogFile = ManagedString.Create(value).Handle;
        }
    }

    public unsafe bool ScriptAutoLoad
    {
        get => this.unmanaged->ScriptAutoLoad == 1;
        set => this.unmanaged->ScriptAutoLoad = value ? 1 : 0;
    }

    public unsafe bool DisableNps
    {
        get => this.unmanaged->DisableNPCs == 1;
        set => this.unmanaged->DisableNPCs = value ? 1 : 0;
    }

    public unsafe float ProxPlayers
    {
        get => this.unmanaged->ProxPlayers;
        set => this.unmanaged->ProxPlayers = value;
    }

    public unsafe float MapBounds
    {
        get => this.unmanaged->MapBounds;
        set => this.unmanaged->MapBounds = value;
    }

    public unsafe int RespawnTime
    {
        get => this.unmanaged->RespawnTime;
        set => this.unmanaged->RespawnTime = value;
    }

    public unsafe string ContentUrl
    {
        get => new ManagedString(this.unmanaged->ContentUrl);
        set
        {
            new ManagedString(this.unmanaged->ContentUrl, true).Dispose();
            this.unmanaged->ContentUrl = ManagedString.Create(value).Handle;
        }
    }

    public unsafe string Password
    {
        get => new ManagedString(this.unmanaged->Password);
        set
        {
            new ManagedString(this.unmanaged->Password, true).Dispose();
            this.unmanaged->Password = ManagedString.Create(value).Handle;
        }
    }

    public unsafe bool ImprovedGates
    {
        get => this.unmanaged->ImprovedGates == 1;
        set => this.unmanaged->ImprovedGates = value ? 1 : 0;
    }

    public unsafe int MapSize
    {
        get => this.unmanaged->MapSize;
        set => this.unmanaged->MapSize = value;
    }

    public unsafe int Advertise
    {
        get => this.unmanaged->Advertise;
        set => this.unmanaged->Advertise = value;
    }

    public unsafe bool AllowJump
    {
        get => this.unmanaged->AllowJump == 1;
        set => this.unmanaged->AllowJump = value ? 1 : 0;
    }

    public unsafe string Description
    {
        get => new ManagedString(this.unmanaged->Description);
        set
        {
            new ManagedString(this.unmanaged->Description, true).Dispose();
            this.unmanaged->Description = ManagedString.Create(value).Handle;
        }
    }

    public unsafe bool FastSlots
    {
        get => this.unmanaged->FastSlots == 1;
        set => this.unmanaged->FastSlots = value ? 1 : 0;
    }

    public unsafe float Gravity
    {
        get => this.unmanaged->Gravity;
        set => this.unmanaged->Gravity = value;
    }

    private readonly unsafe UnmanagedServer* unmanaged;

    [ScriptFunction("void Restart()")]
    public partial void Restart();

    [ScriptFunction("void Console()")]
    public partial void Console(string command);

    [ScriptFunction("string& GetVersion()")]
    public partial string GetVersion();

    [ScriptFunction("int GetUPS()")]
    public partial int GetUps();
}