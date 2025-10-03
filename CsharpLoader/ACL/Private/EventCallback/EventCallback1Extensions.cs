namespace ACL.Private.EventCallback;

using System.Runtime.InteropServices;
using ACL.Feature;
using ACL.Managed;

internal static class EventCallback1Extensions
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void Callback();

    private static readonly ScriptFunction RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK1@)");

    private static string CallbackDeclaration => $"void ACL{Guid.NewGuid():N}()";

    private static readonly Dictionary<EventCallback1, ScriptFunction> EventFunctions =
        new Dictionary<EventCallback1, ScriptFunction>();

    public static void RegisterEventCallback(this EventManager eventManager, EventCallback1 callback, EventType eventType)
    {
        if (EventFunctions.ContainsKey(callback))
        {
            GlobalFunctions.Print("[WARN] Duplicate event registration is not allowed");
            return;
        }

        EventFunctions.Add(callback, eventManager.RegisterEvent(WrapEventCallback(callback), eventType, CallbackDeclaration, RegisterCallbackFunction));
    }

    public static void UnregisterEventCallback(this EventManager eventManager, EventCallback1 callback, EventType eventType)
    {
        if (!EventFunctions.Remove(callback, out var eventFunction))
        {
            return;
        }

        eventManager.UnregisterEvent(eventType, eventFunction);
    }

    private static Callback WrapEventCallback(EventCallback1 callback)
    {
        return WrappedCallback;

        void WrappedCallback()
        {
            try
            {
                callback();
            }
            catch (Exception e)
            {
                GlobalFunctions.Print("[ERROR] Encountered an exception while invoking an event callback: " + e);
            }
        }
    }
}