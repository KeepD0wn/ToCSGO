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
      
        static void Main(string[] args4)
        {
            Console.Title = "Licenser";
            Console.WriteLine("Password: ");
            string g = GetPassword();
            MD5 md = new MD5CryptoServiceProvider();
            byte[] checkSum1 = md.ComputeHash(Encoding.UTF8.GetBytes(g));
            string result1 = BitConverter.ToString(checkSum1).Replace("-", String.Empty);

            if (result1 == "687CA27241454EAAF7BEE30D7DD9EE1F")
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
                    ushort s = 0x9025;

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
                            str = Ed(str, s);

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

                    FileStream aFile1 = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\DONOTSEND.txt", FileMode.OpenOrCreate);
                    StreamWriter sw1 = new StreamWriter(aFile1);
                    aFile1.Seek(0, SeekOrigin.End);
                    sw1.WriteLine(final);
                    sw1.Close();

                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(final));
                    string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);

                    MySqlConnection conn = new MySqlConnection();
                    try
                    {
                        conn = new MySqlConnection(Properties.Resources.String1);
                        conn.Open();

                        var com = new MySqlCommand("USE `MySQL-5846`; " +
                            "insert into `subs` (keyLic, activeLic)" +
                            " values (@keyLic, @activeLic)", conn);
                        com.Parameters.AddWithValue("@keyLic", result);
                        com.Parameters.AddWithValue("@activeLic", 1);
                        com.ExecuteNonQuery();
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
                    

                    FileStream aFile = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\License.lic", FileMode.OpenOrCreate);
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
