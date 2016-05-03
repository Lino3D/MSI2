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
            int minDistance = int.MaxValue;
            int distance = 0;
            List<List<int>> distances = new List<List<int>>();
            foreach(DataClass C in Classes) 
            {
                List<myVector> Vectors = C.GetVectors();
                List<int> classDistance = new List<int>();

                foreach (myVector vector in Vectors)
                {
                 
                    distance = HammingDistance(V, vector);
                    classDistance.Add(distance);
                }
                distances.Add(classDistance);
            }
            List<Neighbour> kNearest = new List<Neighbour>(k);
            foreach(List<int > cd in distances)
            {
                foreach(int d in cd)
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


        public static List<DataClass> CreateTrainingSet(List<DataClass> Classes, BagOfWords BoW, int k)
        {
            List<myVector> Articles = BoW.GetVectorsList();
            Random rand = new Random();

            foreach (myVector V in Articles)
            {             
                List<int> article = V.GetVector();
                string name = V.GetVectorName();
                foreach (DataClass C in Classes)
                {
                    if (C.GetVectors().Count < k)
                    {
                        if (name.Contains(C.GetName()))
                        {
                            C.AddVector(V);
                        }
                    }
                }
            }    
            return Classes;
        }

       
       public static int HammingDistance(myVector a, myVector b)
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
