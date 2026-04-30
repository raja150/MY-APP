using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
namespace TranSmart.Core.Util
{
    public static class Encrypt
    {
        public static string HashPassword(string text)
        {
            return BC.HashPassword(text);
        }

        public static bool Verify(string text, string hash)
        {
            return BC.Verify(text, hash);
        }
    }
}
