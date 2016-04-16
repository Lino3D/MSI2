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

            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            text = rgx.Replace(text, "");
            lst = text.Split(' ').ToList();
            return lst;
        }
    }
}
