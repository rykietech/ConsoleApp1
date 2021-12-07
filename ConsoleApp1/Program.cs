using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoodMatch
{
    public class Person
    {
        public string name;
        public string gender;
        public Person(string name, string gender)
        {
            this.name = name;
            this.gender = gender;
        }

        public Person()
        {

        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
    }
    public class GoodMatchTest
    {
        public static bool CheckIfLetters(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                    return false;
            }
            return true;
        }
        public static string getUserinput()
        {

            Console.WriteLine("Enter the name of person 1: ");
            string name1 = Console.ReadLine();

            Console.WriteLine("Enter the name of person 2: ");
            string name2 = Console.ReadLine();

            return name1 + " " + "matches" + " " + name2;
        }
        public static int[] CountElements(string input)
        {
            var groupedLetters = input.GroupBy(letter => letter);
            int[] arr = new int[groupedLetters.Count() - 1];
            int[] newarr = new int[groupedLetters.Count() - 1];
            int index = 0;
            foreach (var letter in groupedLetters)
            {
                if (letter.Key != ' ')
                {

                    //Console.WriteLine("Count of: " + letter.Key + " " + letter.Count());
                    arr[index] = letter.Count();
                    index++;
                }
            }
            for (int k = 0; k < newarr.Length; k++)
            {
                if (arr[k] != ' ')
                {
                    newarr[k] = arr[k];
                    //Console.WriteLine(newarr[k]);
                }
            }
            return newarr;

        }
        public static int[] ReduceToTwoDigit(int[] arr)
        {
            int[] sum = new int[arr.Length / 2 + 1];
            int k = arr.Length - 1;
            int l = 0;

            if (arr.Length % 2 != 0)
            {
                //for (int j = 0; j< arr.Length /2 ;j++)
                //{
                // Console.WriteLine(arr[j]);
                //}
                //for (int i = arr.Length - 1; i >= (arr.Length / 2) + 1; i--)
                //{
                // Console.WriteLine(arr[i]);
                //}
                while (l < k && l < arr.Length / 2 && k >= (arr.Length / 2) + 1)
                {
                    sum[l] = arr[l] + arr[k];
                    l++;
                    k--;
                }
                sum[sum.Length - 1] = arr[arr.Length / 2];
            }
            else
            {
                while (l < k && l <= arr.Length / 2 && k >= (arr.Length / 2))
                {
                    sum[l] = arr[l] + arr[k];
                    l++;
                    k--;
                }
            }
            return sum;
        }
        public static int[] recusiveCall(int[] sum)
        {
            int[] temp = sum;

            if (temp.Length == 2)
            {
                return temp;
            }
            else
            {
                temp = ReduceToTwoDigit(sum);
                Array.Resize(ref sum, temp.Length);
                temp = recusiveCall(temp);
            }
            return temp;
        }
        public static string ResultPercentage(int[] sum)
        {
            int total = 0;
            string tot = "";
            int total2 = 0;
            char[] char1 = sum[0].ToString().ToCharArray();
            char[] char2 = sum[1].ToString().ToCharArray();

            if (char1.Length < char2.Length)
            {
                total = (int)(char1[0] - '0') + (int)(char2[0] - '0');
                tot = total.ToString() + char2[1];

            }

            else if ((char1.Length > char2.Length))
            {
                total = (int)(char1[0] - '0') + (int)(char2[0] - '0');
                tot = total.ToString() + char1[1];

            }
            else if ((char1.Length == char2.Length))
            {
                total = (int)(char1[0] - '0') + (int)(char2[0] - '0');
                total2 = (int)(char1[1] - '0') + (int)(char2[1] - '0');
                tot = total.ToString() + total2.ToString();
            }
            else
            {
                total = (int)(char1[0] - '0') + (int)(char2[0] - '0');

                tot = total.ToString();

            }
            return tot;
        }
        public static string GetPercentage(string input)
        {
            int[] arr = CountElements(input);
            int[] sum = recusiveCall(arr);
            string sum2 = ResultPercentage(sum);
            return sum2;
        }
        public static string getOutput(string input, string sum)
        {
            if (Convert.ToInt16(sum) >= 80)
            {
                return input + " " + sum + "%" + ",good match";
            }
            else
            {
                return input + " " + sum + "%";
            }
        }

        public static List<PercentageMatch> Sort(List<PercentageMatch> pm)
        {
            var sorted = pm.OrderBy(x => x.person1).ToList();
            sorted.Sort(delegate (PercentageMatch x, PercentageMatch y)
            {
                int a = Int16.Parse(y.percentage).CompareTo(Int16.Parse(x.percentage));

                return a;
            });

            return sorted;
        }
        public static List<PercentageMatch> CSVGoodMatch(string p)
        {
            var peoplemale = new List<Person>();
            var peoplefemale = new List<Person>();
            var percentageMatch = new List<PercentageMatch>();
            string path = p;
            List<string> names = new List<string>();
            List<string> gender = new List<string>();

            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var data = line.Split(';');
                        names.Add(data[0]);
                        gender.Add(data[1]);
                    }
                }
            }
            else
            {
                Console.WriteLine("File does not exist!");
            }

            for (int i = 0; i < names.Count(); i++)
            {
                if (gender.ElementAt(i).Equals("f"))
                {
                    peoplefemale.Add(new Person(names.ElementAt(i), gender.ElementAt(i)));
                }
                else
                {
                    peoplemale.Add(new Person(names.ElementAt(i), gender.ElementAt(i)));
                }
            }
            List<Person> lstFilteredmale = peoplemale.Select(x => new { x.Name, x.Gender }).Distinct().Select(y => new Person()
            { gender = y.Gender, name = y.Name }).ToList();

            List<Person> lstFilteredfemale = peoplefemale.Select(x => new { x.Name, x.Gender }).Distinct().Select(y => new Person()
            { gender = y.Gender, name = y.Name }).ToList();

            for (int i = 0; i < lstFilteredmale.Count; i++)
            {
                for (int j = 0; j < lstFilteredfemale.Count; j++)
                {
                    percentageMatch.Add(
                    new PercentageMatch(
                    lstFilteredmale.ElementAt(i).Name, lstFilteredfemale.ElementAt(j).Name, GetPercentage(
                    lstFilteredmale.ElementAt(i).Name + " " + "matches" + " " + lstFilteredfemale.ElementAt(j).Name)));
                }
            }
            //foreach (var pm in percentageMatch)
            //{
            // if (Convert.ToInt16(pm.percentage) >= 80)
            // {
            // Console.WriteLine( pm.person1 + " "+ "matches" + " "+pm.person2+" "+ pm.percentage + "%" + ",good match");
            // }
            // else
            // {
            // Console.WriteLine(pm.person1 + " " + "matches" + " " + pm.person2 +" "+ pm.percentage + "%");
            // }
            //}
            return percentageMatch;

        }
        public static void WritetoFile(List<PercentageMatch> percentageMatch)
        {
            string fpath = @"C:/Users/f/OneDrive/Desktop/Output.csv";
            StringBuilder Output = new StringBuilder();
            try
            {
                foreach (var p in percentageMatch)
                {
                    if (Convert.ToInt16(p.percentage) >= 80)
                    {
                        Output.Append(p.person1 + " " + "matches" + " " + p.person2 + " " + p.percentage + "%" + ",good match");
                        Output.Append("\n");
                    }
                    else
                    {
                        Output.Append(p.person1 + " " + "matches" + " " + p.person2 + " " + p.percentage + "%");
                        Output.Append("\n");
                    }
                }
                File.WriteAllText(fpath, Output.ToString());
                File.AppendAllText(fpath, Output.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public class PercentageMatch
        {
            public string person1;
            public string person2;
            public string percentage;
            public PercentageMatch(string person1, string person2, string percentage)
            {
                this.person1 = person1;
                this.person2 = person2;
                this.percentage = percentage;
            }
            public string Person1
            {
                get { return person1; }
                set { person1 = value; }
            }
            public string Person2
            {
                get { return person2; }
                set { person2 = value; }
            }

            public string Percentage
            {
                get { return percentage; }
                set { percentage = value; }

            }
        }
        public static void Main()
        {
            string input = getUserinput();
            string sum = GetPercentage(input);
            if (Regex.IsMatch(input, @"^[\p{L}\p{M}' \.\-]+$") == true)
            {
                Console.WriteLine("accepting two strings and calculates percentage as above");
                Console.WriteLine(getOutput(input, sum));

            }
            else
            {
                Console.WriteLine("error: names are not valid");
            }
            // Using csv file
            List<PercentageMatch> percentageMatch = GoodMatchTest.CSVGoodMatch(@"C:/Users/f/OneDrive/Desktop/Input.csv");
            percentageMatch = Sort(percentageMatch);
            WritetoFile(percentageMatch);
            //foreach (var pm in percentageMatch)
            //{
            // if (Convert.ToInt16(pm.percentage) >= 80)
            // {
            // Console.WriteLine(pm.person1 + " " + "matches" + " " + pm.person2 + " " + pm.percentage + "%" + ",good match");
            // }
            // else
            // {
            // Console.WriteLine(pm.person1 + " " + "matches" + " " + pm.person2 + " " + pm.percentage + "%");
            // }
            //}

        }
    }
}
