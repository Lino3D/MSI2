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
            text = text.Replace('\n', ' ');
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            text = rgx.Replace(text, "");
            lst = text.Split(' ').ToList();
            lst.RemoveAll(o => o == "");
            return lst;
        }

        public static void CalculateTFIDF(BagOfWords bow)
        {
           
            int x = 0; //current vector for TFIDF
            var vectors = bow.GetVectorsList();
            List<List<double>> WholeTFIDF = new List<List<double>>();

            while (x < vectors.Count())
            {
                List<double> TFIDF = new List<double>();
                var v = vectors[x].GetVector();
                int index = 0;
                while (index < v.Count())
                {
                    int count = 0;
                    double TF = v[index]; //Term frequency is stored in Bag of Word
                    foreach (var vector in vectors)
                    {
                        List<double> V = vector.GetVector();           
                        if (V[index] > 0)
                            count++;

                    }
                    double IDF = Math.Log(vectors.Count() / count);
                    TFIDF.Add(IDF * TF);
                    index++;
                }
                WholeTFIDF.Add(TFIDF); //adding whole list to list of lists.
                x++;
            }
           double parameter = 5.32;
            List<int> IndexesToRemove = new List<int>();

                for(int j=0; j<WholeTFIDF[0].Count;j++)
                {
                    if(WholeTFIDF[0][j] <= parameter) //zmienić tu
                    {
                    IndexesToRemove.Add(j);
                    }
                }
                foreach(List<double> wordIndexes in WholeTFIDF)
            {
           for(int i=0; i<IndexesToRemove.Count;i++)
                {
                    if(wordIndexes[IndexesToRemove[i]]>= parameter) //zmienić tu
                    {
                        IndexesToRemove.RemoveAt(i);
                    }
                }
            }
            List<string> WordstoRemove = new List<string>();
            for(int i=0; i<IndexesToRemove.Count;i++)
            {
               WordstoRemove.Add( bow.GetWordsList().ElementAt(IndexesToRemove.ElementAt(i)));
            }
            bow.RemoveWords(WordstoRemove);
        }
    }
}
