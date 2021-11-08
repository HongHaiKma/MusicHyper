using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace RhythmGameStarter
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject g_LoseContinuePop;
        public GameObject g_PausePop;
        public GameObject g_LosePop;
        public GameObject g_WinPop;

        public GameObject g_RatePop;

        public GameObject g_MainMenu;
        public GameObject g_FreeplayMenu;
        public GameObject g_StoryMenu;

        public GameObject g_DialoguePop;

        public Button btn_FreeplayMenu;
        public Button btn_StoryMenu;
        public Button btn_FreeBackMainMenu;

        public Button btn_StoryPlay;

        public Animator anim_UI;


        public Button btn_Vibration;

        public Transform tf_On;
        public Transform tf_Off;
        public Transform tf_Knot;

        public GameObject img_On;
        public GameObject img_Off;


        public RectTransform rect_Setting;
        public Button btn_Setting;
        public bool m_SettingOpen;
        public Transform tf_TriIcon;

        public Button btn_Policy;
        public Button btn_Mail;
        public Button btn_RemoveAds;

        public GameObject g_FreeplayScroller;

        private void Awake()
        {
            GUIManager.Instance.AddClickEvent2(btn_FreeplayMenu, OpenFreeplayMenu);
            GUIManager.Instance.AddClickEvent(btn_FreeBackMainMenu, BackToMainMenu);
            GUIManager.Instance.AddClickEvent2(btn_StoryMenu, OpenStoryMenu);
            GUIManager.Instance.AddClickEvent(btn_Vibration, SetVibration);
            GUIManager.Instance.AddClickEvent(btn_Setting, OpenSettingPanel);

            GUIManager.Instance.AddClickEvent(btn_Policy, () =>
            {
                Application.OpenURL("https://bit.ly/2xy7eCk");
                SoundManager.Instance.PlayButtonClickArrow();
            });

            GUIManager.Instance.AddClickEvent(btn_Mail, OpenMail);

            if (ProfileManager.CheckAds())
            {
                GUIManager.Instance.AddClickEvent(btn_RemoveAds, () =>
                {
                    Helper.DebugLog("BBBBBBBBBBBBBB");
                    EventManager.CallEvent(GameEvent.REMOVE_ADS);
                    SoundManager.Instance.PlayButtonClickArrow();
                });
            }
            else
            {
                btn_RemoveAds.gameObject.SetActive(false);
            }
            // GUIManager.Instance.AddClickEvent2(btn_StoryPlay, PlayStory);

            m_SettingOpen = true;
            OpenSettingPanel();

            img_On.SetActive(PlayerPrefs.GetInt("Vibration") == 1);
            img_Off.SetActive(PlayerPrefs.GetInt("Vibration") == 0);

            tf_Knot.localPosition = (PlayerPrefs.GetInt("Vibration") == 1) ? tf_On.localPosition : tf_Off.localPosition;
        }

        public override void OnEnable()
        {
            TutorialManager.Instance.CheckTut(TutorialManager.m_1stClickFreeplay, () => btn_FreeplayMenu.transform.parent = TutorialManager.Instance.go_TutPop.transform);
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.A))
        //     {
        //         TutorialManager.Instance.CheckTut(TutorialManager.m_1stClickFreeplay);
        //     }
        // }

        public void OpenMail()
        {
            //         string email = "skysoftone2018@gmail.com";
            // #if UNITY_EDITOR || UNITY_ANDROID
            //         string subject = MyEscapeURL("Feedback Stickman Warriors-Version " + Application.version);
            // #else
            //         string subject = MyEscapeURL("IOS_Feedback Stickman Warriors-Version " + Application.version);
            // #endif
            //         string body = MyEscapeURL("Please tell us what we can improve in the game.");


            //         Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
            SoundManager.Instance.PlayButtonClickArrow();
            string email = "danghoa28051995@gmail.com";

            string subject = MyEscapeURL("Feedback Hide and Seek 3D: Monster Escape v" + Application.version);

            string body = MyEscapeURL("Please tell us what we can improve in the game.");


            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }

        string MyEscapeURL(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }

        public void OpenSettingPanel()
        {
            SoundManager.Instance.PlayButtonClickArrow();

            m_SettingOpen = !m_SettingOpen;
            if (m_SettingOpen)
            {
                tf_TriIcon.DOLocalRotate(new Vector3(0f, -180f, 0f), 0.5f);
                rect_Setting.DOLocalMoveX(386f, 0.5f);
            }
            else if (!m_SettingOpen)
            {
                tf_TriIcon.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.5f);
                rect_Setting.DOLocalMoveX(543f, 0.5f);
            }
        }

        public void SetVibration()
        {
            SoundManager.Instance.PlayButtonClickArrow();

            int value = (PlayerPrefs.GetInt("Vibration") == 1) ? 0 : 1;
            PlayerPrefs.SetInt("Vibration", value);

            tf_Knot.DOLocalMove((PlayerPrefs.GetInt("Vibration") == 1) ? tf_On.localPosition : tf_Off.localPosition, 0.3f).OnStart(
                () =>
                {
                    img_On.SetActive(PlayerPrefs.GetInt("Vibration") == 1);
                    img_Off.SetActive(PlayerPrefs.GetInt("Vibration") == 0);
                }
            );
        }

        public void OpenFreeplayMenu()
        {
            TutorialManager.Instance.SetTut(TutorialManager.m_1stClickFreeplay, () => btn_FreeplayMenu.transform.parent = g_MainMenu.transform);
            GameManager.Instance.m_ModePlay = ModePlay.FREEPLAY;
            g_MainMenu.SetActive(false);
            g_FreeplayMenu.SetActive(true);
            g_StoryMenu.SetActive(false);
            // UIManager.Instance.anim_UI.SetTrigger("FreePlay");
        }

        public void OpenStoryMenu()
        {
            GameManager.Instance.m_ModePlay = ModePlay.STORY;
            g_MainMenu.SetActive(false);
            g_FreeplayMenu.SetActive(false);
            g_StoryMenu.SetActive(true);
        }

        public void BackToMainMenu()
        {
            SoundManager.Instance.PlayButtonClickArrow();
            g_MainMenu.SetActive(true);
            g_FreeplayMenu.SetActive(false);
            g_StoryMenu.SetActive(false);
        }

        public void OpenRatePopup()
        {
            g_RatePop.SetActive(true);
        }

        public void CloseRatePopup()
        {
            g_RatePop.SetActive(false);
        }

        // public void PlayStory()
        // {
        //     g_StoryMenu.SetActive(false);
        //     int week = ProfileManager.GetWeek();
        //     List<WeekConfig> weekConfigs = GameData.Instance.GetWeekSong(week);
        //     GameManager.Instance.m_StorysongNo = 0;
        //     GameManager.Instance.m_WeekConfigs = weekConfigs;

        //     int count = weekConfigs.Count;
        //     for (int i = 0; i < count; i++)
        //     {
        //         GameManager.Instance.m_WeekSongs.Add(GameManager.Instance.m_Songs[weekConfigs[i].m_Id - 1]);
        //     }

        //     GameManager.Instance.PlaySongStory();
        // }

        public void OpenDialoguePopup(bool _1stSong = false)
        {
            g_DialoguePop.SetActive(true);
            GameManager.Instance.m_1stSong = _1stSong;
        }

        public void CloseDialoguePopup()
        {
            g_DialoguePop.SetActive(false);
        }

        public void OpenLoseContinuePopup(bool _value)
        {
            g_LoseContinuePop.SetActive(_value);
        }

        public void OpenPausePopup(bool _value)
        {
            g_PausePop.SetActive(_value);
        }

        public void OpenLosePopup(bool _value)
        {
            g_LosePop.SetActive(_value);
        }

        public void OpenWinPopup(bool _value)
        {
            // ProfileManager.MyProfile.m_OpenRateUs++;
            // ProfileManager.Instance.SaveData();

            // // if (ProfileManager.MyProfile.m_OpenRateUs > 4 && ProfileManager.MyProfile.m_RateUs == 0)
            // // {
            // //     UIManager.Instance.OpenRatePopup();
            // //     ProfileManager.MyProfile.m_OpenRateUs = 0;
            // //     ProfileManager.Instance.SaveData();
            // // }

            // g_WinPop.SetActive(_value);

            if (_value)
            {
                // GameManager.Instance.ContinueNextFreeSong();
                // EventManager1<int>.CallEvent(GameEvent.RATE, m_StarNo)
                EventManager1<InterType>.CallEvent(GameEvent.WATCH_INTER, InterType.FREE);
                // EventManager1<>
            }
            else
            {
                // GameManager.Instance.ContinueNextStorySong();

                if (GameManager.Instance.IsStoryWeekEnd())
                {
                    // if (ProfileManager.GetWeek() < 8)
                    // {
                    //     ProfileManager.SetWeek(ProfileManager.GetWeek() + 1);
                    // }
                    // AnalysticsManager.LogWinZoneX(GameManager.Instance.m_WeekNo);

                    // g_WinPop.SetActive(true);

                    // Helper.DebugLog("OEPN WINnnnnnnnnnnnnnnnnnnnnnn");
                    EventManager1<InterType>.CallEvent(GameEvent.WATCH_INTER, InterType.END_STORY);
                }
                else
                {
                    // StatsSystem.Instance.score = 0;
                    // StatsSystem.Instance.combo = 0;
                    // StatsSystem.Instance.missed = 0;
                    // StatsSystem.Instance.UpdateScoreDisplay();
                    // ComboSystem.Instance.UpdateComboDisplay();
                    // GameManager.Instance.txt_Miss.text = "MISS:" + StatsSystem.Instance.missed.ToString();

                    // GameManager.Instance.NextWeekSong();

                    EventManager1<InterType>.CallEvent(GameEvent.WATCH_INTER, InterType.STORY);
                }
            }

            // EventManager.CallEvent(GameEvent.WATCH_INTER);
        }
    }
}

public enum InterType
{
    NONE,
    END_STORY,
    STORY,
    FREE
}