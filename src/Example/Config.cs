using EnvironmentVariables;

namespace Example
{
    public class Config
    {
        [Env("ASPNETCORE_ENVIRONMENT")]
        public string AspNetCoreEnvironment { get; set; }

        [Env("MY_ENV_VARIABLE_STRING")]
        public string MyEnvVariableString { get; set; }

        [Env("MY_ENV_VARIABLE_INT")]
        public int MyEnvVariableInt { get; set; }

        [Env("MY_ENV_VARIABLE_BOOL")]
        public bool MyEnvVariableBool { get; set; }

        [Env("MY_ENV_VARIABLE_DOUBLE")]
        public double MyEnvVariableDouble { get; set; }
    }
}
