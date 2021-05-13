using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;
using Microsoft.Win32;

namespace Info
{
    class Program
    {        

        public static string Iujfl()
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
      
        static void Main(string[] args)
        {
            Console.Title = "Get Key";
            try
            {
                ushort s = 0x9025;
                string str = Iujfl();
                str = Ed(str, s);

                FileStream aFile = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\Info.bg", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(aFile);
                aFile.Seek(0, SeekOrigin.End);
                sw.WriteLine(str);
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