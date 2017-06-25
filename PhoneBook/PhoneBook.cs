using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PhoneBook
{
    public class PhoneBook
    {

        private const string PathBook = "../../phoneBook.txt";
        private const string PathBookCalls = "../../phoneBookCalls.txt";
        private const string Pattern = "(\\+359|0|00359)(87|88|89)([2-9])(\\d{6})";
        private Dictionary<string, string> phoneBook = new Dictionary<string, string>();

        public void Set(string name, string number)
        {
            Regex regex = new Regex(Pattern);
            if (regex.IsMatch(number))
            {
                var match = regex.Match(number).Groups;
                string phone = "+359" + match[2].Value + match[3].Value + match[4].Value;
                if (!phoneBook.ContainsKey(name))
                {
                    phoneBook.Add(name, phone);
                }
                else
                {
                    phoneBook[name] = phone;
                }
            }
        }
        /// <summary>
        /// Read file and set key and value in to dictionary 
        /// </summary>
        private void ReadPhoneBookByFile()
        {
            var file = File.ReadLines(PathBook).Where(x => !string.IsNullOrWhiteSpace(x));
            if (file.Count() != 1)
            {
                foreach (string line in file)
                {
                    var name = line.Split()[0];
                    var number = line.Split()[1];
                    Set(name, number);
                }
            }
        }
        /// <summary>
        /// Add name and phone to the text
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void AddContact(string name, string phone)
        {
            ReadPhoneBookByFile();
            if (!phoneBook.ContainsKey(name))
            {
                Set(name, phone);
                SavePhoneBook(name + " " + phone);
                Console.WriteLine($"{name} added successfully");
            }
            else
            {
                Console.WriteLine($"{name} was alredy exist");
            }
        }
        /// <summary>
        /// Find contact by name and print in to console
        /// </summary>
        /// <param name="name"></param>
        public void FindContactByName(string name)
        {
            ReadPhoneBookByFile();
            PrintContactByName(phoneBook.FirstOrDefault(x => x.Key.ToLower() == name.ToLower()), name);
        }
        private static void SavePhoneBook(string nameAndPhone)
        {
            File.AppendAllText(PathBook, Environment.NewLine + nameAndPhone);
        }

        /// <summary>
        /// Delete contact by name
        /// </summary>
        /// <param name="name"></param>
        public void DeleteContactByName(string name)
        {
            ReadPhoneBookByFile();
            if (phoneBook.Remove(name))
            {
                DeleteConatctFromFile(name);
                Console.WriteLine($"{name} was deleted!");
            }
            else
            {
                Console.WriteLine($"Not found {name}");
            }
        }
        /// <summary>
        /// Delete contact by name for file
        /// </summary>
        /// <param name="name"></param>
        private void DeleteConatctFromFile(string name)
        {
            string[] lines = System.IO.File.ReadAllLines(PathBook);

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(PathBook))
            {
                foreach (string line in lines)
                {
                    if (!line.Contains(name))
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
        /// <summary>
        /// Call conatct by name, and save in to file
        /// </summary>
        /// <param name="name"></param>
        public void CallConatct(string name)
        {
            ReadPhoneBookByFile();
            if (phoneBook.ContainsKey(name))
            {
                SaveCall(name);
            }
            else
            {
                Console.WriteLine($"Not found {name}");
            }
        }

        private void SaveCall(string name)
        {
            List<string> lines = File.ReadAllLines(PathBookCalls).ToList();
            var text = File.ReadAllText(PathBookCalls);
            var updateRecentCall = new List<string>();
            if (lines.Count() != 0)
            {
                AddToList(name, lines, text, updateRecentCall);
            }
            else
            {
                updateRecentCall.Add($"{phoneBook.First(x => x.Key == name).Key} - " + $"{phoneBook[name]} (1)");
            }
            WriteToFile(updateRecentCall);
        }

        private void AddToList(string name, List<string> lines, string text, List<string> updateRecentCall)
        {
            lines.ForEach(delegate (string s) { updateRecentCall.Add(s); });
            if (text.Contains(name))
            {
                var currentLine = "";
                updateRecentCall.ForEach(delegate (string s) { if (s.Contains(name)) currentLine = s; });
                updateRecentCall.Remove(currentLine);
                var tokens = currentLine.Split(new[] { '-', '(', ')' });
                updateRecentCall.Add($"{phoneBook.First(x => x.Key == name).Key} - " + $"{phoneBook[name]} " + $"({int.Parse(tokens[2]) + 1 })");
            }
            else
            {
                updateRecentCall.Add($"{phoneBook.First(x => x.Key == name).Key} - " + $"{phoneBook[name]} (1)");
            }
        }

        private static void WriteToFile(List<string> newLines)
        {
            using (var writer = new StreamWriter(PathBookCalls))
            {
                foreach (string line in newLines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Print phone book in to console
        /// </summary>
        public void PrintPhoneBook()
        {
            ReadPhoneBookByFile();
            if (phoneBook.Count != 0)
            {
                foreach (var book in this.phoneBook.OrderBy(name => name.Key))
                {
                    Console.WriteLine($"{book.Key} - {book.Value}");
                }
            }
            else
            {
                Console.WriteLine("contacts not found ");
            }
        }
        /// <summary>
        /// Print recent call in to console
        /// </summary>
        public void PrintRecentCalls()
        {
            var fiveMostCalled = 5;
            var file = File.ReadAllLines(PathBookCalls);
            Dictionary<string, int> sorteDictionary = new Dictionary<string, int>();
            foreach (var line in file)
            {
                var tokens = line.Split(new[] { '-', '(', ')' });
                sorteDictionary.Add(tokens[0] + " - " + tokens[1], Int32.Parse(tokens[2]));
            }
            foreach (var names in sorteDictionary.OrderByDescending(x => x.Value).ThenBy(x => x.Key).Take(fiveMostCalled))
            {
                Console.WriteLine($"{names.Key} ({names.Value})");
            }
        }
        /// <summary>
        /// Print contact by name in to console
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        private void PrintContactByName(KeyValuePair<string, string> value, string name)
        {
            if (value.Key != null)
            {
                Console.WriteLine(value.Key + " - " + value.Value);
            }
            else
            {
                Console.WriteLine($"Not found {name}");
            }

        }
    }
}
