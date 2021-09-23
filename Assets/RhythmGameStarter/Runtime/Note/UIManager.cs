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

        private void Awake()
        {
            GUIManager.Instance.AddClickEvent2(btn_FreeplayMenu, OpenFreeplayMenu);
            GUIManager.Instance.AddClickEvent2(btn_FreeBackMainMenu, BackToMainMenu);
            GUIManager.Instance.AddClickEvent2(btn_StoryMenu, OpenStoryMenu);
            GUIManager.Instance.AddClickEvent2(btn_Vibration, SetVibration);
            GUIManager.Instance.AddClickEvent2(btn_Setting, OpenSettingPanel);
            // GUIManager.Instance.AddClickEvent2(btn_StoryPlay, PlayStory);

            m_SettingOpen = true;
            OpenSettingPanel();

            img_On.SetActive(PlayerPrefs.GetInt("Vibration") == 1);
            img_Off.SetActive(PlayerPrefs.GetInt("Vibration") == 0);

            tf_Knot.localPosition = (PlayerPrefs.GetInt("Vibration") == 1) ? tf_On.localPosition : tf_Off.localPosition;
        }

        public void OpenSettingPanel()
        {
            m_SettingOpen = !m_SettingOpen;
            if (m_SettingOpen)
            {
                rect_Setting.DOLocalMoveX(386f, 0.5f);
            }
            else if (!m_SettingOpen)
            {
                rect_Setting.DOLocalMoveX(543f, 0.5f);
            }
        }

        public void SetVibration()
        {
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
            g_WinPop.SetActive(_value);
        }
    }
}