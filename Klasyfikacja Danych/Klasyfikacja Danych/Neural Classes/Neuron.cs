using Klasyfikacja_Danych.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Neural_Classes
{
    [DataContract(IsReference = true)]
    public class Neuron
    {
        [DataMember]
        private int id;
        private float threshold;
        public List<Neuron> connectionsOld = new List<Neuron>();
        [DataMember]
        private List<Connection> connections = new List<Connection>();
       // [DataMember]
        public List<float> weights = new List<float>();
        // useless parameter
        [DataMember]
        private float weight;
        private float bias = 1;
        [DataMember]
        private float input;
        [DataMember]
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
        public Neuron()
        { }
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
    [DataContract]
    public class Connection
    {
        [DataMember]
        private int id;
        [DataMember]
        private float weight;
        private float newweight;

        [DataMember]
        private Neuron from;
        [DataMember]
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
        public float NewWeight
        {
            get { return newweight; }
            set { newweight = value; }
        }
        public Connection()
        { }
       public Connection(int a, float b, Neuron X, Neuron Y)
        {
            id = a;
            weight = b;
            from = X;
            to = Y;
        }

    }
    [DataContract]
    public class Network
    {
        [DataMember]
        private int id;
        [DataMember]
        public List<Neuron> Neurons = new List<Neuron>();
        [DataMember]
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
        public List<List<float>> ExportConnectionWeights()
        {
            List<List<float>> weights = new List<List<float>>();

            foreach (Neuron n in this.Neurons)
            {
                var list = new List<float>();
                List<Connection> connections = n.GetConnections();
                foreach(Connection c in connections)
                {
                    list.Add(c.Weight);
                }
                weights.Add(list);
            }
            return weights;
        }



    }
}
