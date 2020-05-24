using System.Linq;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;

namespace EnvironmentVariables
{
    /// <summary>
    /// Provider that allows you to access environment variables through object of specified class
    /// </summary>
    /// <typeparam name="T">Class that contains environment variables as props</typeparam>
    public partial class EnvironmentProvider<T> where T : class, new()
    {
        private readonly Type type = typeof(T);
        private readonly List<MemberMap> members = new List<MemberMap>();

        /// <summary>
        /// Values of all defined environment variables
        /// </summary>
        public T Values { get; private set; }

        /// <summary>
        /// Function that uses to access value of environment variable. 
        /// <see cref="Environment.GetEnvironmentVariable(string)" /> by default.
        /// </summary>
        public Func<string, string> EnvProvider { get; set; } =
            (string s) => Environment.GetEnvironmentVariable(s);

        //public Action<string> SelfLog { get; set; } = (string _) => { };

        /// <summary>
        /// Default constructor. Create object of provided type and set values to props.
        /// </summary>
        public EnvironmentProvider() : this(null) { }

        /// <summary>
        /// You can provide initial values with this constructor
        /// </summary>
        /// <param name="initialValues">Object that will be used as initial values</param>
        public EnvironmentProvider(T initialValues)
        {
            Values = initialValues ?? new T();
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
                members.Add(new MemberMap(prop));

            Reload();
        }

        /// <summary>
        /// Reload all values
        /// </summary>
        public void Reload()
        {
            foreach (var member in members)
            {
                //get env value
                var stringValue = EnvProvider(member.EnvName);

                //skip if no value
                if (string.IsNullOrEmpty(stringValue)) continue;

                object propValue = Utils.Convert(stringValue, member.Type);
                member.Setter(Values, propValue);
            }
        }

    }
}
