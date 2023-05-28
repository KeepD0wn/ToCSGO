using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Numerics;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Common;

namespace GetInfo
{
    class Program
    {
        public static string Ed(string str, ushort secretKey)
        {
            var ch = str.ToArray();
            string newStr = "";
            foreach (var c in ch)
                newStr += Some(c, secretKey);
            return newStr;
        }

        public static char Some(char character, ushort secretKey)
        {
            character = (char)(character ^ secretKey);
            return character;
        }

        private static void SetData()
        {
            RegistryKey currentUserKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("Steam_Data", true);
            if (currentUserKey == null)
            {
                RegistryKey createData = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Steam_Data");
                createData.SetValue("user", ""); //2 аргумент это хэш сборки
            }
            currentUserKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("Steam_Data", true);
            string dataReestr = currentUserKey.GetValue("user").ToString();
        }

        private static string GetPassword()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.SetCursorPosition(x - 1, y);
                    Console.Write(" ");
                    Console.SetCursorPosition(x - 1, y);
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    input.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            return input.ToString();
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

        public static byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        static void Main(string[] args4)
        {
            Console.Title = "Licenser";
            Console.WriteLine("Password: ");
            string g = GetPassword();
            MD5 md = new MD5CryptoServiceProvider();
            byte[] checkSum1 = md.ComputeHash(Encoding.UTF8.GetBytes(g));
            string result1 = BitConverter.ToString(checkSum1).Replace("-", String.Empty);

            if (result1 == "AD6B07AE560C4ED221F74D4E904CFE6F")
            {
                try
                {
                    string final = "";
                    string mother = "";
                    string uuid = "";
                    string videoProcessor = "";
                    string processorName = "";
                    string processorNumberOfCores = "";
                    string processorProcessorId = "";
                    string diskModel = "";
                    string diskInterface = "";
                    string diskSerial = "";
                    byte[] key = FromHex("9E111B0A8EB5440711CC485D325C7F410A70C8A1BD94F4B9FB3DFD24DD5D9047");
                    byte[] IV = FromHex("7B6D7F876DFED21517ABA903831378EE");

                    string[] subs = default;
                    if (File.Exists($@"{AppDomain.CurrentDomain.BaseDirectory}\Info.bg"))
                    {
                        string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\Info.bg";
                        string str = "";
                        try
                        {
                            using (StreamReader sr = new StreamReader(path))
                            {
                                str = sr.ReadToEnd();
                            }

                            byte[] bytes = Convert.FromBase64String(str);
                            str = DecryptStringFromBytes_Aes(bytes, key, IV);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        subs = str.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        Console.WriteLine("[SYSTEM] Info.bg not found");
                    }

                    for (int i = 0; i < subs.Length; i++)
                    {
                        if (subs[i].Contains("UUID"))
                        {
                            string[] parts = subs[i].Split(':');
                            uuid = parts[1];
                        }
                        if (subs[i].Contains("MotherBoard"))
                        {
                            string[] parts = subs[i].Split(':');
                            mother = parts[1];
                        }
                        if (subs[i].Contains("VideoProcessor"))
                        {
                            string[] parts = subs[i].Split(':');
                            videoProcessor = parts[1];
                        }
                        if (subs[i].Contains("ProcessorName"))
                        {
                            string[] parts = subs[i].Split(':');
                            processorName = parts[1];
                        }
                        if (subs[i].Contains("ProcessorNumberOfCores"))
                        {
                            string[] parts = subs[i].Split(':');
                            processorNumberOfCores = parts[1];
                        }
                        if (subs[i].Contains("ProcessorProcessorId"))
                        {
                            string[] parts = subs[i].Split(':');
                            processorProcessorId = parts[1];
                        }
                        if (subs[i].Contains("Disk Model"))
                        {
                            string[] parts = subs[i].Split(':');
                            diskModel = parts[1];
                        }
                        if (subs[i].Contains("Disk Interface"))
                        {
                            string[] parts = subs[i].Split(':');
                            diskInterface = parts[1];
                        }
                        if (subs[i].Contains("Disk Serial"))
                        {
                            string[] parts = subs[i].Split(':');
                            diskSerial = parts[1];
                        }
                    }
                    final = uuid +"|"+ mother + "|" + videoProcessor + "|" + processorName + "|" + processorNumberOfCores + "|" + processorProcessorId + "|" + diskModel + "|" + diskInterface + "|" + diskSerial+"|";
                    final = final.Replace(" ", "").Replace("-", "").ToLower();

                    //FileStream aFile1 = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\DONOTSEND.txt", FileMode.Create);
                    //StreamWriter sw1 = new StreamWriter(aFile1);
                    //aFile1.Seek(0, SeekOrigin.End);
                    //sw1.WriteLine(final);
                    //sw1.Close();

                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(final));
                    string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);

                    MySqlConnection conn = new MySqlConnection();
                    try
                    {
                        conn = new MySqlConnection(Properties.Resources.String1);                        
                        conn.Open();

                        var command = new MySqlCommand("USE subs; " +
                            "insert into `subs` (keyLic, activeLic, info)" +
                            " values (@keyLic, @activeLic, @info)", conn);
                        command.Parameters.AddWithValue("@keyLic", result);
                        command.Parameters.AddWithValue("@activeLic", 1);
                        command.Parameters.AddWithValue("@info", final);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch(Exception ex)
                    {                        
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {                        
                        conn.Close();
                    }
                    

                    FileStream aFile = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\License.lic", FileMode.Create);
                    StreamWriter sw = new StreamWriter(aFile);
                    aFile.Seek(0, SeekOrigin.End);
                    sw.WriteLine(result);
                    sw.Close();

                    if (uuid!="" && mother != "" && videoProcessor != "" && processorName != "" && processorNumberOfCores != "" && processorProcessorId != "" && diskModel != "" && diskInterface != "" && diskSerial != "")
                        Console.WriteLine("Done");
                    else
                        Console.WriteLine("Not enought data");
                }
                catch
                {
                    Console.WriteLine("[SYSTEM] Something went wrong");
                }
            }
            else
            {
                Console.WriteLine("Wrong password");
            }    
            Console.ReadKey();      
        }
    }
}
