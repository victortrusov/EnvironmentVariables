using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using EnvironmentVariables;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("To run this example successfully set these environment variables:");
            foreach (var prop in typeof(EnvConfig).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var evnName = prop.GetCustomAttribute<EnvAttribute>()?.Name;
                Console.WriteLine(evnName ?? prop.Name);
            }

            var provider = new EnvironmentProvider<EnvConfig>();

            var values = provider.Values;
            Console.WriteLine("\n----------------------\n");
            Console.WriteLine("Current values:\n");
            Console.WriteLine(values.AspNetCoreEnvironment);
            Console.WriteLine(values.MyEnvString);
            Console.WriteLine(values.MyEnvInt);
            Console.WriteLine(values.MyEnvBool);
            Console.WriteLine(values.MyEnvDouble);

            if (values.MyEnvStringArray is not null)
                Console.WriteLine(string.Join(", ", values.MyEnvStringArray));

            if (values.MyEnvStringList is not null)
                Console.WriteLine(string.Join(", ", values.MyEnvStringList));

            if (values.MyEnvStringEnumerable is not null)
                Console.WriteLine(string.Join(", ", values.MyEnvStringEnumerable));

            if (values.MyEnvIntArray is not null)
                Console.WriteLine(string.Join(", ", values.MyEnvIntArray));

            if (values.MyEnvIntList is not null)
                Console.WriteLine(string.Join(", ", values.MyEnvIntList));

            if (values.MyEnvIntEnumerable is not null)
                Console.WriteLine(string.Join(", ", values.MyEnvIntEnumerable));

            if (values.MyEnvIntDictionary is not null)
                Console.WriteLine(string.Join(", ", values.MyEnvIntDictionary));

            if (values.MyEnvIntStrDictionary is not null)
                Console.WriteLine(string.Join(", ", values.MyEnvIntStrDictionary));

            Console.WriteLine(values.MyNullList is null ? "I should be null and i'm null" : "Something goes wrong");
        }
    }
}
