using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanzApi.Utils.Generators
{
    public static class KeyGenerator
    {
        private static readonly string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";

        public static string Get128BytesCode()
        {
            var rand = new Random();
            var code = "";

            for (int i = 0; i < 16; i++)
            {
                code += chars[rand.Next(chars.Length)];
            }

            return code;
        }
    }
}
