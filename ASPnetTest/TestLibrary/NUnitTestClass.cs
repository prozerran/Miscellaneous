using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;

// ====================================================================================
// NUnit: http://www.nunit.org/index.php?p=home
// Run: NUnit.exe
// Open Project -> Load NUnit Test DLL
// Execute!
// ====================================================================================

namespace TestLibrary
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
        public void TestRun()
        {
            bool res;
            res = TestClass.Run("Hello World");
            Assert.AreEqual(true, res);

            res = TestClass.Run("");
            Assert.AreEqual(false, res);
        }

        [Test]
        public void TestWriteLog()
        {
            bool res = TestClass.WriteLog("Testing from NUnit.");
            Assert.AreEqual(true, res);
        }
    }
}
