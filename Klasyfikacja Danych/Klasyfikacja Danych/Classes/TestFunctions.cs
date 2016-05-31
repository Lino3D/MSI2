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

            int minCount = int.MaxValue;

            foreach(DataClass C in TrainingSet)
            {
                if(minCount > C.GetVectors().Count)
                    minCount = C.GetVectors().Count;
            }
            int nVectors = minCount / 2;


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

        public static TestClass CreateTest2(List<DataClass> classes)
        {
            List<DataClass> TrainingSet = new List<DataClass>();
            List<myVector> TestSet = new List<myVector>();

            foreach (DataClass c in classes)
            {
                TrainingSet.Add(c);
            }


            int minCount = int.MaxValue;

            foreach (DataClass C in classes)
            {
                if (minCount > C.GetVectors().Count)
                    minCount = C.GetVectors().Count;
            }
            int nVectors = minCount / 2;

            int x = 0;
            Random r = new Random();
            List<int> indices = new List<int>();
            foreach (DataClass C in classes)
            {
                int i = classes.IndexOf(C);
                 x = 0;
                List<myVector> vectors = C.GetVectors();
              
                int counter = r.Next(0, minCount);
                while(x< nVectors)
                {
                    int index = r.Next(0, vectors.Count);
                    if (TestSet.Contains(vectors[index]) == false)
                    {
                        TestSet.Add(vectors[index]);
                        TrainingSet[i].GetVectors().RemoveAt(index);
                    }
                    x++;
                }
                
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
        public static TestClass CreateVectorTest(BagOfWords bow)
        {
            TestClass T;
            Random r = new Random();
            var allVectors = bow.GetVectorsList();
            var TrainingVectors = new List<myVector>();
            var TestVectors = new List<myVector>();

            int count = allVectors.Count();

            int trainingCount = (int)(0.8 * count);
            int testCount = count - trainingCount;

            while(TrainingVectors.Count < trainingCount)
            {
                int index = r.Next(0, count);
                if(TrainingVectors.Contains(allVectors[index])==false)
                TrainingVectors.Add(allVectors[index]);
            }

            foreach(myVector v in allVectors)
            {
                if((TrainingVectors.Contains(v))==false)
                {
                    TestVectors.Add(v);
                }
            }

            T = new TestClass(TrainingVectors, TestVectors);
                return T;
        }

    }
}
