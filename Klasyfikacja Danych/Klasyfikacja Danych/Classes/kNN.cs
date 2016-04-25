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
            List<int> distances = new List<int>();
            foreach(DataClass C in Classes)
            {
                List<myVector> Vectors = C.GetVectors();

                foreach(myVector vector in Vectors)
                {
                    distance = HammingDistance(V, vector);
                    if (distance < minDistance)
                        minDistance = distance;
                }
                distances.Add(minDistance);
                minDistance = int.MaxValue;
                distance = 0;
            }
            foreach(int d in distances)
            {
                if (d < minDistance)
                    minDistance = d;
            }
            int id = distances.IndexOf(minDistance);

            Classes[id].AddVector(V);

            return id;
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

    }
}
