using System.Text.RegularExpressions;

namespace SSH.Core.Extensions
{
    public static class StringExtension
    {
        public static string RemoveSpaces(this string reportName)
        {
            return Regex.Replace(reportName, @"\s+", string.Empty);
        }
    }
}
