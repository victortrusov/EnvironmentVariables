using EnvironmentVariables;

namespace Example
{
    public class Config
    {
        [Env("ASPNETCORE_ENVIRONMENT")]
        public string AspNetCoreEnvironment { get; set; }

        [Env("MY_ENV_STRING")]
        public string MyEnvString { get; set; }

        [Env("MY_ENV_INT")]
        public int MyEnvInt { get; set; }

        [Env("MY_ENV_BOOL")]
        public bool MyEnvBool { get; set; }

        [Env("MY_ENV_DOUBLE")]
        public double MyEnvDouble { get; set; }
    }
}
