using System;
using QuestEngine.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuestEngine.Tests.Controllers
{
    [TestClass]
    public class QuestControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            QuestController controller = new QuestController();

            // Act
            var result = controller.Get("");

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
