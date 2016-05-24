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
                           if(d>kNearest[i].Distance &&  (ContainCheck(kNearest,d, distances.IndexOf(cd)) ==false))
                            {
                                kNearest[i].Distance = d;
                                kNearest[i].Id = distances.IndexOf(cd);
                            }
                        }
                    }
                }
            }
            int[] best = new int[Classes.Count];
            for(int i=0; i<best.Count(); i++)
            { best[i] = 0; }
            foreach( Neighbour N in kNearest)
            {
                best[N.Id]++;
            }
            int bestOption = best.ToList().IndexOf(best.Max());

        return bestOption;
        }


        public static bool ContainCheck(List<Neighbour> Neighs, double D, int id)
        {
            bool contains = false;
            foreach (Neighbour N in Neighs)
            {
                if (N.Distance == D && N.Id==id)
                    contains = true;
            }
            return contains;
        }

   



        public static double HammingDistance(myVector a, myVector b)
        {
            List<double> v1 = a.GetVector();
            List<double> v2 = b.GetVector();

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

            List<double> v1 = a.GetVector();
            List<double> v2 = b.GetVector();
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
