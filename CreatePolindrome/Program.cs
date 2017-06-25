using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatePolindrome
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            Console.WriteLine(IsPolindrome(input));
            Console.WriteLine(SmallPolindrome(input));
        }

        private static string SmallPolindrome(string input)
        {
            Dictionary<char, int> charactersCount = new Dictionary<char, int>();
            char[] charcters = input.ToCharArray();
            string firstCharcters = "";
            string secondCharcters = "";
            var removeNumber = ' ';
            InsertInDictionary(input, charactersCount);
            var countEven = charactersCount.Where(x => x.Value % 2 == 1).ToList();
            if (countEven.Count() > 1)
            {
                return input;
            }
            else if (charcters.Length % 2 == 0)
            {
                return PolindromeIsLengthEven(charactersCount, ref firstCharcters, ref secondCharcters);
            }
            else
            {
                return PolindromeIsLengthIsOdd(charactersCount, ref firstCharcters, ref secondCharcters, ref removeNumber);
            }
        }

        private static string PolindromeIsLengthIsOdd(Dictionary<char, int> charactersCount, ref string firstCharcters, ref string secondCharcters, ref char removeNumber)
        {
            foreach (var charcter in charactersCount)
            {
                if (charcter.Value % 2 == 1)
                {
                    removeNumber = charcter.Key;
                }
                else
                {
                    var count = charcter.Value / 2;
                    firstCharcters += new string(charcter.Key, count);
                    secondCharcters += new string(charcter.Key, count);
                }
            }
            char[] chars = secondCharcters.ToCharArray();
            Array.Reverse(chars);
            return firstCharcters + removeNumber + new string(chars);
        }

        private static string PolindromeIsLengthEven(Dictionary<char, int> charactersCount, ref string firstCharcters, ref string secondCharcters)
        {
            foreach (var chararter in charactersCount.OrderBy(x => x.Key))
            {
                var count = chararter.Value / 2;
                firstCharcters += new string(chararter.Key, count);
                secondCharcters += new string(chararter.Key, count);
            }
            char[] chars = secondCharcters.ToCharArray();
            Array.Reverse(chars);
            return firstCharcters + new string(chars);
        }

        private static void InsertInDictionary(string input, Dictionary<char, int> charactersCount)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!charactersCount.ContainsKey(input[i]))
                {
                    charactersCount.Add(input[i], 1);
                }
                else
                {
                    charactersCount[input[i]] += 1;
                }
            }
        }

        private static bool IsPolindrome(string input)
        {
            int min = 0;
            int max = input.Length - 1;
            while (true)
            {
                if (min > max)
                {
                    return true;
                }
                char a = input[min];
                char b = input[max];
                if (char.ToLower(a) != char.ToLower(b))
                {
                    return false;
                }
                min++;
                max--;
            }
        }
    }
}
