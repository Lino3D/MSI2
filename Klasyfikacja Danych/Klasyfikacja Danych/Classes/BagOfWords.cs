using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{

    public class BagOfWords
    {
        #region StopWords
        List<string> StopWords = new List<string> { "a", "aby", "ach", "acz", "aczkolwiek", "aj", "albo", "ale", "ależ",
            "ani", "aż", "bardziej", "bardzo", "bo", "bowiem", "by", "byli", "bynajmniej", "być",
            "był", "była", "było", "były", "będzie", "będą", "cali", "cała", "cały", "ci", "cię", "ciebie", "co",
            "cokolwiek", "coś", "czasami", "czasem", "czemu", "czy", "czyli", "daleko", "dla", "dlaczego", "dlatego",
            "do", "dobrze", "dokąd", "dość", "dużo", "dwa", "dwaj", "dwie", "dwoje", "dziś", "dzisiaj", "gdy", "gdyby",
            "gdyż", "gdzie", "gdziekolwiek", "gdzieś", "i", "ich", "ile", "im", "inna", "inne", "inny", "innych", "iż",
            "ja", "ją", "jak", "jakaś", "jakby", "jaki", "jakichś", "jakie", "jakiś", "jakiż", "jakkolwiek", "jako", "jakoś",
            "je", "jeden", "jedna", "jedno", "jednak", "jednakże", "jego", "jej", "jemu", "jest", "jestem", "jeszcze", "jeśli",
            "jeżeli", "już", "ją", "każdy", "kiedy", "kilka", "kimś", "kto", "ktokolwiek", "ktoś", "która",
            "które", "którego", "której", "który", "których", "którym", "którzy", "ku", "lat", "lecz",
            "lub", "ma", "mają", "mało", "mam", "mi", "mimo", "między", "mną", "mnie", "mogą", "moi", "moim", "moja", "moje",
            "może", "możliwe", "można", "mój", "mu", "musi", "my", "na", "nad", "nam", "nami", "nas", "nasi", "nasz", "nasza",
            "nasze", "naszego", "naszych", "natomiast", "natychmiast", "nawet", "nią", "nic", "nich", "nie", "niech", "niego",
            "niej", "niemu", "nigdy", "nim", "nimi", "niż", "no", "o", "obok", "od", "około", "on", "ona", "one", "oni", "ono",
            "oraz", "oto", "owszem", "pan", "pana", "pani", "po", "pod", "podczas", "pomimo", "ponad", "ponieważ", "powinien",
            "powinna", "powinni", "powinno", "poza", "prawie", "przecież", "przed", "przede", "przedtem", "przez", "przy", "roku",
            "również", "sama", "są", "się", "skąd", "sobie", "sobą", "sposób", "swoje", "ta", "tak", "taka", "taki", "takie", "także",
            "tam", "te", "tego", "tej", "temu", "ten", "teraz", "też", "to", "tobą", "tobie", "toteż", "trzeba", "tu", "tutaj", "twoi",
            "twoim", "twoja", "twoje", "twym", "twój", "ty", "tych", "tylko", "tym", "u", "w", "wam", "wami", "was", "wasz", "wasza",
            "wasze", "we", "według", "wiele", "wielu", "więc", "więcej", "wszyscy", "wszystkich", "wszystkie", "wszystkim", "wszystko",
            "wtedy", "wy", "właśnie", "z", "za", "zapewne", "zawsze", "ze", "zł", "znowu", "znów", "został", "żaden", "żadna", "żadne", "żadnych", "że", "żeby" };
        #endregion

        List<myVector> Vectors = new List<myVector>();
        List<string> Words = new List<string>();

        public void AddWords(string text)
        {
            var tmp = Helper.FormatText(text);
            foreach (var word in tmp)
                if (!StopWords.Contains(word) && !Words.Contains(word))
                    Words.Add(word);
            AdjustVectorLists(null);
        }
        public void AddWords(List<string> words)
        {
            foreach (var word in words)
                if (!StopWords.Contains(word) && !Words.Contains(word))
                    Words.Add(word);
            AdjustVectorLists(null);
        }
        public void RemoveWords(params string[] ToRemove)
        {
            int index;
            if (ToRemove.Count() != 0)
            {
                List<int> RemoveIndices = new List<int>();
                foreach (var RemovedWord in ToRemove)
                {
                    index = Words.IndexOf(RemovedWord);
                    Words.RemoveAt(index);
                    RemoveIndices.Add(index);
                }
                AdjustVectorLists(RemoveIndices.ToArray());
                RemoveIndices = null;
            }
        }
        public void RemoveWords(List<string> ToRemove)
        {
            int index;
            if (ToRemove.Count() != 0)
            {
                List<int> RemoveIndices = new List<int>();
                foreach (var RemovedWord in ToRemove)
                {
                    index = Words.IndexOf(RemovedWord);
                    Words.RemoveAt(index);
                    RemoveIndices.Add(index);
                }
                AdjustVectorLists(RemoveIndices.ToArray());
                RemoveIndices = null;
            }
        }
        private void AdjustVectorLists(params int[] RemovedIndexes)
        {
            foreach (var vector in Vectors)
            {
                var v = vector.GetVector();
                if (v.Count < Words.Count)//&& RemovedIndexes.Count() == 0)
                    for (int i = Words.Count - v.Count; i > 0; i--)
                        v.Add(0);
                else if (v.Count > Words.Count && RemovedIndexes != null && RemovedIndexes.Count() != 0)
                {
                    foreach (var removed in RemovedIndexes)
                        v.RemoveAt(removed);
                }
                else if ((v.Count > Words.Count && RemovedIndexes == null) || (v.Count > Words.Count && RemovedIndexes != null && RemovedIndexes.Count() == 0))
                    throw (new Exception("Nie zespecyfikowana tablica usunietych slow przy usuwaniu słów z Words."));
                else if (v.Count != Words.Count)
                    throw (new Exception("An error occured. Some values in Words list and Vectors list doesnt match"));

            }
        }
        public void AddVector(string text, string Name)
        {
            var tmp = Helper.FormatText(text);
            AddWords(tmp);
            myVector v = new myVector(Words, tmp, Name,StopWords);
            Vectors.Add(v);
        }
        public void AddVector(List<string> words, string Name)
        {
            AddWords(words);
            myVector v = new myVector(Words, words, Name, StopWords);
            Vectors.Add(v);
        }

        public List<string> GetWordsList()
        {
            return Words;
        }
        public List<myVector> GetVectorsList()
        {
            return Vectors;
        }

        public void ResetujBagOfWords()
        {
            if (Words.Count != 0)
                Words.RemoveRange(0, Words.Count);
            if (Vectors.Count != 0)
                Vectors.RemoveRange(0, Vectors.Count);
        }

    }

    public class myVector
    {
        List<double> vector = new List<double>();
        string Name;

        public myVector(List<string> BoWWords, List<string> words, string name, List<string> StopWords)
        {
            int index;
            for (int i = 0; i < BoWWords.Count; i++)
                vector.Add(0);
            foreach (var word in words)
            {
                //Sprawdzanie stopwords jest rowniez przy addwords. Nie jest to zbyt optymalne. Moznaby poprawic kiedys
                if (StopWords.Contains(word))
                    continue;
                index = BoWWords.FindIndex(o => o == word);
                vector[index]++;
            }
            Name = name;
        }

        public List<double> GetVector()
        {
            return vector;
        }
        public string GetVectorName()
        {
            return Name;
        }

    }
}
