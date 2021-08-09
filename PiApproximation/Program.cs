using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PiApproximation
{
    class Program
    {
        static void Main()
        {
            int numberOfCores = Environment.ProcessorCount;
            int iterations = 100000000 * numberOfCores;

            double piApprox, x, y = 0;
            int inCircle = 0;

            int[] localCounters = new int[numberOfCores];
            Task[] tasks = new Task[numberOfCores];

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine($"Number of cores on system: {numberOfCores}");
            Console.WriteLine($"Iteration limit: {iterations}");

            for (int i = 0; i < numberOfCores; i++)
            {
                int procIndex = i;
                tasks[procIndex] = Task.Factory.StartNew(() =>
                {
                    int counter = 0;
                    Random rnd = new Random();

                    for (int j = 0; j < iterations / numberOfCores; j++)
                    {
                        x = rnd.NextDouble();
                        y = rnd.NextDouble();

                        if (Math.Sqrt(x * x + y * y) <= 1.0)
                            counter++;
                    }
                    localCounters[procIndex] = counter;
                });
            }

            Task.WaitAll(tasks);
            inCircle = localCounters.Sum();

            piApprox = 4 * ((double)inCircle / (double)iterations);

            Console.WriteLine($"Approximated pi = {piApprox.ToString("F8")}");

            sw.Stop();
            Console.WriteLine($"Time taken (ms): {sw.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}
