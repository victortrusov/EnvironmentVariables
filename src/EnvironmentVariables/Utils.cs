using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
namespace EnvironmentVariables
{
    internal static class Utils
    {
        public static object Convert(string str, Type type) =>
            type switch
            {
                _ when type == typeof(string) =>
                    str,

                { IsArray: true } =>
                    ConvertCollection(str, type),

                { IsGenericType: true } when typeof(IDictionary).IsAssignableFrom(type) =>
                    ConvertDictionary(str, type),

                { IsGenericType: true } when typeof(IEnumerable).IsAssignableFrom(type) =>
                    ConvertCollection(str, type),

                _ => ConvertBase(str, type)
            };

        private static object ConvertCollection(string str, Type type)
        {
            // get element type
            var elementType = type.IsArray
                ? type.GetElementType()
                : type.GetGenericArguments().Single();

            //converting every element if not string
            var list = elementType == typeof(string)
                ? SplitArray(str)
                : SplitArray(str).Select(x => ConvertBase(x, elementType));

            //cast to change element type
            var castedList = InvokeEnumerableMethod("Cast", elementType, list);

            //convert IEnumerable to somethting else
            return InvokeEnumerableMethod(type.IsArray ? "ToArray" : "ToList", elementType, castedList);
        }

        private static object InvokeEnumerableMethod(string methodName, Type elementType, object list) =>
            typeof(Enumerable).GetMethod(methodName)
                .MakeGenericMethod(new[] { elementType })
                .Invoke(null, new object[] { list });

        private static object ConvertDictionary(string str, Type type)
        {
            // get element types
            var elementTypes = type.GetGenericArguments();

            //split pairs
            var keyValueStrings = SplitArray(str);

            //create new dictionary
            var dictionary = Activator.CreateInstance(type);

            foreach (var keyValueString in keyValueStrings)
            {
                var keyValueStringArray = keyValueString.Split('=');

                // convert both key and value
                var keyValueArray = elementTypes.Select(
                    (type, i) =>
                    {
                        var str = keyValueStringArray.ElementAtOrDefault(i);
                        return type == typeof(string)
                            ? str
                            : ConvertBase(str, type);
                    }
                ).ToArray();

                //add to dictionary
                type.GetMethod("Add", elementTypes)
                    .Invoke(dictionary, keyValueArray);
            }

            return dictionary;
        }

        private static IEnumerable<string> SplitArray(string str) =>
            str.Trim().Split(
                str.Contains(';') ? ';' : ',',
                StringSplitOptions.RemoveEmptyEntries
            )
            .Select(x => x.Trim());

        private static object ConvertBase(string str, Type type) =>
            string.IsNullOrWhiteSpace(str)
                ? Activator.CreateInstance(type)
                : TypeDescriptor.GetConverter(type).ConvertFromString(str);


        public static Action<object, object> GetPropertySetter(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite)
                return (_, __) => { };

            return (obj, value) => propertyInfo.SetValue(obj, value);
        }
    }
}
