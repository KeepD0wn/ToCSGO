using System.Security.Cryptography;
using System.Text;

namespace ecdsa
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("Signature.db")) //если нет файла подписи создаем сообщение
            {
                if (!File.Exists("PrivateKey.db"))//если нет приватного ключа создаем ключ
                {
                    byte[] private_key, public_key;
                    var dsa = CreateECDKey(out private_key, out public_key);//создаем новый ключ
                    File.WriteAllBytes("PrivateKey.db", private_key);//сохраняем приватный и публичный ключ
                    File.WriteAllBytes("PublicKey.db", public_key);
                }
                byte[] msg = GetMessage();//получаем текст сообщения
                var privatekey = File.ReadAllBytes("PrivateKey.db");//читаем приватный ключ
                var signature = GetSignature(privatekey, msg);//получаем подпись сообщения
                File.WriteAllBytes("Signature.db", signature);//сохраняем подпись
                File.WriteAllBytes("Data.txt", msg);//сохраняем сообщение
                Console.WriteLine("Message saved");
            }
            else//если файл подписи существует проверяем сообщение
            {
                var publickey = File.ReadAllBytes("PublicKey.db");//читаем публичный ключ
                byte[] message = File.ReadAllBytes("Data.txt");//читаем сообщение
                Console.WriteLine(Encoding.Default.GetString(message));
                byte[] signature = File.ReadAllBytes("Signature.db");//читаем подпись
                if (VerifyData(publickey, message, signature))//проверяем подпись сообщения
                {
                    Console.WriteLine("Data is good!"); //сообщение корректно
                }
                else
                {
                    Console.WriteLine("Data is bad!"); //сообщение изменено!
                }
                Console.ReadLine();
            }
        }

        static ECDsaCng ecsdKeyVerify;
        private static bool VerifyData(byte[] publickey, byte[] message, byte[] signature)
        {
            if (ecsdKeyVerify == null)
            {
                ecsdKeyVerify = new ECDsaCng(CngKey.Import(publickey, CngKeyBlobFormat.EccPublicBlob));
                ecsdKeyVerify.HashAlgorithm = CngAlgorithm.Sha512;
            }
            if (ecsdKeyVerify.VerifyData(message, signature))
                return true;
            else
                return false;
        }

        static ECDsaCng ecsdKey;
        private static byte[] GetSignature(byte[] privatekey, byte[] msg)
        {
            if (ecsdKey == null)
            {
                ecsdKey = new ECDsaCng(CngKey.Import(privatekey, CngKeyBlobFormat.EccPrivateBlob));
                ecsdKey.HashAlgorithm = CngAlgorithm.Sha512;
            }
            byte[] signature = ecsdKey.SignData(msg);
            if (ecsdKey.VerifyData(msg, signature))
                return signature;
            else
                throw new Exception("Data Verify Failed!");
        }

        private static ECDsaCng CreateECDKey(out byte[] PrivateKey, out byte[] PublicKey, string KeyName = "Ключ шифрования", string keyAlias = "AdminKey")
        {
            var p = new CngKeyCreationParameters
            {
                ExportPolicy = CngExportPolicies.AllowPlaintextExport,
                KeyCreationOptions = CngKeyCreationOptions.OverwriteExistingKey,
                UIPolicy = new CngUIPolicy(CngUIProtectionLevels.ProtectKey, KeyName, null, null, null)
            };
            var key = CngKey.Create(CngAlgorithm.ECDsaP521, keyAlias, p);
            using (ECDsaCng dsa = new ECDsaCng(key))
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha512;
                PublicKey = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
                PrivateKey = dsa.Key.Export(CngKeyBlobFormat.EccPrivateBlob);
                return dsa;
            }
        }

        private static byte[] GetMessage()
        {
            byte[] msg;
            Console.WriteLine("Please write message...");
            var msgtext = Console.ReadLine();
            msg = Encoding.Default.GetBytes(msgtext);
            return msg;
        }
    }
}