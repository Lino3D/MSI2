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
        ArtykulyWindow w = new ArtykulyWindow();
        List<string> Articles = new List<string>();
        public void ReadArticlesFromProgramFile()
        {            
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            var t = Directory.GetDirectories(startupPath, "Artykuly");
            startupPath += @"\Artykuly\";
            if (t.Count() != 0)
            {
                var files = Directory.GetFiles(startupPath);
                if (files.Count() != 0)
                {                    
                    var TextBoxColletion = FindLogicalChildren<TextBox>(w.MainTabControl);
                    foreach (var item in files)
                    {
                        foreach (var TextBoxItem in TextBoxColletion)
                            if (item.Contains(TextBoxItem.Name.ToString()))
                                Articles.Add(System.IO.File.ReadAllText(startupPath + TextBoxItem.Name + ".txt"));
                    }
                }
            }
        }

        public int GetWordsNumber()
        {
            int count = 0;
            foreach (var article in Articles)
                count += Helper.FormatText(article).Count();
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
