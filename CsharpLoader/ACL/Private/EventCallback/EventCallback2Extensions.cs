namespace ACL.Private.EventCallback;

using System.Runtime.InteropServices;
using ACL.Feature;
using ACL.Managed;
using ACL.Managed.ScriptObject;

internal static class EventCallback2Extensions
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Callback(AngelObject obj);

    private static readonly ScriptFunction RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK2@)");

    private static string CallbackDeclaration => $"void ACL{Guid.NewGuid():N}(Player)";

    private static readonly Dictionary<EventCallback2, ScriptFunction> EventFunctions =
        new Dictionary<EventCallback2, ScriptFunction>();

    public static void RegisterEventCallback(this EventManager eventManager, EventCallback2 callback, EventType eventType)
    {
        if (EventFunctions.ContainsKey(callback))
        {
            GlobalFunctions.Print("[WARN] Duplicate event registration is not allowed");
            return;
        }

        EventFunctions.Add(callback, eventManager.RegisterEvent(WrapEventCallback(callback), eventType, CallbackDeclaration, RegisterCallbackFunction));
    }

    public static void UnregisterEventCallback(this EventManager eventManager, EventCallback2 callback, EventType eventType)
    {
        if (!EventFunctions.Remove(callback, out var eventFunction))
        {
            return;
        }

        eventManager.UnregisterEvent(eventType, eventFunction);
    }

    private static Callback WrapEventCallback(EventCallback2 callback)
    {
        return WrappedCallback;

        unsafe void WrappedCallback(AngelObject obj)
        {
            try
            {
                var managedPlayer = new ManagedPlayer((AngelObject*)obj.GetPointer());
                var player = Player.Get(managedPlayer);

                callback(player);
            }
            catch (Exception e)
            {
                GlobalFunctions.Print("[ERROR] Encountered an exception while invoking an event callback: " + e);
            }
        }
    }
}