using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        char[] characters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz123456789. ".ToCharArray();

        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        private long Calculate_e(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

        private long Calculate_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) //если имеют общие делители
                {
                    d--;
                    i = 1;
                }

            return d;
        }

        private List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result.Add(bi.ToString());
            }

            return result;
        }

        private string RSA_Dedoce(List<string> input, long d, long n)
        {
            string result = "";

            BigInteger bi;

            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += characters[index].ToString();
            }

            return result;
        }

        private void ToCipher()
        {
            Console.WriteLine("Введите p: ");
            long p = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Введите q: ");
            long q = Convert.ToInt64(Console.ReadLine());

            if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
            {
                string s = "";

                StreamReader sr = new StreamReader("Info1.bg");

                while (!sr.EndOfStream)
                {
                    s += sr.ReadLine();
                }

                sr.Close();

                // s = s.ToUpper();

                long n = p * q;
                long m = (p - 1) * (q - 1);
                long d = Calculate_d(m);
                long e_ = Calculate_e(d, m);

                List<string> result = RSA_Endoce(s, e_, n);

                StreamWriter sw = new StreamWriter("out1.txt");
                foreach (string item in result)
                    sw.WriteLine(item);
                sw.Close();

                Console.WriteLine("d: " +d.ToString());
                Console.WriteLine("n: " + n.ToString());

                //Process.Start("out1.txt");
            }
            else
            Console.WriteLine("p или q - не простые числа!");
        }

        private void Decipher()
        {
            Console.WriteLine("Введите d: ");
            long d = Convert.ToInt64(Console.ReadLine());
            Console.WriteLine("Введите n: ");
            long n = Convert.ToInt64(Console.ReadLine());

            List<string> input = new List<string>();

            StreamReader sr = new StreamReader("out1.txt");

            while (!sr.EndOfStream)
            {
                input.Add(sr.ReadLine());
            }

            sr.Close();

            string result = RSA_Dedoce(input, d, n);

            StreamWriter sw = new StreamWriter("out2.txt");
            sw.WriteLine(result);
            sw.Close();
            Console.WriteLine(result);

            //Process.Start("out2.txt"); 
        }

        static void Main(string[] args)
        {
            Program k = new Program();
            k.ToCipher();

            k.Decipher();
            Console.ReadKey();
        }
    }
}
