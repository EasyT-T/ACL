namespace ACL.Managed.ScriptObject;

using ACL.SourceGenerators;

[ScriptClass("Graphics")]
public partial class ManagedGraphics : ScriptObjectBase
{
    public static ManagedGraphics Instance { get; } =
        new ManagedGraphics(ScriptEngine.GetGlobalProperty("Graphics graphics").Handle);

    internal ManagedGraphics(IntPtr handle) : base(handle)
    {
    }

    [ScriptFunction("GUIElement CreateOval(Player player, float x, float y, float width, float height, bool align=false)")]
    public partial ManagedGuiElement CreateOval(ManagedPlayer player, float x, float y, float width, float height, bool align = false);

    [ScriptFunction("GUIElement CreateRect(Player player, float x, float y, float width, float height, bool align=false)")]
    public partial ManagedGuiElement CreateRect(ManagedPlayer player, float x, float y, float width, float height, bool align = false);

    [ScriptFunction("GUIElement CreateProgressBar(Player player, float time, float x, float y, float width, float height, bool align=false)")]
    public partial ManagedGuiElement CreateProgressBar(ManagedPlayer player, float time, float x, float y, float width, float height, bool align = false);

    [ScriptFunction("GUIElement CreateProgressBar(Player player, float time, float x, float y, float width, float height, bool align, string &in callback)")]
    public partial ManagedGuiElement CreateProgressBar(ManagedPlayer player, float time, float x, float y, float width, float height, bool align, string callback);

    [ScriptFunction("GUIElement CreateProgressBar(Player player, float time, float x, float y, float width, float height, bool align, ref &in callback)")]
    public partial ManagedGuiElement CreateProgressBar(ManagedPlayer player, float time, float x, float y, float width, float height, bool align, IntPtr callback);

    [ScriptFunction("GUIElement CreateText(Player player, int fontid, string &in text, float x, float y, bool align=false)")]
    public partial ManagedGuiElement CreateText(ManagedPlayer player, int fontid, string text, float x, float y, bool align = false);

    [ScriptFunction("GUIElement CreateImage(Player player, string &in filename, float x, float y, float width, float height, bool align=false)")]
    public partial ManagedGuiElement CreateImage(ManagedPlayer player, string filename, float x, float y, float width, float height, bool align = false);
}