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

            var values = provider.Values;

            Console.WriteLine(values.AspNetCoreEnvironment);
            Console.WriteLine(values.MyEnvString);
            Console.WriteLine(values.MyEnvInt);
            Console.WriteLine(values.MyEnvBool);
            Console.WriteLine(values.MyEnvDouble);
            Console.WriteLine(string.Join(", ", values.MyEnvStringArray));
            Console.WriteLine(string.Join(", ", values.MyEnvStringList));
            Console.WriteLine(string.Join(", ", values.MyEnvStringEnumerable));
            Console.WriteLine(string.Join(", ", values.MyEnvIntArray));
            Console.WriteLine(string.Join(", ", values.MyEnvIntList));
            Console.WriteLine(string.Join(", ", values.MyEnvIntEnumerable));
            Console.WriteLine(string.Join(", ", values.MyEnvIntDictionary));
            Console.WriteLine(string.Join(", ", values.MyEnvIntStrDictionary));
        }
    }
}
