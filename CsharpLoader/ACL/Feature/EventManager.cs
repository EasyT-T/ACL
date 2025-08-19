namespace ACL.Feature;

using System.Runtime.InteropServices;
using ACL.Managed;
using ACL.Managed.ScriptObject;
using ACL.Private;
using ManagedPlayer = ACL.Managed.ScriptObject.ManagedPlayer;

public class EventManager(ModuleContext moduleContext)
{
    private static int globalFuncIndex;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OnServerUpdate();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OnPlayerUpdate(Player player);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OnPlayerConnect(Player player);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate bool OnPlayerChat(Player player, string message);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OnPlayerDisconnect(Player player);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void EventCallback1();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void EventCallback2(Player player);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate bool EventCallback4(Player player, string str);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void EventCallbackVoidObject(AngelObject obj);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate bool EventCallbackBoolObjectObject(AngelObject obj, AngelObject obj2);

    public event OnServerUpdate ServerUpdate
    {
        add => this.RegisterEvent(new EventCallback1(value), EventType.ServerUpdateC, ServerUpdateCallbackDeclaration,
            EventCallback1RegisterCallbackFunction);
        remove => throw new NotSupportedException("Remove event is not supported");
    }

    public event OnPlayerUpdate PlayerUpdate
    {
        add => this.RegisterEvent(WrapEventCallback2(new EventCallback2(value)), EventType.PlayerUpdateC,
            PlayerUpdateCallbackDeclaration, EventCallback2RegisterCallbackFunction);
        remove => throw new NotSupportedException("Remove event is not supported");
    }

    public event OnPlayerConnect PlayerConnect
    {
        add => this.RegisterEvent(WrapEventCallback2(new EventCallback2(value)), EventType.PlayerConnectC,
            PlayerConnectCallbackDeclaration, EventCallback2RegisterCallbackFunction);
        remove => throw new NotSupportedException("Remove event is not supported");
    }

    public event OnPlayerChat PlayerChat
    {
        add => this.RegisterEvent(WrapEventCallback4(new EventCallback4(value)), EventType.PlayerChatC,
            PlayerChatCallbackDeclaration, EventCallback4RegisterCallbackFunction);
        remove => throw new NotSupportedException("Remove event is not supported");
    }

    public event OnPlayerDisconnect PlayerDisconnect
    {
        add => this.RegisterEvent(WrapEventCallback2(new EventCallback2(value)), EventType.PlayerDisconnectC,
            PlayerDisconnectCallbackDeclaration, EventCallback2RegisterCallbackFunction);
        remove => throw new NotSupportedException("Remove event is not supported");
    }

    private static string ServerUpdateCallbackDeclaration => $"void ACL{nameof(OnServerUpdate)}{globalFuncIndex}()";

    private static string PlayerUpdateCallbackDeclaration =>
        $"void ACL{nameof(OnPlayerUpdate)}{globalFuncIndex}(Player)";

    private static string PlayerConnectCallbackDeclaration =>
        $"void ACL{nameof(OnPlayerConnect)}{globalFuncIndex}(Player)";

    private static string PlayerChatCallbackDeclaration =>
        $"bool ACL{nameof(OnPlayerChat)}{globalFuncIndex}(Player, string)";

    private static string PlayerDisconnectCallbackDeclaration =>
        $"void ACL{nameof(OnPlayerDisconnect)}{globalFuncIndex}(Player)";

    private static readonly ScriptFunction EventCallback1RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK1@)");

    private static readonly ScriptFunction EventCallback2RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK2@)");

    private static readonly ScriptFunction EventCallback4RegisterCallbackFunction =
        ScriptEngine.GetGlobalFunction("bool RegisterCallback(int, _EVENT_CALLBACK4@)");

    private void RegisterEvent(Delegate callback, EventType eventType, string callbackDeclaration,
        ScriptFunction registerCallbackFunction)
    {
        var handle = GCHandle.Alloc(callback);

        try
        {
            var scriptCallback = AngelCallback.Create(callback);

            var context = moduleContext.Context;
            var module = moduleContext.Module;

            var callbackFunctionHandle = NativeBindings.TL_Tool_RegisterFunctionToModule(module.Handle,
                callbackDeclaration,
                scriptCallback.Handle,
                (int)CallConv.AsCallStdcall, IntPtr.Zero);

            var function = new ScriptFunction(callbackFunctionHandle);

            GlobalFunctions.Print(function.GetDeclaration());

            context.Prepare(registerCallbackFunction);
            context.SetArgument(0, (uint)eventType);
            context.SetArgument(1, callbackFunctionHandle);
            context.Execute();

            globalFuncIndex++;
        }
        catch (Exception e)
        {
            handle.Free();

            Console.WriteLine("Unable to register event: " + e);
        }
    }

    private static EventCallbackVoidObject WrapEventCallback2(EventCallback2 callback)
    {
        return WrappedCallback;

        void WrappedCallback(AngelObject obj)
        {
            using var ptr = obj.GetPointer();
            var player = new Player(new ManagedPlayer(ptr.Handle));
            callback.Invoke(player);
        }
    }

    private static EventCallbackBoolObjectObject WrapEventCallback4(EventCallback4 callback)
    {
        return WrappedCallback;

        bool WrappedCallback(AngelObject obj, AngelObject obj2)
        {
            using var ptr = obj.GetPointer();
            var player = new Player(new ManagedPlayer(ptr.Handle));

            using var ptr2 = obj2.GetPointer();
            var str = new ManagedString(ptr2.Handle).ToString();

            return callback.Invoke(player, str);
        }
    }
}