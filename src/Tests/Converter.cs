using System;
using Xunit;
using EnvironmentVariables;

namespace Tests
{
    public class Converter
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("test")]
        [InlineData("test;test,test")]
        public void Strings(string str) =>
            Assert.Equal(str, Utils.Convert(str, typeof(string)));

        [Theory]
        [InlineData(typeof(byte), null, (byte)0)]
        [InlineData(typeof(byte), "", (byte)0)]
        [InlineData(typeof(byte), "1", (byte)1)]
        [InlineData(typeof(int), null, 0)]
        [InlineData(typeof(int), "", 0)]
        [InlineData(typeof(int), "1", 1)]
        [InlineData(typeof(long), null, (long)0)]
        [InlineData(typeof(long), "", (long)0)]
        [InlineData(typeof(long), "1", (long)1)]
        [InlineData(typeof(double), null, 0.0d)]
        [InlineData(typeof(double), "", 0.0d)]
        [InlineData(typeof(double), "1", 1.0d)]
        [InlineData(typeof(double), "1.1", 1.1d)]
        [InlineData(typeof(float), null, 0.0f)]
        [InlineData(typeof(float), "", 0.0f)]
        [InlineData(typeof(float), "1", 1.0f)]
        [InlineData(typeof(float), "1.1", 1.1f)]
        public void Numbers(Type type, string str, object result) =>
            Assert.Equal(result, Utils.Convert(str, type));

        [Fact]
        public void Decimal1() => Assert.Equal(0m, Utils.Convert(null, typeof(decimal)));
        [Fact]
        public void Decimal2() => Assert.Equal(0m, Utils.Convert("", typeof(decimal)));
        [Fact]
        public void Decimal3() => Assert.Equal(1m, Utils.Convert("1", typeof(decimal)));
        [Fact]
        public void Decimal4() => Assert.Equal(1.1m, Utils.Convert("1.1", typeof(decimal)));

    }
}
