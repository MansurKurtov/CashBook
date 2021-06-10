using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Helpers
{
    public static class CryptoPasword
    {
        public const int SALT_SIZE = 32; // size in bytes
        public const int HASH_SIZE = 64; // size in bytes
        public const int ITERATIONS = 1000; // number of pbkdf2 iterations

        public class HashSalt
        {
            public string Hash { get; set; }
            public string Salt { get; set; }
        }

        public static HashSalt CreateHashSalted(string password)
        {

            var saltBytes = new byte[SALT_SIZE];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);

            var salt = ByteArrayToString(saltBytes);
            Byte[] byteValue = Encoding.UTF8.GetBytes(salt);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, byteValue, ITERATIONS);
            var hashPassword = ByteArrayToString(rfc2898DeriveBytes.GetBytes(HASH_SIZE));


            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

        public static string GetHashSalted(string password, string salt)
        {

            byte[] bsalt = Encoding.Default.GetBytes(salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bsalt, ITERATIONS);
            var hashPassword = ByteArrayToString(rfc2898DeriveBytes.GetBytes(HASH_SIZE));
            return hashPassword;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

    }
}
