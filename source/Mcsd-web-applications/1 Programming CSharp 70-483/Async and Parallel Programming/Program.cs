using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Async_and_Parallel_Programming
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sequental example
            var value = 0;
            value = value + new ComputationThing(2).Compute();
            value = value + new ComputationThing(1).Compute();
            value = value + new ComputationThing(7).Compute();
            Console.WriteLine("Sequental example: " + value);

            //Bad task example that uses shared variable reference
            var tasks = new List<Task>();
            var value2 = 0;
            tasks.Add(Task.Factory.StartNew(() => {
                value2 = value2 + new ComputationThing(2).Compute();
            }));
            tasks.Add(Task.Factory.StartNew(() => {
                value2 = value2 + new ComputationThing(1).Compute();
            }));
            tasks.Add(Task.Factory.StartNew(() => {
                value2 = value2 + new ComputationThing(7).Compute();
            }));
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("Bad task example: " + value2);

            //Task example with critical section Lock example
            var tasks2 = new List<Task>();
            var value3 = 0;
            var l = new object();
            tasks2.Add(Task.Factory.StartNew(() => {
                var val = new ComputationThing(2).Compute();
                lock(l) //Lock the critical section
                {
                    value3 = value3 + val;
                }                
            }));
            tasks2.Add(Task.Factory.StartNew(() => {
                var val = new ComputationThing(1).Compute();
                lock (l) //Lock the critical section
                {
                    value3 = value3 + val;
                }
            }));
            tasks2.Add(Task.Factory.StartNew(() => {
                var val = new ComputationThing(7).Compute();
                lock (l) //Lock the critical section
                {
                    value3 = value3 + val;
                }
            }));
            Task.WaitAll(tasks2.ToArray());

            Console.WriteLine("Task example with critical section Lock example: " + value3);


            Console.ReadLine();
        }
    }

    public class ComputationThing
    {
        int waitTime;
        int _value;


        public ComputationThing(int value)
        {
            _value = value;

            Random rnd = new Random();
            waitTime = rnd.Next(900, 1000);
        }

        public int Compute()
        {
            Thread.Sleep(waitTime);
            return _value;
        }
    }
}
