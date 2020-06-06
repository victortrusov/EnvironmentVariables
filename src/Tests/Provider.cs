using System;
using System.Collections.Generic;
using EnvironmentVariables;
using Xunit;

namespace Tests
{
    public class EnvConfig
    {
        [Env("STRING")]
        public string String { get; set; }

        [Env("DECIMAL")]
        public decimal Decimal { get; set; } = 2.135m;

        [Env("ARRAY")]
        public int[] Array { get; set; }

        [Env("LIST")]
        public List<bool> List { get; set; }

        [Env("DICTIONARY")]
        public Dictionary<string, double> Dictionary { get; set; }

        public string NoName { get; set; }
    }

    public class Provider
    {
        [Fact]
        public void String()
        {
            Environment.SetEnvironmentVariable("STRING", "test");
            var provider = new EnvironmentProvider<EnvConfig>();
            Assert.Equal("test", provider.Values.String);
        }

        [Fact]
        public void Decimal()
        {
            Environment.SetEnvironmentVariable("DECIMAL", "1.111");
            var provider = new EnvironmentProvider<EnvConfig>();
            Assert.Equal(1.111m, provider.Values.Decimal);
        }

        [Fact]
        public void Array()
        {
            Environment.SetEnvironmentVariable("ARRAY", "1,2,3");
            var provider = new EnvironmentProvider<EnvConfig>();
            Assert.Equal(new[] { 1, 2, 3 }, provider.Values.Array);
        }

        [Fact]
        public void List()
        {
            Environment.SetEnvironmentVariable("LIST", "true, FALSE, True");
            var provider = new EnvironmentProvider<EnvConfig>();
            Assert.Equal(new List<bool> { true, false, true }, provider.Values.List);
        }

        [Fact]
        public void Dictionary()
        {
            Environment.SetEnvironmentVariable("DICTIONARY", "test=1;test2=2.22");
            var provider = new EnvironmentProvider<EnvConfig>();
            Assert.Equal(new Dictionary<string, double>() { { "test", 1 }, { "test2", 2.22 } }, provider.Values.Dictionary);
        }

        [Fact]
        public void NoName()
        {
            var provider = new EnvironmentProvider<EnvConfig>();
            Assert.Null(provider.Values.NoName);
        }
    }
}
