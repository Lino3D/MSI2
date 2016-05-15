using Klasyfikacja_Danych.Classes;
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

        public static Network CreateDefaultNetwork(int size, int CatSize)
        {
            Random rand = new Random();
            Network Net = new Network();

            List<Neuron> NeuronList = new List<Neuron>();

            List<Neuron> OutputNeuronList = new List<Neuron>();

            for (int i = 0; i < size; i++)
                NeuronList.Add(new Neuron(i, 1, (float)rand.NextDouble()));

            for (int i = 0; i < CatSize; i++)
                OutputNeuronList.Add(new Neuron(-i - 1, 1, 1));

            foreach (var neuron in NeuronList)
            {
                foreach (var outputNeuron in OutputNeuronList)
                    neuron.connect(outputNeuron, (float)rand.NextDouble());
                Net.addNeuron(neuron);
            }

            foreach (var neuron in OutputNeuronList)
                Net.addNeuron(neuron);

            return Net;
        }



        public static void NewSampleInput(myVector sampleInput, Network N)
        {
            var vector = sampleInput.GetVector();

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
                var ConnectionVector = neuron.GetConnections();

                for( int j = vector.Count,  k = 0 ; k < ConnectionVector.Count; j++, k++)
                {
                    N.getNetwork().ElementAt(j).Input += neuron.Input * neuron.GetWeights()[k];
                }
            }
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
