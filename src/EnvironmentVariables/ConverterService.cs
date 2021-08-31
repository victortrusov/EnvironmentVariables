using System;
using System.Collections;
using EnvironmentVariables.Converters;

namespace EnvironmentVariables
{
    internal class ConverterService
    {
        private readonly IConverter primitiveConverter = new PrimitiveConverter();
        private readonly IConverter dictionaryConverter = new DictionaryConverter();
        private readonly IConverter collectionConverter = new CollectionConverter();

        public object? Convert(string? str, Type type)
        {
            IConverter converter = type switch
            {
                { IsArray: true } =>
                    collectionConverter,

                { IsGenericType: true } when typeof(IDictionary).IsAssignableFrom(type) =>
                    dictionaryConverter,

                { IsGenericType: true } when typeof(IEnumerable).IsAssignableFrom(type) =>
                    collectionConverter,

                _ => primitiveConverter
            };

            return converter.Convert(str, type);
        }
    }
}
