using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UriConverterAttribute : Attribute
    {
        private readonly Type _type;
        private readonly object[] _paramaters;
        public UriConverterAttribute(Type type)
        {
            this._type = type;
            this._paramaters = new object[0];
        }

        public UriConverterAttribute(Type type, params object[] parameters)
        {
            this._type = type;
            this._paramaters = parameters;
        }

        public object[] Parameters
        {
            get { return this._paramaters; }
        }

        public Type Type
        {
            get { return this._type; }
        }
    }
}
