using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Infrastructures.Extensions
{
    public static class StringExtentions
    {
        public static string Replace(this string str, Dictionary<string,string> replacements)
        {
            foreach (string textToReplace in replacements.Keys)
            {
                str = str.Replace(textToReplace, replacements[textToReplace]);
            }
            return str;
        }
    }
}
