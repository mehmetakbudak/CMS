using System;
using System.Security.Cryptography;
using System.Text;

namespace CMS.Service.Infrastructure
{
    public static class Security
    {
        public static string MD5Crypt(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(text);
            btr = md5.ComputeHash(btr);
            StringBuilder sb = new StringBuilder();
            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        public static string RandomBase64(int bitCount = 64)
        {
            var provider = new RNGCryptoServiceProvider();

            var buffer = new byte[bitCount];

            provider.GetBytes(buffer);

            var randomKey = Convert.ToBase64String(buffer);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(randomKey));
        }
    }
}