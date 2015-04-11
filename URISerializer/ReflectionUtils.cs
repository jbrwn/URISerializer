using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace URISerializer
{
    internal static class ReflectionUtils
    {
        public static ConstructorInfo GetDefaultCtor(Type type)
        {
            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).SingleOrDefault(c => !c.GetParameters().Any());
        }

        public static ConstructorInfo GetAttributeCtor(Type type, Type attribute)
        {
            ConstructorInfo ci = null;
            IList<ConstructorInfo> ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(c => c.IsDefined(attribute, true)).ToList();
            if (ctors.Count > 1)
                throw new Exception(string.Format("Multiple constructors with Attribute: {0} not allowed", attribute.Name));
            else if (ctors.Count == 1)
                ci = ctors[0];

            return ci;
        }

        public static ConstructorInfo GetParameterizedCtor(Type type)
        {
            ConstructorInfo ci = null;
            IList<ConstructorInfo> constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).ToList();
            if (constructors.Count == 1)
                ci = constructors[0];

            return ci;
        }

        public static T GetAttribute<T>(ParameterInfo parameter) where T : Attribute
        {
            T attribute = null;
            object[] attributes = parameter.GetCustomAttributes(typeof(T), true);
            if (attributes.Length == 1)
                attribute = (T)attributes[0];

            return attribute;
        }

        public static T GetAttribute<T>(PropertyInfo property) where T : Attribute
        {
            T attribute = null;
            object[] attributes = property.GetCustomAttributes(typeof(T), true);
            if (attributes.Length == 1)
                attribute = (T)attributes[0];

            return attribute;
        }

        public static T InvokeType<T>(Type type, object[] parameters)
        {
            return (T)Activator.CreateInstance(type, parameters);
        }
    }
}
