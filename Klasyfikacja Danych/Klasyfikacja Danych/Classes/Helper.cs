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
           
            int j = 0; //current vector for TFIDF
            var vectors = bow.GetVectorsList();
            List<List<double>> WholeTFIDF = new List<List<double>>();

            while (j < vectors.Count())
            {
                List<double> TFIDF = new List<double>();
                var v = vectors[j].GetVector();
                int index = 0;
                while (index < v.Count())
                {
                    int count = 0;
                    int TF = v[index]; //Term frequency is stored in Bag of Word
                    foreach (var vector in vectors)
                    {
                        List<int> V = vector.GetVector();           
                        if (V[index] > 0)
                            count++;

                    }
                    double IDF = Math.Log(vectors.Count() / count);
                    TFIDF.Add(IDF * TF);
                    index++;
                }
                WholeTFIDF.Add(TFIDF); //adding whole list to list of lists.
                j++;
            }

        }
    }
}
