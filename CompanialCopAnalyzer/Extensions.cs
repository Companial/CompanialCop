using System.Text.RegularExpressions;

namespace CompanialCopAnalyzer
{
    internal static class Extensions
    {
        internal static bool ContainsWhiteSpace(string str) => Regex.IsMatch(str, "\\s+", RegexOptions.Compiled);
        internal static bool ContainsWildCardSymbols(string str) => Regex.IsMatch(str, "[\\%\\&]+", RegexOptions.Compiled);
    }
}
