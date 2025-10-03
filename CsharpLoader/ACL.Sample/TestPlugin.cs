namespace ACL.Sample;

using ACL.Feature;
using ACL.Loader;
using ACL.Managed.ScriptObject;

public class TestPlugin : Plugin
{
    public override string Name => "TestPlugin";

    public override void EnablePlugin()
    {
        this.EventManager.PlayerConnect += OnPlayerConnect;
    }

    public static void OnPlayerConnect(Player player)
    {
        player.Base.ShowDialog(0, 0, "你好，世界！", "世界，你好！", "左侧按钮", "右侧按钮");
    }
}