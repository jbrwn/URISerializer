using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer
{
    public class PrimitiveConverter : IUriConverter
    {
        public bool CanConvert(Type type)
        {
            return type.IsPrimitive
                || type == typeof(String)
                || type == typeof(Decimal);
        }

        public object ReadValue(Type type, String[] values)
        {
            if (values.Length > 1)
            {
                throw new ArgumentException("Cannot convert more than one value", "values");
            }
            return Convert.ChangeType(values[0], type);
        }

        public string WriteValue(Type type, object value)
        {
            return (string)Convert.ChangeType(value, typeof(string));
        }
    }
}
