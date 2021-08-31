using System;
using System.Linq;

namespace EnvironmentVariables.Converters
{
    internal class CollectionConverter : IConverter
    {
        private readonly IConverter primitiveConverter = new PrimitiveConverter();

        public object? Convert(string? str, Type type)
        {
            // get element type
            var elementType = type.IsArray
                ? type.GetElementType()
                : type.GetGenericArguments().Single();

            if (elementType is null)
                throw new Exception("Can't get the collection element type");

            //converting every element
            var list = Utils.SplitArray(str).Select(x => primitiveConverter.Convert(x, elementType));

            //cast to change element type
            var castedList = InvokeEnumerableMethod("Cast", elementType, list);

            //convert IEnumerable to somethting else
            return InvokeEnumerableMethod(type.IsArray ? "ToArray" : "ToList", elementType, castedList);
        }

        private static object? InvokeEnumerableMethod(string methodName, Type elementType, object? list)
        {
            if (list is null)
                return null;

            return typeof(Enumerable).GetMethod(methodName)
                ?.MakeGenericMethod(new[] { elementType })
                .Invoke(null, new object[] { list });
        }
    }
}
