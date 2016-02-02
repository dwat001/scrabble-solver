using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kakariki.Scrabble.Logic
{
    public static class ExtensionMethods
    {
        private const string ITEM_SEPERATOR = ", ";
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
            enumerable.All(t => { sb.Append(t).Append(ITEM_SEPERATOR); return true; });
            if (sb.Length > 1)
            {
                sb.Length -= ITEM_SEPERATOR.Length;
            }
            sb.Append(']');
            return sb.ToString();
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static T ThrowIfNull<T>(this T obj, string parameterName = null)
                where T : class
        {

            if (obj == null) throw new ArgumentNullException(parameterName);
            return obj;
        }
    }
}
