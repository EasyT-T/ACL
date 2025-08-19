namespace ACL.Extension;

using System.Drawing;

public static class ColorExtensions
{
    public static int ToHex(this Color color)
    {
        var r = color.R;
        var g = color.G;
        var b = color.B;
        var a = color.A;
        var hexColor = r << 24 | g << 16 | b << 8 | a;

        return hexColor;
    }

    public static Color ToColor(this int hex)
    {
        var hexColor = (uint)hex;
        var r = (int)(hexColor & 0xFF000000 >> 24);
        var g = (int)(hexColor & 0x00FF0000 >> 16);
        var b = (int)(hexColor & 0x0000FF00 >> 8);
        var a = (int)(hexColor & 0x000000FF);

        return Color.FromArgb(a, r, g, b);
    }
}