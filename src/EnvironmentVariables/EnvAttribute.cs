using System;

namespace EnvironmentVariables
{
    public class EnvAttribute : Attribute
    {
        public bool IsEnv;
        public string Name;
        public EnvAttribute() => IsEnv = true;
        public EnvAttribute(string name) : this() => Name = name;
    }
}
