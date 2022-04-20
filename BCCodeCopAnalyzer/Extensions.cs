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
        internal static readonly Lazy<Regex> EmailPatternLazy = new Lazy<Regex>((Func<Regex>)(() => new Regex("([\\w\\-\\.]+)@((\\[([0-9]{1,3}\\.){3}[0-9]{1,3}\\])|(([\\w\\-]+\\.)+)([a-zA-Z]{2,4}))", RegexOptions.Compiled)));
        internal static readonly Lazy<Regex> PhoneNoPatternLazy = new Lazy<Regex>((Func<Regex>)(() => new Regex("(([+][(]?[0-9]{1,3}[)]?)|([(]?[0-9]{4}[)]?))\\s*[)]?[-\\s\\.]?[(]?[0-9]{1,3}[)]?([-\\s\\.]?[0-9]{3})([-\\s\\.]?[0-9]{3,4})", RegexOptions.Compiled)));
    }
}
