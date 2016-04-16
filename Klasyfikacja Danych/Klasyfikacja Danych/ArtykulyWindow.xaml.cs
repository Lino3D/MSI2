using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Klasyfikacja_Danych
{
    /// <summary>
    /// Interaction logic for ArtykulyWindow.xaml
    /// </summary>
    public partial class ArtykulyWindow : MetroWindow
    {
        public ArtykulyWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            var t = Directory.GetDirectories(startupPath, "Artykuly");
            startupPath += @"\Artykuly";
            if (t.Count() == 0)
                Directory.CreateDirectory(startupPath);

            var TextBoxCollection = FindLogicalChildren<TextBox>(MainTabControl);

            foreach( var item in TextBoxCollection)
            {
                System.IO.File.WriteAllText(startupPath + @"\" + item.Name + ".txt", item.Text);
            }

        }

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                var t = LogicalTreeHelper.GetChildren(depObj);
                foreach( var child1 in t)
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




        private void MainTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            var t = Directory.GetDirectories(startupPath, "Artykuly");
            startupPath += @"\Artykuly\";
            if (t.Count() != 0)
            {
                var files = Directory.GetFiles(startupPath);
                if (files.Count() != 0)
                {
                    var TextBoxColletion = FindLogicalChildren<TextBox>(MainTabControl);
                    foreach (var item in files)
                    {
                        foreach (var TextBoxItem in TextBoxColletion)
                            if (item.Contains(TextBoxItem.Name.ToString()))
                                TextBoxItem.Text = System.IO.File.ReadAllText(startupPath + TextBoxItem.Name + ".txt");
                    }
                }
            }
        }
    }
}
