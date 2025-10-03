namespace ACL.Private.EventCallback;

using System.Runtime.InteropServices;
using ACL.Feature;
using ACL.Managed;
using ACL.Managed.ScriptObject;

internal static class EventCallback9Extensions
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Callback(AngelObject obj, int num, bool boolean, AngelObject obj2, int num2);

    private static readonly ScriptFunction RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK9@)");

    private static string CallbackDeclaration => $"void ACL{Guid.NewGuid():N}(Player, int, bool, string, int)";

    private static readonly Dictionary<EventCallback9, ScriptFunction> EventFunctions =
        new Dictionary<EventCallback9, ScriptFunction>();

    public static void RegisterEventCallback(this EventManager eventManager, EventCallback9 callback, EventType eventType)
    {
        if (EventFunctions.ContainsKey(callback))
        {
            GlobalFunctions.Print("[WARN] Duplicate event registration is not allowed");
            return;
        }

        EventFunctions.Add(callback, eventManager.RegisterEvent(WrapEventCallback(callback), eventType, CallbackDeclaration, RegisterCallbackFunction));
    }

    public static void UnregisterEventCallback(this EventManager eventManager, EventCallback9 callback, EventType eventType)
    {
        if (!EventFunctions.Remove(callback, out var eventFunction))
        {
            return;
        }

        eventManager.UnregisterEvent(eventType, eventFunction);
    }

    private static Callback WrapEventCallback(EventCallback9 callback)
    {
        return WrappedCallback;

        unsafe void WrappedCallback(AngelObject obj, int num, bool boolean, AngelObject obj2, int num2)
        {
            try
            {
                var managedPlayer = new ManagedPlayer((AngelObject*)obj.GetPointer());
                var player = Player.Get(managedPlayer);

                var str = new ManagedString(obj2.GetPointer());

                callback(player, num, boolean, str, num2);
            }
            catch (Exception e)
            {
                GlobalFunctions.Print("[ERROR] Encountered an exception while invoking an event callback: " + e);
            }
        }
    }
}