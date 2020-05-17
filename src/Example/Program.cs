using System;
using System.Diagnostics;
using EnvironmentVariables;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var provider = new EnvironmentProvider<Config>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var i = 0; i < 10000; i++)
                provider = new EnvironmentProvider<Config>();
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            stopWatch.Restart();
            for (var i = 0; i < 10000; i++)
                provider.Load();
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            Console.WriteLine(provider.Values.AspNetCoreEnvironment);
            Console.WriteLine(provider.Values.MyEnvString);
            Console.WriteLine(provider.Values.MyEnvInt);
            Console.WriteLine(provider.Values.MyEnvBool);
            Console.WriteLine(provider.Values.MyEnvDouble);
        }
    }
}
