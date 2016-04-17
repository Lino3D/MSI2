using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{
    public class Article
    {
        List<string> Words;
        string Name;

        public Article(List<string> words, string name)
        {
            Words = words;
            Name = name;
        }

        public List<string> GetArticle()
        {
            return Words;
        }
        public string GetName()
        {
            return Name;
        }
    }
}
