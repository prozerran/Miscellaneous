using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using TestLibrary;

// ====================================================================================
// NUnit: http://www.nunit.org/index.php?p=home
// Run: NUnit.exe
// Open Project -> Load NUnit Test DLL
// Execute!
// ====================================================================================

namespace WindowsService1
{
    [TestFixture]
    public class TestNUnitClass
    {
        // declare any objects here

        [SetUp]
        public void Init()
        {
            // instantiate any objects here
        }

        [Test]
        public void TestWriteInfoLog()
        {
            Logger.Instance.WriteInfoLog("Write to Info Log.");
        }

        [Test]
        public void TestWriteErrorLog()
        {
            Logger.Instance.WriteErrorLog("Write to Error Log.");
        }

        [Test]
        public void TestWriteInfoFormatLog()
        {
            Logger.Instance.WriteInfoLog("Write TextString Num:{0}, String:{1}, Double:{2}, Boolean:{3}",
                123, "Key", 3.14159, true);
        }

        [Test]
        public void TestWriteErrorFormatLog()
        {
            Logger.Instance.WriteErrorLog("Write TextString Num:{0}, String:{1}, Double:{2}, Boolean:{3}",
                -123, "Value", -3.14159, false);
        }

        [Test]
        public void TestServiceFoo()
        {
            string str = "";
            bool b = Service1.Foo(ref str);
            Console.WriteLine(str);
            Assert.IsTrue(b);
        }

        [Test]
        public void TestAppSetting()
        {
            string str;
            str = TestClass.ReadAppSetting("trace");
            Assert.AreEqual("true", str);

            str = TestClass.ReadAppSetting("debug");
            Assert.AreEqual("false", str);

            str = TestClass.ReadAppSetting("NULL");
            Assert.AreEqual(null, str);
        }

        [Test]
        public void TestConnectString()
        {
            string str;
            str = TestClass.ReadConnectString("ldap_path");
            Assert.AreEqual(true, str.StartsWith(@"LDAP://"));

            str = TestClass.ReadConnectString("db_name");
            Assert.AreEqual(true, str.StartsWith("server="));

            str = TestClass.ReadConnectString("NULL");
            Assert.AreEqual(string.Empty, str);
        }
    }
}
