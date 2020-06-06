using System;
using Xunit;
using EnvironmentVariables;
using System.Collections;
using System.Collections.Generic;

namespace Tests
{
    public class Converter
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("null")]
        public void StringsNull(string str) =>
            Assert.Null(Utils.Convert(str, typeof(string)));

        [Theory]
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
        [InlineData("Two")]
        [InlineData("two")]
        [InlineData("TWO")]
        [InlineData("2")]
        public void Enum(string str) => Assert.Equal(
                TestEnum.Two,
                Utils.Convert(str, typeof(TestEnum))
            );


        [Theory]
        [InlineData(typeof(int[]), null, new int[0])]
        [InlineData(typeof(int[]), "", new int[0])]
        [InlineData(typeof(int[]), "null", new int[] { 0 })]
        [InlineData(typeof(int[]), ",,  ,,,,,,  ,,", new int[0])]
        [InlineData(typeof(string[]), "  ;;;;  ;;;  ;;;", new string[0])]
        [InlineData(typeof(string[]), "null, null", new string[] { null, null })]
        [InlineData(typeof(int[]), "1", new int[] { 1 })]
        [InlineData(typeof(int[]), "1,2,3", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), ",1,2,3,", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), "  ,  1,2, 3   ", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), "1;2;3", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), ";1;2;3;", new int[] { 1, 2, 3 })]
        [InlineData(typeof(int[]), "  ;  1;2; 3   ", new int[] { 1, 2, 3 })]
        [InlineData(typeof(double[]), "1,2.2,3.3", new double[] { 1, 2.2, 3.3 })]
        [InlineData(typeof(string[]), "o,two,3", new string[] { "o", "two", "3" })]
        [InlineData(typeof(string[]), ",o,two,3,", new string[] { "o", "two", "3" })]
        [InlineData(typeof(string[]), "  ,  o,two, 3   ", new string[] { "o", "two", "3" })]
        [InlineData(typeof(bool[]), "true,false,true", new bool[] { true, false, true })]
        [InlineData(typeof(TestEnum[]), "Two,one,ZERO", new TestEnum[] { TestEnum.Two, TestEnum.One, TestEnum.Zero })]
        [InlineData(typeof(TestEnum[]), " , Two,one  ,ZERO,,", new TestEnum[] { TestEnum.Two, TestEnum.One, TestEnum.Zero })]
        [InlineData(typeof(TestEnum[]), " 2 ; 1 ;;;0", new TestEnum[] { TestEnum.Two, TestEnum.One, TestEnum.Zero })]
        public void Arrays(Type type, string str, object result) =>
            Assert.Equal(result, Utils.Convert(str, type));

        [Fact]
        public void NullableIntArray() =>
            Assert.Equal(new int?[] { 1, 2, 3, null }, Utils.Convert("  ,  1,2, 3, null   ", typeof(int?[])));

        [Theory]
        [InlineData("o,two,3")]
        [InlineData(",o,two,3,")]
        [InlineData("  ,  o,two, 3   ")]
        public void ListsString(string str) =>
            Assert.Equal(new List<string> { "o", "two", "3" }, Utils.Convert(str, typeof(List<string>)));

        [Theory]
        [InlineData("1,2.2222222,3.333")]
        [InlineData(",1,2.2222222,3.333,")]
        [InlineData("  ,  1,    2.2222222,3.333   ")]
        public void IEnumerableDecimal(string str) =>
            Assert.Equal(new List<decimal> { 1, 2.2222222m, 3.333m }, Utils.Convert(str, typeof(IEnumerable<decimal>)));

        [Theory]
        [InlineData("true,false,true")]
        [InlineData(",  true,false ,true,     ")]
        [InlineData("true;false;true;;;;")]
        public void ICollectionBool(string str) =>
            Assert.Equal(new List<bool> { true, false, true }, Utils.Convert(str, typeof(ICollection<bool>)));

        [Theory]
        [InlineData("1=one;2=two;0=")]
        [InlineData("1=one,2=two,Zero=")]
        [InlineData("one:one;two:two;zero:")]
        [InlineData("1:one,2:two,0:")]
        [InlineData("OnE = one; 2 = two; ZERO = ;")]
        [InlineData("  1=one  ,  2=two   ,   0=   ")]
        [InlineData("one: one; two: two; 0: ; ; ; ;  ;;")]
        [InlineData("1   :one::,2    :two,zero    ::")]
        public void DictionaryEnumString(string str) =>
            Assert.Equal(
                new Dictionary<TestEnum, string> { { TestEnum.One, "one" }, { TestEnum.Two, "two" }, { TestEnum.Zero, null } },
                Utils.Convert(str, typeof(Dictionary<TestEnum, string>))
            );

        [Theory]
        [InlineData("1=true;2.2=false;3.33=null")]
        [InlineData("1=true,2.2=false,3.33=null")]
        [InlineData("1:true;2.2:false;3.33:null")]
        [InlineData("1:true,2.2:false,3.33:null")]
        [InlineData("1 = true; 2.2 = false; 3.33 =;")]
        [InlineData("  1=true  ,  2.2=false   ,   3.33=null   ")]
        [InlineData("1: true; 2.2: false; 3.33: ; ; ; ;  ;;")]
        [InlineData("1   :true::,2.2    :false,3.33    :null:")]
        public void DictionaryDoubleBool(string str) =>
            Assert.Equal(
                new Dictionary<double, bool?> { { 1, true }, { 2.2, false }, { 3.33, null } },
                Utils.Convert(str, typeof(Dictionary<double, bool?>))
            );
    }

    public enum TestEnum
    {
        Zero,
        One,
        Two
    }
}
