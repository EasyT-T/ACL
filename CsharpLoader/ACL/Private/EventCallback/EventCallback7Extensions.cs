namespace ACL.Private.EventCallback;

using System.Runtime.InteropServices;
using ACL.Feature;
using ACL.Managed;
using ACL.Managed.ScriptObject;

internal static class EventCallback7Extensions
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate bool Callback(AngelObject obj, AngelObject obj2);

    private static readonly ScriptFunction RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK7@)");

    private static string CallbackDeclaration => $"bool ACL{Guid.NewGuid():N}(Player, Items)";

    private static readonly Dictionary<EventCallback7, ScriptFunction> EventFunctions =
        new Dictionary<EventCallback7, ScriptFunction>();

    public static void RegisterEventCallback(this EventManager eventManager, EventCallback7 callback, EventType eventType)
    {
        if (EventFunctions.ContainsKey(callback))
        {
            GlobalFunctions.Print("[WARN] Duplicate event registration is not allowed");
            return;
        }

        EventFunctions.Add(callback, eventManager.RegisterEvent(WrapEventCallback(callback), eventType, CallbackDeclaration, RegisterCallbackFunction));
    }

    public static void UnregisterEventCallback(this EventManager eventManager, EventCallback7 callback, EventType eventType)
    {
        if (!EventFunctions.Remove(callback, out var eventFunction))
        {
            return;
        }

        eventManager.UnregisterEvent(eventType, eventFunction);
    }

    private static Callback WrapEventCallback(EventCallback7 callback)
    {
        return WrappedCallback;

        unsafe bool WrappedCallback(AngelObject obj, AngelObject obj2)
        {
            try
            {
                var managedPlayer = new ManagedPlayer((AngelObject*)obj.GetPointer());
                var player = Player.Get(managedPlayer);

                var managedItems = new ManagedItems((AngelObject*)obj2.GetPointer());
                var item = new Items(managedItems);

                return callback(player, item);
            }
            catch (Exception e)
            {
                GlobalFunctions.Print("[ERROR] Encountered an exception while invoking an event callback: " + e);

                return true;
            }
        }
    }
}