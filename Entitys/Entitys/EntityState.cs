using System;
using System.Text.RegularExpressions;

namespace Entitys
{
    public class EntityState
    {
        public static DateTime AddSpan(TimeSpan expire)
        {

            return DateTime.Now.AddDays(expire.Days);
        }
        public static string GenerateRandomInt(int count)
        {
            var random = new Random();
            var result = "";
            for (var i = 0; i < count; i++) result += random.Next(9);
            return result;
        }
        public static string ParsePhone(string phoneNumber)
        {
            var resultString = Regex.Match(phoneNumber, @"\d+").Value;
            if (resultString.Length == 12)
            {
                return resultString;
            }
            if (resultString.Length == 9)
            {
                return "998" + resultString;
            }
            return null;
        }
    }
}
