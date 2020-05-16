using System;
using System.Reflection;

namespace EnvironmentVariables
{
    public class EnvironmentProvider<T> where T : new()
    {
        public T Values { get; private set; }
        public EnvironmentProvider()
        {
            Values = new T();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var name = property.GetCustomAttribute<EnvAttribute>()?.Name;

                var value = name != null
                    ? Environment.GetEnvironmentVariable(name)
                    : null;

                property.SetValue(Values, value);
            }
        }
    }
}
