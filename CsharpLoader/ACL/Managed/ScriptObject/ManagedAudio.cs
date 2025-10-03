namespace ACL.Managed.ScriptObject;

using ACL.Private;
using ACL.SourceGenerators;

[ScriptClass("Audio")]
public partial class ManagedAudio : ScriptObjectBase
{
    public static unsafe ManagedAudio Instance { get; } =
        new ManagedAudio((AngelObject*)ScriptEngine.GetGlobalProperty("Audio audio").Handle);

    internal unsafe ManagedAudio(AngelObject* handle) : base((IntPtr)handle)
    {
    }

    [ScriptFunction("void Play3DSound(string &in filename, Player player, float range, float volume, bool norange=false)")]
    public partial void Play3DSound(string filename, ManagedPlayer player, float range, float volume, bool norange = false);

    [ScriptFunction("void Play3DSound(string &in filename, Entity entity, float range, float volume, bool norange=false)")]
    public partial void Play3DSound(string filename, ManagedEntity entity, float range, float volume, bool norange = false);

    [ScriptFunction("void Play3DSound(string &in filename, float x, float y, float z, float range, float volume, bool norange=false)")]
    public partial void Play3DSound(string filename, float x, float y, float z, float range, float volume, bool norange = false);

    [ScriptFunction("void PlaySound(string &in filename)")]
    public partial void PlaySound(string filename);

    [ScriptFunction("void PlaySoundForPlayer(Player player, string &in filename)")]
    public partial void PlaySoundForPlayer(ManagedPlayer player, string filename);

    [ScriptFunction("void Play3DSoundForPlayer(Player player, string &in filename, Entity entity, float range, float volume, bool norange=false)")]
    public partial void Play3DSoundForPlayer(ManagedPlayer targetPlayer, string filename, ManagedEntity entity, float range, float volume, bool norange = false);

    [ScriptFunction("void Play3DSoundForPlayer(Player player, string &in filename, float x, float y, float z, float range, float volume, bool norange=false)")]
    public partial void Play3DSoundForPlayer(ManagedPlayer targetPlayer, string filename, float x, float y, float z, float range, float volume, bool norange = false);

    [ScriptFunction("void Play3DSoundForPlayer(Player player, string &in filename, Player sourcePlayer, float range, float volume, bool norange=false)")]
    public partial void Play3DSoundForPlayer(ManagedPlayer targetPlayer, string filename, ManagedPlayer sourcePlayer, float range, float volume, bool norange = false);
}