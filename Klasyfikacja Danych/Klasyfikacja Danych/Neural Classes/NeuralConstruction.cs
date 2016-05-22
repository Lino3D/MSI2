﻿using Klasyfikacja_Danych.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Neural_Classes
{
    public static class NeuralConstruction
    {
        public static Network SampleNetwork()
        {
            Random rand = new Random();
            Neuron a = new Neuron(0, 1, 1);
            Neuron b = new Neuron(1, 1, (float)(rand.NextDouble() * 2) - 1);
            Neuron c = new Neuron(2, 1, (float)(rand.NextDouble() * 2) - 1);
            Neuron d = new Neuron(3, 1, (float)(rand.NextDouble() * 2) - 1);
            Neuron e = new Neuron(4, 1, (float)(rand.NextDouble() * 2) - 1);
            Neuron f = new Neuron(5, 1, (float)(rand.NextDouble() * 2) - 1);


            //a.connect(b,c,d,e);
            //b.connect(f);
            //c.connect(f);
            //d.connect(f);
            //e.connect(f);
            Network N = new Network();
            N.addNeuron(a, b, c, d, e, f);

            return N;
        }

        public static Network CreateDefaultNetwork(int size, List<DataClass> Classes)
        {
            Random rand = new Random();
            Network Net = new Network();

            List<Neuron> InputNeuronList = new List<Neuron>();

            List<Neuron> OutputNeuronList = new List<Neuron>();

            for (int i = 0; i < size; i++)
                InputNeuronList.Add(new Neuron(i, 1,0));// (float)rand.NextDouble()));
//
            for (int i = 0; i < Classes.Count; i++)
                OutputNeuronList.Add(new Neuron( -i - 1, Classes[i].GetName(),2));

            foreach (var neuron in InputNeuronList)
            {
                foreach (var outputNeuron in OutputNeuronList)
                    neuron.connectOld(outputNeuron, 0);// (float)rand.NextDouble());
                Net.addNeuron(neuron);
            }

            foreach (var neuron in OutputNeuronList)
                Net.addNeuron(neuron);

            return Net;
        }
        public static Network CreateNewDefaultNetwork(int size, List<DataClass> Classes, int k)
        {
            Random rand = new Random();
            Network Net = new Network();

            List<Neuron> InputNeuronList = new List<Neuron>();

            List<Neuron> HiddenNeuronList = new List<Neuron>();

            List<Neuron> OutputNeuronList = new List<Neuron>();

            for (int i = 0; i < size; i++)
                InputNeuronList.Add(new Neuron(i, 1, (float)rand.NextDouble(),0));// (float)rand.NextDouble()));
                                                    //
            for (int i = 0; i < Classes.Count; i++)
                OutputNeuronList.Add(new Neuron(-i - 1, Classes[i].GetName(),2));

            for (int i = 0; i < k; i++)
                HiddenNeuronList.Add(new Neuron(i, 1, (float)rand.NextDouble(),1));

            //foreach (var neuron in InputNeuronList)
            //{
            //    foreach (var outputNeuron in OutputNeuronList)
            //        neuron.connectOld(outputNeuron, 0);// (float)rand.NextDouble());
            //    Net.addNeuron(neuron);
            //}
            int j = 0;
            foreach (var inputNeuron in InputNeuronList)
            {
               
                foreach(var hiddenNeuron in HiddenNeuronList)
                {
                    inputNeuron.Connect(j, hiddenNeuron, (float)rand.NextDouble());
                        j++;
                }
            }

            foreach (var hiddenNeuron in HiddenNeuronList)
            {

                foreach (var outputNeuron in HiddenNeuronList)
                {
                    hiddenNeuron.Connect(j, outputNeuron, (float)rand.NextDouble());
                    j++;
                }
            }
            foreach (var neuron in InputNeuronList)
                Net.addNeuron(neuron);

            foreach (var neuron in HiddenNeuronList)
                Net.addNeuron(neuron);

            foreach (var neuron in OutputNeuronList)
                Net.addNeuron(neuron);

            return Net;
        }



        public static int NewSampleInput(myVector sampleInput, Network N)
        {
            var vector = sampleInput.GetVector();
            int id = 0;
            // Zerujemy inputy
            foreach( var neuron in N.getNetwork())
                neuron.Input = 0;


            // Pierwsza iteracja
            for (int i = 0; i < vector.Count; i++)
            {
                // int index = vector[i];
                var neuron = N.getNetwork().ElementAt(i);
                neuron.Input = (float)vector[i];
            }

            // Druga iteracja
            for (int i = 0; i < vector.Count; i++)
            {
                // int index = vector[i];
                var neuron = N.getNetwork().ElementAt(i);
                var ConnectionVector = neuron.GetConnectionsOld();

                for( int j = vector.Count,  k = 0 ; k < ConnectionVector.Count; j++, k++)
                {
                    N.getNetwork().ElementAt(j).Input += neuron.Input * neuron.GetWeights()[k];
                }
            }
            int connectionCount = N.getNetwork()[0].GetConnectionsOld().Count();

            double max = 0;
            for(int i=vector.Count; i< vector.Count + connectionCount; i++)
            {
                var neuron = N.getNetwork().ElementAt(i);
                if (neuron.Input > max)
                {
                    max = neuron.Input;
                    id = (-neuron.ID) -1;
                }
            }



            return id;
        }

        public static void SampleWeight(Network N, List<myVector> Vectors, List<DataClass> Classes)
        {
            double sum = 0;
            int index;
            List<double> weights = new List<double>();

            //          Vectors.First().
            for (int i = 0; i < Classes.Count; i++)
                weights.Add(0);
            for (int i = 0; i < Vectors.First().GetVector().Count; i++)
            {
                sum = 0;
                for (int j = 0; j < weights.Count; j++)
                    weights[j] = 0;

                    for (int j = 0; j < Vectors.Count; j++)
                    {
                        index = Classes.IndexOf( Classes.Where(o => o.GetName().Contains(Vectors[j].GetVectorName().Substring(0, 5))).First());
                        weights[index] += Vectors[j].GetVector()[i];
                    }
                foreach (var elem in weights)
                    sum += elem;

                for( int k = 0; k < weights.Count; k++)
                {
                    N.getNetwork()[i].GetWeights()[k] = (float) (weights[k] / sum);
                }


            }
        }

        private static List<string> ZnajdzKategorie(List<myVector> lst)
        {
            List<string> kat = new List<string>();

            foreach( var item in lst)
            {
                if (!kat.Contains(item.GetVectorName().Substring(0, 5)))
                    kat.Add(item.GetVectorName().Substring(0, 5));
            }

            return kat;


        }

        public static void sampleInput(myVector sampleInput, Network N)
        {
            //List<Neuron> neurons = N.getNetwork();

            //Neuron inputNeuron = neurons[0];

            //inputNeuron.Input = sampleInput;


            //foreach (Neuron n in inputNeuron.GetConnections())
            //{
            //    var tmp = new myVector(inputNeuron.Input.GetVector().Count);

            //    var vector = inputNeuron.Input.GetVector();
            //    for (int i = 0; i < vector.Count; i++)
            //        tmp.GetVector()[i] = vector[i] * n.Weight;

            //    n.Input = tmp;
            //}



            //var tmp2 = new myVector(inputNeuron.Input.GetVector().Count);

            //var vector1 = neurons[1];
            //var vector2 = neurons[2];
            //var vector3 = neurons[3];
            //var vector4 = neurons[4];

            //for (int i = 0; i < vector1.Input.GetVector().Count; i++)
            //    tmp2.GetVector()[i] = vector1.Input.GetVector()[i] + vector2.Input.GetVector()[i] + vector3.Input.GetVector()[i] + vector4.Input.GetVector()[i];

            //neurons[5].Input = tmp2;



        }




    }
}
