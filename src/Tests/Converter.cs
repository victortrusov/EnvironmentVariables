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
        [InlineData("null")]
        [InlineData("test")]
        [InlineData("test;test,test")]
        public void Strings(string str) =>
            Assert.Equal(str, Utils.Convert(str, typeof(string)));

        [Theory]
        [InlineData(null, '\x0000')]
        [InlineData("", '\x0000')]
        [InlineData("null", '\x0000')]
        [InlineData("t", 't')]
        [InlineData("\x0001", '\x0001')]
        public void Chars(string str, char ch) =>
            Assert.Equal(ch, Utils.Convert(str, typeof(char)));

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("null")]
        public void NullableChars(string str) =>
           Assert.Null(Utils.Convert(str, typeof(char?)));

        [Theory]
        [InlineData(typeof(byte), null, (byte)0)]
        [InlineData(typeof(byte), "", (byte)0)]
        [InlineData(typeof(byte), "1", (byte)1)]
        [InlineData(typeof(int), null, 0)]
        [InlineData(typeof(int), "", 0)]
        [InlineData(typeof(int), "null", 0)]
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

        [Theory]
        [InlineData(typeof(byte?), null)]
        [InlineData(typeof(byte?), "")]
        [InlineData(typeof(int?), null)]
        [InlineData(typeof(int?), "")]
        [InlineData(typeof(int?), "null")]
        [InlineData(typeof(long?), null)]
        [InlineData(typeof(long?), "")]
        [InlineData(typeof(double?), null)]
        [InlineData(typeof(double?), "")]
        [InlineData(typeof(float?), null)]
        [InlineData(typeof(float?), "")]
        public void NullableNumbers(Type type, string str) =>
            Assert.Null(Utils.Convert(str, type));

        [Fact]
        public void Decimal1() => Assert.Equal(0m, Utils.Convert(null, typeof(decimal)));
        [Fact]
        public void Decimal2() => Assert.Equal(0m, Utils.Convert("", typeof(decimal)));
        [Fact]
        public void Decimal3() => Assert.Equal(1m, Utils.Convert("1", typeof(decimal)));
        [Fact]
        public void Decimal4() => Assert.Equal(1.1m, Utils.Convert("1.1", typeof(decimal)));
        [Fact]
        public void Decimal5() => Assert.Null(Utils.Convert(null, typeof(decimal?)));
        [Fact]
        public void Decimal6() => Assert.Null(Utils.Convert("", typeof(decimal?)));
        [Fact]
        public void Decimal7() => Assert.Null(Utils.Convert("null", typeof(decimal?)));

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("FALSE", false)]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("TRUE", true)]
        public void Booleans(string str, bool result) =>
            Assert.Equal(result, Utils.Convert(str, typeof(bool)));

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("null")]
        public void NullableBooleans(string str) =>
            Assert.Null(Utils.Convert(str, typeof(bool?)));

        [Theory]
        [InlineData(typeof(int[]), null, new int[0])]
        [InlineData(typeof(int[]), "", new int[0])]
        [InlineData(typeof(int[]), "null", new int[] { 0 })]
        [InlineData(typeof(int[]), ",,  ,,,,,,  ,,", new int[0])]
        [InlineData(typeof(string[]), "  ;;;;  ;;;  ;;;", new string[0])]
        [InlineData(typeof(string[]), "null, null", new string[] { "null", "null" })]
        [InlineData(typeof(int[]), "1", new int[] { 1 })]
        [InlineData(typeof(int[]), "1,2,3", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), ",1,2,3,", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), "  ,  1,2, 3   ", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), "1;2;3", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), ";1;2;3;", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), "  ;  1;2; 3   ", new int[] { 1, 2, 3 })]
        [InlineData(typeof(string[]), "o,two,3", new string[] { "o", "two", "3" })]
        [InlineData(typeof(string[]), ",o,two,3,", new string[] { "o", "two", "3" })]
        [InlineData(typeof(string[]), "  ,  o,two, 3   ", new string[] { "o", "two", "3" })]
        [InlineData(typeof(bool[]), "true,false,true", new bool[] { true, false, true })]
        public void Arrays(Type type, string str, object result) =>
            Assert.Equal(result, Utils.Convert(str, type));

        [Fact]
        public void NullableIntArray() =>
            Assert.Equal(new int?[] { 1, 2, 3, null }, Utils.Convert("  ,  1,2, 3, null   ", typeof(int?[])));
    }
}
