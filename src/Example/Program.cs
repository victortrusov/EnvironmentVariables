using System;
using EnvironmentVariables;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var envProvider = new EnvironmentProvider<Config>();
            Console.WriteLine(envProvider.Values.AspNetCoreEnvironment ?? "null");
            Console.WriteLine(envProvider.Values.MyEnvVariable ?? "null");
        }
    }
}
