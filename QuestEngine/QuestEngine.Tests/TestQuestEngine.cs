using System;
using QuestEngine.Handler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuestEngine.Tests
{
    [TestClass]
    public class TestQuestEngine
    {
        const string player_name = "Player_X";
        /*
        [ClassInitialize]
        public void ClassInit() { }

        [TestInitialize]
        public void TestInit() {}

        [TestCleanup]
        public void TestTerm() {}

        [ClassCleanup]
        public void ClassTerm() { }
        */

        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var qh = QuestHandler.Instance;

            // Act
            var sr = qh.GetState(player_name);

            // Assert
            Assert.IsTrue(sr.TotalQuestPercentCompleted > 0.0);
            Assert.IsTrue(sr.LastQuestIndexCompleted > 0);
            Assert.IsTrue(sr.LastMilestoneIndexCompleted > 0);
        }
    }
}
