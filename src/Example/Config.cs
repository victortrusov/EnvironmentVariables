using System.Collections.Generic;
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

        [Env("MY_ENV_STRING_ARRAY")]
        public string[] MyEnvStringArray { get; set; }

        [Env("MY_ENV_STRING_ARRAY")]
        public List<string> MyEnvStringList { get; set; }

        [Env("MY_ENV_STRING_ARRAY")]
        public IEnumerable<string> MyEnvStringEnumerable { get; set; }

        [Env("MY_ENV_INT_ARRAY")]
        public int[] MyEnvIntArray { get; set; }

        [Env("MY_ENV_INT_ARRAY")]
        public List<int> MyEnvIntList { get; set; }

        [Env("MY_ENV_INT_ARRAY")]
        public IEnumerable<int> MyEnvIntEnumerable { get; set; }
    }
}
