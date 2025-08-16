namespace ERF.Loader;

using System.Diagnostics.CodeAnalysis;
using ERF.Feature;
using ERF.Managed;

public abstract class Plugin
{
    public abstract string Name { get; }

    protected internal ModuleContext ModuleContext { get; internal set; } = null!;

    protected internal EventManager EventManager { get; internal set; } = null!;

    public abstract void EnablePlugin();
}