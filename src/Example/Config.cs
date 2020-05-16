using EnvironmentVariables;

namespace Example
{
    public class Config
    {
        [Env("ASPNETCORE_ENVIRONMENT")]
        public string AspNetCoreEnvironment { get; set; }

        [Env("MY_ENV_VARIABLE")]
        public string MyEnvVariable { get; set; }
    }
}
