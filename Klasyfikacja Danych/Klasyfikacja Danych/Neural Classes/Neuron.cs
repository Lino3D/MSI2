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
        private List<Neuron> connectionsOld = new List<Neuron>();
        private List<Connection> connections = new List<Connection>();
        private List<float> weights = new List<float>();
        // useless parameter
        private float weight;
        private float bias = 1;
        private float input;
        private string KatName;
        private int Type;  // 0 -> input, 1-> hidden, 2-> output


        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int type
        {
            get { return Type; }
            set { Type = value; }
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
        public string Category
        {
            get { return KatName; }
            set { KatName = Category; }
        }

        public Neuron(int a, string Name, int type)
        {
            id = a;
            threshold = 1;
            weight = 1;
            KatName = Name;
            this.Type = type;
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

        public Neuron(int a, float b, float c, int type)
        {
            id = a;
            threshold = b;
            weight = c;
            this.Type = type;
            // connections = null;
        }


        public void connectOld(Neuron neuron, float weight)
        {
            connectionsOld.Add(neuron);
            weights.Add(weight);
        }
        public void connectOld(params KeyValuePair<Neuron, float>[] neurons)
        {
            foreach (var n in neurons)
            {
                connectionsOld.Add(n.Key);
                weights.Add(n.Value);
            }
        }

        public void Connect(int id,Neuron neuron, float weight)
        {
            Connection C = new Connection(id, weight, this, neuron);
            connections.Add(C);
        }


        public List<Neuron> GetConnectionsOld()
        {
            return connectionsOld;
        }

        public List<Connection> GetConnections()
        {
            return connections;
        }

        public List<float> GetWeights()
        {
            return weights;
        }


    }
    public class Connection
    {
        private int id;
        private float weight;
        private Neuron from;
        private Neuron to;

        public Neuron From
        {
            get { return from; }
            set { from = value; }
        }
        public Neuron To
        {
            get { return to; }
            set { to = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }
       public Connection(int a, float b, Neuron X, Neuron Y)
        {
            id = a;
            weight = b;
            from = X;
            to = Y;
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
