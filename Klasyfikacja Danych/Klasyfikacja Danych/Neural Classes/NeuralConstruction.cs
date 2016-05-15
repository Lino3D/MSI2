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
            Neuron b = new Neuron(1, 1, rand.Next(-1, 1));
            Neuron c = new Neuron(2, 1, rand.Next(-1, 1));
            Neuron d = new Neuron(3, 1, rand.Next(-1, 1));
            Neuron e = new Neuron(4, 1, rand.Next(-1, 1));
            Neuron f = new Neuron(5, 1, rand.Next(-1, 1));

            a.connect(b,c,d,e);
            b.connect(f);
            c.connect(f);
            d.connect(f);
            e.connect(f);
            Network N = new Network();
            N.addNeuron(a, b, c, d, e, f);

            return N;
        }
        public static void sampleInput(myVector sampleInput, Network N)
        {
            List<Neuron> neurons = N.getNetwork();

            Neuron inputNeuron = neurons[0];

            inputNeuron.Input = sampleInput;
      

            foreach(Neuron n in inputNeuron.GetConnections())
            {
                var tmp = inputNeuron.Input;

                foreach(double x in tmp.GetVector())
                {
                    var y = x;
                    y = x * n.Weight;
                //    myVector V = new myVector();


                }
               // n.Input = inputNeuron.Input * (double)n.Weight;
            }
        }




    }
}
