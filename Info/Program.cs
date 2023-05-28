using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Info
{
    class Program
    {        

        public static string GetInfo()
        {
            StringBuilder stringBuild = new StringBuilder();
            String host = System.Net.Dns.GetHostName();
            System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];            
            stringBuild.AppendLine("MotherBoard: "+ Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\BIOS", "BaseBoardProduct", 0).ToString());

            Console.WriteLine("Wait");
            ManagementObjectSearcher searcher11 =new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_VideoController");
            foreach (ManagementObject queryObj in searcher11.Get())
            {        
                stringBuild.AppendLine($"VideoAdapterRAM: {queryObj["AdapterRAM"]}");
                stringBuild.AppendLine($"VideoCaption: {queryObj["Caption"]}");
                stringBuild.AppendLine($"VideoDescription: {queryObj["Description"]}");
                stringBuild.AppendLine($"VideoProcessor: {queryObj["VideoProcessor"]}");
            }
          
            ManagementObjectSearcher searcher5 = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject queryObj in searcher5.Get())
            {
                stringBuild.AppendLine($"OperatingSystemBuildNumber: {queryObj["BuildNumber"]}");
                stringBuild.AppendLine($"OperatingSystemCaption: {queryObj["Caption"]}");
                stringBuild.AppendLine($"OperatingSystemName: {queryObj["Name"]}");
                stringBuild.AppendLine($"OperatingSystemRegisteredUser: {queryObj["RegisteredUser"]}");
                stringBuild.AppendLine($"OperatingSystemSerialNumber: {queryObj["SerialNumber"]}");
            }
           
            ManagementObjectSearcher searcher8 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher8.Get())
            {
                stringBuild.AppendLine($"ProcessorName: {queryObj["Name"]}");
                stringBuild.AppendLine($"ProcessorNumberOfCores: {queryObj["NumberOfCores"]}");
                stringBuild.AppendLine($"ProcessorProcessorId: {queryObj["ProcessorId"]}");
            }

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject info in searcher.Get())
            {
                if (info["DeviceID"].ToString().Contains("PHYSICALDRIVE0"))
                {
                    stringBuild.AppendLine("Disk Model: " + info["Model"].ToString());
                    stringBuild.AppendLine("Disk Interface: " + info["InterfaceType"].ToString());
                    stringBuild.AppendLine("Disk Serial: " + info["SerialNumber"].ToString());
                }
            }

            ManagementObjectSearcher searcher10 = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT UUID FROM Win32_ComputerSystemProduct");
            foreach (ManagementObject queryObj in searcher10.Get())
                stringBuild.AppendLine("UUID: " + queryObj["UUID"].ToString());

            ManagementObjectSearcher searcher1 =new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_Volume");
            foreach (ManagementObject queryObj in searcher1.Get())
            {        
                stringBuild.AppendLine($"VolumeCapacity: {queryObj["Capacity"]}");
                stringBuild.AppendLine($"VolumeCaption: {queryObj["Caption"]}");
                stringBuild.AppendLine($"VolumeDriveLetter: {queryObj["DriveLetter"]}");
                stringBuild.AppendLine($"VolumeDriveType: {queryObj["DriveType"]}");
                stringBuild.AppendLine($"VolumeFileSystem: {queryObj["FileSystem"]}");
                stringBuild.AppendLine($"VolumeFreeSpace: {queryObj["FreeSpace"]}");
            }

            return stringBuild.ToString();
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {            
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

        public static byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        static void Main(string[] args)
        {
            Console.Title = "Get Key";
            try
            {
                byte[] key = FromHex("9E111B0A8EB5440711CC485D325C7F410A70C8A1BD94F4B9FB3DFD24DD5D9047");
                byte[] IV = FromHex("7B6D7F876DFED21517ABA903831378EE");

                string info = GetInfo();
                byte[] encryptedByte = EncryptStringToBytes_Aes(info, key, IV);
                string encryptedStr = Convert.ToBase64String(encryptedByte);

                FileStream aFile = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\Info.bg", FileMode.Create);
                StreamWriter sw = new StreamWriter(aFile);
                aFile.Seek(0, SeekOrigin.End);
                sw.WriteLine(encryptedStr);
                sw.Close();

                Console.WriteLine("Done");
            }
            catch
            {
                Console.WriteLine("[SYSTEM] Something went wrong");
            }
            Console.ReadKey();
        }
    }
}