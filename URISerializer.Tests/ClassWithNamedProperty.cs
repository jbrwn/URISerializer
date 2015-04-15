using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer.Tests
{
    public class ClassWithNamedProperty
    {
        private String _StringProperty;
        private String _Resource;
        private Int32 _Int32Property;

        public ClassWithNamedProperty([UriProperty("StringProperty")] String S)
        {
            this._StringProperty = S;
        }

        public String StringProperty { get { return _StringProperty; } }

        [UriProperty("Resource")]
        public String R
        {
            get { return this._Resource; }
            set { this._Resource = value; }
        }

        [UriProperty("Int32Property")]
        public Int32 I
        {
            get { return this._Int32Property; }
            set { this._Int32Property = value; }
        }

    }
}
