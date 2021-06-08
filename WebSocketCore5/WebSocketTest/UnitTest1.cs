
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;
using WebSocketCore.Controllers;

// NUnit3 Test setup/cases
// https://nunit.org/
// TEST -> Right click, debug test [Test Explorer]
// Following sample test WebApi2 MVC endpoints

namespace WebSocketTest
{
    public class Tests
    {
        private HomeController ctr = null;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ctr = new HomeController(null);
            Console.WriteLine("OneTimeSetUp");
        }

        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("SetUp");
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Console.WriteLine("OneTimeTearDown");
        }

        [Test]
        public void TestGet()
        {
            // Arrange

            // Act
            var res = ctr.Get();
            var okResult = res as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public void TestPost()
        {
            // Arrange
            var req = new ReqMessage { Name = "TimHsu", Age = 23 };

            // Act
            var res = ctr.GetJsonString(req);
            var okResult = res as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsTrue(okResult.Value is ReqMessage);
            //Assert.IsType<ReqMessage>(okResult.Value);

            var val = okResult.Value as ReqMessage;
            Assert.AreEqual(val.Name, "TimHsu");
            Assert.AreEqual(val.Age, 23);
        }

        [Test]
        public async Task TestAsync()
        {
            // Arrange

            // Act
            var res = await ctr.AsynchronousCall();
            var okResult = res as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsTrue(okResult.Value is string);
            //Assert.IsType<string>(okResult.Value);

            var val = okResult.Value as string;
            Assert.AreEqual(val, "this is async call");
        }
    }
}