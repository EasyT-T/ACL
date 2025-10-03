namespace ACL.Private.EventCallback;

using System.Runtime.InteropServices;
using ACL.Feature;
using ACL.Managed;
using ACL.Managed.ScriptObject;

internal static class EventCallback4Extensions
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate bool Callback(AngelObject obj, AngelObject obj2);

    private static readonly ScriptFunction RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK4@)");

    private static string CallbackDeclaration => $"bool ACL{Guid.NewGuid():N}(Player, string)";

    private static readonly Dictionary<EventCallback4, ScriptFunction> EventFunctions =
        new Dictionary<EventCallback4, ScriptFunction>();

    public static void RegisterEventCallback(this EventManager eventManager, EventCallback4 callback, EventType eventType)
    {
        if (EventFunctions.ContainsKey(callback))
        {
            GlobalFunctions.Print("[WARN] Duplicate event registration is not allowed");
            return;
        }

        EventFunctions.Add(callback, eventManager.RegisterEvent(WrapEventCallback(callback), eventType, CallbackDeclaration, RegisterCallbackFunction));
    }

    public static void UnregisterEventCallback(this EventManager eventManager, EventCallback4 callback, EventType eventType)
    {
        if (!EventFunctions.Remove(callback, out var eventFunction))
        {
            return;
        }

        eventManager.UnregisterEvent(eventType, eventFunction);
    }

    private static Callback WrapEventCallback(EventCallback4 callback)
    {
        return WrappedCallback;

        unsafe bool WrappedCallback(AngelObject obj, AngelObject obj2)
        {
            try
            {
                var managedPlayer = new ManagedPlayer((AngelObject*)obj.GetPointer());
                var player = Player.Get(managedPlayer);

                var str = new ManagedString(obj2.GetPointer());

                return callback(player, str);
            }
            catch (Exception e)
            {
                GlobalFunctions.Print("[ERROR] Encountered an exception while invoking an event callback: " + e);

                return true;
            }
        }
    }
}