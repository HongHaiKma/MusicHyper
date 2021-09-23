using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGameStarter
{
    [DefaultExecutionOrder(-93)]
    public class ProfileManager : MonoBehaviour
    {
        private static ProfileManager m_Instance;
        public static ProfileManager Instance
        {
            get
            {
                return m_Instance;
            }
        }

        public static PlayerProfile MyProfile
        {
            get
            {
                return m_Instance.m_LocalProfile;
            }
        }
        private PlayerProfile m_LocalProfile;

        public BigNumber m_Gold;
        public BigNumber m_Gold2 = new BigNumber(0);

        private void Awake()
        {
            if (m_Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                m_Instance = this;
                InitProfile();
                DontDestroyOnLoad(gameObject);
            }

            // MyProfile.AddGold(5f);
        }

        private void Update()
        {
            if (m_LocalProfile != null)
            {
                m_LocalProfile.Update();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Helper.DebugLog("Next Add" + MyProfile.m_InterTime.GetTimeToNextAdd(1, ""));
            }
        }

        private void OnEnable()
        {
            StartListenToEvent();
        }

        private void OnDisable()
        {
            StopListenToEvent();
        }

        private void OnDestroy()
        {
            StopListenToEvent();
        }

        public void StartListenToEvent()
        {
            // EventManager.AddListener(GameEvent.END_GAME, PassLevel);
            // EventManagerWithParam<int>.AddListener(GameEvent.EQUIP_CHAR, EquipChar);
        }

        public void StopListenToEvent()
        {
            // EventManager.RemoveListener(GameEvent.END_GAME, PassLevel);
            // EventManagerWithParam<int>.RemoveListener(GameEvent.EQUIP_CHAR, EquipChar);
        }

        public void InitProfile()
        {
            CreateOrLoadLocalProfile();
        }

        private void CreateOrLoadLocalProfile()
        {
            Debug.Log("Create Or Load Data");
            LoadDataFromPref();
        }

        private void LoadDataFromPref()
        {
            Debug.Log("Load Data");
            string dataText = PlayerPrefs.GetString("SuperFetch", "");
            //Debug.Log("Data " + dataText);
            if (string.IsNullOrEmpty(dataText))
            {
                // Dont have -> create new player and save;
                CreateNewPlayer();
            }
            else
            {
                // Have -> Load data
                LoadDataToPlayerProfile(dataText);
            }
        }

        private void CreateNewPlayer()
        {
            m_LocalProfile = new PlayerProfile();
            m_LocalProfile.CreateNewPlayer();
            SaveData();
        }

        private void LoadDataToPlayerProfile(string data)
        {
            m_LocalProfile = JsonMapper.ToObject<PlayerProfile>(data);
            m_LocalProfile.LoadLocalProfile();
            m_Gold = m_LocalProfile.GetGold();
        }

        public void SaveData()
        {
            m_LocalProfile.SaveDataToLocal();
        }

        public void SaveDataText(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                PlayerPrefs.SetString("SuperFetch", data);
            }
        }
        public void TestDisplayGold()
        {
            // string a = 
            // Helper.DebugLog("Profile Gold: " + MyProfile.GetGold());
            // Helper.DebugLog("Profile Level: " + MyProfile.m_Level);
        }

        public static int GetWeek()
        {
            return MyProfile.GetWeek();
        }

        public static void SetWeek(int _week)
        {
            MyProfile.SetWeek(_week);
        }

        public void PassLevel()
        {
            // int level = GetLevel();
            // Dictionary<int, LevelConfig> configs = GameData.Instance.GetLevelConfig();
            // int pipeCount = InGameObjectsManager.Instance.m_Char.m_SpringManager.springBones.Count - 1;

            // if (pipeCount == 0)
            // {
            //     pipeCount = 1;
            // }

            // bool claimGold = false;

            // for (int i = 1; i <= configs.Count; i++)
            // {
            //     if (configs[i].CheckInRange(level))
            //     {
            //         BigNumber totalGold = (configs[i].m_MinGold + (1 + (level - 1) * 0.5f) * 1f) * pipeCount;
            //         GameManager.Instance.m_GoldWin = totalGold;
            //         AddGold(totalGold / 2);
            //         claimGold = true;
            //         break;
            //     }
            //     else
            //     {
            //         BigNumber totalGold = (configs[i].m_MinGold + (1 + (level - 1) * 0.5f) * 1f) * pipeCount;
            //         GameManager.Instance.m_GoldWin = totalGold;
            //     }
            // }

            // if (!claimGold)
            // {
            //     AddGold(GameManager.Instance.m_GoldWin / 2);
            // }

            MyProfile.PassLevel();
        }

        public void PassOnlyLevel()
        {
            MyProfile.PassLevel();
        }

        public static int GetLevel()
        {
            return MyProfile.GetLevel();
        }

        public string GetLevel2()
        {
            return MyProfile.GetLevel().ToString();
        }

        public void SetLevel(int _level)
        {
            MyProfile.SetLevel(_level);
        }

        #region GENERAL
        public static string GetGold()
        {
            return MyProfile.GetGold().ToString3();
            // return MyProfile.GetGold().ToInt();
            // return MyProfile.GetGold().ToString3();
            // return MyProfile.GetGold().ToCharacterFormat();
            // return MyProfile.GetGold().RoundToInt().ToCharacterFormat();
            // return MyProfile.GetGold().Normalize().ToCharacterFormat();
            // return MyProfile.GetGold().ToString3();
        }

        public static BigNumber GetGold2()
        {
            return MyProfile.GetGold();
        }

        public static void AddGold(BigNumber _gold)
        {
            MyProfile.AddGold(_gold);
        }

        public static void SetGold(BigNumber _gold)
        {
            MyProfile.SetGold(_gold);
        }

        public static void ConsumeGold(BigNumber _gold)
        {
            MyProfile.ConsumeGold(_gold);
        }

        public static bool IsEnoughGold(BigNumber _gold)
        {
            return MyProfile.IsEnoughGold(_gold);
        }

        #endregion

        #region CHARACTER

        public static int GetSelectedCharacter()
        {
            return MyProfile.GetSelectedCharacter();
        }

        public static void SetSelectedCharacter(int _id)
        {
            MyProfile.SetSelectedCharacter(_id);
        }

        public void EquipChar(int _id)
        {
            MyProfile.SetSelectedCharacter(_id);
        }

        #endregion


        #region KEYS

        public static void AddKeys(BigNumber _value)
        {
            MyProfile.AddKeys(_value);
        }

        public static BigNumber GetKeys()
        {
            return MyProfile.GetKeys();
        }

        public static void SetKeys(BigNumber _keys)
        {
            MyProfile.SetKeys(_keys);
        }

        #endregion

        public static int GetSelectedChar()
        {
            return MyProfile.m_SelectedCharacter;
        }

        public static bool CheckAds()
        {
            return MyProfile.CheckAds();
        }

        public static void SetAds(int _value)
        {
            MyProfile.SetAds(_value);
        }

        public void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveData();
            }
        }
        public void OnApplicationQuit()
        {
            SaveData();
        }

        public static void UnlockSong(int _id)
        {
            MyProfile.UnlockSong(_id);
        }

        public static List<SongProfile> GetSongProfiles()
        {
            return MyProfile.GetSongProfile();
        }

        public static SongProfile GetSongProfiles(int _id)
        {
            return MyProfile.GetSongProfile(_id);
        }

        public static void UnlockWeek(int _id)
        {
            MyProfile.UnlockWeek(_id);
        }

        public static WeekProfile GetWeekProfiles(int _id)
        {
            return MyProfile.GetWeekProfile(_id);
        }

        public static SongWeekProfile GetSongWeekProfiles(int _id)
        {
            return MyProfile.GetSongWeekProfile(_id);
        }

        public static List<WeekProfile> GetWeekProfiles()
        {
            return MyProfile.GetWeekProfiles();
        }
    }
}