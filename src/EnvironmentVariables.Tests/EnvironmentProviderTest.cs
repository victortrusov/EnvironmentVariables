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

        [Env("ENUM")]
        public TestEnum Enum { get; set; }

        [Env("NULLABLEENUM")]
        public TestEnum? NullableEnum { get; set; }

        [Env("ARRAY")]
        public int[] Array { get; set; }

        [Env("LIST")]
        public List<bool> List { get; set; }

        [Env("DICTIONARY")]
        public Dictionary<string, double> Dictionary { get; set; }

        [Env("STRING")]
        public string NoSetter { get; }

        public string NoName { get; set; }
    }

    public class EnvironmentProviderTest : IDisposable
    {
        EnvironmentProvider<EnvConfig> provider;
        public EnvironmentProviderTest()
        {
            Environment.SetEnvironmentVariable("STRING", "test");
            Environment.SetEnvironmentVariable("DECIMAL", "1.111");
            Environment.SetEnvironmentVariable("ENUM", "Two");
            Environment.SetEnvironmentVariable("ARRAY", "1,2,3");
            Environment.SetEnvironmentVariable("LIST", "true, FALSE, True");
            Environment.SetEnvironmentVariable("DICTIONARY", "test=1;test2=2.22");
            provider = new EnvironmentProvider<EnvConfig>();
        }

        public void Dispose()
        {
            provider.Dispose();
        }

        [Fact]
        public void String() =>
            Assert.Equal("test", provider.Values.String);

        [Fact]
        public void Decimal() =>
            Assert.Equal(1.111m, provider.Values.Decimal);

        [Fact]
        public void Enum() =>
            Assert.Equal(TestEnum.Two, provider.Values.Enum);

        [Fact]
        public void NullableEnum() =>
            Assert.Null(provider.Values.NullableEnum);

        [Fact]
        public void Array() =>
            Assert.Equal(new[] { 1, 2, 3 }, provider.Values.Array);

        [Fact]
        public void List() =>
            Assert.Equal(new List<bool> { true, false, true }, provider.Values.List);

        [Fact]
        public void Dictionary() =>
            Assert.Equal(new Dictionary<string, double>() { { "test", 1 }, { "test2", 2.22 } }, provider.Values.Dictionary);

        [Fact]
        public void NoSetter() =>
            Assert.Null(provider.Values.NoSetter);

        [Fact]
        public void NoName() =>
            Assert.Null(provider.Values.NoName);
    }
}
