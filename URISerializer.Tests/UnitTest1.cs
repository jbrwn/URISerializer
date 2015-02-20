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
    }

    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}
