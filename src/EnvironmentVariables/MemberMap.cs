using System;
using System.Reflection;

namespace EnvironmentVariables
{
    internal class MemberMap
    {
        public MemberMap(PropertyInfo prop)
        {
            var env = prop.GetCustomAttribute<EnvAttribute>()?.Name;

            PropertyName = prop.Name;
            Type = prop.PropertyType;
            EnvName = env ?? PropertyName;
            Setter = Utils.GetPropertySetter(prop);
        }
        public string EnvName;
        public string PropertyName;
        public Type Type;
        public Action<object, object> Setter;
    }
}

