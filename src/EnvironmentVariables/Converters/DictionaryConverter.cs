using System;
using System.Linq;

namespace EnvironmentVariables.Converters
{
    internal class DictionaryConverter : IConverter
    {
        private readonly IConverter primitiveConverter = new PrimitiveConverter();

        public object? Convert(string? str, Type type)
        {
            var elementTypes = type.GetGenericArguments();

            //split pairs
            var keyValueStrings = Utils.SplitArray(str);

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
                        return primitiveConverter.Convert(str, type);
                    }
                ).ToArray();

                //add to dictionary
                type.GetMethod("Add", elementTypes)?
                    .Invoke(dictionary, keyValueArray);
            }

            return dictionary;
        }
    }
}
