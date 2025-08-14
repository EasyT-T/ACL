namespace ERF.Loader;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

public class PluginLoader
{
    [MemberNotNullWhen(true, nameof(Instance))]
    public static bool InstanceSet { get; private set; }

    public static PluginLoader? Instance { get; private set; }

    public string PluginDirectory { get; } = Path.Combine(Environment.CurrentDirectory, "plugins");

    internal List<IPlugin> Plugins { get; } = [];

    private readonly AssemblyLoadContext context = new PluginAssemblyLoadContext("ERF", Assembly.GetExecutingAssembly());

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
            var assembly = this.context.LoadFromAssemblyPath(file.FullName);

            this.LoadPluginInternal(assembly);
        }
    }

    private void LoadPluginInternal(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (!typeof(IPlugin).IsAssignableFrom(type))
            {
                continue;
            }

            if (Activator.CreateInstance(type) is not IPlugin plugin)
            {
                continue;
            }

            plugin.EnablePlugin();

            this.Plugins.Add(plugin);
        }
    }
}