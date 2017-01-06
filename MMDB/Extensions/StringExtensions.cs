using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDB.Extensions
{
    public static class StringExtensions
    {
        public static String Substring(this String str, String start, String end)
        {
            int from = str.IndexOf(start, StringComparison.Ordinal) + start.Length;
            int to = str.LastIndexOf(end, StringComparison.Ordinal);
            return str.Substring(from, to - from);
        }
    }
}
