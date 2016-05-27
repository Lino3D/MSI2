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
        int id;

        public DataClass(myVector x, string y)
        {
            Vectors.Add(x);
            ClassName = y;
            AssignId();
        }
        public DataClass(List<myVector> x, string y)
        {
            foreach(myVector V in x)
            {
                Vectors.Add(V);
            }
            ClassName = y;
            AssignId();
        }
        public int GetID()
        {
            return id;
        }
        public DataClass(string x)
        {
            ClassName = x;
            AssignId();
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

        private void AssignId()
        {
            switch (ClassName)
            {
                case "Kulinaria":
                    id = 0;
                    break;
                case "Motoryzacja":
                    id = 1;
                    break;
                case "Zoologia":
                    id = 3;
                    break;
                case "Technologia":
                    id = 2;
                    break;
            }
        }
        public static List<DataClass> CreateDataClasses(BagOfWords BoW )
        {
            List<String> ClassNames = new List<String>();
            List<DataClass> Classes = new List<DataClass>();

            foreach(myVector v in BoW.GetVectorsList())
            {
                string Classname = v.GetVectorName();
                Classname = Classname.Split('_').First();
                if (!ClassNames.Contains(Classname))
                    ClassNames.Add(Classname);
            }
            foreach (string s in ClassNames)
                Classes.Add(new DataClass(s));

            return Classes;
        }
    }
    public class Neighbour
    {
        private double distance;
        private int id;

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public Neighbour(double a, int b)
        {
            distance = a;
            id = b;
        }
 
    }
    public class TestClass
    {
        private List<DataClass> trainingClasslist = new List<DataClass>();
        private List<myVector> trainingVectorlist = new List<myVector>();
        private List<myVector> testlist = new List<myVector>();


    

        public TestClass()
        {

        }

        public TestClass(List<DataClass> c, List<myVector> V)
        {
            trainingClasslist = c;
            testlist = V;
        }
        public TestClass(List<myVector> TrainingVectors, List<myVector> TestVectors)
        {
            trainingVectorlist = TrainingVectors;
            testlist = TestVectors;
        }
        public List<DataClass> GetTrainingClasses()
        {
            return trainingClasslist;
        }
        public List<myVector> GetTrainingtVectors()
        {
            return trainingVectorlist;
        }
        public List<myVector> GetTestVectors()
        {
            return testlist;
        }

    }





}
