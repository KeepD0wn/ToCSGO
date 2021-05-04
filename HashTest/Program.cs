using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var k="";
            using (var md5 = MD5.Create())
            {
                String path = $@"C:\Users\{Environment.UserName}\Desktop\fg\HashTest.exe";
                using (var stream = File.OpenRead(path))
                {
                    k = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }

            Console.WriteLine(k);
            Console.ReadKey();

        }
    }
}
