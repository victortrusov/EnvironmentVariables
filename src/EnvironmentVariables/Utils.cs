using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnvironmentVariables
{
    internal static class Utils
    {
        public static IEnumerable<string> SplitArray(string? str) =>
            str?.Trim()?.Split(
                str.Contains(';') ? ';' : ',',
                StringSplitOptions.RemoveEmptyEntries
            )
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            ?? Array.Empty<string>();


        public static Action<object, object?> GetPropertySetter(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite)
                return (_, __) => { };

            return (obj, value) => propertyInfo.SetValue(obj, value);
        }
    }
}
