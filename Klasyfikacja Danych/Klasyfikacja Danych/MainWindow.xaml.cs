using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Klasyfikacja_Danych.Classes;
using Klasyfikacja_Danych.Neural_Classes;
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
        List<DataClass> classes = new List<DataClass>();
        Network NeuralNetwork = new Network();
        List<TestResult> kNNResults = new List<TestResult>();
        List<TestResult> NNResults = new List<TestResult>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Refresh();
           InitializeNetwork();
        }

        public void InitializeNetwork()
        {
 
            myVector V = bow.GetVectorsList()[0];
            classes = DataClass.CreateDataClasses(bow);
    //        NeuralNetwork = NeuralConstruction.CreateDefaultNetwork(V.GetVector().Count, classes);

            NeuralNetwork = NeuralConstruction.CreateNewDefaultNetwork(V.GetVector().Count, classes, 6);

         //   NeuralConstruction.SampleWeight(NeuralNetwork, bow.GetVectorsList(), classes);
        //    int id = NeuralConstruction.NewSampleInput(V, NeuralNetwork);
            
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
            NumberOfReadArticlesLabel.Content = ra.GetLoadedArticles().Count;
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

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.ShowDialog();
        }

        private void _Start_Click(object sender, RoutedEventArgs e)
        {

            //Helper.CalculateTFIDF(bow);

            classes = DataClass.CreateDataClasses(bow);
            classes = kNN.CreateFullSet(classes, bow);

            myVector x = bow.GetVectorsList()[0];


            TestClass T = kNN.CreateTest(classes);
      
            List<myVector> vectors = T.GetTestVectors();

            NeuralNetwork = NeuralConstruction.CreateDefaultNetwork(x.GetVector().Count, classes);

            NeuralConstruction.SampleWeight(NeuralNetwork, bow.GetVectorsList(), classes);

            List<int> kNNResultsIds = new List<int>();
            List<int> NNResultsIds = new List<int>();

            // Liczymy
            foreach (myVector V in vectors)
            {
                int id = 0;
                id = kNN.CalculateKNN(V, classes, 3);
                kNNResultsIds.Add(id);
                id = NeuralConstruction.NewSampleInput(V, NeuralNetwork);
                NNResultsIds.Add(id);
            }
            //  String classname = classes[id].GetName();


           // Dodajemy wyniki dla KNN
           for(int i=0; i<kNNResultsIds.Count; i++)
            {
                TestResult testresult = new TestResult("kNNAlgorithm");
                testresult.filltestData(classes, kNNResultsIds[i], vectors[i]);
                kNNResults.Add(testresult);
            }
            // Dodajemy wyniki dla sieci neuronowej
            for (int i = 0; i < NNResultsIds.Count; i++)
            {
                TestResult testresult = new TestResult("kNNAlgorithm");
                testresult.filltestData(classes, NNResultsIds[i], vectors[i]);
                NNResults.Add(testresult);
            }
            ListView1.ItemsSource = kNNResults;
            ListView2.ItemsSource = NNResults;
             kNN.TestResults(T);


            //   int id = kNN.CalculateKNN(v, classes, 3);
            // String classname = classes[id].GetName();
            // MessageBox.Show(classname);

        }
    }
}
