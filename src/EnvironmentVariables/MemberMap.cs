using System;

namespace EnvironmentVariables
{
    internal class MemberMap
    {
        public string EnvName;
        public Type Type;
        public Action<object, object> Setter;
    }
}

