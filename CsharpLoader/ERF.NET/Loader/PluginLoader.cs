namespace ERF.Loader;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using ERF.Feature;
using ERF.Managed;

public class PluginLoader
{
    [MemberNotNullWhen(true, nameof(Instance))]
    public static bool InstanceSet { get; private set; }

    public static PluginLoader? Instance { get; private set; }

    public string PluginDirectory { get; } = Path.Combine(Environment.CurrentDirectory, "plugins");

    internal Dictionary<Assembly, Plugin> Plugins { get; } = [];

    private readonly AssemblyLoadContext loadContext = new PluginAssemblyLoadContext("ERF", Assembly.GetExecutingAssembly());

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
            plugin.EventManager = new EventManager(plugin);

            plugin.EnablePlugin();

            this.Plugins.Add(assembly, plugin);

            return;
        }
    }
}