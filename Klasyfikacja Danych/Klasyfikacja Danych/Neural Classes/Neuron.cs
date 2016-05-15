using Klasyfikacja_Danych.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Neural_Classes
{
    public class Neuron
    {
        private int id;
        private float threshold;
        private List<Neuron> connections = new List<Neuron>();
        private List<float> weights = new List<float>();
        // useless parameter
        private float weight;
        private float bias = 1;
        private float input;
        private string KatName;


        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public float Input
        {
            get { return input; }
            set { input = value; }
        }

        public float Treshhold
        {
            get { return threshold; }
            set { threshold = value; }
        }
        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public Neuron(int a, string Name)
        {
            id = a;
            threshold = 1;
            weight = 1;
            KatName = Name;
            // connections = null;
        }
        public Neuron(int a, float b)
        {
            id = a;
            threshold = b;
            weight = 1;
            //  connections = null;
        }
        public Neuron(int a, float b, float c)
        {
            id = a;
            threshold = b;
            weight = c;
            // connections = null;
        }


        public void connect(Neuron neuron, float weight)
        {
            connections.Add(neuron);
            weights.Add(weight);
        }
        public void connect(params KeyValuePair<Neuron, float>[] neurons)
        {
            foreach (var n in neurons)
            {
                connections.Add(n.Key);
                weights.Add(n.Value);
            }
        }
        public List<Neuron> GetConnections()
        {
            return connections;
        }

        public List<float> GetWeights()
        {
            return weights;
        }


    }
    public class Network
    {
        private int id;
        private List<Neuron> Neurons = new List<Neuron>();
        private string Name;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public void addNeuron(params Neuron[] neurons)
        {
            foreach (Neuron n in neurons)
            {
                Neurons.Add(n);
            }
        }
        public List<Neuron> getNetwork()
        {
            return Neurons;
        }



    }
}
