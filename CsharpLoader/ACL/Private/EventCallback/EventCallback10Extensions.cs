namespace ACL.Private.EventCallback;

using System.Runtime.InteropServices;
using ACL.Feature;
using ACL.Managed;
using ACL.Managed.ScriptObject;

internal static class EventCallback10Extensions
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate bool Callback(AngelObject obj, AngelObject obj2, float single, float single2, float single3, float single4, bool boolean);

    private static readonly ScriptFunction RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK10@)");

    private static string CallbackDeclaration => $"bool ACL{Guid.NewGuid():N}(Player, Player, float, float, float, float, bool)";

    private static readonly Dictionary<EventCallback10, ScriptFunction> EventFunctions =
        new Dictionary<EventCallback10, ScriptFunction>();

    public static void RegisterEventCallback(this EventManager eventManager, EventCallback10 callback, EventType eventType)
    {
        if (EventFunctions.ContainsKey(callback))
        {
            GlobalFunctions.Print("[WARN] Duplicate event registration is not allowed");
            return;
        }

        EventFunctions.Add(callback, eventManager.RegisterEvent(WrapEventCallback(callback), eventType, CallbackDeclaration, RegisterCallbackFunction));
    }

    public static void UnregisterEventCallback(this EventManager eventManager, EventCallback10 callback, EventType eventType)
    {
        if (!EventFunctions.Remove(callback, out var eventFunction))
        {
            return;
        }

        eventManager.UnregisterEvent(eventType, eventFunction);
    }

    private static Callback WrapEventCallback(EventCallback10 callback)
    {
        return WrappedCallback;

        unsafe bool WrappedCallback(AngelObject obj, AngelObject obj2, float single, float single2, float single3, float single4, bool boolean)
        {
            try
            {
                var managedPlayer = new ManagedPlayer((AngelObject*)obj.GetPointer());
                var player = Player.Get(managedPlayer);

                var managedPlayer2 = new ManagedPlayer((AngelObject*)obj2.GetPointer());
                var player2 = Player.Get(managedPlayer2);

                return callback(player, player2, single, single2, single3, single4, boolean);
            }
            catch (Exception e)
            {
                GlobalFunctions.Print("[ERROR] Encountered an exception while invoking an event callback: " + e);

                return true;
            }
        }
    }
}