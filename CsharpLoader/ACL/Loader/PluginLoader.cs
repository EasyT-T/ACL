namespace ACL.Loader;

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using ACL.Feature;
using ACL.Managed;

public class PluginLoader
{
    [MemberNotNullWhen(true, nameof(Instance))]
    public static bool InstanceSet { get; private set; }

    public static PluginLoader? Instance { get; private set; }

    public string PluginDirectory { get; } = Path.Combine(Environment.CurrentDirectory, "plugins");

    public FrozenSet<Plugin> Plugins => this.PluginMap.Values.ToFrozenSet();

    internal Dictionary<Assembly, Plugin> PluginMap { get; } = [];

    private readonly AssemblyLoadContext loadContext = new PluginAssemblyLoadContext("ACL", Assembly.GetExecutingAssembly());

    private PluginLoader()
    {
        var moduleContext = ModuleContext.Create(ScriptModule.Create("acl"), ScriptEngine.CreateContext());
        moduleContext.Module.SetUserData(moduleContext, 0);
        moduleContext.Context.SetUserData(moduleContext, 0);
        var eventManager = new EventManager(moduleContext);

        Player.Register(eventManager);
    }

    [UnmanagedCallersOnly(EntryPoint = "CreateLoader")]
    internal static void CreateLoader()
    {
        try
        {
            InstanceSet = true;
            Instance = new PluginLoader();

            Instance.LoadPlugins();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void LoadPlugins()
    {
        var directory = new DirectoryInfo(this.PluginDirectory);

        foreach (var file in directory.GetFiles("*.dll"))
        {
            var assembly = this.loadContext.LoadFromAssemblyPath(file.FullName);

            this.LoadPluginInternal(assembly);
        }
    }

    private void LoadPluginInternal(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (!typeof(Plugin).IsAssignableFrom(type))
            {
                continue;
            }

            if (Activator.CreateInstance(type) is not Plugin plugin)
            {
                continue;
            }

            var module = ScriptModule.Create(plugin.Name);
            var context = ScriptEngine.CreateContext();
            var moduleContext = ModuleContext.Create(module, context);

            module.SetUserData(moduleContext, 0);
            context.SetUserData(moduleContext, 0);

            plugin.ModuleContext = moduleContext;
            plugin.EventManager = new EventManager(moduleContext);

            plugin.EnablePlugin();

            this.PluginMap.Add(assembly, plugin);

            return;
        }
    }
}