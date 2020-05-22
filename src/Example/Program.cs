using System.Linq;
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
            Console.WriteLine(string.Join(", ", provider.Values.MyEnvStringArray));
            Console.WriteLine(string.Join(", ", provider.Values.MyEnvStringList));
            Console.WriteLine(string.Join(", ", provider.Values.MyEnvStringEnumerable));
            Console.WriteLine(string.Join(", ", provider.Values.MyEnvIntArray));
            Console.WriteLine(string.Join(", ", provider.Values.MyEnvIntList));
            Console.WriteLine(string.Join(", ", provider.Values.MyEnvIntEnumerable));
        }
    }
}
