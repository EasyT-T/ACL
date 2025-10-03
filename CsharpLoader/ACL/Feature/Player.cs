namespace ACL.Feature;

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using ACL.Extension;
using ACL.Managed;
using ManagedPlayer = ACL.Managed.ScriptObject.ManagedPlayer;

public class Player
{
    public static IEnumerable<Player> List => PlayerCache.Values;

    private static readonly Dictionary<ManagedPlayer, Player> PlayerCache = new Dictionary<ManagedPlayer, Player>();

    private bool isInvisible;

    internal Player(ManagedPlayer basePlayer)
    {
        this.Base = basePlayer;
    }

    public ManagedPlayer Base { get; }

    public string Name
    {
        get => this.Base.GetName();
        set => this.Base.SetName(value);
    }

    public string SteamId
    {
        get => this.Base.GetSteamId();
        set => this.Base.SetSteamId(value);
    }

    public int Ping => this.Base.GetPing();

    public Vector2 ScreenSize
    {
        get
        {
            this.Base.GetScreenSize(out var width, out var height);
            return new Vector2(width, height);
        }
    }

    public string Language => this.Base.GetLanguage();

    public string Ip => this.Base.GetIp();

    public string HardwareId => this.Base.GetHwid();

    public int Id => this.Base.GetIndex();

    public string GameVersion => this.Base.GetVersion();

    public bool IsInvisible
    {
        get => this.isInvisible;
        set
        {
            this.Base.SetInvisible(value);
            this.isInvisible = value;
        }
    }

    public string DialogData
    {
        get => this.Base.GetDialogData();
        set => this.Base.SetDialogData(value);
    }

    public bool IsDesync
    {
        get => this.Base.IsDesync();
        set => this.Base.Desync(value);
    }

    public float Injuries
    {
        get => this.Base.GetInjuries();
        set => this.Base.SetInjuries(value);
    }

    public float Bloodloss
    {
        get => this.Base.GetBloodloss();
        set => this.Base.SetBloodloss(value);
    }

    public bool IsGodmode
    {
        get => this.Base.GetGodmode();
        set => this.Base.SetGodmode(value);
    }

    public Color NameListColor
    {
        get => this.Base.GetColor().ToColor();
        set => this.Base.SetColor(value.ToHex());
    }

    public Vector3 Position
    {
        get
        {
            this.Base.GetNetworkPosition(out var x, out var y, out var z);
            return new Vector3(x, y, z);
        }
        set => this.Base.SetNetworkPosition(value.X, value.Y, value.Z);
    }

    public Quaternion Rotation
    {
        get
        {
            this.Base.GetNetworkRotation(out var pitch, out var yaw);
            return new Quaternion(pitch, yaw, 0.0f, 1.0f);
        }
        set => this.Base.SetNetworkRotation(value.X * value.W, value.Y * value.W);
    }

    public bool IsAdmin
    {
        get => this.Base.IsAdmin();
        set => this.Base.SetAdmin(value);
    }

    public bool IsGlobalTransmission
    {
        get => this.Base.IsGlobalTransmission();
        set => this.Base.SetGlobalTransmission(value);
    }

    public float BlinkTimer
    {
        get => this.Base.GetBlinkTimer();
        set => this.Base.SetBlinkTimer(value);
    }

    public int RadioChannel => this.Base.GetRadio();

    public bool IsBlinking => this.Base.IsBlinking();

    public float SpeedMultiplier
    {
        get => this.Base.GetSpeedMultiplier();
        set => this.Base.SetSpeedMultiplier(value);
    }

    public float StaminaMultiplier
    {
        get => this.Base.GetStaminaMultiplier();
        set => this.Base.SetStaminaMultiplier(value);
    }

    public ModelType ModelType
    {
        get
        {
            var value = this.Base.GetModel();

            if (!Enum.IsDefined(typeof(ModelType), value))
            {
                return ModelType.Unknown;
            }

            return (ModelType)value;
        }
        set => this.Base.SetModel((int)value);
    }

    public float ScaleMultiplier
    {
        get => this.Base.GetModelSize();
        set => this.Base.SetModelSize(value);
    }

    public float SoundVolume => this.Base.GetVolume();

    public bool IsCrouching => this.Base.IsCrouch();

    public float Gravity
    {
        get => this.Base.GetGravity();
        set => this.Base.SetGravity(value);
    }

    public int ShootsCount
    {
        get => this.Base.GetShootsCount();
        set => this.Base.SetShootsCount(value);
    }

    public bool IsBot => this.Base.IsBot();

    public static Player Get(int id)
    {
        var (result, managedPlayer) = GlobalFunctions.GetPlayer(id);

        if (result != ScriptErrorType.AsSuccess || managedPlayer == null)
        {
            throw new ArgumentException("Invalid player id", nameof(id));
        }

        if (PlayerCache.TryGetValue(managedPlayer, out var player))
        {
            return player;
        }

        player = new Player(managedPlayer);

        PlayerCache.Add(managedPlayer, player);
        return player;
    }

    public static bool TryGet(int id, [NotNullWhen(true)] out Player? player)
    {
        var (result, managedPlayer) = GlobalFunctions.GetPlayer(id);

        if (result != ScriptErrorType.AsSuccess || managedPlayer == null)
        {
            player = null;
            return false;
        }

        if (PlayerCache.TryGetValue(managedPlayer, out player))
        {
            return true;
        }

        player = new Player(managedPlayer);

        PlayerCache.Add(managedPlayer, player);
        return false;
    }

    internal static Player Get(ManagedPlayer managedPlayer)
    {
        if (PlayerCache.TryGetValue(managedPlayer, out var player))
        {
            return player;
        }

        player = new Player(managedPlayer);
        PlayerCache.Add(managedPlayer, player);

        return player;
    }

    internal static void Register(EventManager eventManager)
    {
        // eventManager.PlayerDisconnect += OnPlayerDisconnect;
    }

    private static void OnPlayerDisconnect(Player player)
    {
        PlayerCache.Remove(player.Base);
    }

    public void SetPlayerTag(int index, PlayerTag tag)
    {
        this.Base.SetTagText(index, tag.Text);
        this.Base.SetTagScales(index, tag.Scale.X, tag.Scale.Y);
        this.Base.SetTagOffset(index, tag.Offset);
        this.Base.SetTagColors(index, tag.StartColor.ToHex(), tag.EndColor.ToHex());

        if (tag.FontName != null)
        {
            this.Base.SetTagFont(index, tag.FontName);
        }
    }

    public void Explode(bool thrust = false)
    {
        this.Base.Explode(thrust);
    }

    public void Kick()
    {
        this.Base.Kick();
    }

    public bool Kill(bool bloody = false, bool createcorpse = true)
    {
        return this.Base.Kill(bloody, createcorpse);
    }

    public bool Respawn()
    {
        return this.Base.Respawn();
    }
}