namespace ACL.Util;

using System.Text;

public static class BlitzTool
{
    public static string HashIdent(string ident)
    {
        var stringBuilder = new StringBuilder();
        var hashed = HashFunction(ident).ToString();

        for (var i = 0; i < hashed.Length; i++)
        {
            stringBuilder.Append(hashed[i] % 2 == 0 && i > 0 ? hashed[i] : (char)(65 + hashed[i] % 26));
        }

        return stringBuilder.ToString();

        int HashFunction(string str)
        {
            var hash = 0;

            foreach (var c in str)
            {
                hash += c;
                hash += hash << 10;
                hash ^= hash >> 6;
            }

            hash += hash << 3;
            hash ^= hash >> 11;
            hash += hash << 15;

            return hash;
        }
    }
}