using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography;

namespace Hard_Chatik_WPF
{
    internal static class Cryptographer
    {
        public static string Encrypt(string password)
        {
            SHA256 sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder hashedPassword = new StringBuilder();
            foreach (byte b in hash)
            {
                hashedPassword.Append(b.ToString("x2"));
            }

            return hashedPassword.ToString();
        }
    }
}
