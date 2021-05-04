using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReestr
{
    class Program
    {
        static void Main(string[] args)
        {
            RegistryKey currentUserKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("Steam_Data", true);
            if (currentUserKey==null)
            {
                RegistryKey createData = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Steam_Data");
                createData.SetValue("user", ""); //2 аргумент это хэш сборки
            }
            currentUserKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("Steam_Data", true);
            string dataReestr = currentUserKey.GetValue("user").ToString();
            //currentUserKey.Close();

            // string login = helloKey.GetValue("user").ToString();
             
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
