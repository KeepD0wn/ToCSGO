using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSATest
{
    //[LicenseProvider(typeof(MyControlLicenseProvider))]
    class Program
    {
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerStepThrough]
        [DebuggerNonUserCode]
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.NoInlining)]
        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            //Create a new instance of RSACryptoServiceProvider.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Import the RSA Key information. This only needs
            //toinclude the public key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Encrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            //Create a new instance of RSACryptoServiceProvider.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Import the RSA Key information. This needs
            //to include the private key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Decrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            RSAParameters privateKey;
            RSAParameters publicKey;

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            //Пункт 2
            privateKey = RSA.ExportParameters(true);
            publicKey = RSA.ExportParameters(false);            

            UnicodeEncoding byteConverter = new UnicodeEncoding();
            string toEncrypt = "qwerty1122";
            Console.WriteLine($"To encode: {toEncrypt}");

            byte[] encBytes = RSAEncrypt(byteConverter.GetBytes(toEncrypt), publicKey, false);

            string encrypt = byteConverter.GetString(encBytes);
            Console.WriteLine("Encrypt str: " + encrypt);
            FileStream aFile = new FileStream(@"C:\Users\gvozd\Desktop\а\1.lic", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(aFile);
            aFile.Seek(0, SeekOrigin.End);
            sw.WriteLine(encrypt);
            sw.Close();
            Console.WriteLine("Encrypt bytes: " + string.Join(", ", encBytes));

            byte[] decBytes = RSADecrypt(encBytes, privateKey, false);

            Console.WriteLine("Decrypt str: " + byteConverter.GetString(decBytes));
            Console.WriteLine("Decrypt bytes: " + string.Join(", ", byteConverter.GetBytes(encrypt)));

            Console.ReadKey();
        }
    }
}
