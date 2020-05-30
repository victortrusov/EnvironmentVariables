using System;

namespace EnvironmentVariables
{
    public class EnvAttribute : Attribute
    {
        public string Name;
        public EnvAttribute(string name) => Name = name;
    }
}
