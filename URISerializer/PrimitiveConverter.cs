using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer
{
    public class PrimitiveConverter : IUriConverter
    {
        public bool CanRead { get { return true; } }

        public bool CanWrite { get { return true; } }

        public bool CanConvert(Type type)
        {
            return type.IsPrimitive || type == typeof(string);
        }

        public object ReadValue(Type type, string value)
        {
            return Convert.ChangeType(value, type);
        }

        public string WriteValue(Type type, object value)
        {
            return (string)Convert.ChangeType(value, typeof(string));
        }
    }
}
