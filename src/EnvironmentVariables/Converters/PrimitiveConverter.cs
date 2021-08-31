using System;
using System.ComponentModel;

namespace EnvironmentVariables.Converters
{
    internal class PrimitiveConverter : IConverter
    {
        public object? Convert(string? str, Type type)
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
    }
}
