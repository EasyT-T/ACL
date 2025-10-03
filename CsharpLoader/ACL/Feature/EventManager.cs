namespace ACL.Feature;

using System.Runtime.InteropServices;
using ACL.Managed;
using ACL.Private;
using ACL.Private.EventCallback;

public class EventManager
{
    private readonly ModuleContext moduleContext;

    public EventManager(ModuleContext moduleContext)
    {
        this.moduleContext = moduleContext;
        this.moduleContext.Module.SetUserData(this.moduleContext, 0);
        this.moduleContext.Context.SetUserData(this.moduleContext, 0);
    }

    public delegate void OnServerUpdate();

    public delegate void OnPlayerUpdate(Player player);

    public delegate void OnPlayerConnect(Player player);

    public delegate bool OnPlayerChat(Player player, string message);

    public delegate void OnPlayerDisconnect(Player player);

    public delegate void OnPlayerAttachesUpdate(Player player);

    public delegate bool OnPlayerTakeItem(Player player, Items item);

    public delegate bool OnPlayerDropItem(Player player, Items item);

    public delegate void OnPlayerDialogAction(Player player, int index, bool response, string input, int selectedItem);

    public delegate bool OnPlayerShootPlayer(Player attacker, Player target, float x, float y, float z, float damage,
        bool headshot);

    public event OnServerUpdate ServerUpdate
    {
        add => this.RegisterEventCallback(value.Invoke, EventType.ServerUpdateC);
        remove => this.UnregisterEventCallback(value.Invoke, EventType.ServerUpdateC);
    }

    public event OnPlayerConnect PlayerConnect
    {
        add => this.RegisterEventCallback(value.Invoke, EventType.PlayerConnectC);
        remove => this.RegisterEventCallback(value.Invoke, EventType.PlayerConnectC);
    }

    public event OnPlayerUpdate PlayerUpdate
    {
        add => this.RegisterEventCallback(value.Invoke, EventType.PlayerUpdateC);
        remove => this.UnregisterEventCallback(value.Invoke, EventType.PlayerUpdateC);
    }

    public event OnPlayerChat PlayerChat
    {
        add => this.RegisterEventCallback(value.Invoke, EventType.PlayerChatC);
        remove => this.UnregisterEventCallback(value.Invoke, EventType.PlayerChatC);
    }

    public event OnPlayerDisconnect PlayerDisconnect
    {
        add => this.RegisterEventCallback(value.Invoke, EventType.PlayerDisconnectC);
        remove => this.UnregisterEventCallback(value.Invoke, EventType.PlayerDisconnectC);
    }

    private readonly Dictionary<ScriptFunction, GCHandle> eventCallbacks = new Dictionary<ScriptFunction, GCHandle>();

    // So hacky...
    internal ScriptFunction RegisterEvent<TCallback>(TCallback callback, EventType eventType,
        string callbackDeclaration,
        ScriptFunction registerCallbackFunction) where TCallback : Delegate
    {
        try
        {
            var scriptCallback = AngelCallback.Create(callback);

            var context = this.moduleContext.Context;
            var module = this.moduleContext.Module;

            var callbackFunctionHandle = NativeBindings.TL_Tool_RegisterFunctionToModule(module.Handle,
                callbackDeclaration,
                scriptCallback.Handle,
                (int)CallConv.AsCallStdcall, IntPtr.Zero);

            var function = new ScriptFunction(callbackFunctionHandle);

            context.Prepare(registerCallbackFunction);
            context.SetArgument(0, (uint)eventType);
            context.SetArgument(1, callbackFunctionHandle);
            context.Execute();

            context.Unprepare();

            this.eventCallbacks.Add(function, GCHandle.Alloc(callback));

            return function;
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to register event: " + e);

            throw;
        }
    }

    internal void UnregisterEvent(EventType eventType, ScriptFunction eventFunction)
    {
        if (!this.eventCallbacks.Remove(eventFunction, out var handle))
        {
            return;
        }

        handle.Free();

        try
        {
            var registry = NativeBindings.TL_Tool_GetEventRegistry((int)eventType);
            NativeBindings.TL_Tool_Vector_EraseFromValue(registry, eventFunction.Handle.ToInt32());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
    }
}