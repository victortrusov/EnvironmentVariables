using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

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
                    ConvertAndCastToArray(str, type),

                { IsGenericType: true } when typeof(IEnumerable).IsAssignableFrom(type) =>
                    ConvertAndCastToList(str, type),

                _ => ConvertBase(str, type)
            };

        public static object ConvertAndCastToArray(string str, Type type)
        {
            var elementType = type.GetElementType();
            var list = ConvertToEnumerable(str, type, elementType);
            var toArrayMethod = typeof(Enumerable)
                .GetMethod("ToArray").MakeGenericMethod(
                new[] {
                    elementType
                });
            return toArrayMethod.Invoke(null, new object[] { list });
        }

        public static object ConvertAndCastToList(string str, Type type)
        {
            var elementType = type.GetGenericArguments().Single();
            var list = ConvertToEnumerable(str, type, elementType);
            var toListMethod = typeof(Enumerable)
                .GetMethod("ToList").MakeGenericMethod(
                new[] {
                    elementType
                });
            return toListMethod.Invoke(null, new object[] { list });
        }

        public static object ConvertToEnumerable(string str, Type type, Type elementType)
        {
            var list = elementType == typeof(string)
                ? SplitArray(str)
                : SplitArray(str).Select(x => ConvertBase(x, elementType));

            var castMethod = typeof(Enumerable)
                .GetMethod("Cast")
                .MakeGenericMethod(new[] { elementType });

            return castMethod.Invoke(null, new object[] { list });
        }


        public static string[] SplitArray(string str) => str.Split(new[] { ',', ';' });

        public static object ConvertBase(string str, Type type) => TypeDescriptor.GetConverter(type).ConvertFromString(str);


        public static Action<object, object> GetPropertySetter(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite)
            {
                var message = string.Format(
                    "The property '{0} {1}' of class '{2}' has no 'set' accessor.",
                    propertyInfo.PropertyType.FullName, propertyInfo.Name, propertyInfo.DeclaringType.FullName);
                throw new Exception(message);
            }

            return (obj, value) => propertyInfo.SetValue(obj, value);
        }

        // expression method is inefficient here

        // private Action<object, object> GetPropertySetter(PropertyInfo propertyInfo, Type classType)
        // {
        //     var setMethodInfo = propertyInfo.SetMethod;
        //     if (!propertyInfo.CanWrite)
        //     {
        //         var message = string.Format(
        //             "The property '{0} {1}' of class '{2}' has no 'set' accessor.",
        //             propertyInfo.PropertyType.FullName, propertyInfo.Name, propertyInfo.DeclaringType.FullName);
        //         throw new Exception(message);
        //     }

        //     // lambdaExpression = (obj, value) => ((TClass) obj).SetMethod((TMember) value)
        //     var objParameter = Expression.Parameter(typeof(object), "obj");
        //     var valueParameter = Expression.Parameter(typeof(object), "value");
        //     var lambdaExpression = Expression.Lambda<Action<object, object>>(
        //         Expression.Call(
        //             Expression.Convert(objParameter, classType),
        //             setMethodInfo,
        //             Expression.Convert(valueParameter, propertyInfo.PropertyType)
        //         ),
        //         objParameter,
        //         valueParameter
        //     );

        //     return lambdaExpression.Compile();
        // }
    }
}
