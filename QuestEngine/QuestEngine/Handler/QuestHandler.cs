using QuestEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestEngine.Handler
{
    // singleton
    public class QuestHandler
    {
        private PlayerDataManager pdm = PlayerDataManager.Instance;
        private QuestConfigManager qcm = QuestConfigManager.Instance;

        private static readonly Lazy<QuestHandler> lazy =
            new Lazy<QuestHandler>(() => new QuestHandler());

        public static QuestHandler Instance { get { return lazy.Value; } }

        private QuestHandler() { }

        public StateResponse GetState(string playerid)
        {
            var sr = new StateResponse();
            var pd = pdm.Load(playerid);

            if (pd != null)
            {
                if (qcm.GetQuest(pd.QuestIndex) is Quest q)
                {
                    sr.TotalQuestPercentCompleted = 
                        GetTotalQuestPercentCompleted(pd.PointsEarned, q.PointsCompleted);

                    sr.LastMilestoneIndexCompleted = pd.MilestoneIndex;
                    sr.LastQuestIndexCompleted = pd.QuestIndex;
                }
            }
            return sr;
        }

        public ProgressResponse GetProgress(ProgressRequest pr)
        {
            // calculate points for this round
            long qpa = GetQuestPoint(pr.ChipAmountBet, pr.PlayerLevel);

            // get player data
            var pp = pdm.GetOrCreatePlayer(pr.PlayerId);

            // add qpa to total player saved
            long qpe = qpa + pp.PointsEarned;
            pp.PointsEarned = qpe;      // update total points earned

            // get player current quest
            Quest qq = qcm.GetQuest(pp.QuestIndex).Value;

            // calc total quest % complete
            double qpc = GetTotalQuestPercentCompleted(qpe, qq.PointsCompleted);

            // calc milestone completed
            var ms = GetMileStoneCompleted(qq, pp);

            // update & save player latest info
            pp.MilestoneIndex = ms.MilestoneIndex;
            pdm.Save(pp);

            // determine if quest leveled up to new quest!
            LeveledUpQuest(qq, pp);

            var prc = new ProgressResponse()
            {
                QuestPointsEarned = qpe,
                TotalQuestPercentCompleted = qpc,
                MilestonesCompleted = new Milestone(ms.MilestoneIndex, ms.ChipsAwarded)
            };
            return prc;
        }

        public void Delete(string playerid)
        {
            pdm.Delete(playerid);
        }

        private Milestone GetMileStoneCompleted(Quest q, PlayerProfile p)
        {
            var ms = new Milestone(p.MilestoneIndex, 0);
            var m = q.MileStones[p.MilestoneIndex];
            
            if (p.PointsEarned > m.PointsCompleted)
            {
                ms.MilestoneIndex = p.MilestoneIndex + 1;   // level up milestone!
                ms.ChipsAwarded = m.ChipsAwarded;              
            }
            return ms;
        }
        
        private void LeveledUpQuest(Quest q, PlayerProfile p)
        {
            if (p.PointsEarned >= q.PointsCompleted)
            {
                p.QuestIndex += 1;

                if (qcm.GetQuest(p.QuestIndex) != null)
                {
                    // calc spillage, from excess points earned from previous quest
                    p.PointsEarned -= q.PointsCompleted;
                    p.MilestoneIndex = 0;  // reset milestone for new quest
                    pdm.Save(p);
                }
            }
        }

        private double GetTotalQuestPercentCompleted(double qpe, double pts_com)
        {
            double qpc = (qpe / pts_com) * 100.0;
            return Math.Round(qpc, 2);
        }

        private long GetQuestPoint(int chipsbet, int playerlvl)
        {
            return (chipsbet * qcm.GetQuestSetting("RateFromBet)")) +
                (playerlvl * qcm.GetQuestSetting("LevelBonusRate)"));
        }
    }
}