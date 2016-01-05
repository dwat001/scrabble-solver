using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakariki.Scrabble.Logic
{
    public static class ExtensionMethods
    {
        public static IEnumerable<int> AllIndexesOf(this string s, char c)
        {
            int startIndex = 0;
            int foundIndex;
            while ((foundIndex = s.IndexOf(c, startIndex)) >= 0)
            {
                startIndex = foundIndex + 1;
                yield return foundIndex;
            }
            yield break;
        }

        public static string ToExpandedString<T>(this IEnumerable<T> enumerable)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            enumerable.All(t => { sb.Append(t).Append(", "); return true; });
            sb.Append(']');
            return sb.ToString();
        }
    }
}
