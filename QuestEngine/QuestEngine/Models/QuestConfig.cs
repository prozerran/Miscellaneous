using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace QuestEngine.Models
{
    public struct MileStone
    {
        public int PointsCompleted { get; set; }
        public int ChipsAwarded { get; set; }
    }

    public struct Quest
    {
        public int PointsCompleted { get; set; }
        public List<MileStone> MileStones { get; set; }
    }

    public struct QuestConfig
    {
        public int RateFromBet { get; set; }
        public int LevelBonusRate { get; set; }
        public List<Quest> Quests { get; set; }
    }

    // Singleton - Full lazy initialization
    public class QuestConfigManager
    {
        private QuestConfig m_qcfg = new QuestConfig();

        private static readonly Lazy<QuestConfigManager> lazy =
            new Lazy<QuestConfigManager>(() => new QuestConfigManager());

        public static QuestConfigManager Instance { get { return lazy.Value; } }

        private QuestConfigManager()
        {
            // deserialize JSON directly from a file
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "QuestConfig.json");

            using (StreamReader file = File.OpenText(path))
            {
                var js = new JsonSerializer();
                m_qcfg = (QuestConfig)js.Deserialize(file, typeof(QuestConfig));
            }
        }

        public int GetQuestSetting(string key)
        {
            try
            {
                return Convert.ToInt32(m_qcfg.GetType().GetField(key).GetValue(m_qcfg));
            }
            catch { }
            return 1;
        }

        public Quest? GetQuest(int idx)
        {
            try
            {
                return m_qcfg.Quests[idx];
            }
            catch { }
            return null;
        }
    }
}