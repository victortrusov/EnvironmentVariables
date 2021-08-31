using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EnvironmentVariables.Tests")]
namespace EnvironmentVariables
{
    /// <summary>
    /// Provider that allows you to access environment variables through object of specified class
    /// </summary>
    /// <typeparam name="T">Class that contains environment variables as props</typeparam>
    public class EnvironmentProvider<T> : IDisposable where T : class, new()
    {
        private readonly Type type = typeof(T);
        private readonly List<MemberMap> members = new List<MemberMap>();
        private readonly ConverterService converterService = new ConverterService();

        /// <summary>
        /// Values of all defined environment variables
        /// </summary>
        public T Values { get; private set; }

        /// <summary>
        /// Function that uses to access value of environment variable. 
        /// <see cref="Environment.GetEnvironmentVariable(string)" /> by default.
        /// </summary>
        public Func<string, string?> EnvProvider { get; set; } = Environment.GetEnvironmentVariable;

        public Action<string> SelfLog { get; set; } = (string _) => { };

        /// <summary>
        /// Default constructor. Create object of provided type and set values to props.
        /// </summary>
        public EnvironmentProvider() : this(null) { }

        /// <summary>
        /// You can provide initial values with this constructor
        /// </summary>
        /// <param name="initialValues">Object that will be used as initial values</param>
        public EnvironmentProvider(T? initialValues)
        {
            Values = initialValues ?? new T();
            var properties = GetProperties();

            foreach (var prop in properties)
                members.Add(new MemberMap(prop));

            Reload();
        }

        private PropertyInfo[] GetProperties() =>
            type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// Reload all values
        /// </summary>
        public void Reload()
        {
            foreach (var member in members)
                try
                {
                    var stringValue = EnvProvider(member.EnvName);

                    if (string.IsNullOrEmpty(stringValue)) continue;

                    object? propValue = converterService.Convert(stringValue, member.Type);
                    member.Setter(Values, propValue);
                }
                catch (Exception ex)
                {
                    SelfLog($"{ex.Message} {ex.StackTrace}");
                }
        }

        public void Dispose()
        {
            if (Values is IDisposable)
                ((IDisposable)Values).Dispose();
        }

    }
}
