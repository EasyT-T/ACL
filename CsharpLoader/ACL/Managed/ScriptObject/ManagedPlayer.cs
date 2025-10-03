namespace ACL.Managed.ScriptObject;

using ACL.Managed;
using ACL.Private;
using ACL.SourceGenerators;

[ScriptClass("Player")]
public partial class ManagedPlayer : ScriptObjectBase
{
    private unsafe UnmanagedPlayer* unmanaged;

    internal unsafe ManagedPlayer(AngelObject* handle) : base((IntPtr)handle)
    {
        this.unmanaged = (UnmanagedPlayer*)handle->FieldsPointer;
    }

    #nullable disable

    [ScriptFunction("Entity GetHitbox()")]
    public partial ManagedEntity GetHitbox();

    [ScriptFunction("Entity GetHead()")]
    public partial ManagedEntity GetHead();

    [ScriptFunction("Entity GetEntity()")]
    public partial ManagedEntity GetEntity();

    [ScriptFunction("void GetScreenSize(int& out width, int& out height)")]
    public partial void GetScreenSize(out int width, out int height);

    [ScriptFunction("string& GetLanguage()")]
    public partial string GetLanguage();

    [ScriptFunction("string& GetIP()")]
    public partial string GetIp();

    [ScriptFunction("string& GetSteamID()")]
    public partial string GetSteamId();

    [ScriptFunction("string& GetHWID()")]
    public partial string GetHwid();

    [ScriptFunction("string& GetName()")]
    public partial string GetName();

    [ScriptFunction("void SetSteamID(string &in steamid64)")]
    public partial void SetSteamId(string steamid64);

    [ScriptFunction("void SetName(string &in name)")]
    public partial void SetName(string name);

    [ScriptFunction("int GetPing()")]
    public partial int GetPing();

    [ScriptFunction("int GetIndex()")]
    public partial int GetIndex();

    [ScriptFunction("string& GetVersion()")]
    public partial string GetVersion();

    [ScriptFunction("void SetInvisible(bool inv)")]
    public partial void SetInvisible(bool inv);

    [ScriptFunction("void SetLocalInvisible(Player player, bool inv)")]
    public partial void SetLocalInvisible(ManagedPlayer player, bool inv);

    [ScriptFunction("void Kick(int code = 0, string& in custom = \"\")")]
    public partial void Kick(int code = 0, string custom = "");

    [ScriptFunction(
        "void ShowDialog(int type, int index, string& in header, string& in message, string& in leftbutton, string& in rightbutton = \"\", bool align = true)")]
    public partial void ShowDialog(int type, int index, string header, string message, string leftbutton,
        string rightbutton = "", bool align = true);

    [ScriptFunction(
        "void ShowDialog(int type, DIALOGCALLBACK@ callback, string& in header, string& in message, string& in leftbutton, string& in rightbutton = \"\", bool align = true)")]
    public partial void ShowDialog(int type, Delegate callback, string header, string message, string leftbutton,
        string rightbutton = "", bool align = true);

    [ScriptFunction("void SetDialogData(string& in data)")]
    public partial void SetDialogData(string data);

    [ScriptFunction("string& GetDialogData()")]
    public partial string GetDialogData();

    [ScriptFunction("void HideDialog()")]
    public partial void HideDialog();

    [ScriptFunction("void SendMessage(string& in message, float time = 6.0f, bool localized = false)")]
    public partial void SendMessage(string message, float time = 6.0f, bool localized = false);

    [ScriptFunction("void Desync(bool value)")]
    public partial void Desync(bool value);

    [ScriptFunction("bool IsDesync()")]
    public partial bool IsDesync();

    [ScriptFunction("Player GetSpectatePlayer()")]
    public partial ManagedPlayer GetSpectatePlayer();

    [ScriptFunction("bool Kill(bool bloody = false, bool createcorpse = true)")]
    public partial bool Kill(bool bloody = false, bool createcorpse = true);

    [ScriptFunction("bool Respawn()")]
    public partial bool Respawn();

    [ScriptFunction("bool IsDead()")]
    public partial bool IsDead();

    [ScriptFunction("void SetInjuries(float val)")]
    public partial void SetInjuries(float val);

    [ScriptFunction("float GetInjuries()")]
    public partial float GetInjuries();

    [ScriptFunction("void SetBloodloss(float val)")]
    public partial void SetBloodloss(float val);

    [ScriptFunction("float GetBloodloss()")]
    public partial float GetBloodloss();

    [ScriptFunction("bool GetGodmode()")]
    public partial bool GetGodmode();

    [ScriptFunction("void SetGodmode(bool val)")]
    public partial void SetGodmode(bool val);

    [ScriptFunction("void SetColor(int hx)")]
    public partial void SetColor(int hx);

    [ScriptFunction("int GetColor()")]
    public partial int GetColor();

    [ScriptFunction("void GetNetworkPosition(float& out x, float& out y, float& out z)")]
    public partial void GetNetworkPosition(out float x, out float y, out float z);

    [ScriptFunction("void GetNetworkRotation(float& out pitch, float& out yaw)")]
    public partial void GetNetworkRotation(out float pitch, out float yaw);

    [ScriptFunction("void SetNetworkPosition(float x, float y, float z)")]
    public partial void SetNetworkPosition(float x, float y, float z);

    [ScriptFunction("void SetNetworkRotation(float pitch, float yaw)")]
    public partial void SetNetworkRotation(float pitch, float yaw);

    [ScriptFunction("void SetPosition(float x, float y, float z, Room room = null, bool updatepivot = true)")]
    public partial void SetPosition(float x, float y, float z, ManagedRoom room = null, bool updatepivot = true);

    [ScriptFunction("void SetRotation(float pitch, float yaw)")]
    public partial void SetRotation(float pitch, float yaw);

    [ScriptFunction("void SetRoom(Room room)")]
    public partial void SetRoom(ManagedRoom room);

    [ScriptFunction("Room GetRoom()")]
    public partial ManagedRoom GetRoom();

    [ScriptFunction(
        "void SetPositionBounds(Room room, float x = 0.0f, float y = 0.0f, float z = 0.0f, float distance = 0.0f)")]
    public partial void SetPositionBounds(ManagedRoom room, float x = 0.0f, float y = 0.0f, float z = 0.0f,
        float distance = 0.0f);

    [ScriptFunction("void Explode(bool thrust = false)")]
    public partial void Explode(bool thrust = false);

    [ScriptFunction("void MovePlayer(float speedmult, float angle)")]
    public partial void MovePlayer(float speedmult, float angle);

    [ScriptFunction("void IgnoreProximity(bool value)")]
    public partial void IgnoreProximity(bool value);

    [ScriptFunction(
        "void SendDamage(Player player, float force, bool headshot, float offsetx, float offsety, float offsetz)")]
    public partial void SendDamage(ManagedPlayer player, float force, bool headshot, float offsetx, float offsety,
        float offsetz);

    [ScriptFunction("void SetAdmin(bool val)")]
    public partial void SetAdmin(bool val);

    [ScriptFunction("bool IsAdmin()")]
    public partial bool IsAdmin();

    [ScriptFunction("void SetGlobalTransmission(bool val)")]
    public partial void SetGlobalTransmission(bool val);

    [ScriptFunction("bool IsGlobalTransmission()")]
    public partial bool IsGlobalTransmission();

    [ScriptFunction("bool SendVoice(int bank, int radio = 0, bool global = false, Player target = null)")]
    public partial bool SendVoice(int bank, int radio = 0, bool global = false, ManagedPlayer target = null);

    [ScriptFunction("int GetItemsCount()")]
    public partial int GetItemsCount();

    [ScriptFunction("Items GetInventory(int slot)")]
    public partial ManagedItems GetInventory(int slot);

    [ScriptFunction("Items GetSelectedItem()")]
    public partial ManagedItems GetSelectedItem();

    [ScriptFunction("float GetBlinkTimer()")]
    public partial float GetBlinkTimer();

    [ScriptFunction("void SetBlinkTimer(float time)")]
    public partial void SetBlinkTimer(float time);

    [ScriptFunction("bool IsBlinking()")]
    public partial bool IsBlinking();

    [ScriptFunction("void SetBlinkEffect(float effectvalue, float time)")]
    public partial void SetBlinkEffect(float effectvalue, float time);

    [ScriptFunction("void GetBlinkEffect(float &out effectvalue, float &out time)")]
    public partial void GetBlinkEffect(out float effectvalue, out float time);

    [ScriptFunction("int GetRadio()")]
    public partial int GetRadio();

    [ScriptFunction("void SetNetworkAnimation(int animid)")]
    public partial void SetNetworkAnimation(int animid);

    [ScriptFunction("void SetAnimation(int animid)")]
    public partial void SetAnimation(int animid);

    [ScriptFunction("int GetAnimation()")]
    public partial int GetAnimation();

    [ScriptFunction("void SetSpeedMultiplier(float multiplier)")]
    public partial void SetSpeedMultiplier(float multiplier);

    [ScriptFunction("void SetStaminaMultiplier(float multiplier)")]
    public partial void SetStaminaMultiplier(float multiplier);

    [ScriptFunction("float GetSpeedMultiplier()")]
    public partial float GetSpeedMultiplier();

    [ScriptFunction("float GetStaminaMultiplier()")]
    public partial float GetStaminaMultiplier();

    [ScriptFunction("void SetAttach(int bodyindex, int attachmodelindex, Items item = null)")]
    public partial void SetAttach(int bodyindex, int attachmodelindex, ManagedItems item = null);

    [ScriptFunction("int GetAttach(int bodyindex)")]
    public partial int GetAttach(int bodyindex);

    [ScriptFunction("Items GetAttachItem(int bodyindex)")]
    public partial ManagedItems GetAttachItem(int bodyindex);

    [ScriptFunction("int GetModel()")]
    public partial int GetModel();

    [ScriptFunction("void SetModel(int modelid, int textureid = -1)")]
    public partial void SetModel(int modelid, int textureid = -1);

    [ScriptFunction("void SetModelSize(float size)")]
    public partial void SetModelSize(float size);

    [ScriptFunction("float GetModelSize()")]
    public partial float GetModelSize();

    [ScriptFunction("void SetModelTexture(int textureid)")]
    public partial void SetModelTexture(int textureid);

    [ScriptFunction("int GetModelTexture()")]
    public partial int GetModelTexture();

    [ScriptFunction("float GetVolume()")]
    public partial float GetVolume();

    [ScriptFunction("bool IsCrouch()")]
    public partial bool IsCrouch();

    [ScriptFunction("void SetGravity(float multiplier)")]
    public partial void SetGravity(float multiplier);

    [ScriptFunction("float GetGravity()")]
    public partial float GetGravity();

    [ScriptFunction("void SetTagText(int index, string& in text)")]
    public partial void SetTagText(int index, string text);

    [ScriptFunction("void SetTagScales(int index, float scaleX, float scaleY)")]
    public partial void SetTagScales(int index, float scaleX, float scaleY);

    [ScriptFunction("void SetTagOffset(int index, float offset)")]
    public partial void SetTagOffset(int index, float offset);

    [ScriptFunction("void SetTagColors(int index, int color1, int color2)")]
    public partial void SetTagColors(int index, int color1, int color2);

    [ScriptFunction("void SetTagFont(int index, string& in font)")]
    public partial void SetTagFont(int index, string font);

    [ScriptFunction("int GetShootsCount()")]
    public partial int GetShootsCount();

    [ScriptFunction("void SetShootsCount(int count)")]
    public partial void SetShootsCount(int count);

    [ScriptFunction("void RedirectMove(bool move)")]
    public partial void RedirectMove(bool move);

    [ScriptFunction("bool IsBot()")]
    public partial bool IsBot();

    [ScriptFunction("void SetWearData(int bodyindex, Items item)")]
    public partial void SetWearData(int bodyindex, ManagedItems item);

    [ScriptFunction("void Console(string& in message)")]
    public partial void Console(string message);

    [ScriptFunction("bool GetKeyState(int keytype)")]
    public partial bool GetKeyState(int keytype);

    [ScriptFunction("void GetTeleportData(float& out x, float& out y, float& out z, Room& out room, int& out tick)")]
    public partial void GetTeleportData(out float x, out float y, out float z, out ManagedRoom room, out int tick);
}