using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Danych.Classes
{
    public class TestResult
    {
        private string algorithmname;
        private string correctclass;
        private string foundclass;
        private bool result = false;

        public string AlgorithmName
        {
            get { return algorithmname; }
            set { algorithmname = value; }
        }

        public string CorrectClass
        {
            get { return correctclass; }
            set { correctclass = value; }
        }
        public string FoundClass
        {
            get { return foundclass; }
            set { foundclass = value; }
        }
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }
        
        public TestResult(string name)
        {
            algorithmname = name;
        }
        public TestResult filltestData(List<DataClass> classes, int id, myVector v)
        {
      
                foundclass = classes[id].GetName();
            
            correctclass = v.GetVectorName();
            correctclass = correctclass.Remove(correctclass.Length - 2);

            if(correctclass==foundclass)
            {
                result = true;
            }
            return this;
        }



    }
}
