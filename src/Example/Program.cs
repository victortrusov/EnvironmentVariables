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

            var config = new EnvironmentProvider<Config>().Values;

            Console.WriteLine(config.AspNetCoreEnvironment);
            Console.WriteLine(config.MyEnvVariableString);
            Console.WriteLine(config.MyEnvVariableInt);
            Console.WriteLine(config.MyEnvVariableBool);
            Console.WriteLine(config.MyEnvVariableDouble);
        }
    }
}
