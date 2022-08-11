using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCCodeCopAnalyzer
{
    internal static class Extensions
    {
        internal static bool ContainsWhiteSpace(string str) => Regex.IsMatch(str, "\\s+", RegexOptions.Compiled);
        internal static bool ContainsWildCardSymbols(string str) => Regex.IsMatch(str, "[\\%\\&]+", RegexOptions.Compiled);
    }
}
