using System.Linq;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;

namespace EnvironmentVariables
{
    public partial class EnvironmentProvider<T> where T : class, new()
    {
        private readonly Type type = typeof(T);
        private readonly List<MemberMap> members = new List<MemberMap>();

        public T Values { get; private set; } = new T();

        public EnvironmentProvider()
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                var env = prop.GetCustomAttribute<EnvAttribute>()?.Name;
                if (string.IsNullOrEmpty(env))
                    continue;

                members.Add(new MemberMap
                {
                    EnvName = env,
                    Type = prop.PropertyType,
                    Setter = Utils.GetPropertySetter(prop)
                });
            }

            Load();
        }

        public void Load()
        {
            foreach (var member in members)
            {
                //skip if no value
                var stringValue = Environment.GetEnvironmentVariable(member.EnvName);
                if (string.IsNullOrEmpty(stringValue)) continue;

                object propValue = member.Type != typeof(string)
                    ? TypeDescriptor.GetConverter(member.Type).ConvertFromString(stringValue)
                    : stringValue;

                member.Setter(Values, propValue);
            }
        }

    }
}
