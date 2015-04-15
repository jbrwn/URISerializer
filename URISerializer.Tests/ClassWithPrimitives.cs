using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer.Tests
{
    public class ClassWithPrimitives
    {
        public ClassWithPrimitives() { }
        public String Resource { get; set; }
        public String StringProperty { get; set; }
        public Char CharProperty { set; get; }
        public Boolean BooleanProperty { get; set; }
        public Byte ByteProperty { get; set; }
        public SByte SByteProperty { get; set; }
        public UInt16 UInt16Property { get; set; }
        public UInt32 UInt32Property { get; set; }
        public UInt64 UInt64Property { get; set; }
        public Int16 Int16Property { get; set; }
        public Int32 Int32Property { get; set; }
        public Int64 Int64Property { get; set; }
        public Single SingleProperty { get; set; }
        public Double DoubleProperty { get; set; }
        public Decimal DecimalProperty { get; set; }
    }
}
