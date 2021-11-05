using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
// using UnityEngine.Pool;

namespace RhythmGameStarter
{
    public class PopupStory : MonoBehaviour
    {
        public int m_Level;
        public int m_Week;

        ObjectsPool pool;
        // public IObject

        // public TextMeshProUGUI txt_Level;
        public TextMeshProUGUI txt_Week;
        public TextMeshProUGUI txt_SongName;
        public TextMeshProUGUI txt_HighScore;
        public TextMeshProUGUI txt_Price;
        public TextMeshProUGUI txt_OutPrice;
        public TextMeshProUGUI txt_TotalGold;
        public TextMeshProUGUI txt_BossName;

        public Button btn_LevelLeft;
        public Button btn_LevelRight;

        public Button btn_WeekUp;
        public Button btn_WeekDown;

        public Button btn_Back;

        public Button btn_Play;
        public Button btn_BuyWeek;

        public BigNumber m_PriceWeek;

        public Button btn_Easy;
        public Button btn_Normal;
        public Button btn_Hard;

        public GameObject g_EasyOn;
        public GameObject g_EasyOff;
        public GameObject g_NormalOn;
        public GameObject g_NormalOff;
        public GameObject g_HardOn;
        public GameObject g_HardOff;

        public GameObject g_Unlock;
        public GameObject g_OutOfGold;
        public GameObject g_UnlockBoss;

        public Image img_EnemyAva;

        public Image img_BGColor;
        public Color[] m_BGColor;

        public void Awake()
        {
            GUIManager.Instance.AddClickEvent(btn_LevelLeft, SetLevelLeft);
            GUIManager.Instance.AddClickEvent(btn_LevelRight, SetLevelRight);
            GUIManager.Instance.AddClickEvent(btn_WeekUp, SetWeekUp);
            GUIManager.Instance.AddClickEvent(btn_WeekDown, SetWeekDown);
            GUIManager.Instance.AddClickEvent2(btn_Play, Play);
            GUIManager.Instance.AddClickEvent(btn_Back, UIManager.Instance.BackToMainMenu);
            GUIManager.Instance.AddClickEvent(btn_BuyWeek, BuyWeek);

            GUIManager.Instance.AddClickEvent(btn_Easy, ClickLevelEasy);
            GUIManager.Instance.AddClickEvent(btn_Normal, ClickLevelNormal);
            GUIManager.Instance.AddClickEvent(btn_Hard, ClickLevelHard);
        }

        private void OnEnable()
        {
            txt_TotalGold.text = ProfileManager.GetGold();
            // m_Level = 0;
            m_Week = ProfileManager.GetWeek();
            ClickLevelEasy();
            SetLevel();
            SetWeek();

            img_BGColor.color = m_BGColor[(m_Week - 1) % 8];

            EventManager.AddListener(GameEvent.UNLOCK_WEEK, UnlockWeek);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(GameEvent.UNLOCK_WEEK, UnlockWeek);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(GameEvent.UNLOCK_WEEK, UnlockWeek);
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.V))
        //     {
        //         SetWeek();
        //     }
        // }

        public void SetLevelLeft()
        {
            m_Level--;
            if (m_Level < 0)
            {
                m_Level = 2;
            }
            SetLevel();
        }

        public void SetLevelRight()
        {
            m_Level++;
            if (m_Level > 2)
            {
                m_Level = 0;
            }
            SetLevel();
        }

        public void SetWeekUp()
        {
            SoundManager.Instance.PlayButtonClickArrow();
            m_Week--;
            ProfileManager.SetWeek(m_Week);
            if (m_Week < 1)
            {
                // m_Week = ProfileManager.GetWeek();
                m_Week = 11;
                ProfileManager.SetWeek(m_Week);
            }
            SetWeek();
        }

        public void SetWeekDown()
        {
            SoundManager.Instance.PlayButtonClickArrow();
            m_Week++;
            ProfileManager.SetWeek(m_Week);
            if (m_Week > 11)
            {
                m_Week = 1;
                ProfileManager.SetWeek(m_Week);
            }
            SetWeek();
        }

        public void SetWeek()
        {
            // Helper.DebugLog("Set Week");
            img_BGColor.DOColor(m_BGColor[(m_Week - 1) % 8], 0.35f);

            GameManager.Instance.m_WeekNo = m_Week;

            if (m_Week >= 9)
            {
                img_EnemyAva.sprite = SpriteManager.Instance.m_EnemyAvas[m_Week];
            }
            else
            {
                img_EnemyAva.sprite = SpriteManager.Instance.m_EnemyAvas[m_Week - 1];
            }

            GameManager.Instance.sr_BG.sprite = SpriteManager.Instance.m_BGInGame[m_Week - 1];

            img_EnemyAva.SetNativeSize();
            txt_Week.text = "week " + m_Week.ToString();
            List<WeekConfig> songConfigs = GameData.Instance.GetWeekSong(m_Week);
            txt_SongName.text = songConfigs[0].m_Name;
            // txt_SongName.text += System.Environment.NewLine;
            for (int i = 1; i < songConfigs.Count; i++)
            {
                txt_SongName.text += System.Environment.NewLine + songConfigs[i].m_Name;
            }

            txt_Price.text = songConfigs[0].m_Price.ToString3();
            txt_OutPrice.text = songConfigs[0].m_Price.ToString3();

            WeekProfile weekProfile = ProfileManager.GetWeekProfiles(m_Week);
            if (weekProfile != null)
            {
                btn_Play.gameObject.SetActive(true);
                btn_BuyWeek.gameObject.SetActive(false);

                int week = GameManager.Instance.m_WeekNo;
                List<WeekConfig> weeks = GameData.Instance.GetWeekSong(week);
                List<int> ids = new List<int>();
                for (int i = 0; i < weeks.Count; i++)
                {
                    ids.Add(weeks[i].m_Id);
                }
                BigNumber score = new BigNumber(0);
                for (int i = 0; i < ids.Count; i++)
                {
                    if (GameManager.Instance.m_StoryLevel == StoryLevel.EASY)
                    {
                        BigNumber songScore = new BigNumber(ProfileManager.GetSongWeekProfiles(ids[i]).m_EasyScore);
                        score += songScore;
                        // Helper.DebugLog("Easy score: " + songScore);
                    }
                    else if (GameManager.Instance.m_StoryLevel == StoryLevel.NORMAL)
                    {
                        BigNumber songScore = new BigNumber(ProfileManager.GetSongWeekProfiles(ids[i]).m_NormalScore);
                        score += songScore;
                        // Helper.DebugLog("Normal score: " + songScore);
                    }
                    else if (GameManager.Instance.m_StoryLevel == StoryLevel.HARD)
                    {
                        BigNumber songScore = new BigNumber(ProfileManager.GetSongWeekProfiles(ids[i]).m_HardScore);
                        score += songScore;
                        // Helper.DebugLog("Hard score: " + songScore);
                    }
                }

                txt_HighScore.text = "score: " + score.ToString3();
            }
            else
            {
                btn_Play.gameObject.SetActive(false);
                btn_BuyWeek.gameObject.SetActive(true);
                // txt_Price.text = songConfigs[0].m_Price.ToString3();
                m_PriceWeek = songConfigs[0].m_Price;
                txt_HighScore.text = "score: 0";
            }

            txt_BossName.text = songConfigs[0].m_EnemyName;

            WeekProfile weekProfil = ProfileManager.GetWeekProfiles(m_Week);
            if (weekProfil != null)
            {
                btn_BuyWeek.interactable = true;
                g_Unlock.SetActive(true);
                g_OutOfGold.SetActive(false);
                g_UnlockBoss.SetActive(false);

                // Helper.DebugLog("Enough");

                // ClickLevelEasy();

                if (GameManager.Instance.m_StoryLevel == StoryLevel.EASY)
                {
                    g_EasyOn.SetActive(true);
                    g_EasyOff.SetActive(false);
                    g_NormalOn.SetActive(false);
                    g_NormalOff.SetActive(true);
                    g_HardOn.SetActive(false);
                    g_HardOff.SetActive(true);
                }
                else if (GameManager.Instance.m_StoryLevel == StoryLevel.NORMAL)
                {
                    g_EasyOn.SetActive(false);
                    g_EasyOff.SetActive(true);
                    g_NormalOn.SetActive(true);
                    g_NormalOff.SetActive(false);
                    g_HardOn.SetActive(false);
                    g_HardOff.SetActive(true);
                }
                else if (GameManager.Instance.m_StoryLevel == StoryLevel.HARD)
                {
                    g_EasyOn.SetActive(false);
                    g_EasyOff.SetActive(true);
                    g_NormalOn.SetActive(false);
                    g_NormalOff.SetActive(true);
                    g_HardOn.SetActive(true);
                    g_HardOff.SetActive(false);
                }
            }
            else
            {
                if (ProfileManager.IsEnoughGold(m_PriceWeek))
                {
                    btn_BuyWeek.interactable = true;
                    g_Unlock.SetActive(true);
                    g_OutOfGold.SetActive(false);
                    g_UnlockBoss.SetActive(true);

                    // Helper.DebugLog("Enough");

                    // ClickLevelEasy();

                    if (GameManager.Instance.m_StoryLevel == StoryLevel.EASY)
                    {
                        g_EasyOn.SetActive(true);
                        g_EasyOff.SetActive(false);
                        g_NormalOn.SetActive(false);
                        g_NormalOff.SetActive(true);
                        g_HardOn.SetActive(false);
                        g_HardOff.SetActive(true);
                    }
                    else if (GameManager.Instance.m_StoryLevel == StoryLevel.NORMAL)
                    {
                        g_EasyOn.SetActive(false);
                        g_EasyOff.SetActive(true);
                        g_NormalOn.SetActive(true);
                        g_NormalOff.SetActive(false);
                        g_HardOn.SetActive(false);
                        g_HardOff.SetActive(true);
                    }
                    else if (GameManager.Instance.m_StoryLevel == StoryLevel.HARD)
                    {
                        g_EasyOn.SetActive(false);
                        g_EasyOff.SetActive(true);
                        g_NormalOn.SetActive(false);
                        g_NormalOff.SetActive(true);
                        g_HardOn.SetActive(true);
                        g_HardOff.SetActive(false);
                    }
                }
                else
                {
                    btn_BuyWeek.interactable = false;
                    g_Unlock.SetActive(false);
                    g_OutOfGold.SetActive(false);
                    g_UnlockBoss.SetActive(true);

                    // Helper.DebugLog("Not Enough");

                    g_EasyOn.SetActive(false);
                    g_EasyOff.SetActive(true);
                    g_NormalOn.SetActive(false);
                    g_NormalOff.SetActive(true);
                    g_HardOn.SetActive(false);
                    g_HardOff.SetActive(true);
                }
            }
        }

        public void BuyWeek()
        {
            // if (ProfileManager.IsEnoughGold(m_PriceWeek))
            // {
            //     SoundManager.Instance.PlayButtonClickConfirm();
            //     ProfileManager.ConsumeGold(m_PriceWeek);
            //     ProfileManager.UnlockWeek(m_Week);
            //     btn_Play.gameObject.SetActive(true);
            //     btn_BuyWeek.gameObject.SetActive(false);
            //     txt_TotalGold.text = ProfileManager.GetGold();
            //     GameManager.Instance.txt_TotalGold.text = ProfileManager.GetGold();
            //     g_UnlockBoss.SetActive(false);
            // }
            AdsManager.Instance.WatchRewardVideo(RewardType.UNLOCK_WEEK);
        }

        public void UnlockWeek()
        {
            AnalysticsManager.LogUnlockChallengeSong();

            SoundManager.Instance.PlayButtonClickConfirm();
            // ProfileManager.ConsumeGold(m_PriceWeek);
            ProfileManager.UnlockWeek(m_Week);
            btn_Play.gameObject.SetActive(true);
            btn_BuyWeek.gameObject.SetActive(false);
            txt_TotalGold.text = ProfileManager.GetGold();
            GameManager.Instance.txt_TotalGold.text = ProfileManager.GetGold();
            g_UnlockBoss.SetActive(false);
        }

        public void Play()
        {
            // if (Level)
            // {

            // }

            GameManager.Instance.m_RenderCam.cullingMask = GameManager.Instance.m_InGame;

            AnalysticsManager.LogPlayZoneX(GameManager.Instance.m_WeekNo);

            GameManager.Instance.txt_Time.gameObject.SetActive(false);

            UIManager.Instance.g_StoryMenu.SetActive(false);
            List<WeekConfig> weekConfigs = GameData.Instance.GetWeekSong(m_Week);
            GameManager.Instance.m_StorysongNo = 0;
            GameManager.Instance.m_StorySongID = weekConfigs[0].m_Id;
            GameManager.Instance.m_WeekConfigs = weekConfigs;

            int count = weekConfigs.Count;
            for (int i = 0; i < count; i++)
            {
                GameManager.Instance.m_WeekSongs.Add(GameManager.Instance.m_Songs[weekConfigs[i].m_Id - 1]);
            }

            SongConfig songs = GameData.Instance.GetSongConfig(GameManager.Instance.m_StorySongID);

            GameManager.Instance.txt_SongName.text = songs.m_Name;

            UIManager.Instance.OpenDialoguePopup(true);

            WeekConfig song = GameManager.Instance.m_WeekConfigs[GameManager.Instance.m_StorysongNo];
            if (GameManager.Instance.m_Enemy != null)
            {
                Destroy(GameManager.Instance.m_Enemy.gameObject);
            }

            if (GameManager.Instance.m_WeekNo != 1)
            {
                GameObject enemy = PrefabManager.Instance.SpawnEnemyPool(song.m_EnemyName, Vector3.zero);
                enemy.transform.parent = GameManager.Instance.tf_EnemyHolder;
                enemy.transform.localPosition = Vector3.zero;

                enemy.transform.localRotation = Quaternion.Euler(0f, -360f, 0f);

                enemy.transform.localScale = new Vector3(1f, 1f, 1f);
                GameManager.Instance.m_Enemy = enemy.GetComponent<Enemy>();
            }

            // GameManager.Instance.PlaySongStory();
        }

        public void SetLevel()
        {
            GameManager.Instance.m_StoryLevel = (StoryLevel)m_Level;
            if (GameManager.Instance.m_StoryLevel == StoryLevel.EASY)
            {
                TrackManager.Instance.beatSize = 1.5f;
            }
            else if (GameManager.Instance.m_StoryLevel == StoryLevel.NORMAL)
            {
                TrackManager.Instance.beatSize = 2.5f;
            }
            else if (GameManager.Instance.m_StoryLevel == StoryLevel.HARD)
            {
                TrackManager.Instance.beatSize = 4f;
            }
            // txt_Level.text = GameManager.Instance.m_StoryLevel.ToString();
        }

        public void ClickLevelEasy()
        {
            SoundManager.Instance.PlayButtonClickArrow();

            m_Level = 0;

            g_EasyOn.SetActive(true);
            g_EasyOff.SetActive(false);
            g_NormalOn.SetActive(false);
            g_NormalOff.SetActive(true);
            g_HardOn.SetActive(false);
            g_HardOff.SetActive(true);

            SetLevel();
            SetWeek();
        }

        public void ClickLevelNormal()
        {
            SoundManager.Instance.PlayButtonClickArrow();

            m_Level = 1;

            g_EasyOn.SetActive(false);
            g_EasyOff.SetActive(true);
            g_NormalOn.SetActive(true);
            g_NormalOff.SetActive(false);
            g_HardOn.SetActive(false);
            g_HardOff.SetActive(true);

            SetLevel();
            SetWeek();
        }

        public void ClickLevelHard()
        {
            SoundManager.Instance.PlayButtonClickArrow();

            m_Level = 2;

            g_EasyOn.SetActive(false);
            g_EasyOff.SetActive(true);
            g_NormalOn.SetActive(false);
            g_NormalOff.SetActive(true);
            g_HardOn.SetActive(true);
            g_HardOff.SetActive(false);

            SetLevel();
            SetWeek();
        }
    }
}