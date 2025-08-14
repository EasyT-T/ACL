namespace ERF.Loader;

using System.Reflection;
using System.Runtime.Loader;

public class PluginAssemblyLoadContext : AssemblyLoadContext
{
    public PluginAssemblyLoadContext(string? name, Assembly baseAssembly) : base(name)
    {
        Resolving += (_, assemblyName) => assemblyName.FullName == baseAssembly.FullName ? baseAssembly : null;
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        return null;
    }
}