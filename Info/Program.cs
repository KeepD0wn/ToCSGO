using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Security.Cryptography;

namespace Info
{
    class Program
    {        

        public static string ShowSystemInfo()
        {
            StringBuilder stringBuild = new StringBuilder();
            String host = System.Net.Dns.GetHostName();
            System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
            string ipAdress = ip.ToString();
            string userName = Environment.UserName;

            Console.WriteLine("Wait");
            ManagementObjectSearcher searcher =new ManagementObjectSearcher("root\\CIMV2","Select Name, CommandLine From Win32_Process");
            foreach (ManagementObject instance in searcher.Get())
            {
                stringBuild.AppendLine($"Process: {instance["Name"]}");
            }
            
            stringBuild.AppendLine("UserIP: " + ipAdress);

            Console.WriteLine("Wait");
            ManagementObjectSearcher searcher_soft =new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_Product");
            foreach (ManagementObject queryObj in searcher_soft.Get())
            {
                stringBuild.AppendLine($"ProductCaption: {queryObj["Caption"]} ; InstallDate: {queryObj["InstallDate"]}</soft>");
            }

            stringBuild.AppendLine("UserName: " + userName);

            Console.WriteLine("Wait");
            ManagementObjectSearcher searcher11 =new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_VideoController");
            foreach (ManagementObject queryObj in searcher11.Get())
            {               
                stringBuild.AppendLine("----------- Win32_VideoController instance -----------");
                stringBuild.AppendLine($"VideoAdapterRAM: {queryObj["AdapterRAM"]}");
                stringBuild.AppendLine($"VideoCaption: {queryObj["Caption"]}");
                stringBuild.AppendLine($"VideoDescription: {queryObj["Description"]}");
                stringBuild.AppendLine($"VideoProcessor: {queryObj["VideoProcessor"]}");
            }

            Console.WriteLine("Wait");
            ManagementObjectSearcher searcher5 = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject queryObj in searcher5.Get())
            {
                stringBuild.AppendLine("------------------- Win32_OperatingSystem instance ------------------------");
                stringBuild.AppendLine($"OperatingSystemBuildNumber: {queryObj["BuildNumber"]}");
                stringBuild.AppendLine($"OperatingSystemCaption: {queryObj["Caption"]}");
                stringBuild.AppendLine($"OperatingSystemName: {queryObj["Name"]}");
                stringBuild.AppendLine($"OperatingSystemRegisteredUser: {queryObj["RegisteredUser"]}");
                stringBuild.AppendLine($"OperatingSystemSerialNumber: {queryObj["SerialNumber"]}");
            }

            Console.WriteLine("Wait");
            ManagementObjectSearcher searcher8 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher8.Get())
            {
                stringBuild.AppendLine("------------- Win32_Processor instance ---------------");
                stringBuild.AppendLine($"ProcessorName: {queryObj["Name"]}");
                stringBuild.AppendLine($"ProcessorNumberOfCores: {queryObj["NumberOfCores"]}");
                stringBuild.AppendLine($"ProcessorProcessorId: {queryObj["ProcessorId"]}");
            }

            Console.WriteLine("Wait");
            ManagementObjectSearcher searcher1 =new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_Volume");
            foreach (ManagementObject queryObj in searcher1.Get())
            {        
                stringBuild.AppendLine("------------------- Win32_Volume instance -------------------");
                stringBuild.AppendLine($"VolumeCapacity: {queryObj["Capacity"]}");
                stringBuild.AppendLine($"VolumeCaption: {queryObj["Caption"]}");
                stringBuild.AppendLine($"VolumeDriveLetter: {queryObj["DriveLetter"]}");
                stringBuild.AppendLine($"VolumeDriveType: {queryObj["DriveType"]}");
                stringBuild.AppendLine($"VolumeFileSystem: {queryObj["FileSystem"]}");
                stringBuild.AppendLine($"VolumeFreeSpace: {queryObj["FreeSpace"]}");
            }

            return stringBuild.ToString();
        }

        static void Main(string[] args)
        {
            try
            {
                string str = ShowSystemInfo();

                FileStream aFile = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}\Info.bg", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(aFile);
                aFile.Seek(0, SeekOrigin.End);
                sw.WriteLine(str);
                sw.Close();

                Console.WriteLine("Done");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }            

            Console.ReadKey();
        }
    }
}
