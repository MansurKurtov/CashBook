using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitys.Helper.NumberToText
{
    public class Numbers
    {
        static string[] nums_1_9 = "ноль бир икки уч тўрт беш олти етти саккиз тўққиз".Split();
        static string[] nums_10_19 = { "ўн", "ўн бир", "ўн икки", "ўн уч", "ўн тўрт", "ўн беш", "ўн олти", "ўн етти", "ўн саккиз", "ўн тўққиз" };
        static string[] nums_20_90 = "ноль ўн йигирма ўттиз қирқ эллик олтмиш етмиш саксон тўқсон".Split();
        static string[] nums_100_900 = { "ноль", "бир юз", "икки юз", "уч юз", "тўрт юз", "беш юз", "олти юз", "етти юз", "саккиз юз", "тўққиз юз" };
        static string[] razrad = @" минг миллион миллиард триллион квадриллион квинтиллион секстиллион септиллион октиллион нониллион дециллион андециллион дуодециллион тредециллион кваттордециллион квиндециллион сексдециллион септемдециллион октодециллион новемдециллион вигинтиллион анвигинтиллион дуовигинтиллион тревигинтиллион кватторвигинтиллион квинвигинтиллион сексвигинтиллион септемвигинтиллион октовигинтиллион новемвигинтиллион тригинтиллион антригинтиллион".Split();



        public static string GetNumber(string number) 
        {
            string result = string.Empty;
            foreach (var s in solve(splitIntoCategories(number))) result=result+s;

            return result;
        }
        static IEnumerable<string> splitIntoCategories(string s)
        {
            s = s.PadLeft(s.Length + 3 - s.Length % 3, '0');
            return Enumerable.Range(0, s.Length / 3).Select(i => s.Substring(i * 3, 3));
        }

        static IEnumerable<string> solve(IEnumerable<string> n)
        {
            var ii = 0;
            foreach (var s in n)
            {
                var countdown = n.Count() - ++ii;
                yield return
                    String.Format(@"{0} {1} {2} {3}",
                        s[0] == '0' ? "" : nums_100_900[getDigit(s[0])],
                        getE1(s[1], s[2]),
                        getE2(s[1], s[2], countdown),
                        s == "000" ? "" : getRankName(s, countdown)
                    );
            }

        }

        private static string getE1(char p1, char p2)
        {
            if (p1 != '0')
            {
                if (p1 == '1')
                    return nums_10_19[getDigit(p2)];
                return nums_20_90[getDigit(p1)];
            }
            return "";
        }

        private static string getE2(char p1, char p2, int cd)
        {
            if (p1 != '1')
            {
                if (p2 == '0') return "";
                return nums_1_9[getDigit(p2)];
            }
            return "";
        }

        private static int getDigit(char p1)
        {
            return Int32.Parse(p1.ToString());
        }

        private static string getRankName(string s, int ii)
        {
            if (ii == 0) return "";
            var r = razrad[ii];
            return r;
        }
    }
}
