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
        Network WithoutHiddenLayerNetwork = new Network();
        List<TestResult> kNNResults = new List<TestResult>();
        List<TestResult> NNResults = new List<TestResult>();
        List<TestResult> WHNNResults = new List<TestResult>();

        TestClass test;
       
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Refresh();
            InitializeNetwork();
            test = TestFunctions.CreateVectorTest(bow);
        }

        public void InitializeNetwork()
        {
            classes = DataClass.CreateDataClasses(bow);
            classes = TestFunctions.CreateFullSet(classes, bow);
            myVector x = new myVector();
          //  if (bow.GetVectorsList().Count > 0)
            {
                x = bow.GetVectorsList()[0];

                WithoutHiddenLayerNetwork = NeuralConstruction.CreateDefaultNetwork(x.GetVector().Count, classes);
                NeuralConstruction.SampleWeight(WithoutHiddenLayerNetwork, bow.GetVectorsList(), classes);

                NeuralNetwork = NeuralConstruction.CreateNewDefaultNetwork(x.GetVector().Count, classes, 10);
            }


     
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
            Helper.CalculateTFIDF(bow);
            UpdateLabels();
        }

        private void WczytajPDF(object sender, RoutedEventArgs e)
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

        

        private void _Start_Copy_Click(object sender, RoutedEventArgs e)
        {
         //   Helper.CalculateTFIDF(bow);
            myVector x = bow.GetVectorsList()[0];
          //  test = TestFunctions.CreateVectorTest(bow);
            //TestClass test = TestFunctions.CreateVectorTest(bow);
            //foreach (myVector testvector in test.GetTestVectors())
            //{
            //    int id = NeuralConstruction.SampleInput(testvector, NeuralNetwork);
            //    var output = NeuralNetwork.getNetwork().Where(o => o.type == 2).ToList();
            //    //   NNResultsIds.Add(id);
            //}


            TestClass T = TestFunctions.CreateTest2(classes);
            //  List<myVector> vectors = T.GetTestVectors();           
            List<myVector> vectors = test.GetTestVectors();
           
            List<int> kNNResultsIds = new List<int>();
            List<int> NNResultsIds = new List<int>();
            List<int> WHNNResultsIds = new List<int>();

            // Liczymy
            foreach (myVector V in vectors)
            {
                int id = 0;
                id = kNN.CalculateKNN(V, classes, 3);
                kNNResultsIds.Add(id);
                id = NeuralConstruction.OldSampleInput(V, WithoutHiddenLayerNetwork);
                WHNNResultsIds.Add(id);
                id = NeuralConstruction.SampleInput(V, NeuralNetwork);
                NNResultsIds.Add(id);
            }

            // Dodajemy wyniki dla KNN
            for (int i = 0; i < kNNResultsIds.Count; i++)
            {
                TestResult testresult = new TestResult("kNNAlgorithm");
                testresult.filltestData(T.GetTrainingClasses(), kNNResultsIds[i], vectors[i]);
                kNNResults.Add(testresult);
            }
            // Dodajemy wyniki dla sieci neuronowej
            for (int i = 0; i < NNResultsIds.Count; i++)
            {
                TestResult testresult = new TestResult("NNAlgorithm");
                testresult.filltestData(classes, NNResultsIds[i], vectors[i]);
                NNResults.Add(testresult);
            }
            // Dodajemy wyniki dla sieci neuronowej bez warstwy ukrytej
            for (int i = 0; i < WHNNResultsIds.Count; i++)
            {
                TestResult testresult = new TestResult("WHNNAlgorithm");
                testresult.filltestData(classes, WHNNResultsIds[i], vectors[i]);
                WHNNResults.Add(testresult);
            }
            ListView1.ItemsSource = kNNResults;
            ListView2.ItemsSource = NNResults;
            ListView3.ItemsSource = WHNNResults;

          //  TestFunctions.TestResults(T);
        }

        private void UczenieSieci_Click(object sender, RoutedEventArgs e)
        {
           
            myVector x = bow.GetVectorsList()[0];

            BackPropagation.UczenieSieci(100, test.GetTrainingtVectors(), NeuralNetwork, classes);
            Console.Beep();

            foreach (myVector testvector in test.GetTestVectors())
            {
                int id =NeuralConstruction.SampleInput(testvector, NeuralNetwork);
                var output = NeuralNetwork.getNetwork().Where(o => o.type == 2).ToList();
            }

        }

        private void Wczytaj_Sieć(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XML Files (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileName = dlg.FileName;
                try
                {
                    NeuralNetwork = SerializationClass.ConDeSerializer(NeuralNetwork, fileName);

                    Network randomnet = SerializationClass.ConDeSerializer(NeuralNetwork, fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Failed to open File");
        }

        private void Zapisz_Sieć(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "XML Files (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
           
            if (result == true)
            {
                string fileName = dlg.FileName;
                try
                {
                    SerializationClass.ConSerializer(NeuralNetwork, fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Failed to save File");
        }

        private void SerializeTest_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "XML Files (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileName = dlg.FileName;
                try
                {
                    SerializationClass.ConSerializer(test, fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Failed to save File");
        }

        private void DeSerializeTest_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XML Files (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileName = dlg.FileName;
                try
                {
                    test = SerializationClass.ConDeSerializer(test, fileName);

                   }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Failed to open File");
        }

        private void DeSerializeBoW_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XML Files (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileName = dlg.FileName;
                try
                {
                    bow = SerializationClass.ConDeSerializer(bow, fileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Failed to open File");
        }

        private void SerializeBoW_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "XML Files (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileName = dlg.FileName;
                try
                {
                    SerializationClass.ConSerializer(bow, fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Failed to save File");
        }
    }
}
