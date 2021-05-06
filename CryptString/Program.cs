using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptString
{
    class Program
    {
        public static string EncodeDecrypt(string str, ushort secretKey)
        {
            var ch = str.ToArray(); //преобразуем строку в символы
            string newStr = "";      //переменная которая будет содержать зашифрованную строку
            foreach (var c in ch)  //выбираем каждый элемент из массива символов нашей строки
                newStr += TopSecret(c, secretKey);  //производим шифрование каждого отдельного элемента и сохраняем его в строку
            return newStr;
        }

        public static char TopSecret(char character, ushort secretKey)
        {
            character = (char)(character ^ secretKey); //Производим XOR операцию
            return character;
        }

        static void Main(string[] args)
        {
            ushort secretKey = 0x0088; // Секретный ключ (длина - 16 bit).


            string str = "Hello World"; //это строка которую мы зашифруем

            str = EncodeDecrypt(str, secretKey); //производим шифрование
            Console.WriteLine(str);  //выводим в консоль зашифрованную строку

            str = EncodeDecrypt(str, secretKey); //производим рассшифровку 
            Console.WriteLine(str);             //выводим в консоль расшифрованную строку
        }
    }
}
