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

            Console.WriteLine(provider.Values.AspNetCoreEnvironment);
            Console.WriteLine(provider.Values.MyEnvString);
            Console.WriteLine(provider.Values.MyEnvInt);
            Console.WriteLine(provider.Values.MyEnvBool);
            Console.WriteLine(provider.Values.MyEnvDouble);
        }
    }
}
