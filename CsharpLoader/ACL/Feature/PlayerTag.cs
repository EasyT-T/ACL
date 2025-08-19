namespace ACL.Feature;

using System.Drawing;
using System.Numerics;

public readonly struct PlayerTag(string text, Vector2 scale, float offset, Color startColor, Color endColor, string? fontName = null)
{
    public string Text { get; } = text;

    public Vector2 Scale { get; } = scale;

    public float Offset { get; } = offset;

    public Color StartColor { get; } = startColor;

    public Color EndColor { get; } = endColor;

    public string? FontName { get; } = fontName;
}