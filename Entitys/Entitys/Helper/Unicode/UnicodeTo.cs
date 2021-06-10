using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.Helper.Unicode
{
    public class UnicodeTo
    {
        public static string unicodeToString(string str)
        {
            int index = str.IndexOf("&#");
            if (index > -1) 
            {
                int get = str.IndexOf(";") - index;
                string strs = str.Substring(index + 2, get - 2);
                str = str.Replace("&#", "");
                str = str.Replace(";", "");
                string b = Convert.ToString(Convert.ToChar(Convert.ToInt32(strs)));
                str = str.Replace(strs, b);
            }

            return str;
        }
    }
}
