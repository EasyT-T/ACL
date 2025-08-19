namespace ACL.Managed.ScriptObject;

using ACL.SourceGenerators;

[ScriptClass("GUIElement")]
public partial class ManagedGuiElement : ScriptObjectBase
{
    internal ManagedGuiElement(IntPtr handle) : base(handle)
    {
    }

    [ScriptFunction("void GetPosition(float &out x, float &out y)")]
    public partial void GetPosition(out float x, out float y);

    [ScriptFunction("void SetPosition(float x, float y)")]
    public partial void SetPosition(float x, float y);

    [ScriptFunction("void SetScale(float width, float height)")]
    public partial void SetScale(float width, float height);

    [ScriptFunction("void GetScale(float &out width, float &out height)")]
    public partial void GetScale(out float width, out float height);

    [ScriptFunction("void SetData(string &in data)")]
    public partial void SetData(string data);

    [ScriptFunction("void SetText(string &in text)")]
    public partial void SetText(string text);

    [ScriptFunction("void SetSelectable(bool selectable)")]
    public partial void SetSelectable(bool selectable);

    [ScriptFunction("void SetShadow(bool shadowed)")]
    public partial void SetShadow(bool shadowed);

    [ScriptFunction("void SetOpacity(float target, float lerp)")]
    public partial void SetOpacity(float target, float lerp);

    [ScriptFunction("void SetColor(int r, int g, int b)")]
    public partial void SetColor(int r, int g, int b);

    [ScriptFunction("Player GetPlayer()")]
    public partial ManagedPlayer GetPlayer();

    [ScriptFunction("void SetAttach(Player player)")]
    public partial void SetAttach(ManagedPlayer player);

    [ScriptFunction("Player GetAttach()")]
    public partial ManagedPlayer GetAttach();

    [ScriptFunction("string & GetText()")]
    public partial string GetText();

    [ScriptFunction("string & GetData()")]
    public partial string GetData();

    [ScriptFunction("int IsSelectable()")]
    public partial int IsSelectable();

    [ScriptFunction("bool IsHidden()")]
    public partial bool IsHidden();

    [ScriptFunction("void SetCallback(string &in funcname)")]
    public partial void SetCallback(string funcname);

    [ScriptFunction("void SetCallback(GUICALLBACK @gc)")]
    public partial void SetCallback(Delegate gc);

    [ScriptFunction("void Hide()")]
    public partial void Hide();

    [ScriptFunction("void Show()")]
    public partial void Show();

    [ScriptFunction("void Remove()")]
    public partial void Remove();
}