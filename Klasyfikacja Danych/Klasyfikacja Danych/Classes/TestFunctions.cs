using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{
   public static class TestFunctions
    {
        public static List<DataClass> CreateFullSet(List<DataClass> Classes, BagOfWords BoW)
        {
            List<myVector> Articles = BoW.GetVectorsList();
            Random rand = new Random();

            foreach (myVector V in Articles)
            {
                List<double> article = V.GetVector();
                string name = V.GetVectorName();
                foreach (DataClass C in Classes)
                {
                    if (name.Contains(C.GetName()))
                    {
                        C.AddVector(V);
                    }
                }
            }
            return Classes;
        }
        public static TestClass CreateTest(List<DataClass> TrainingSet)
        {
            // List<DataClass> TrainingSet = new List<DataClass>();
            List<myVector> TestSet = new List<myVector>();

            Random r = new Random();
            List<int> indices = new List<int>();
            foreach (DataClass C in TrainingSet)
            {
                List<myVector> vectors = C.GetVectors();
                int random = r.Next(0, vectors.Count);
                indices.Add(random);
            }
            for (int i = 0; i < indices.Count; i++)
            {
                List<myVector> V = TrainingSet[i].GetVectors();
                TestSet.Add(V[indices[i]]);
                TrainingSet[i].GetVectors().RemoveAt(indices[i]);

            }
            TestClass T = new TestClass(TrainingSet, TestSet);

            return T;
        }


        public static void TestResults(TestClass Test)
        {
            List<DataClass> classes = Test.GetTrainingClasses();
            List<myVector> incorrectVectors = new List<myVector>();
            foreach (DataClass c in classes)
            {
                List<myVector> vectors = c.GetVectors();
                foreach (myVector v in vectors)
                {
                    string name = v.GetVectorName();
                    name = name.Remove(name.Length - 2);
                    if (c.GetName() != name)
                    {
                        incorrectVectors.Add(v);
                    }
                }
            }
            int Precision = (Test.GetTestVectors().Count - incorrectVectors.Count) / Test.GetTestVectors().Count;
            int Recall = (Test.GetTestVectors().Count - incorrectVectors.Count) / Test.GetTestVectors().Count;
        }
        public static void CreateVectorTest(BagOfWords bow)
        {

        }

    }
}
