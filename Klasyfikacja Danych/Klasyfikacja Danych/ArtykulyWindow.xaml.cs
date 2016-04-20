using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
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

            foreach (var item in TextBoxCollection)
            {
                System.IO.File.WriteAllText(startupPath + @"\" + item.Name + ".txt", item.Text);
            }

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




        private void MainTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            bool FileVisited = false;
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            var t = Directory.GetDirectories(startupPath, "Artykuly");
            startupPath += @"\Artykuly\";
            if (t.Count() != 0)
            {
                var files = Directory.GetFiles(startupPath);
                if (files.Count() != 0)
                {
                    var TextBoxColletion = FindLogicalChildren<TextBox>(MainTabControl);
                    List<string> UnvisitedFiles = new List<string>();
                    foreach (var item in files)
                    {
                        FileVisited = false;
                        foreach (var TextBoxItem in TextBoxColletion)
                            if (item.Contains(TextBoxItem.Name.ToString()))
                            {
                                TextBoxItem.Text = System.IO.File.ReadAllText(startupPath + TextBoxItem.Name + ".txt");
                                FileVisited = true;
                            }
                        if (!FileVisited)
                            UnvisitedFiles.Add(item);
                    }
                    foreach (var item in UnvisitedFiles)
                        File.Delete(item);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int PDFCounter = 0;
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            var t = Directory.GetDirectories(startupPath, "ArtykulyPDF");
            startupPath += @"\ArtykulyPDF";
            if (t.Count() == 0)
            {
                Directory.CreateDirectory(startupPath);
                MessageBox.Show("Folder z plikami PDF jest pusty");
                return;
            }

            var FolderyPDF = Directory.GetDirectories(startupPath + @"\");
            var TextBoxColletion = FindLogicalChildren<TextBox>(MainTabControl);

            foreach (var folder in FolderyPDF)
            {
                foreach (var TB in TextBoxColletion)
                {
                    if (folder.Split('\\').Last().Contains(TB.Name.ToString().Replace("TextBox", "")))
                    {
                        PDFCounter += LoadAllPDFs(folder, TB);
                    }
                }
            }
            PDFInfoPanel.Visibility = System.Windows.Visibility.Visible;
            PDFLabel.Content = "" + PDFCounter;
        }

        private static int LoadAllPDFs(string folder, TextBox TB)
        {
            int PDFCounter = 0;
            StringBuilder text = new StringBuilder();
            var files = Directory.GetFiles(folder + @"\");
            foreach (var file in files)
            {
                using (PdfReader pdfReader = new PdfReader(file))
                {

                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                        
                        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                        text.Append(currentText);
                    }
                    pdfReader.Close();
                }
                PDFCounter++;
                text.Append("\n{NEWARTICLE}\n");
            }
            TB.Text = text.ToString();
            return PDFCounter;
        }
    }
}
