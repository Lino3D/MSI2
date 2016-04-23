using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{
    class DataClass
    {
        List<Vector> Vectors = new List<Vector>();
        string ClassName;

        public DataClass(Vector x, string y)
        {
            Vectors.Add(x);
            ClassName = y;
        }
        public DataClass(List<Vector> x, string y)
        {
            foreach(Vector V in x)
            {
                Vectors.Add(V);
            }
            ClassName = y;
        }
        public DataClass(string x)
        {
            ClassName = x;
        }
        public static List<DataClass> CreateDataClasses(BagOfWords BoW )
        {
            List<String> ClassNames = new List<String>();
            List<DataClass> Classes = new List<DataClass>();

            foreach(Vector v in BoW.GetVectorsList())
            {
                string Classname = v.GetVectorName();
                Classname = Classname.Remove(Classname.Length - 2);
                if (!ClassNames.Contains(Classname))
                    ClassNames.Add(Classname);
            }
            foreach (string s in ClassNames)
                Classes.Add(new DataClass(s));

            return Classes;
        }
    }
}
