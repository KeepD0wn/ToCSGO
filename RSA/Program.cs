using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        public static string Encrypt(string data, RSAParameters key)
        {

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(key);
                var byteData = Encoding.UTF8.GetBytes(data);
                var encryptData = rsa.Encrypt(byteData, false);
                return Convert.ToBase64String(encryptData);
            }
        }

        public static string Decrypt(string cipherText, RSAParameters key)
        {

            using (var rsa = new RSACryptoServiceProvider())
            {
                var cipherByteData = Convert.FromBase64String(cipherText);
                rsa.ImportParameters(key);

                var encryptData = rsa.Decrypt(cipherByteData, false);
                return Encoding.UTF8.GetString(encryptData);
            }
        }

        static void Main(string[] args)
        {
            CspParameters cs = new CspParameters();
            cs.KeyContainerName = "fa";
            

            RSACryptoServiceProvider RsaKey = new RSACryptoServiceProvider(cs);
            string publickey = RsaKey.ToXmlString(false); //получим открытый ключ
            string privatekey = RsaKey.ToXmlString(true); //получим закрытый ключ

           

            File.WriteAllText("private.xml", privatekey, Encoding.UTF8);
            File.WriteAllText("public.xml", publickey, Encoding.UTF8);


            byte[] EncryptedData;
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes("Hello");
            EncryptedData = RsaKey.Encrypt(data, false);


            byte[] DecryptedData = new byte[1024];
            DecryptedData = RsaKey.Decrypt(EncryptedData, false);


            RSACryptoServiceProvider RsaKey_n = new RSACryptoServiceProvider();
            RsaKey_n.FromXmlString(publickey + privatekey); //можно использовать только открытый ключ, или пару открытый и закрыты 
            DecryptedData = new byte[1024];
            DecryptedData = RsaKey.Decrypt(EncryptedData, false);
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(DecryptedData));


            Console.Read();
        }
    }
}
