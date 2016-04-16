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
        List<string> StopWords = new List<string> { "a", "aby", "ach", "acz", "aczkolwiek", "aj", "albo", "ale", "ależ", "ani", "aż", "bardziej", "bardzo", "bo", "bowiem", "by", "byli", "bynajmniej", "być", "był", "była", "było", "były", "będzie", "będą", "cali", "cała", "cały", "ci", "cię", "ciebie", "co", "cokolwiek", "coś", "czasami", "czasem", "czemu", "czy", "czyli", "daleko", "dla", "dlaczego", "dlatego", "do", "dobrze", "dokąd", "dość", "dużo", "dwa", "dwaj", "dwie", "dwoje", "dziś", "dzisiaj", "gdy", "gdyby", "gdyż", "gdzie", "gdziekolwiek", "gdzieś", "i", "ich", "ile", "im", "inna", "inne", "inny", "innych", "iż", "ja", "ją", "jak", "jakaś", "jakby", "jaki", "jakichś", "jakie", "jakiś", "jakiż", "jakkolwiek", "jako", "jakoś", "je", "jeden", "jedna", "jedno", "jednak", "jednakże", "jego", "jej", "jemu", "jest", "jestem", "jeszcze", "jeśli", "jeżeli", "już", "ją", "każdy", "kiedy", "kilka", "kimś", "kto", "ktokolwiek", "ktoś", "która", "które", "którego", "której", "który", "których", "którym", "którzy", "ku", "lat", "lecz", "lub", "ma", "mają", "mało", "mam", "mi", "mimo", "między", "mną", "mnie", "mogą", "moi", "moim", "moja", "moje", "może", "możliwe", "można", "mój", "mu", "musi", "my", "na", "nad", "nam", "nami", "nas", "nasi", "nasz", "nasza", "nasze", "naszego", "naszych", "natomiast", "natychmiast", "nawet", "nią", "nic", "nich", "nie", "niech", "niego", "niej", "niemu", "nigdy", "nim", "nimi", "niż", "no", "o", "obok", "od", "około", "on", "ona", "one", "oni", "ono", "oraz", "oto", "owszem", "pan", "pana", "pani", "po", "pod", "podczas", "pomimo", "ponad", "ponieważ", "powinien", "powinna", "powinni", "powinno", "poza", "prawie", "przecież", "przed", "przede", "przedtem", "przez", "przy", "roku", "również", "sama", "są", "się", "skąd", "sobie", "sobą", "sposób", "swoje", "ta", "tak", "taka", "taki", "takie", "także", "tam", "te", "tego", "tej", "temu", "ten", "teraz", "też", "to", "tobą", "tobie", "toteż", "trzeba", "tu", "tutaj", "twoi", "twoim", "twoja", "twoje", "twym", "twój", "ty", "tych", "tylko", "tym", "u", "w", "wam", "wami", "was", "wasz", "wasza", "wasze", "we", "według", "wiele", "wielu", "więc", "więcej", "wszyscy", "wszystkich", "wszystkie", "wszystkim", "wszystko", "wtedy", "wy", "właśnie", "z", "za", "zapewne", "zawsze", "ze", "zł", "znowu", "znów", "został", "żaden", "żadna", "żadne", "żadnych", "że", "żeby" };
        #endregion

        List<Vector> Vectors = new List<Vector>();
        List<string> Words = new List<string>();


        bool VectorsAdded = false;
        public void AddWords(string text, params string[] MoreText)
        {
            if (VectorsAdded)
                throw (new Exception("Wczesniej zostaly dodane wektory. Nie mozna kontynuowac, gdyz nie zgadzaja sie wymiary"));
            var tmp = Helper.FormatText(text);
            foreach (var word in tmp)
                if (!StopWords.Contains(word) && !Words.Contains(word))
                    Words.Add(word);
            // Params part of the declaration of function
            if( MoreText != null && MoreText.Count() != 0)
                foreach( var txt in MoreText)
                {
                    var tmp2 = Helper.FormatText(text);
                    foreach (var word in tmp2)
                        if (!StopWords.Contains(word) && !Words.Contains(word))
                            Words.Add(word);
                }
        }
        public void AddVector(string text, string Name)
        {
            VectorsAdded = true;
            var tmp = Helper.FormatText(text);
            Vector v = new Vector(Words,tmp,Name);
            Vectors.Add(v);
        }

        public List<string> GetWordsList()
        {
            return Words;
        }

        public void ResetujBagOfWords()
        {
            Words.RemoveRange(0, Words.Count - 1);
            Vectors.RemoveRange(0, Vectors.Count - 1);
            VectorsAdded = false;
        }

        
    }

    class Vector
    {
        int[] vector;
        string Name;

        public Vector(List<string> BoWWords, List<string> words, string name )
        {
            int index;
            vector = new int[BoWWords.Count];
            for( int i = 0; i < BoWWords.Count; i++)
                vector[i] = 0;
            foreach( var word in words )
            {
                index = BoWWords.FindIndex( o => o == word);
                vector[index]++;
            }
            Name = name;
        }
    }
}
