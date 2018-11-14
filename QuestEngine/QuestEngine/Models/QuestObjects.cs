using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestEngine.Models
{
    public struct Milestone
    {
        public Milestone(int m, int c)
        {
            MilestoneIndex = m;
            ChipsAwarded = c;
        }

        public int MilestoneIndex { get; set; }
        public int ChipsAwarded { get; set; }
    }

    public struct StateResponse
    {
        public double TotalQuestPercentCompleted { get; set; }
        public int LastMilestoneIndexCompleted { get; set; }
        public int LastQuestIndexCompleted { get; set; }
    }

    public struct ProgressRequest
    {
        public string PlayerId { get; set; }
        public int PlayerLevel { get; set; }
        public int ChipAmountBet { get; set; }
    }

    public struct ProgressResponse
    {
        public long QuestPointsEarned { get; set; }
        public double TotalQuestPercentCompleted { get; set; }
        public Milestone MilestonesCompleted { get; set; }
    }
}