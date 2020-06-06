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
        public static object? Convert(string? str, Type type) =>
            type switch
            {
                { IsArray: true } =>
                    ConvertCollection(str, type),

                { IsGenericType: true } when typeof(IDictionary).IsAssignableFrom(type) =>
                    ConvertDictionary(str, type),

                { IsGenericType: true } when typeof(IEnumerable).IsAssignableFrom(type) =>
                    ConvertCollection(str, type),

                _ => ConvertBase(str, type)
            };

        private static object ConvertCollection(string? str, Type type)
        {
            // get element type
            var elementType = type.IsArray
                ? type.GetElementType()
                : type.GetGenericArguments().Single();

            //converting every element
            var list = SplitArray(str).Select(x => ConvertBase(x, elementType));

            //cast to change element type
            var castedList = InvokeEnumerableMethod("Cast", elementType, list);

            //convert IEnumerable to somethting else
            return InvokeEnumerableMethod(type.IsArray ? "ToArray" : "ToList", elementType, castedList);
        }

        private static object InvokeEnumerableMethod(string methodName, Type elementType, object list) =>
            typeof(Enumerable).GetMethod(methodName)
                .MakeGenericMethod(new[] { elementType })
                .Invoke(null, new object[] { list });

        private static object ConvertDictionary(string? str, Type type)
        {
            // get element types
            var elementTypes = type.GetGenericArguments();

            //split pairs
            var keyValueStrings = SplitArray(str);

            //create new dictionary
            var dictionary = Activator.CreateInstance(type);

            foreach (var keyValueString in keyValueStrings)
            {
                var separator = keyValueString.Contains('=') ? '=' : ':';
                var keyValueStringArray = keyValueString.Split(separator);

                // convert both key and value
                var keyValueArray = elementTypes.Select(
                    (type, i) =>
                    {
                        var str = keyValueStringArray.ElementAtOrDefault(i)?.Trim();
                        return ConvertBase(str, type);
                    }
                ).ToArray();

                //add to dictionary
                type.GetMethod("Add", elementTypes)
                    .Invoke(dictionary, keyValueArray);
            }

            return dictionary;
        }

        private static IEnumerable<string> SplitArray(string? str) =>
            str?.Trim()?.Split(
                str.Contains(';') ? ';' : ',',
                StringSplitOptions.RemoveEmptyEntries
            )
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            ?? Array.Empty<string>();

        private static object? ConvertBase(string? str, Type type)
        {
            if (string.IsNullOrWhiteSpace(str) || str == "null" || str == "default")
                return type == typeof(string) || Nullable.GetUnderlyingType(type) != null
                    ? null
                    : Activator.CreateInstance(type);

            str = str?.Trim();

            return type == typeof(string)
                ? str
                : TypeDescriptor.GetConverter(type).ConvertFromString(str);
        }

        public static Action<object, object?> GetPropertySetter(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite)
                return (_, __) => { };

            return (obj, value) => propertyInfo.SetValue(obj, value);
        }
    }
}
