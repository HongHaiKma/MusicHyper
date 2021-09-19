using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
// using Newtonsoft.Json;

namespace RhythmGameStarter
{
    public class PlayerProfile
    {
        private BigNumber m_Gold = new BigNumber(0);
        public string ic_Gold = "0";

        private BigNumber m_Keys = new BigNumber(0);
        public string ic_Keys = "0";

        public int m_Level;
        public int m_Ads;

        public int m_StoryWeek;

        public int m_SelectedCharacter = 0;

        public List<SongProfile> m_SongData = new List<SongProfile>();
        public List<WeekProfile> m_WeekData = new List<WeekProfile>();
        public List<SongWeekProfile> m_SongWeekData = new List<SongWeekProfile>();

        public int m_Week;

        public void LoadLocalProfile()
        {
            m_Gold = new BigNumber(ic_Gold);
            m_Keys = new BigNumber(ic_Keys);
            LoadSongData();
            LoadWeekData();
            // if (GetCharacterProfile(CharacterType.BATMAN) != null)
            // {
            //     Helper.DebugLog("Batman existed!!!");
            // }
            // int a = 2;
            // a = data1["m_Gold"].To;
        }

        public void SaveDataToLocal()
        {
            string piJson = this.ObjectToJsonString();
            ProfileManager.Instance.SaveDataText(piJson);
        }

        public string ObjectToJsonString()
        {
            return JsonMapper.ToJson(this);
        }

        public JsonData StringToJsonObject(string _data)
        {
            return JsonMapper.ToObject(_data);
        }

        public void CreateNewPlayer()
        {
            // PlayerPrefs.SetInt(ConfigKeys.noAds, 0);
            // PlayerPrefs.SetInt(ConfigKeys.rateUs, 1);

            // ic_Gold = "8999999";
            ic_Gold = "1000";
            m_Gold = new BigNumber(ic_Gold);

            ic_Keys = "0";
            m_Keys = new BigNumber(ic_Keys);

            m_Level = 1;
            m_Ads = 1;

            m_Week = 1;

            UnlockSong(1);
            UnlockWeek(GetWeek());
            LoadSongData();
            LoadWeekData();
        }

        public int GetWeek()
        {
            return m_Week;
        }

        public void SetWeek(int _week)
        {
            m_Week = _week;
            SaveDataToLocal();
            // UnlockWeek(_week);
        }

        #region GOLD
        public BigNumber GetGold()
        {
            return m_Gold;
        }

        public string GetGold(bool a = false)
        {
            return (m_Gold + 1).ToString();
        }

        public bool IsEnoughGold(BigNumber _value)
        {
            // _value += 0;
            return (m_Gold >= _value);
        }

        public void AddGold(BigNumber _value)
        {
            m_Gold += _value;
            ic_Gold = m_Gold.ToString();
            Helper.DebugLog("m_Gold = " + m_Gold);
            // ProfileManager.Instance.SaveData();
            SaveDataToLocal();
            // EventManager.TriggerEvent("UpdateGold");
        }

        public void ConsumeGold(BigNumber _value)
        {
            m_Gold -= _value;
            ic_Gold = m_Gold.ToString();
            // ProfileManager.Instance.SaveData();
            SaveDataToLocal();
            // EventManager.TriggerEvent("UpdateGold");
        }

        public void SetGold(BigNumber _value)
        {
            m_Gold = _value;
            ic_Gold = m_Gold.ToString();
            // ProfileManager.Instance.SaveData();
            SaveDataToLocal();
            // EventManager.TriggerEvent("UpdateGold");
        }

        #endregion

        #region 

        public void AddKeys(BigNumber _value)
        {
            m_Keys += _value;
            ic_Keys = m_Keys.ToString();
            SaveDataToLocal();
        }

        public BigNumber GetKeys()
        {
            return m_Keys;
        }

        #endregion

        public bool CheckAds()
        {
            if (m_Ads == 1)
            {
                return true;
            }

            return false;
        }

        public void SetAds(int _value)
        {
            m_Ads = _value;
            SaveDataToLocal();
        }

        #region LEVEL

        public void PassLevel()
        {
            // AnalysticsManager.LogWinLevel(m_Level);

            // if (m_Level < 50)
            // {
            m_Level++;
            // }

            SaveDataToLocal();
        }

        public int GetLevel()
        {
            return m_Level;
        }

        public void SetLevel(int _level)
        {
            m_Level = _level;
            SaveDataToLocal();
        }

        public void SetKeys(BigNumber _keys)
        {
            m_Keys = _keys;
            SaveDataToLocal();
        }

        #endregion

        #region CHARACTER

        public int GetSelectedCharacter()
        {
            return m_SelectedCharacter;
        }

        public void SetSelectedCharacter(int _id)
        {
            m_SelectedCharacter = _id;
            SaveDataToLocal();
            // UnlockCharacter((CharacterType)_id);
            // SetSelectedCharacter(CharacterType.BLUEBOY);
            // LoadCharacterData();

        }

        #endregion

        public void LoadSongData()
        {
            for (int i = 0; i < m_SongData.Count; i++)
            {
                SongProfile cpd = m_SongData[i];
                cpd.Load();
            }
        }

        public void LoadWeekData()
        {
            for (int i = 0; i < m_WeekData.Count; i++)
            {
                WeekProfile cpd = m_WeekData[i];
                cpd.Load();
            }
        }

        public SongProfile GetSongProfile(int _id)
        {
            for (int i = 0; i < m_SongData.Count; i++)
            {
                SongProfile cpd = m_SongData[i];
                if (cpd.m_Id == _id)
                {
                    return cpd;
                }
            }
            return null;
        }

        public List<SongProfile> GetSongProfile()
        {
            return m_SongData;
        }

        public void UnlockSong(int _id)
        {
            if (GetSongProfile(_id) == null)
            {
                SongProfile newSong = new SongProfile();
                newSong.Init(_id);
                newSong.Load();
                m_SongData.Add(newSong);
                SaveDataToLocal();
            }
        }

        public WeekProfile GetWeekProfile(int _id)
        {
            for (int i = 0; i < m_WeekData.Count; i++)
            {
                WeekProfile wp = m_WeekData[i];
                if (wp.m_Id == _id)
                {
                    return wp;
                }
            }
            return null;
        }

        public List<WeekProfile> GetWeekProfiles()
        {
            return m_WeekData;
        }

        public SongWeekProfile GetSongWeekProfile(int _id)
        {
            for (int i = 0; i < m_SongWeekData.Count; i++)
            {
                SongWeekProfile wp = m_SongWeekData[i];
                if (wp.m_Id == _id)
                {
                    return wp;
                }
            }
            return null;
        }

        public SongWeekProfile GetSongWeekByWeekProfile(int _id)
        {
            for (int i = 0; i < m_SongWeekData.Count; i++)
            {
                SongWeekProfile wp = m_SongWeekData[i];
                if (wp.m_Id == _id)
                {
                    return wp;
                }
            }
            return null;
        }

        public void UnlockWeek(int _id)
        {
            if (GetWeekProfile(_id) == null)
            {
                WeekProfile newWeek = new WeekProfile();
                newWeek.Init(_id);
                m_WeekData.Add(newWeek);
                Helper.DebugLog("UnlockWeek");

                List<WeekConfig> weekSongConfig = GameData.Instance.GetWeekSong(_id);

                for (int i = 0; i < weekSongConfig.Count; i++)
                {
                    SongWeekProfile songWeek = new SongWeekProfile();
                    songWeek.Init(weekSongConfig[i].m_Id, weekSongConfig[i].m_Name);
                    m_SongWeekData.Add(songWeek);
                }

                SaveDataToLocal();
            }
        }
    }
}