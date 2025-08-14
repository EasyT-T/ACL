namespace ERF.Loader;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ERF.Loader.Binding;

public class Loader
{
    [MemberNotNullWhen(true, nameof(Instance))]
    public static bool InstanceSet { get; private set; }

    public static Loader? Instance { get; private set; }

    public string PluginDirectory { get; } = Path.Combine(Environment.CurrentDirectory, "plugins");

    [UnmanagedCallersOnly(EntryPoint = "CreateLoader", CallConvs = [typeof(CallConvCdecl)])]
    internal static void CreateLoader()
    {
        InstanceSet = true;
        Instance = new Loader();

        Instance.LoadPlugins();
    }

    public void LoadPlugins()
    {
        var directory = new DirectoryInfo(this.PluginDirectory);

        foreach (var file in directory.GetFiles("*.dll"))
        {
            NativeBindings.TL_Tool_Call_Library(file.FullName, "EnablePlugin");
        }
    }
}