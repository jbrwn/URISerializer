using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using URISerializer;
using System.Reflection;

namespace URISerializer.Tests
{
    public class test
    {
        public test() { }
        public test(int p1, string p2) { }
        public string Resource { get; set; }
        public int p1 { get; set; }
        public string p2 { get; set; }
        public int p3 { get; set; }
    }

    [TestClass]
    public class DeserializationTests
    {


        [TestMethod]
        public void TestMethod1()
        {
            Uri uri = new Uri("test://path/to/test/resource?p1=1&p3=5");
            SchemeResolver sm = new SchemeResolver();
            sm.Add("test", typeof(test));

            UriSerializer ser = new UriSerializer(sm);
            test t = ser.Deserialize<test>(uri);
            Assert.IsNotNull(t);

            Uri u = ser.Serialize<test>(t);
            Assert.IsNotNull(u);
        }
    }
}
