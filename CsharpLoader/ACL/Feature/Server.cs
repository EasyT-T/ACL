namespace ACL.Feature;

using ACL.Managed.ScriptObject;

public static class Server
{
    public static void Restart()
    {
        ManagedServer.Instance.Restart();
    }

    public static void SendConsoleCommand(string command)
    {
        ManagedServer.Instance.Console(command);
    }
}