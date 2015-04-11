using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UriPropertyAttribute :Attribute
    {
        private readonly string _name;

        public UriPropertyAttribute(string name)
        {
            this._name = name;
        }

        public string Name
        {
            get { return this._name; }
        }
    }
}
