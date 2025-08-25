namespace ACL.Sample;

using ACL.Feature;
using ACL.Loader;
using ACL.Managed.ScriptObject;

public class TestPlugin : Plugin
{
    public override string Name => "TestPlugin";

    private long tickCount;

    public override void EnablePlugin()
    {
        GlobalFunctions.Print(ManagedServer.Instance.HostName);

        GlobalFunctions.Print("Hello World!");

        //this.EventManager.ServerUpdate += this.OnServerUpdate;
        this.EventManager.PlayerConnect += OnPlayerConnect;
    }

    public void OnServerUpdate()
    {
        this.tickCount++;
    }

    public static void OnPlayerConnect(Player player)
    {
        GlobalFunctions.Print(player.Name + " 加入了服务器");
        player.Base.ShowDialog(0, 0, "你好，世界！", "世界，你好！", "左侧按钮", "右侧按钮");
    }
}