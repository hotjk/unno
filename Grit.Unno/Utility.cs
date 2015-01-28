using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grit.Unno
{
    public static class Utility
    {
        private static Regex alphabet = new Regex(@"^[A-Za-z0-9]+$");
        public static void EnsoureRegularKey(this string str)
        {
            if (str.Length > UIConstants.MaxUnnoKeyLength || !alphabet.IsMatch(str))
            {
                throw new ApplicationException("Invalid key.");
            }
        }
        public static string ToDateString(this DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return string.Empty;
            }
            if(dt.Value.Date == dt.Value)
            {
                return dt.Value.ToString("yyyy-MM-dd");
            }
            return dt.Value.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
