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
        #region NetworkWithoutHiddenLayer
        public static Network CreateDefaultNetwork(int size, List<DataClass> Classes)
        {
            Random rand = new Random();
            Network Net = new Network();

            List<Neuron> InputNeuronList = new List<Neuron>();

            List<Neuron> OutputNeuronList = new List<Neuron>();

            for (int i = 0; i < size; i++)
                InputNeuronList.Add(new Neuron(i, 1, 0));// (float)rand.NextDouble()));
            //
            for (int i = 0; i < Classes.Count; i++)
                OutputNeuronList.Add(new Neuron(-i - 1, Classes[i].GetName(), 2));

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

        public static int OldSampleInput(myVector sampleInput, Network N)
        {
            var vector = sampleInput.GetVector();
            int id = 0;
            // Zerujemy inputy
            foreach (var neuron in N.getNetwork())
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

                for (int j = vector.Count, k = 0; k < ConnectionVector.Count; j++, k++)
                {
                    N.getNetwork().ElementAt(j).Input += neuron.Input * neuron.GetWeights()[k];
                }
            }
            int connectionCount = N.getNetwork()[0].GetConnectionsOld().Count();

            double max = 0;
            for (int i = vector.Count; i < vector.Count + connectionCount; i++)
            {
                var neuron = N.getNetwork().ElementAt(i);
                if (neuron.Input > max)
                {
                    max = neuron.Input;
                    id = (-neuron.ID) - 1;
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
                    index = Classes.IndexOf(Classes.Where(o => o.GetName().Contains(Vectors[j].GetVectorName().Substring(0, 5))).First());
                    weights[index] += Vectors[j].GetVector()[i];
                }
                foreach (var elem in weights)
                    sum += elem;

                for (int k = 0; k < weights.Count; k++)
                {
                    N.getNetwork()[i].GetWeights()[k] = (float)(weights[k] / sum);
                }


            }
        }

        #endregion


        public static Network CreateNewDefaultNetwork(int size, List<DataClass> Classes, int k)
        {
            Random rand = new Random();
            Network Net = new Network();

            List<Neuron> InputNeuronList = new List<Neuron>();

            List<Neuron> HiddenNeuronList = new List<Neuron>();

            List<Neuron> OutputNeuronList = new List<Neuron>();

            for (int i = 0; i < size; i++)
                InputNeuronList.Add(new Neuron(i, 1, (float)rand.NextDouble(), 0));// (float)rand.NextDouble()));
            //
            for (int i = 0; i < Classes.Count; i++)
                OutputNeuronList.Add(new Neuron(-i - 1, Classes[i].GetName(), 2));

            for (int i = 0; i < k; i++)
                HiddenNeuronList.Add(new Neuron(i, 1, (float)rand.NextDouble(), 1));

            int j = 0;
            //connect input and hidden
            foreach (var inputNeuron in InputNeuronList)
            {

                foreach (var hiddenNeuron in HiddenNeuronList)
                {
                    inputNeuron.Connect(j, hiddenNeuron, (float)rand.NextDouble());
                    j++;
                }
            }
            //connect hidden with output;
            foreach (var hiddenNeuron in HiddenNeuronList)
            {

                foreach (var outputNeuron in OutputNeuronList)
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

        public static int SampleInput(myVector sampleInput, Network N)
        {
            float sum;
            float Error = 0;
            int foundID = -35;
            int correctID = 25;
            int Counter = 0;

            ClearInputs(N);
            //float NormalizationCoef = 1 / sampleInput.GetVector().Count;
            float NormalizationCoef = 1.0f / 50.0f;
            var WordsList = sampleInput.GetVector();
            var InputLayer = N.getNetwork().Where(o => o.type == 0).ToList();


            var HiddenLayer = N.getNetwork().Where(o => o.type == 1).ToList();
            var OutputLayer = N.getNetwork().Where(o => o.type == 2).ToList();


            //while (foundID != correctID && Counter < 20)
            //{
            //    sum = 0;
            //    N = ClearInputs(N);

            // Pierwsza iteracja           
            for (int i = 0; i < sampleInput.GetVector().Count; i++)
                InputLayer[i].Input = (float)WordsList[i];

            // Druga iteracja
            CalculateInput(InputLayer);
            SigmoidFunction(HiddenLayer, NormalizationCoef);

            // Trzecia iteracja           
            CalculateInput(HiddenLayer);
            SigmoidFunction(OutputLayer, NormalizationCoef);

            // Liczenie wyniku
            foundID = -OutputLayer.OrderBy(o => o.Input).Reverse().FirstOrDefault().ID - 1;
            //     correctID = -CorrectId(sampleInput, OutputLayer) - 1;
            //
            //    // Dopasowywanie wag
            //    sum = SumInputs(OutputLayer, sum);
            //    Error = (OutputLayer[correctID].Input - OutputLayer[foundID].Input) / sum;
            //    if (foundID != correctID)
            //    {
            //        AdjustWeights(HiddenLayer, Error);
            //        AdjustWeights(InputLayer, Error);
            //    }
            //    Counter++;
            //}



            return foundID;
        }
        private static void AdjustWeights(List<Neuron> Layer, float error)
        {
            foreach (var neuron in Layer)
            {
                foreach (var connection in neuron.GetConnections())
                {
                    connection.Weight = connection.Weight * error;
                }
            }
        }
        private static float SumInputs(List<Neuron> OutputLayer, float sum)
        {
            foreach (Neuron n in OutputLayer)
            {
                sum += n.Input;
            }

            return sum;
        }
        private static int CorrectId(myVector sample, List<Neuron> outputlayer)
        {
            int correctID = 0;
            string correctclassName = sample.GetVectorName();
            correctclassName = correctclassName.Remove(correctclassName.Length - 2);
            foreach (Neuron n in outputlayer)
            {
                if (n.Category == correctclassName)
                    correctID = n.ID;
            }
            return correctID;


        }
        private static void CheckResult(int CorrectID, int ReturnedId)
        {

        }




        private static void CalculateInput(List<Neuron> Layer)
        {
            foreach (var neuron in Layer)
            {
                foreach (var connection in neuron.GetConnections())
                {
                    connection.To.Input += connection.From.Input * connection.Weight;
                }
            }
        }
        private static void ClearInputs(Network net)
        {
            foreach (var neuron in net.getNetwork())
            {
                neuron.Input = 0.0f;
            }
        }
        private static void SigmoidFunction(List<Neuron> Layer, float NormalizationCoef)
        {
            foreach (var neuron in Layer)
            {
                neuron.Input =  1 / (1 + (float)Math.Pow(Math.E, -(double)neuron.Input*NormalizationCoef));
            //    neuron.Input *= NormalizationCoef*10.0f;
            }            
        }

        

    }
}
