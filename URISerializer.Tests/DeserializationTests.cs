using System;
using System.Web;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using URISerializer;
using System.Reflection;

namespace URISerializer.Tests
{
    [TestClass]
    public class DeserializationTests
    {
        [TestMethod]
        public void Deserialize_ClassWithPrimitives()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString["StringProperty"] = "string";
            qString["CharProperty"] = "c";
            qString["BooleanProperty"] = "true";
            qString["ByteProperty"] = "1";
            qString["SByteProperty"] = "1";
            qString["UInt16Property"] = "1";
            qString["UInt32Property"] = "1";
            qString["UInt64Property"] = "1";
            qString["Int16Property"] = "1";
            qString["Int32Property"] = "1";
            qString["Int64Property"] = "1";
            qString["SingleProperty"] = ".1";
            qString["DoubleProperty"] = ".1";
            qString["DecimalProperty"] = ".1";

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithPrimitives));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithPrimitives c = ser.Deserialize<ClassWithPrimitives>(uri);

            Assert.IsNotNull(c);
            Assert.AreEqual("resource", c.Resource);
            Assert.AreEqual("string", c.StringProperty);
            Assert.AreEqual('c', c.CharProperty);
            Assert.AreEqual(true, c.BooleanProperty);
            Assert.AreEqual((Byte)1, c.ByteProperty);
            Assert.AreEqual((SByte)1, c.SByteProperty);
            Assert.AreEqual((UInt16)1, c.UInt16Property);
            Assert.AreEqual((UInt32)1, c.UInt32Property);
            Assert.AreEqual((UInt64)1, c.UInt64Property);
            Assert.AreEqual((Int16)1, c.Int16Property);
            Assert.AreEqual((Int32)1, c.Int32Property);
            Assert.AreEqual((Int64)1, c.Int64Property);
            Assert.AreEqual((Single).1, c.SingleProperty);
            Assert.AreEqual((Double).1, c.DoubleProperty);
            Assert.AreEqual((Decimal).1, c.DecimalProperty);
        }

        [TestMethod]
        public void Deserialize_MissingKeys()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString["Int32Property"] = "1";

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithPrimitives));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithPrimitives c = ser.Deserialize<ClassWithPrimitives>(uri);

            Assert.IsNotNull(c);
            Assert.AreEqual(1, c.Int32Property);
            Assert.IsNull(c.StringProperty);
            Assert.AreEqual((UInt32)0, c.UInt32Property);
            Assert.AreEqual((Double)0, c.DoubleProperty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Deserialize_ExtraKeys()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString.Add("Int32Property", "1");
            qString.Add("Int32Property", "2");

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithPrimitives));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithPrimitives c = ser.Deserialize<ClassWithPrimitives>(uri);
        }

        [TestMethod]
        public void Deserialize_KeyCase()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString["INT32PROPERTY"] = "1";

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithPrimitives));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithPrimitives c = ser.Deserialize<ClassWithPrimitives>(uri);

            Assert.IsNotNull(c);
            Assert.AreEqual(1, c.Int32Property);
        }

        [TestMethod]
        public void Deserialize_AttributeCtor()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString["StringProperty"] = "string";
            qString["Int32Property"] = "1";

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithAttributeCtor));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithAttributeCtor c = ser.Deserialize<ClassWithAttributeCtor>(uri);

            Assert.IsNotNull(c);
            Assert.AreEqual("resource", c.Resource);
            Assert.AreEqual("string", c.StringProperty);
            Assert.AreEqual(1, c.Int32Property);
        }

        [TestMethod]
        public void Deserialize_ParameterizedCtor()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString["StringProperty"] = "string";
            qString["Int32Property"] = "1";

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithParameterizedCtor));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithParameterizedCtor c = ser.Deserialize<ClassWithParameterizedCtor>(uri);

            Assert.IsNotNull(c);
            Assert.AreEqual("resource", c.Resource);
            Assert.AreEqual("string", c.StringProperty);
            Assert.AreEqual(1, c.Int32Property);
        }

        [TestMethod]
        public void Deserialize_DefaultCtor()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString["StringProperty"] = "string";
            qString["Int32Property"] = "1";

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithDefaultCtor));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithDefaultCtor c = ser.Deserialize<ClassWithDefaultCtor>(uri);

            Assert.IsNotNull(c);
            Assert.AreEqual("resource", c.Resource);
            Assert.AreEqual("string", c.StringProperty);
            Assert.AreEqual(0, c.Int32Property);
        }

        [TestMethod]
        public void Deserialize_OptionalParam()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString["StringProperty"] = "string";

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithOptionalParam));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithOptionalParam c = ser.Deserialize<ClassWithOptionalParam>(uri);

            Assert.IsNotNull(c);
            Assert.AreEqual("resource", c.Resource);
            Assert.AreEqual("string", c.StringProperty);
            Assert.AreEqual(10, c.Int32Property);
        }

        [TestMethod]
        public void Deserialize_NamedProperty()
        {
            NameValueCollection qString = HttpUtility.ParseQueryString("");
            qString["StringProperty"] = "string";
            qString["Int32Property"] = "10";

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "test";
            builder.Host = "resource";
            builder.Query = qString.ToString();
            Uri uri = builder.Uri;

            SchemeResolver sr = new SchemeResolver();
            sr.Add("test", typeof(ClassWithNamedProperty));
            UriSerializer ser = new UriSerializer(sr);
            ClassWithNamedProperty c = ser.Deserialize<ClassWithNamedProperty>(uri);

            Assert.IsNotNull(c);
            Assert.AreEqual("resource", c.R);
            Assert.AreEqual("string", c.StringProperty);
            Assert.AreEqual(10, c.I);
        }
    }
}
