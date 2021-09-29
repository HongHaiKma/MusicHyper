using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RhythmGameStarter
{
    public class PopupLose : MonoBehaviour
    {
        public CanvasGroup m_CanvasGroup;
        public Button btn_Replay;
        public Button btn_Home;

        private void Awake()
        {
            GUIManager.Instance.AddClickEvent(btn_Replay, Replay);
            GUIManager.Instance.AddClickEvent(btn_Home, Home);
        }

        private void OnEnable()
        {
            SoundManager.Instance.PlayBGM(1);

            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                AnalysticsManager.LogFailZoneX(GameManager.Instance.m_WeekNo);
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                int songID = GameManager.Instance.m_DefaultSong;
                SongConfig songs = GameData.Instance.GetSongConfig(songID);
                AnalysticsManager.LogWinFreeplaySong(songs.m_Name);
            }
            FadeIn();
        }

        public void Replay()
        {
            SoundManager.Instance.PauseBGM();

            FadeOut();
            StatsSystem.Instance.missed = 0;
            StatsSystem.Instance.combo = 0;
            StatsSystem.Instance.score = 0;
            StatsSystem.Instance.UpdateScoreDisplay();
            GUIManager.Instance.SetBlockPopup(GameManager.Instance.m_ModePlay == ModePlay.STORY ? false : true);

            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                List<WeekConfig> weekConfigs = GameData.Instance.GetWeekSong(ProfileManager.GetWeek());
                GameManager.Instance.m_StorysongNo = 0;
                GameManager.Instance.m_StorySongID = weekConfigs[0].m_Id;
                SongManager.Instance.defaultSong = GameManager.Instance.m_WeekSongs[GameManager.Instance.m_StorysongNo];

                AnalysticsManager.LogReplayZone(GameManager.Instance.m_WeekNo);
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                int songID = GameManager.Instance.m_DefaultSong;
                SongConfig songs = GameData.Instance.GetSongConfig(songID);
                AnalysticsManager.LogReplayFreeplaySong(songs.m_Name);
            }

            GameManager.Instance.ResetSong();
        }

        public void Home()
        {
            AdsManager.Instance.WatchInterstitial();

            FadeOut();
            Note.m_ReturnHome = true;
            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                GUIManager.Instance.LoadPlayScene(() => UIManager.Instance.OpenStoryMenu());
                // UIManager.Instance.OpenStoryMenu();
                // GameManager.Instance.StopSong();
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                GUIManager.Instance.LoadPlayScene(() => UIManager.Instance.OpenFreeplayMenu());
                // UIManager.Instance.OpenFreeplayMenu();
                // GameManager.Instance.StopSong();
            }
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