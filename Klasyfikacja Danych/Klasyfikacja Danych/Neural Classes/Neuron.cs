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
        private float weight;
        private float bias =1;


        public int ID
        {
            get { return id; }
            set { id = value; }
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

        public Neuron(int a)
        {
            id = a;
            threshold = 0;
            weight = 1;
            connections = null;
        }
        public Neuron(int a, float b)
        {
            id = a;
            threshold =b;
            weight = 1;
            connections = null;
        }
        public Neuron(int a, float b, float c)
        {
            id = a;
            threshold = b;
            weight = c;
            connections = null;
        }

        public void connect(Neuron b)
        {
            connections.Add(b);
        }
        public List<Neuron> GetConnections()
        {
            return connections;
        }

    }
    public class Network
    {
        private int id;
        private List<Neuron> Neurons = new List<Neuron>();
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public void addNeuron(Neuron a)
        {
            Neurons.Add(a);
        }
        public List<Neuron> getNetwork()
        {
            return Neurons;
        }

    }
}
