using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{
    public class DataClass
    {
        List<myVector> Vectors = new List<myVector>();
        string ClassName;

        public DataClass(myVector x, string y)
        {
            Vectors.Add(x);
            ClassName = y;
        }
        public DataClass(List<myVector> x, string y)
        {
            foreach(myVector V in x)
            {
                Vectors.Add(V);
            }
            ClassName = y;
        }
        public DataClass(string x)
        {
            ClassName = x;
        }
        public string GetName()
        {
            return ClassName;
        }
        public List<myVector> GetVectors()
        {
            return Vectors;
        }
        public void AddVector(myVector V)
        {
            Vectors.Add(V);
        }
        public static List<DataClass> CreateDataClasses(BagOfWords BoW )
        {
            List<String> ClassNames = new List<String>();
            List<DataClass> Classes = new List<DataClass>();

            foreach(myVector v in BoW.GetVectorsList())
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
