using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Klasyfikacja_Danych.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Klasyfikacja_Danych
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        BagOfWords bow = new BagOfWords();
        ReadArticles ra = new ReadArticles();
        StringBuilder text = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();
            Refresh();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ArtykulyWindow window = new ArtykulyWindow();
            window.ShowDialog();
            Refresh();
        }
        private void UpdateLabels()
        {
            ra.ReadArticlesFromProgramFile();
            LabelNumberOfWords.Content = ra.GetWordsNumber();

            LabelBoW.Content = bow.GetWordsList().Count;
        }
        private void LoadBagOfWords()
        {
            var articles = ra.GetLoadedArticles();
            bow.ResetujBagOfWords();
            foreach (var item in articles)
                bow.AddVector(item.GetArticle(), item.GetName());
        }
        private void Refresh()
        {
            ra.ReadArticlesFromProgramFile();
            LoadBagOfWords();
            UpdateLabels();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string fileName = dlg.FileName;
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }
                pdfReader.Close();
                MessageBox.Show(text.ToString());
            }
        }
    }
}
