using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Klasyfikacja_Danych.Classes
{
    public class ReadArticles
    {
        List<Article> Articles = new List<Article>();
        public void ReadArticlesFromProgramFile()
        {
            Articles.Clear();
            
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            var t = Directory.GetDirectories(startupPath, "Artykuly");
            startupPath += @"\Artykuly\";
            if (t.Count() != 0)
            {
                var files = Directory.GetFiles(startupPath);
                if (files.Count() != 0)
                {
                    foreach (var item in files)
                    {
                        CreateArticlesFromTXTFile(item);                        
                    }
                }
            }
        }

        private void CreateArticlesFromTXTFile(string item)
        {
            //Wycieki pamieci???
            var text = System.IO.File.ReadAllText(item);
            int id = 0;
            int index = 0;
            while (index != -1)
            {
                index = text.IndexOf("\n{NEWARTICLE}\n");
                if (index != -1)
                {
                    Articles.Add(new Article(Helper.FormatText(text.Substring(0, index)), item.Split('\\').Last().Replace("TextBox.txt", "").ToString() + "_" + id++));
                    text = text.Substring(index + 14);

                }
            }
        }
        public List<Article> GetLoadedArticles()
        {
            return Articles;                
        }
        public int GetWordsNumber()
        {
            int count = 0;
            foreach (var article in Articles)
                count += article.GetArticle().Count;
            return count;
        }
        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                var t = LogicalTreeHelper.GetChildren(depObj);
                foreach (var child1 in t)
                {
                    DependencyObject child = child1 as DependencyObject;
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindLogicalChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
