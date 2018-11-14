using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FlatBuffers;
using QuestEngine.PlayerData;

// https://exiin.com/blog/flatbuffers-for-unity-sample-code/
// 1. Install Flatc + Flatbuffers.dll
// 2. flatc -n SaveSchema.fbs --gen-onefile
// 3. Save data -> into flatbuffers
// 4. Load

namespace QuestEngine.Models
{
    public class PlayerProfile
    {
        public string PlayerName { get; set; }
        public int QuestIndex { get; set; }
        public int MilestoneIndex { get; set; }
        public long PointsEarned { get; set; }
    }

    // TODO: Currently, uses flatbuffers to store data in file to maintain quest state.
    // Need to refactor to use DB instead (redis, sqlite?)

    public class PlayerDataManager
    {
        private static readonly Lazy<PlayerDataManager> lazy =
            new Lazy<PlayerDataManager>(() => new PlayerDataManager());

        public static PlayerDataManager Instance { get { return lazy.Value; } }

        private PlayerDataManager() { }

        private string GetFilePath(string playerid)
        {
            var p1 = string.Format($"Players\\{playerid}.bin");
            var p2 = AppDomain.CurrentDomain.BaseDirectory.ToString();

            if (string.IsNullOrEmpty(p2))
                p2 = Directory.GetCurrentDirectory();

            var path = Path.Combine(p2, p1);
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            return path;
        }

        public bool Exist(string playerid)
        {
            return File.Exists(GetFilePath(playerid));
        }

        public PlayerProfile Load(string playerid)
        {
            if (!Exist(playerid))
                return null;

            try
            {
                var path = GetFilePath(playerid);
                var pd = new PlayerProfile();
                var bb = new ByteBuffer(File.ReadAllBytes(path));

                // load data from flatbuffer file
                var data = Player.GetRootAsPlayer(bb);
                pd.PlayerName = data.Name;
                pd.QuestIndex = data.Quest;
                pd.MilestoneIndex = data.Milestone;
                pd.PointsEarned = data.Pointsearned;

                return pd;
            }
            catch { }
            return null;
        }

        public bool Save(PlayerProfile p)
        {
            try
            {
                var fbb = new FlatBufferBuilder(1);
                var ids = fbb.CreateString(p.PlayerName);

                // build our player information
                Player.StartPlayer(fbb);
                Player.AddName(fbb, ids);
                Player.AddQuest(fbb, p.QuestIndex);
                Player.AddMilestone(fbb, p.MilestoneIndex);
                Player.AddPointsearned(fbb, p.PointsEarned);
                var offset = Player.EndPlayer(fbb);

                // serialize player data
                Player.FinishPlayerBuffer(fbb, offset);

                // save flatbuffer to file
                using (var ms = fbb.DataBuffer.ToMemoryStream(fbb.DataBuffer.Position, fbb.Offset))
                {
                    string path = GetFilePath(p.PlayerName);
                    File.WriteAllBytes(path, ms.ToArray());
                }
                return true;
            }
            catch { }
            return false;
        }

        // remove player data
        public void Delete(string playerid)
        {
            File.Delete(GetFilePath(playerid));
        }

        public PlayerProfile GetOrCreatePlayer(string playerid)
        {
            if (!Exist(playerid))
            {
                Save(new PlayerProfile()
                {
                    PlayerName = playerid,
                    QuestIndex = 0,
                    MilestoneIndex = 0,
                    PointsEarned = 0
                });
            }
            return Load(playerid);
        }
    }
}