using Klasyfikacja_Danych.Classes;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
        
    }
}
