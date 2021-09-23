using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using RhythmGameStarter;

[DefaultExecutionOrder(-95)]
public class GameData : Singleton<GameData>
{
    public List<TextAsset> m_DataText = new List<TextAsset>();

    private Dictionary<int, SongConfig> m_SongConfigs = new Dictionary<int, SongConfig>();
    private Dictionary<int, WeekConfig> m_WeekConfigs = new Dictionary<int, WeekConfig>();
    private Dictionary<int, Dialogue> m_DialogueConfigs = new Dictionary<int, Dialogue>();

    private void Awake()
    {
        LoadSongConfig();
        LoadWeekConfig();
        LoadDialogueConfig();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     // Helper.DebugLog("Song Count: " + GetSongConfig()[15].m_Name);
        //     // Helper.DebugLog("Song Count: " + m_SongConfigs.Count);
        //     // for (int i = 1; i <= GetWeekConfigs().Count; i++)
        //     // {
        //     //     Helper.DebugLog("Id: " + GetWeekConfigs()[i].m_Id.ToString());
        //     //     Helper.DebugLog("name: " + GetWeekConfigs()[i].m_Name);
        //     //     Helper.DebugLog("Easy: " + GetWeekConfigs()[i].m_Easy.ToString());
        //     //     Helper.DebugLog("Normal: " + GetWeekConfigs()[i].m_Normal.ToString());
        //     //     Helper.DebugLog("Hard: " + GetWeekConfigs()[i].m_Hard.ToString());
        //     //     Helper.DebugLog("Enemy: " + GetWeekConfigs()[i].m_EnemyName);
        //     //     Helper.DebugLog("Week: " + GetWeekConfigs()[i].m_Week.ToString());
        //     // }

        //     // for (int i = 1; i <= GetSongConfigs().Count; i++)
        //     // {
        //     //     Helper.DebugLog("Id: " + GetSongConfigs()[i].m_Id);
        //     //     Helper.DebugLog("name: " + GetSongConfigs()[i].m_Name);
        //     //     Helper.DebugLog("Easy: " + GetSongConfigs()[i].m_1stReward.ToString());
        //     //     Helper.DebugLog("Normal: " + GetSongConfigs()[i].m_ReplayReward.ToString());
        //     //     Helper.DebugLog("Hard: " + GetSongConfigs()[i].m_Price.ToString());
        //     //     Helper.DebugLog("Enemy: " + GetSongConfigs()[i].m_EnemyName);
        //     // }

        //     // GetWeekSong(4);

        //     // for (int i = 1; i <= m_DialogueConfigs.Count; i++)
        //     // {
        //     //     Helper.DebugLog(m_DialogueConfigs[i].m_Dialogue);
        //     // }

        //     // List<Dialogue> dias = GetDialogueBySongID(5);
        //     // for (int i = 1; i <= m_DialogueConfigs.Count; i++)
        //     // {
        //     //     Helper.DebugLog(m_DialogueConfigs[i].m_EnemyTurn);
        //     //     // Helper.DebugLog(dias[i].m_Dialogue);
        //     // }

        //     // // BigNumber score = new BigNumber(ProfileManager.GetSongWeekProfiles(1).m_NormalScore);
        //     // BigNumber score = new BigNumber("100");

        //     // ProfileManager.GetSongWeekProfiles(2).m_EasyScore = "100".ToString();
        //     // ProfileManager.GetSongWeekProfiles(2).m_NormalScore = "200".ToString();
        //     // ProfileManager.GetSongWeekProfiles(2).m_HardScore = "300".ToString();
        //     // ProfileManager.GetSongWeekProfiles(3).m_EasyScore = "100".ToString();
        //     // ProfileManager.GetSongWeekProfiles(3).m_NormalScore = "200".ToString();
        //     // ProfileManager.GetSongWeekProfiles(3).m_HardScore = "300".ToString();
        //     // ProfileManager.GetSongWeekProfiles(4).m_EasyScore = "100".ToString();
        //     // ProfileManager.GetSongWeekProfiles(4).m_NormalScore = "200".ToString();
        //     // ProfileManager.GetSongWeekProfiles(4).m_HardScore = "300".ToString();
        //     // ProfileManager.Instance.SaveData();

        //     // Helper.DebugLog("Easy: " + ProfileManager.GetSongWeekProfiles(1).m_NormalScore);
        //     // Helper.DebugLog("Easy: " + ProfileManager.GetSongWeekProfiles(1).m_HardScore);

        //     List<WeekConfig> week = GetWeekSong(6);
        //     for (int i = 0; i < week.Count; i++)
        //     {
        //         Helper.DebugLog("ID: " + week[i].m_Name);
        //     }
        // }
    }

    public void LoadSongConfig()
    {
        m_SongConfigs.Clear();
        TextAsset ta = GetDataAssets(GameDataType.SONG);
        var js1 = JSONNode.Parse(ta.text);
        for (int i = 0; i < js1.Count; i++)
        {
            JSONNode iNode = JSONNode.Parse(js1[i].ToString());

            int id = int.Parse(iNode["ID"]);

            string colName = "";

            string name = "";
            if (iNode["Name"].ToString().Length > 0)
            {
                name = iNode["Name"];
            }

            int bpm = int.Parse(iNode["BPM"]);

            colName = "First Reward";
            BigNumber firstReward = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                firstReward = new BigNumber(iNode[colName]);
            }

            colName = "Replay Reward";
            BigNumber replayReward = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                replayReward = new BigNumber(iNode[colName]);
            }

            colName = "Breakscore Reward";
            BigNumber breakScore = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                breakScore = new BigNumber(iNode[colName]);
            }

            colName = "Price";
            BigNumber price = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                price = new BigNumber(iNode[colName]);
            }

            string enemyName = "";
            if (iNode["Enemy"].ToString().Length > 0)
            {
                enemyName = iNode["Enemy"];
            }

            int enemyNo = int.Parse(iNode["Enemy No"]);

            SongConfig song = new SongConfig();
            song.Init(id, name, bpm, firstReward, replayReward, breakScore, price, enemyName, enemyNo);
            m_SongConfigs.Add(id, song);
        }
    }

    public void LoadWeekConfig()
    {
        m_WeekConfigs.Clear();
        TextAsset ta = GetDataAssets(GameDataType.WEEK);
        var js1 = JSONNode.Parse(ta.text);
        for (int i = 0; i < js1.Count; i++)
        {
            JSONNode iNode = JSONNode.Parse(js1[i].ToString());

            int id = int.Parse(iNode["ID"]);
            int week = int.Parse(iNode["Week"]);

            string colName = "";

            string name = "";
            if (iNode["Name"].ToString().Length > 0)
            {
                name = iNode["Name"];
            }

            colName = "Easy";
            BigNumber easy = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                easy = new BigNumber(iNode[colName]) + 0;
            }

            colName = "Normal";
            BigNumber normal = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                normal = new BigNumber(iNode[colName]);
            }

            colName = "Hard";
            BigNumber hard = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                hard = new BigNumber(iNode[colName]);
            }

            string enemyName = "";
            if (iNode["Enemy"].ToString().Length > 0)
            {
                enemyName = iNode["Enemy"];
            }

            colName = "Price";
            BigNumber price = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                price = new BigNumber(iNode[colName]);
            }

            colName = "Breakscore Reward";
            BigNumber breakReward = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                breakReward = new BigNumber(iNode[colName]);
            }

            colName = "Replay Reward";
            BigNumber replayReward = 0;
            if (iNode[colName].ToString().Length > 0)
            {
                replayReward = new BigNumber(iNode[colName]) + 0;
            }

            WeekConfig song = new WeekConfig();
            song.Init(id, name, easy, normal, hard, enemyName, breakReward, replayReward, week, price);
            m_WeekConfigs.Add(id, song);
        }
    }

    public void LoadDialogueConfig()
    {
        m_DialogueConfigs.Clear();
        TextAsset ta = GetDataAssets(GameDataType.DIALOGUE);
        var js1 = JSONNode.Parse(ta.text);
        for (int i = 0; i < js1.Count; i++)
        {
            JSONNode iNode = JSONNode.Parse(js1[i].ToString());

            int id = int.Parse(iNode["ID"]);


            string colName = "";

            colName = "Dialogue";
            string dialogue = "";
            if (iNode[colName].ToString().Length > 0)
            {
                dialogue = iNode[colName];
            }

            int enemyTurn = int.Parse(iNode["Enemy"]);
            int songId = int.Parse(iNode["SongID"]);

            Dialogue song = new Dialogue();
            song.Init(id, dialogue, enemyTurn, songId);
            m_DialogueConfigs.Add(id, song);
        }
    }

    public TextAsset GetDataAssets(GameDataType _id)
    {
        return m_DataText[(int)_id];
    }

    public SongConfig GetSongConfig(int _id)
    {
        return m_SongConfigs[_id];
    }

    public int GetSongCount()
    {
        return m_SongConfigs.Count;
    }

    public List<SongConfig> GetSongConfigs()
    {
        List<SongConfig> configs = new List<SongConfig>();
        for (int i = 1; i <= m_SongConfigs.Count; i++)
        {
            configs.Add(m_SongConfigs[i]);
        }
        return configs;
    }

    public Dictionary<int, WeekConfig> GetWeekConfigs()
    {
        return m_WeekConfigs;
    }

    public List<WeekConfig> GetWeekSong(int _week)
    {
        List<WeekConfig> weeks = new List<WeekConfig>();
        int count = m_WeekConfigs.Count;
        for (int i = 0; i < count; i++)
        {
            if (m_WeekConfigs[i + 1].m_Week == _week)
            {
                weeks.Add(m_WeekConfigs[i + 1]);
            }
        }

        return weeks;
    }

    public List<Dialogue> GetDialogueBySongID(int _songID)
    {
        List<Dialogue> dialogue = new List<Dialogue>();
        int count = m_DialogueConfigs.Count;
        for (int i = 0; i < count; i++)
        {
            if (m_DialogueConfigs[i + 1].m_SongID == _songID)
            {
                dialogue.Add(m_DialogueConfigs[i + 1]);
            }
        }

        return dialogue;
    }

    public enum GameDataType
    {
        SONG = 0,
        WEEK = 1,
        DIALOGUE = 2,
    }
}

public class SongConfig
{
    public int m_Id;
    public string m_Name;
    public int m_BPM;
    public BigNumber m_1stReward;
    public BigNumber m_ReplayReward;
    public BigNumber m_BreakScoreReward;
    public BigNumber m_Price;
    public string m_EnemyName;
    public int m_EnemyNo;

    public void Init(int _id, string _name, int _bpm, BigNumber _1stReward, BigNumber _ReplayReward, BigNumber _breakScoreReward, BigNumber _price, string _enemyName, int _enemyNo)
    {
        m_Id = _id;
        m_Name = _name;
        m_BPM = _bpm;
        m_1stReward = _1stReward;
        m_ReplayReward = _ReplayReward;
        m_BreakScoreReward = _breakScoreReward;
        m_Price = _price;
        m_EnemyName = _enemyName;
        m_EnemyNo = _enemyNo;
    }
}

public class WeekConfig
{
    public int m_Id;
    public string m_Name;
    public BigNumber m_Easy;
    public BigNumber m_Normal;
    public BigNumber m_Hard;
    public string m_EnemyName;
    public BigNumber m_BreakScoreReward;
    public BigNumber m_ReplayReward;
    public int m_Week;
    public BigNumber m_Price;

    public void Init(int _id, string _name, BigNumber _easy, BigNumber _normal, BigNumber _hard, string _enemyName, BigNumber _breakScoreReward, BigNumber _replayReward, int _week, BigNumber _price)
    {
        m_Id = _id;
        m_Name = _name;
        m_Easy = _easy;
        m_Normal = _normal;
        m_Hard = _hard;
        m_EnemyName = _enemyName;
        m_BreakScoreReward = _breakScoreReward;
        m_ReplayReward = _replayReward;
        m_Week = _week;
        m_Price = _price;
    }
}

public class Dialogue
{
    public int m_ID;
    public string m_Dialogue;
    public int m_EnemyTurn;
    public int m_SongID;

    public void Init(int _id, string _dialogue, int _enemyTurn, int _songID)
    {
        m_ID = _id;
        m_Dialogue = _dialogue;
        m_EnemyTurn = _enemyTurn;
        m_SongID = _songID;
    }
}

namespace RhythmGameStarter
{
    public class SongProfile
    {
        public int m_Id;
        public int m_HighScore;
        public int m_FinishFirst;

        public void Init(int _id)
        {
            m_Id = _id;
            m_HighScore = 0;
            m_FinishFirst = 0;
        }

        public void Load()
        {
            // SongConfig cdc = GameData.Instance.GetSongConfig(m_Id);
        }

        public void SetHighScore(int _highScore)
        {
            m_HighScore = _highScore;
            ProfileManager.Instance.SaveData();
        }

        public void Set1stFinish()
        {
            m_FinishFirst = 1;
            ProfileManager.Instance.SaveData();
        }
    }

    public class WeekProfile
    {
        public int m_Id;
        public int m_RewardLevel;
        public int m_HighScore;

        public void Init(int _id)
        {
            m_Id = _id;
            m_RewardLevel = -1;
            m_HighScore = 0;
        }

        public void Load()
        {
            // SongConfig cdc = GameData.Instance.GetSongConfig(m_Id);
        }

        public void SetRewardLevel(int _level)
        {
            m_RewardLevel = _level;
            ProfileManager.Instance.SaveData();
        }

        public void SetHighScore(int _score)
        {
            m_HighScore = _score;
            ProfileManager.Instance.SaveData();
        }
    }

    public class SongWeekProfile
    {
        public int m_Id;
        public string m_Name;
        public int m_RewardLevel;

        public int m_RepeatEasy;
        public int m_RepeatNormal;
        public int m_RepeatHard;

        public string m_EasyScore;
        public string m_NormalScore;
        public string m_HardScore;

        public void Init(int _id, string _name)
        {
            m_Id = _id;
            m_Name = _name;
            m_RewardLevel = -1;

            m_RepeatEasy = 0;
            m_RepeatNormal = 0;
            m_RepeatHard = 0;

            m_EasyScore = "0";
            m_NormalScore = "0";
            m_HardScore = "0";
        }

        public void SetRewardLevel(int _level)
        {
            m_RewardLevel = _level;
            ProfileManager.Instance.SaveData();
        }

        public void SetScoreByLevel(StoryLevel _level, BigNumber _score)
        {
            if (_level == StoryLevel.EASY)
            {
                m_RepeatEasy++;
                BigNumber scoreEasy = new BigNumber(m_EasyScore);
                if (_score > scoreEasy)
                {
                    m_EasyScore = _score.ToString3();
                }
            }
            else if (_level == StoryLevel.NORMAL)
            {
                m_RepeatNormal++;
                BigNumber scoreNormal = new BigNumber(m_NormalScore);
                if (_score > scoreNormal)
                {
                    m_NormalScore = _score.ToString3();
                }
            }
            else if (_level == StoryLevel.HARD)
            {
                m_RepeatHard++;
                BigNumber scoreHard = new BigNumber(m_HardScore);
                if (_score > scoreHard)
                {
                    m_HardScore = _score.ToString3();
                }
            }
            ProfileManager.Instance.SaveData();
        }

        public BigNumber GetScore(StoryLevel _level)
        {
            BigNumber totalReward = new BigNumber(0);

            if (_level == StoryLevel.EASY)
            {
                BigNumber score = new BigNumber(m_EasyScore);
                totalReward = score;
            }
            else if (_level == StoryLevel.NORMAL)
            {
                BigNumber score = new BigNumber(m_NormalScore);
                totalReward = score;
            }
            else if (_level == StoryLevel.HARD)
            {
                BigNumber score = new BigNumber(m_HardScore);
                totalReward = score;
            }

            return totalReward;
        }

        public BigNumber CalReward(StoryLevel _level, BigNumber _curScore)
        {
            WeekConfig weekConfigs = GameManager.Instance.m_WeekConfigs[GameManager.Instance.m_StorysongNo];
            BigNumber totalReward = new BigNumber(0);

            if (_level == StoryLevel.EASY)
            {
                BigNumber score = new BigNumber(m_EasyScore);
                if (m_RepeatEasy == 0)
                {
                    totalReward = weekConfigs.m_Easy;
                }
                else
                {
                    if (_curScore <= score)
                    {
                        totalReward = weekConfigs.m_ReplayReward;
                    }
                    else
                    {
                        totalReward = weekConfigs.m_BreakScoreReward;
                    }
                }
            }
            else if (_level == StoryLevel.NORMAL)
            {
                BigNumber score = new BigNumber(m_NormalScore);
                if (m_RepeatNormal == 0)
                {
                    totalReward = weekConfigs.m_Easy + 20;
                }
                else
                {
                    if (_curScore <= score)
                    {
                        totalReward = weekConfigs.m_ReplayReward;
                    }
                    else
                    {
                        totalReward = weekConfigs.m_BreakScoreReward;
                    }
                }
            }
            else if (_level == StoryLevel.HARD)
            {
                BigNumber score = new BigNumber(m_HardScore);
                if (m_RepeatHard == 0)
                {
                    totalReward = weekConfigs.m_Easy + 50;
                }
                else
                {
                    if (_curScore <= score)
                    {
                        totalReward = weekConfigs.m_ReplayReward;
                    }
                    else
                    {
                        totalReward = weekConfigs.m_BreakScoreReward;
                    }
                }
            }

            return totalReward;
        }
    }
}