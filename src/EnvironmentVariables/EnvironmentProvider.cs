using System.Linq;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Reflection;

namespace EnvironmentVariables
{
    public class EnvironmentProvider<T> where T : class, new()
    {
        public T Values { get; private set; } = new T();
        private readonly Dictionary<string, PropertyInfo> properties =
            typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(
                    x => x.GetCustomAttribute<EnvAttribute>()?.Name,
                    x => x
                );

        public EnvironmentProvider() => Load();

        public void Load()
        {
            foreach (var property in properties)
            {
                // skip if no EnvAttribute or if not writable
                if (string.IsNullOrEmpty(property.Key) || !property.Value.CanWrite) continue;

                //skip if no value
                var stringValue = Environment.GetEnvironmentVariable(property.Key);
                if (string.IsNullOrEmpty(stringValue)) continue;

                object propValue = property.Value.PropertyType != typeof(string)
                    ? TypeDescriptor.GetConverter(property.Value.PropertyType).ConvertFromString(stringValue)
                    : stringValue;

                property.Value.SetValue(Values, propValue);
            }
        }


    }
}
