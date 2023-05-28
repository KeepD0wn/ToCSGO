﻿using System.Security.Cryptography;
using System.Text;

namespace aes
{
    internal class Program
    {
        private static byte[] GetIV(string ivSecret)
        {
            using MD5 md5 = MD5.Create();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(ivSecret));
        }
        private static byte[] GetKey(string key)
        {
            using SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        }


        static void Main(string[] args)
        {
            //string sourceFileName = "secretfile.txt"; //файл, который будем шифровать
            string outputFileName = "secretfile.enc"; //файл, который будет содержать зашифрованные данные
            string key = "dj308QmcdlZkv308GFm1Amr9ZfcepVlg40AmvdzZlOA"; //ключ для шифрования
            string ivSecret = "вектор"; //вектор инициализации
            using Aes aes = Aes.Create();
            aes.IV = GetIV(ivSecret);
            aes.Key = GetKey(key);
            //using FileStream inStream = new FileStream(sourceFileName, FileMode.Open); //создаем файловый поток на чтение           
            using FileStream outStream = new FileStream(outputFileName, FileMode.Create);//создаем файловый поток на запись
                                                                                         //поток для шифрования данных
            CryptoStream encStream = new CryptoStream(outStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);
            long readTotal = 0;

            int len;
            int tempSize = 100; //размер временного хранилища
            byte[] bin = new byte[tempSize];    //временное Хранилище для зашифрованной информации
            //while (readTotal < inStream.Length)
            //{
            //    len = inStream.Read(bin, 0, tempSize);
            //    encStream.Write(bin, 0, len);
            //    readTotal = readTotal + len;
            //    Console.WriteLine($"{readTotal} байт обработано");
            //}
            //encStream.Close();
            //outStream.Close();
            //inStream.Close();
        }
    }
}