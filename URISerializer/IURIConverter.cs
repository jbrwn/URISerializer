using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer
{
    public interface IUriConverter
    {
        bool CanConvert(Type type);
        object ReadValue(Type type, string value);
        string WriteValue(Type type, object value);

        bool CanRead { get; }
        bool CanWrite { get; }
    }
}
