using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

namespace RhythmGameStarter
{
    public class PopupPause : MonoBehaviour
    {
        public CanvasGroup m_CanvasGroup;
        public Button btn_Resume;
        public Button btn_Replay;
        public Button btn_Home;

        public Button btn_Vibration;

        public Transform tf_On;
        public Transform tf_Off;
        public Transform tf_Knot;

        public GameObject img_On;
        public GameObject img_Off;

        private void Awake()
        {
            GUIManager.Instance.AddClickEvent(btn_Resume, Resume);
            GUIManager.Instance.AddClickEvent(btn_Replay, Replay);
            GUIManager.Instance.AddClickEvent(btn_Home, Home);
            GUIManager.Instance.AddClickEvent(btn_Vibration, SetVibration);
        }

        private void OnEnable()
        {
            FadeIn();

            img_On.SetActive(PlayerPrefs.GetInt("Vibration") == 1);
            img_Off.SetActive(PlayerPrefs.GetInt("Vibration") == 0);

            tf_Knot.localPosition = (PlayerPrefs.GetInt("Vibration") == 1) ? tf_On.localPosition : tf_Off.localPosition;
        }

        public void Resume()
        {
            FadeOut();
            GameManager.Instance.ResumeSong();
        }

        public void Replay()
        {
            AdsManager.Instance.WatchInterstitial2();

            if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                // GameManager.Instance.m_DefaultSong++;
                // SongManager.Instance.defaultSong = GameManager.Instance.m_Songs[GameManager.Instance.m_DefaultSong - 1];
                int songID = GameManager.Instance.m_DefaultSong;
                SongConfig songs = GameData.Instance.GetSongConfig(songID);
                AnalysticsManager.LogReplayFreeplaySong(songs.m_Name);
            }

            FadeOut();
            StatsSystem.Instance.missed = 0;
            StatsSystem.Instance.combo = 0;
            StatsSystem.Instance.score = 0;
            GameManager.Instance.txt_Miss.text = "MISS:" + StatsSystem.Instance.missed.ToString();
            ComboSystem.Instance.UpdateComboDisplay();
            StatsSystem.Instance.UpdateScoreDisplay();
            GUIManager.Instance.SetBlockPopup(true);
            GameManager.Instance.ResetSong();
        }

        public void Home()
        {
            FadeOut();
            // GameManager.Instance.m_ReturnHome = true;
            Note.m_ReturnHome = true;
            // GUIManager.Instance.LoadPlayScene();
            GUIManager.Instance.ChangeToPlayScene(true);
            // StartCoroutine(DelayHome());
        }

        // IEnumerator DelayHome()
        // {
        //     GUIManager.Instance.SetLoadingPopup(true);
        //     int count = GameManager.Instance.m_NoteInGame.Count;
        //     // for (int i = 0; i < count; i++)
        //     // {
        //     //     GameManager.Instance.m_NoteInGame[i].gameObject.SetActive(false);
        //     //     GameManager.Instance.m_NoteInGame.Remove(GameManager.Instance.m_NoteInGame[i]);
        //     // }

        //     while (GameManager.Instance.m_NoteInGame.Count > 0)
        //     {
        //         GameManager.Instance.m_NoteInGame[0].gameObject.SetActive(false);
        //     }

        //     yield return new WaitUntil(() => GameManager.Instance.m_NoteInGame.Count == 0);
        //     // yield return null;

        //     GUIManager.Instance.LoadPlayScene();
        // }

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

        public virtual void FadeOut()
        {
            m_CanvasGroup.DOFade(0, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
            transform.DOScale(1.05f, 0.2f).SetEase(Ease.Flash).OnComplete(() => { gameObject.SetActive(false); }).SetUpdate(UpdateType.Late, true);
        }
        public void FadeIn()
        {
            if (m_CanvasGroup != null)
            {
                m_CanvasGroup.alpha = 0;
                transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                m_CanvasGroup.DOFade(1, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true);
                transform.DOScale(1, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
            }
        }
    }
}