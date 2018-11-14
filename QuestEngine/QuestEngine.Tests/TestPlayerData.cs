using System;
using QuestEngine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuestEngine.Tests
{
    [TestClass]
    public class TestPlayerData
    {
        const string player_name = "Player_X";

        [TestMethod]
        public void TestSave()
        {
            // Arrange
            var pdm = PlayerDataManager.Instance;
            var pd = new PlayerProfile()
            {
                PlayerName = player_name,
                QuestIndex = 2,
                PointsEarned = 777
            };

            // Act
            var b = pdm.Save(pd);

            // Assert
            Assert.IsTrue(b);
            Assert.IsTrue(pdm.Exist(player_name));
        }

        [TestMethod]
        public void TestLoad()
        {
            // Arrange
            var pdm = PlayerDataManager.Instance;

            // Act
            var pd = pdm.Load(player_name);

            // Assert
            Assert.AreEqual(player_name, pd.PlayerName);
            Assert.AreEqual(2, pd.QuestIndex);
            Assert.AreEqual(777, pd.PointsEarned);
        }
    }
}
