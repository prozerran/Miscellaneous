using System;
using QuestEngine.Models;
using QuestEngine.Handler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace QuestEngine.Tests.Handler
{
    [TestClass]
    public class QuestHandlerTest
    {
        const string player_name1 = "PlayStudio1";
        const string player_name2 = "PlayStudio2";
        const string player_name3 = "PlayStudio3";
        const string player_nameX = "PlayStudioX";
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
        public void Test_1_GetProgress()
        {
            GetProgress(player_nameX);
        }

        [TestMethod]
        public void Test_2_GetState()
        {
            GetState(player_nameX);
        }

        //[TestMethod]
        public void Test_3_Delete()
        {
            Delete(player_nameX);
        }

        [TestMethod]
        public void Test_4_Simulate_Milestone()
        {
            // simulate a single user questing, achieving a milestone
            // assume config doesnt change

            // delete everything of that user first
            QuestHandler.Instance.Delete(player_name2);

            Simulate_Milestone(player_name2);

            // Act
            var sr = GetState(player_name2);

            // Assert, after looping, we should have reached 1st milestone
            Assert.IsTrue(sr.LastMilestoneIndexCompleted > 0);
        }

        [TestMethod]
        public void Test_5_Simulate_Quest_Single()
        {
            // simulate a single user questing, achieving a quest
            // assume config doesnt change

            Simulate_Quest_Single(player_name3);
        }

        [TestMethod]
        public void Test_6_Simulate_Quest_Multiple()
        {
            // simulate multiple users questing simultaneously, 
            // assume config doesnt change

            Thread t1 = new Thread(Simulate_Quest_Single);
            t1.Start(player_name1);

            Thread t2 = new Thread(Simulate_Quest_Single);
            t2.Start(player_name2);

            Thread t3 = new Thread(Simulate_Quest_Single);
            t3.Start(player_name3);

            t1.Join();
            t2.Join();
            t3.Join();
        }

        #region private_methods

        private ProgressResponse GetProgress(string name)
        {
            // Arrange
            var qh = QuestHandler.Instance;
            var pq = new ProgressRequest()
            {
                PlayerId = name,
                PlayerLevel = 1,
                ChipAmountBet = 233
            };

            // Act
            var pr = qh.GetProgress(pq);

            // Assert
            Assert.IsTrue(pr.QuestPointsEarned > 0);
            Assert.IsTrue(pr.TotalQuestPercentCompleted > 0.0);

            return pr;
        }

        private StateResponse GetState(string name)
        {
            // Arrange
            var qh = QuestHandler.Instance;

            // Act
            var sr = qh.GetState(name);

            // Assert           
            Assert.IsTrue(sr.TotalQuestPercentCompleted > 0.0);
            Assert.IsTrue(sr.LastQuestIndexCompleted >= 0);
            Assert.IsTrue(sr.LastMilestoneIndexCompleted >= 0);

            return sr;
        }

        private void Delete(string name)
        {
            QuestHandler.Instance.Delete(name);
        }

        private void Simulate_Milestone(string name)
        {
            // loop through 5 times, we should achieve 1st milestone in Q1
            for (int i = 0; i < 5; i++)
            {
                // Act
                var pr = GetProgress(name);

                // this is only when we reached a milestone!
                if (pr.MilestonesCompleted != null)
                {
                    Assert.IsTrue(pr.MilestonesCompleted.MilestoneIndex > 0);
                    Assert.IsTrue(pr.MilestonesCompleted.ChipsAwarded > 0);
                }
            }
        }

        private void Simulate_Quest_Single(object obj)
        {
            Simulate_Quest_Single(obj.ToString());
        }

        private void Simulate_Quest_Single(string name)
        {
            // simulate a single user questing, achieving a quest
            // assume config doesnt change

            // delete everything of that user first
            QuestHandler.Instance.Delete(name);

            // Act - simulate achieving some milestones
            for (int i = 0; i < 50; i++)
            {
                Simulate_Milestone(name);
            }

            // Act
            var sr = GetState(name);

            // Assert, we should have reached next quest
            Assert.IsTrue(sr.LastQuestIndexCompleted > 0);
        }

        #endregion
    }
}
