using System;
using QuestEngine.Models;
using QuestEngine.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using System.Net.Http;
using System.Web.Http;

namespace QuestEngine.Tests.Controllers
{
    [TestClass]
    public class QuestControllerTest
    {
        const string player_name = "Player_Y";

        [TestMethod]
        public void Test_1_Post()
        {
            // Arrange
            var c = new QuestController();
            c.Request = new HttpRequestMessage();
            c.Configuration = new HttpConfiguration();

            var pr = new ProgressRequest()
            {
                PlayerId = player_name,
                PlayerLevel = 1,
                ChipAmountBet = 233
            };

            // Act
            var r = c.Post(pr);
            var cr = r as OkNegotiatedContentResult<ProgressResponse>;

            // Assert
            Assert.IsNotNull(r);
            Assert.IsNotNull(cr);
            Assert.IsNotNull(cr.Content);
            Assert.IsTrue(cr.Content.QuestPointsEarned > 0);
        }

        [TestMethod]
        public void Test_2_Get()
        {
            // Arrange
            var c = new QuestController();
            c.Request = new HttpRequestMessage();
            c.Configuration = new HttpConfiguration();

            // Act
            var r = c.Get(player_name);
            var cr = r as OkNegotiatedContentResult<StateResponse>;

            // Assert
            Assert.IsNotNull(r);
            Assert.IsNotNull(cr);
            Assert.IsNotNull(cr.Content);
            Assert.IsTrue(cr.Content.TotalQuestPercentCompleted > 0.0);
        }

        //[TestMethod]
        public void Test_3_Delete()
        {
            // Arrange
            var c = new QuestController();
            c.Request = new HttpRequestMessage();
            c.Configuration = new HttpConfiguration();

            // Act
            var r = c.Delete(player_name);

            // Assert
            Assert.IsNotNull(r);
            Assert.IsInstanceOfType(r, typeof(OkResult));
        }
    }
}
