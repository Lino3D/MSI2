using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Program
    {
        //public static int IndexOfLongestRun(string str)
        //{

        //    List<int> runs = new List<int>();
        //    List<int> indices = new List<int>();
        //    int counter = 1;
        //    for(int i=1; i<str.Length; i++)
        //    {
        //        if (str[i] == str[i - 1])
        //            counter++;
        //        else
        //        {
        //            runs.Add(counter);
        //            indices.Add(i-counter);
        //            counter =1;
        //        }               
        //    }
        //    int maxcounter = int.MinValue;
        //    int index = 0;
        //       foreach(int z in runs)
        //    {
        //        if (z > maxcounter)
        //            maxcounter = z;
        //        index = runs.IndexOf(maxcounter);
        //    }
        //    return indices[index];
        //}

        //public static void Main(string[] args)
        //{
        //    Console.WriteLine(IndexOfLongestRun("abbcccddddcccbba"));
        //    Console.ReadKey();
        //}

        public static int NumberOfWays(int n)
        { 
            int twos = 0;
            int ones = 0;
            while(n>0)
            {
                if (n % 2 == 0)
                {
                    twos++;
                    n = n - 2;
                }
                else
                {
                    n = n - 1;
                    ones++;
                }
            } 

            int counter = twos * 2 + +1 + ones * (twos-1);
            return counter;

        }
        public static int Sum()
        {
            int sum = 0;

            for(int i=0; i<10; i++)
            {
                if(i%5 ==0 || i%3 ==0)
                {
                    sum = sum + i;
                }
            }

            return sum;
        }
        public static int fib(int n)
        {
            if (n == 0)
                return 0;
            if (n == 1)
                return 1;

            int[] Fib = new int[n];
            Fib[0] = 0;
            Fib[1] = 1;

            for (int i = 2; i < n; n++)
            {
                Fib[i] = Fib[i - 1] + Fib[i - 2];
            }

            return Fib[n];


        }
        public static void Main(String[] args)
        {
            //  Console.WriteLine(NumberOfWays(5));

            Console.WriteLine(Sum());



            Console.ReadKey();
        }



    }
}
