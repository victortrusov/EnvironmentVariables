using System;

namespace EnvironmentVariables.Converters
{
    internal interface IConverter
    {
        object? Convert(string? str, Type type);
    }
}
