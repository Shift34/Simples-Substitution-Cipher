using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Simples_Substitution_Cipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[] letters = new char[32] { 'Л', 'Т', 'О', 'И', 'Р', 'Ю', 'К', 'Щ', 'В', 'Ц', 'Ф', 'Ж', 'Б', 'Ы', 'Э', 'Я', 'Д', 'Н', 'Ь', 'П', 'Г', 'Е', 'З', 'М', 'Х', 'Ш', 'Й', 'С', 'А', 'Ч', 'У',' '};
            char[] letters1 = new char[32]{ 'О', 'Е', 'А', 'И', 'Р', 'Т', 'Н', 'С', 'В', 'Л', 'М', 'П', 'Й', 'К', 'Д', 'Я', 'Ь', 'Ы', 'З', 'Г', 'Б', 'Ч', 'У', 'Х', 'Ж', 'Ш', 'Ю', 'Ц', 'Щ', 'Э', 'Ф',' '};
            Dictionary<char, char> dictionary = new Dictionary<char, char>();
            for (int i = 0; i < 32; i++)
            {
                dictionary.Add(letters[i], letters1[i]);
            }

            Dictionary<char, int> findingFrequencyciphretext = FindingFrequency("Ciphertext.txt");
            findingFrequencyciphretext = findingFrequencyciphretext.OrderBy(pair => pair.Value).Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<string, int> findingBigramciphretext = FindingBigram("Ciphertext.txt");
            findingBigramciphretext = findingBigramciphretext.OrderBy(pair => pair.Value).Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<string, int> findingBigramcytext = FindingBigram("WarAndPeace.txt");
            findingBigramcytext = findingBigramcytext.OrderBy(pair => pair.Value).Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);

            string text = Decryption("Ciphertext.txt", dictionary);
            Console.WriteLine(text);

            Console.WriteLine(Analysator("Ciphertext.txt", dictionary, findingBigramcytext));

            Console.ReadKey();
        }
        public static Dictionary<char, int> FindingFrequency(string input) 
        {
            Dictionary<char, int> letterFrequency = new Dictionary<char, int>();
            string pattern = @"[А-Яа-я]";
            using (StreamReader sr = new StreamReader(input))
            {
                string text = sr.ReadToEnd().ToLower();
                foreach (Match item in Regex.Matches(text, pattern, RegexOptions.IgnoreCase))
                {
                    string text2 = item.Value;
                    for (int i = 0; i < text2.Length; i++)
                    {
                        if (letterFrequency.ContainsKey(text2[i]))
                        {
                            letterFrequency[text2[i]]++;
                        }
                        else letterFrequency.Add(text2[i], 1);
                    }
                }          
            }
            return letterFrequency;
        }
        public static Dictionary<string,int> FindingBigram(string input)
        {
            Dictionary<string, int> letterFrequency = new Dictionary<string, int>();
            string pattern = @"[А-Яа-я]+";
            using (StreamReader sr = new StreamReader(input))
            {
                string text = sr.ReadToEnd().ToLower();
                foreach (Match item in Regex.Matches(text, pattern, RegexOptions.IgnoreCase))
                {
                    for (int i = 1; i < item.Length; i++)
                    {
                        string text1 = item.ToString().Substring(i - 1, 2);
                        if (letterFrequency.ContainsKey(text1))
                        {
                            letterFrequency[text1]++;
                        }
                        else letterFrequency.Add(text1, 1);
                    }
                }
            }
            return letterFrequency;
        }
        public static string Decryption(string input, Dictionary<char, char> dictionary)
        {
            string text1 = null;
            using (StreamReader sr = new StreamReader(input))
            {
                string text = sr.ReadToEnd().ToUpper();
                for(int i = 0 ; i < text.Length; i++)
                {
                    text1 += dictionary[text[i]];
                }
            }
            return text1;
        }
        public static double Analysator(string input, Dictionary<char, char> dictionary, Dictionary<string, int> dictionary1)
        {
            string text1 = null;
            double count = 0;
            HashSet<string> words = new HashSet<string>();
            using (StreamReader sr = new StreamReader(input))
            {
                string text = sr.ReadToEnd().ToUpper();
                text1 += dictionary[text[0]];
                for (int i = 1; i < text.Length; i++)
                {
                    text1 += dictionary[text[i]];
                    string text2 = text1.Substring(i - 1, 2).ToLower();
                    if (dictionary1.ContainsKey(text2))
                    {
                        count++;
                    }
                    else
                    {
                        words.Add(text2);
                    }
                }
                count /= (text.Length - 1);
            }
            foreach(var item in words)
            {
                Console.WriteLine(item);
            }
            return count;
        }
    }
}
