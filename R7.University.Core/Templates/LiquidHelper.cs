using System;

namespace R7.University.Core.Templates
{
    public static class LiquidHelper
    { 
        public static string UnwrapLiquidObject (string obj)
        {
            return obj.TrimStart ('{').TrimEnd ('}').Trim ();
        }

        public static string UnwrapLiquidTag (string obj)
        {
            return obj.TrimStart ('{').TrimEnd ('}').Trim ('%').Trim ();
        }

        public static bool IsLiquidObject (string value)
        {
            return value.StartsWith ("{{", StringComparison.Ordinal) && value.EndsWith ("}}", StringComparison.Ordinal);
        }

        public static bool IsLiquidTag (string value)
        {
            return value.StartsWith ("{%", StringComparison.Ordinal) && value.EndsWith ("%}", StringComparison.Ordinal);
        }
    }
}
