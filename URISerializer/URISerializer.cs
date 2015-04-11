using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web;
using System.Collections;
using System.Linq;

namespace URISerializer
{
    public class UriSerializer
    {
        private readonly SchemeResolver _map;
        private readonly IEnumerable<IUriConverter> _baseConverters;
        private readonly IEnumerable<IUriConverter> _converters;

        public UriSerializer(SchemeResolver map)
        {
            this._map = map;
            this._converters = Enumerable.Empty<IUriConverter>();
            this._baseConverters = new List<IUriConverter>() { new PrimitiveConverter() };
        }

        public UriSerializer(SchemeResolver map, params IUriConverter[] converters)
        {
            this._map = map;
            this._converters = converters;
            this._baseConverters = new List<IUriConverter>() { new PrimitiveConverter() };
        }

        public Uri Serialize<T>(T obj)
        {

            Type type = typeof(T);
            string scheme = this._map[type];
            if (scheme == null)
            {
                throw new Exception(string.Format("Cannot resolve scheme: {0}", scheme));
            }

            NameValueCollection vars = HttpUtility.ParseQueryString("");
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                MethodInfo mi = properties[i].GetGetMethod();
                if (mi != null)
                {
                    string pname = GetNameFromAttribute(properties[i]) ?? properties[i].Name;
                    Type ptype = properties[i].PropertyType;

                    IUriConverter converter = GetConverterFromAttribute(properties[i])
                        ?? GetConverter(this._converters, ptype)
                        ?? GetConverter(this._baseConverters, ptype);

                    if (converter == null)
                    {
                        throw new Exception(string.Format("No converter found for Type {0}", ptype.Name));
                    }

                    object pvalue = mi.Invoke(obj, null);
                    string value = converter.WriteValue(ptype, pvalue);
                    vars.Add(pname, value);
                }
            }

            UriBuilder builder = new UriBuilder();
            builder.Scheme = scheme;
            builder.Host = vars.Get("resource");
            vars.Remove("resource");
            builder.Query = vars.ToString();
            return builder.Uri;
        }

        public T Deserialize<T>(Uri uri)
        {
            string scheme = uri.Scheme;
            string resource = uri.Host + uri.AbsolutePath;
            NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
            IDictionary<string, string> vars = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            vars.Add("resource", resource);
            var items = query.AllKeys.SelectMany(query.GetValues, (k, v) => new { key = k, value = v });
            foreach (var item in items)
            {
                vars.Add(item.key, item.value);
            }

            Type type = this._map[scheme];
            if (type == null)
            {
                throw new Exception(string.Format("Scheme: {0} is not registered", scheme));
            }

            ConstructorInfo ci = ReflectionUtils.GetAttributeCtor(type, typeof(UriConstructorAttribute))
                ?? ReflectionUtils.GetDefaultCtor(type)
                ?? ReflectionUtils.GetParameterizedCtor(type);
            
            if (ci == null)
            {
                throw new Exception(string.Format("Cannot find constructor for {0}", type.Name));
            }

            // Invoke ctor
            ParameterInfo[] parameters = ci.GetParameters();
            object[] args = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                string pname = GetNameFromAttribute(parameters[i]) ?? parameters[i].Name;
                Type ptype = parameters[i].ParameterType;
                string arg;
                if (vars.TryGetValue(pname, out arg))
                {
                    IUriConverter converter = GetConverterFromAttribute(parameters[i])
                        ?? GetConverter(this._converters, ptype)
                        ?? GetConverter(this._baseConverters, ptype);
                    
                    if (converter == null)
                    {
                        throw new Exception(string.Format("No converter found for Type {0}", ptype.Name));
                    }

                    args[i] = converter.ReadValue(ptype, arg);
                    vars.Remove(arg);
                }
                // try get default
                else if (parameters[i].IsOptional)
                {
                    args[i] = parameters[i].RawDefaultValue;
                }
                else
                {
                    throw new Exception(string.Format("No value provided for constructor paramater {0}", pname));
                }
            }
            object obj = ci.Invoke(args);

            // Set Properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                MethodInfo mi = properties[i].GetSetMethod();
                if (mi != null)
                {
                    string pname = GetNameFromAttribute(properties[i]) ?? properties[i].Name;
                    Type ptype = properties[i].PropertyType;

                    string arg;
                    if (vars.TryGetValue(pname, out arg))
                    {
                        IUriConverter converter = GetConverterFromAttribute(properties[i])
                            ?? GetConverter(this._converters, ptype)
                            ?? GetConverter(this._baseConverters, ptype);

                        if (converter == null)
                        {
                            throw new Exception(string.Format("No converter found for Type {0}", ptype.Name));
                        }

                        object[] value = new object[] { converter.ReadValue(ptype, arg) };
                        mi.Invoke(obj, value);
                    }
                }
            }
            return (T)obj;
        }

        private IUriConverter GetConverter(IEnumerable<IUriConverter> converters, Type type)
        {
            foreach (IUriConverter converter in converters)
            {
                if (converter.CanConvert(type))
                    return converter;
            }
            return null;
        }

        private string GetNameFromAttribute(ParameterInfo parameter)
        {
            string name = null;
            UriPropertyAttribute attribute = ReflectionUtils.GetAttribute<UriPropertyAttribute>(parameter);
            if (attribute != null)
            {
                name = attribute.Name;
            }
            return name;
        }

        private string GetNameFromAttribute(PropertyInfo property)
        {
            string name = null;
            UriPropertyAttribute attribute = ReflectionUtils.GetAttribute<UriPropertyAttribute>(property);
            if (attribute != null)
            {
                name = attribute.Name;
            }
            return name;
        }

        private IUriConverter GetConverterFromAttribute(ParameterInfo parameter)
        {
            IUriConverter converter = null;
            UriConverterAttribute attribute = ReflectionUtils.GetAttribute<UriConverterAttribute>(parameter);
            if (attribute != null)
            {
                converter = ReflectionUtils.InvokeType<IUriConverter>(attribute.Type, attribute.Parameters);
            }
            return converter;
        }

        private IUriConverter GetConverterFromAttribute(PropertyInfo property)
        {
            IUriConverter converter = null;
            UriConverterAttribute attribute = ReflectionUtils.GetAttribute<UriConverterAttribute>(property);
            if (attribute != null)
            {
                converter = ReflectionUtils.InvokeType<IUriConverter>(attribute.Type, attribute.Parameters);
            }
            return converter;
        }


    }
}
