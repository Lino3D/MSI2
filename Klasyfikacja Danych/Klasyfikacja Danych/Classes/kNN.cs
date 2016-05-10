using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{
    public static class kNN
    {
        public static int CalculateKNN(myVector V, List<DataClass> Classes, int k)
        {
          //  int minDistance = int.MaxValue;
            double distance = 0;
            List<List<double>> distances = new List<List<double>>();
            foreach(DataClass C in Classes) 
            {
                List<myVector> Vectors = C.GetVectors();
                List<double> classDistance = new List<double>();

                foreach (myVector vector in Vectors)
                {
                 
                    distance = CosineDistance(V, vector);
                    classDistance.Add(distance);
                }
                distances.Add(classDistance);
            }
            List<Neighbour> kNearest = new List<Neighbour>(k);
            foreach(List<double> cd in distances)
            {
                foreach(double d in cd)
                {
                 if(kNearest.Count<k)
                    {
                        kNearest.Add(new Neighbour(d, distances.IndexOf(cd)));
                    }
                 else
                    {
                       for(int i=0; i<k; i++)
                        {
                           if(d<kNearest[i].Distance)
                            {
                                kNearest[i].Distance = d;
                                kNearest[i].Id = distances.IndexOf(cd);
                            }
                        }
                    }
                }
            }
            int[] best = new int[Classes.Count];
            for(int i=0; i<k; i++)
            { best[i] = 0; }
            foreach( Neighbour N in kNearest)
            {
                best[N.Id]++;
            }
            int bestOption = best.ToList().IndexOf(best.Max());
            Classes[bestOption].AddVector(V);

        return bestOption;
        }


        //public static List<DataClass> CreateTrainingSet(List<DataClass> Classes, BagOfWords BoW, int k)
        //{
        //    List<myVector> Articles = BoW.GetVectorsList();
        //    Random rand = new Random();

        //    foreach (myVector V in Articles)
        //    {             
        //        List<int> article = V.GetVector();
        //        string name = V.GetVectorName();
        //        foreach (DataClass C in Classes)
        //        {
        //            if (C.GetVectors().Count < k)
        //            {
        //                if (name.Contains(C.GetName()))
        //                {
        //                    C.AddVector(V);
        //                }
        //            }
        //        }
        //    }    
        //    return Classes;
        //}
        public static List<DataClass> CreateFullSet(List<DataClass> Classes, BagOfWords BoW)
        {
            List<myVector> Articles = BoW.GetVectorsList();
            Random rand = new Random();

            foreach (myVector V in Articles)
            {
                List<int> article = V.GetVector();
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
        public static  TestClass CreateTest(List<DataClass> TrainingSet)
        {
           // List<DataClass> TrainingSet = new List<DataClass>();
            List<myVector> TestSet = new List<myVector>();

            Random r = new Random();
            List<int> indices = new List<int>();
            foreach(DataClass C in TrainingSet)
            {
                List<myVector> vectors = C.GetVectors();
                int random = r.Next(0, vectors.Count);
                indices.Add(random);
            }
            for(int i=0; i<indices.Count; i++)
            {
                List < myVector > V = TrainingSet[i].GetVectors();
                TestSet.Add(V[indices[i]]);
                TrainingSet[i].GetVectors().RemoveAt(indices[i]);
           
            }
            TestClass T = new TestClass(TrainingSet, TestSet);

            return T;
        }




        public static double HammingDistance(myVector a, myVector b)
        {
            List<int> v1 = a.GetVector();
            List<int> v2 = b.GetVector();

            int Distance = 0;
            if(v1.Count != v2.Count)
            { return -1; }

            else
            {
                for (int i = 0; i < v1.Count; i++)
                {
                    if (v1[i] != v2[i])
                        Distance++;
                }
            }

           return Distance;
        }
        public static double CosineDistance(myVector a, myVector  b)
            {

            List<int> v1 = a.GetVector();
            List<int> v2 = b.GetVector();

            double Distance = 0;

            if (v1.Count != v2.Count)
            { return -1; }
            else
            {
                double top=0;
                double bottom1=0;
                double bottom2 = 0;

                for (int i = 0; i < v1.Count(); i++)
                {
                    top += v1[i] * v2[i];
                    bottom1 += Math.Pow(Math.Abs(v1[i]),2);
                    bottom2 += Math.Pow(Math.Abs(v2[i]),2);
                }

                Distance = top / (Math.Sqrt(bottom1) * Math.Sqrt(bottom2));

            }

            return Distance;

        }


    }
}
