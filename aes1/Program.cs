using System.Security.Cryptography;
using System.Text;

namespace aes1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string original = "Here is some data to encrypt! sdfffffffffffffssssssssss dfsdfsrt sdeg4wet  gsg sdg sdg g ewg x g3 dfgedg3t fg dg32 egdfg 3 dfgd d  3 tegx gdg3t dfg dg3 tdfg dg 3t dfg dg 3t4 fdg";

            byte[] key = Convert.FromHexString("9E111B0A8EB5440711CC485D325C7F410A70C8A1BD94F4B9FB3DFD24DD5D9047");
            byte[] IV = Convert.FromHexString("7B6D7F876DFED21517ABA903831378EE");

            // Encrypt the string to an array of bytes.
            byte[] encryptedByte = EncryptStringToBytes_Aes(original, key, IV);
            string encrypted = Convert.ToBase64String(encryptedByte);
            Console.WriteLine(encrypted);

            // Decrypt the bytes to a string.
            byte[] bytes = Convert.FromBase64String(encrypted);
            string roundtrip = DecryptStringFromBytes_Aes(bytes, key, IV);

            

            //Display the original data and the decrypted data.
            Console.WriteLine("key:   {0}", Convert.ToHexString(key));
            Console.WriteLine("vector:   {0}", Convert.ToHexString(IV));
            Console.WriteLine("Original:   {0}", original);            
            Console.WriteLine("Round Trip: {0}", roundtrip);
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}