using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{
    public static class Helper
    {
        public static List<string> FormatText(string text)
        {
            List<string> lst = new List<string>();
            text = text.Replace('\n', ' ');
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            text = rgx.Replace(text, "");
            lst = text.Split(' ').ToList();
            lst.RemoveAll(o => o == "");
            return lst;
        }

        public static void CalculateTFIDF(BagOfWords bow)
        {
            int index = 0;
            int count = 0;
            List<int> Frequency = new List<int>();
            var vectors = bow.GetVectorsList();
            foreach( var word in bow.GetWordsList())
            {
                count = 0;
                foreach (var v in vectors)
                    count += v.GetVector().ElementAt(index);
                Frequency.Add(count);


                index++;
            }
        }
    }
}
