using Klasyfikacja_Danych.Neural_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{
    public static class BackPropagation
    {

        public static void UczenieSieci(int k, List<myVector> V, Network N, List<DataClass> classes)
        {
            float Eta = 0.1f;
            List<float> ErrorLst = new List<float>();
            float ErrorTotal;
            float delta;
            float target;
            float coef = 0.0f;
            var middle = N.getNetwork().Where(o => o.type == 1).ToList();
            var output = N.getNetwork().Where(o => o.type == 2).ToList();
            int count = k;

            while (k > 0)
            {
                foreach (var vector in V)
                {
                    NeuralConstruction.SampleInput(vector, N);
                    int id = classes.Where(o => o.GetVectors().Contains(vector)).First().GetID();

                    // Teaching the Output Part basing on the Hidden Part
                    var HiddenLayer = N.getNetwork().Where(o => o.type == 1);
                    foreach (var neuron in HiddenLayer)
                    {
                        var connections = neuron.GetConnections();
                        for (int i = 0; i < connections.Count; i++)
                        {
                            target = (id == i) ? 1 : 0;
                            delta = (connections[i].To.Input - target) * (connections[i].To.Input * (1 - connections[i].To.Input)) * neuron.Input;
                            connections[i].NewWeight = connections[i].Weight - (Eta * delta);
                        }
                    }
                    // Teaching the Hidden Part basing on the Input Part

                    // Calculating the coeficient - I made it compute in here because it is constatnt for all outputs
                    // And the second part of the computations
                    var InputLayer = N.getNetwork().Where(o => o.type == 0);

                    foreach (var neuron in InputLayer)
                    {
                        var connections = neuron.GetConnections();

                        for (int i = 0; i < connections.Count; i++)
                        {
                            var ConTo = connections[i].To.GetConnections();
                            coef = 0;
                            for (int j = 0; j < ConTo.Count; j++)
                            {
                                target = (id == j) ? 1 : 0;
                                coef += ConTo[j].Weight * (ConTo[j].To.Input * (1 - ConTo[j].To.Input)) * (ConTo[j].To.Input - target);
                            }

                            delta = coef * (connections[i].To.Input * (1 - connections[i].To.Input)) * neuron.Input;

                            connections[i].NewWeight = connections[i].Weight - (Eta * delta);
                        }
                    }

                    // Finalisation
                    AssingNewWeights(N);
                }
                k--;
            }
        }

        // Tego nie uzywamy ostatecznie
        private static float CalculateError(Network net, myVector V, List<DataClass> classes)
        {
            var outputLayer = net.getNetwork().Where(o => o.type == 2).ToList();
            int id = classes.Where(o => o.GetVectors().Contains(V)).First().GetID();
            float Error = 0;

            for (int i = 0; i < outputLayer.Count(); i++)
            {
                if (id == i)
                {
                    Error += (float)0.5 * (float)Math.Pow(((double)1 - outputLayer[i].Input), 2);
                }
                else
                {
                    Error += (float)0.5 * (float)Math.Pow(((double)0 - outputLayer[i].Input), 2);
                }
            }
            //      int id = classes.Where(o => o.GetVectors().Contains(V));


            return Error;
        }

        private static void AssingNewWeights(Network net)
        {
            foreach (var neuron in net.getNetwork())
            {
                foreach (var con in neuron.GetConnections())
                {
                    con.Weight = con.NewWeight;
                    con.NewWeight = 0;
                }
            }
        }

    }
}
