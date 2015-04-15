using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer.Tests
{
    public class ClassWithOptionalParam
    {
        private String _StringProperty;
        private String _Resource;
        private Int32 _Int32Property;

        public ClassWithOptionalParam(String StringProperty, String Resource, Int32 Int32Property = 10)
        {
            this._StringProperty = StringProperty;
            this._Resource = Resource;
            this._Int32Property = Int32Property;
        }

        public String StringProperty { get { return _StringProperty; } }

        public String Resource { get { return _Resource; } }

        public Int32 Int32Property { get { return _Int32Property; } }
    }
}
