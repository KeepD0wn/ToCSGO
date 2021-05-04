using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GetInfo
{
    class Program
    {
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

        static void Main(string[] args)
        {           
            Console.WriteLine("Password: ");
            string g =Console.ReadLine();
            MD5 md = new MD5CryptoServiceProvider();
            byte[] checkSum1 = md.ComputeHash(Encoding.UTF8.GetBytes(g));
            string result1 = BitConverter.ToString(checkSum1).Replace("-", String.Empty);

            if (result1 == "687CA27241454EAAF7BEE30D7DD9EE1F")
            {
                try
                {
                    string final = "";
                    string userIP = "";
                    string userName = "";
                    string videoProcessor = "";
                    string processorName = "";
                    string processorNumberOfCores = "";
                    string processorProcessorId = "";

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
                        if (subs[i].Contains("UserIP"))
                        {
                            string[] parts = subs[i].Split(':');
                            userIP = parts[1];
                        }
                        if (subs[i].Contains("UserName"))
                        {
                            string[] parts = subs[i].Split(':');
                            userName = parts[1];
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
                    }
                    final = userIP + userName + videoProcessor + processorName + processorNumberOfCores + processorProcessorId;
                    final = final.Replace(" ", "").Replace("-", "").ToLower();

                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(final));
                    string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);


                    FileStream aFile = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\License.lic", FileMode.OpenOrCreate);
                    StreamWriter sw = new StreamWriter(aFile);
                    aFile.Seek(0, SeekOrigin.End);
                    sw.WriteLine(result);
                    sw.Close();

                    if (userIP != "" && userName != "" && videoProcessor != "" && processorName != "" && processorNumberOfCores != "" && processorProcessorId != "")
                        Console.WriteLine("Done");
                    else
                        Console.WriteLine("Not enought data");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
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
