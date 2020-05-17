using System;
using System.Reflection;

namespace EnvironmentVariables
{
    internal static class Utils
    {
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
