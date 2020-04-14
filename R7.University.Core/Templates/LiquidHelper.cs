using System;
using System.Collections.Generic;

namespace R7.University.Core.Templates
{
    public static class LiquidHelper
    {
        static string GetNextLiquidObject (string value, int startIndex, out int liquidObjectIdx)
        {
            liquidObjectIdx = -1;
            var openBracketIdx = value.IndexOf ("{{", startIndex, StringComparison.Ordinal);
            var closeBracketIdx = value.IndexOf ("}}", startIndex, StringComparison.Ordinal);

            if (openBracketIdx >= 0 && closeBracketIdx >= 0 && openBracketIdx < closeBracketIdx) {
                liquidObjectIdx = openBracketIdx;
                return value.Substring (openBracketIdx, closeBracketIdx - openBracketIdx + 2);
            }

            return null;
        }

        public static IEnumerable<string> GetLiquidObjects (string value)
        {
            var liquidObject = GetNextLiquidObject (value, 0, out int liquidObjectIdx);
            while (liquidObject != null) {
                yield return liquidObject;
                liquidObject = GetNextLiquidObject (value, liquidObjectIdx + liquidObject.Length, out liquidObjectIdx);
            }
        }

        public static bool ContainsLiquidTag (string value)
        {
            var openBracketIdx = value.IndexOf ("{%", StringComparison.Ordinal);
            var closeBracketIdx = value.IndexOf ("%}", StringComparison.Ordinal);

            return openBracketIdx >= 0 && closeBracketIdx >= 0 && openBracketIdx < closeBracketIdx;
        }

        public static string UnwrapLiquidObject (string obj)
        {
            return obj.TrimStart ('{').TrimEnd ('}').Trim ();
        }

        public static string UnwrapLiquidTag (string obj)
        {
            return obj.TrimStart ('{').TrimEnd ('}').Trim ('%').Trim ();
        }
    }
}
