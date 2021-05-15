using System.Text;

namespace He.Iota.Net.Client.Extensions
{
    public static class StringExtension
    {
        public static string ToHex(this string source)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.UTF8.GetBytes(source);

            foreach (var t in bytes)
            {
                sb.Append(t.ToString("x"));
            }

            return sb.ToString();
        }
    }
}
