using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace RhythmGameStarter
{
    public class PopupWin : MonoBehaviour
    {
        public CanvasGroup m_CanvasGroup;
        public Button btn_X3Claim;
        public Button btn_Claim;
        public Button btn_Mode;
        public Button btn_Home;
        public Button btn_NextZone;
        public BigNumber m_GoldWin;
        public BigNumber m_RewardPercent = new BigNumber(0);
        public TextMeshProUGUI txt_GoldClaim;
        public TextMeshProUGUI txt_X3GoldClaim;
        public TextMeshProUGUI txt_TotalGold;
        public TextMeshProUGUI txt_ScoreSong;
        public TextMeshProUGUI txt_Mode;

        public GameObject g_HighScore;
        public TextMeshProUGUI txt_Combo;
        public TextMeshProUGUI txt_Miss;

        private void Awake()
        {
            GUIManager.Instance.AddClickEvent(btn_X3Claim, X3Claim);
            GUIManager.Instance.AddClickEvent(btn_Claim, Claim);
            GUIManager.Instance.AddClickEvent(btn_Mode, Mode);
            GUIManager.Instance.AddClickEvent(btn_Home, Home);
            GUIManager.Instance.AddClickEvent(btn_NextZone, NextZone);
        }

        private void OnEnable()
        {
            FadeIn();

            btn_Mode.gameObject.SetActive(false);
            btn_Home.gameObject.SetActive(false);

            txt_Combo.text = "COMBO: " + StatsSystem.Instance.combo.ToString();
            // txt_Combo.text += System.Environment.NewLine;
            // txt_Combo.text += StatsSystem.Instance.combo.ToString();

            txt_Miss.text = "MISS: " + StatsSystem.Instance.missed.ToString(); ;
            // txt_Miss.text += System.Environment.NewLine;
            // txt_Miss.text += StatsSystem.Instance.missed.ToString();

            btn_Claim.gameObject.SetActive(false);
            btn_NextZone.gameObject.SetActive(false);

            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                txt_Mode.text = "STORY";
                if (GameManager.Instance.IsStoryWeekEnd())
                {
                    StartCoroutine(DelayNextZone());
                    btn_Claim.gameObject.SetActive(false);
                }
                else
                {
                    btn_NextZone.gameObject.SetActive(false);
                    StartCoroutine(DelayClaim());
                }
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                txt_Mode.text = "FREEPLAY";
                StartCoroutine(DelayClaim());
            }

            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                // WeekProfile weekProfile = ProfileManager.GetWeekProfiles(GameManager.Instance.m_WeekNo);
                // BigNumber totalReward = new BigNumber(0);
                // totalReward = weekProfile.CalReward((int)GameManager.Instance.m_StoryLevel);

                SongWeekProfile songWeekProfile = ProfileManager.GetSongWeekProfiles(GameManager.Instance.m_StorySongID);
                BigNumber totalReward = new BigNumber(0);
                totalReward = songWeekProfile.CalReward(GameManager.Instance.m_StoryLevel, StatsSystem.Instance.score);

                if (StatsSystem.Instance.score <= songWeekProfile.GetScore(GameManager.Instance.m_StoryLevel))
                {
                    g_HighScore.SetActive(false);
                }
                else
                {
                    g_HighScore.SetActive(true);
                }

                txt_ScoreSong.text = "SCORE: " + StatsSystem.Instance.score;
                txt_GoldClaim.text = totalReward.ToString();
                txt_X3GoldClaim.text = (totalReward * 3).ToString();
                txt_TotalGold.text = ProfileManager.GetGold();

                btn_X3Claim.gameObject.SetActive(totalReward > 0 ? true : false);
                txt_GoldClaim.gameObject.SetActive(totalReward > 0 ? true : false);

                return;
            }

            int knot = GameManager.Instance.m_knot;
            if (knot <= 5)
            {
                m_RewardPercent = 1;
            }
            else if (knot <= 10)
            {
                m_RewardPercent = 0.8;
            }
            else if (knot <= 15)
            {
                m_RewardPercent = 0.6;
            }
            else if (knot < 20)
            {
                m_RewardPercent = 0.4;
            }

            int songId = GameManager.Instance.m_DefaultSong;
            SongProfile song = ProfileManager.GetSongProfiles(songId);
            if (StatsSystem.Instance.score >= song.m_HighScore)
            {
                txt_ScoreSong.text = "SCORE: " + StatsSystem.Instance.score.ToString();
                g_HighScore.SetActive(true);
            }
            else
            {
                txt_ScoreSong.text = "SCORE: " + StatsSystem.Instance.score.ToString();
                g_HighScore.SetActive(false);
            }

            txt_GoldClaim.text = CalGoldWin().ToString();
            txt_X3GoldClaim.text = (CalGoldWin() * 3).ToString();

            txt_TotalGold.text = ProfileManager.GetGold();
        }

        public BigNumber CalGoldWin()
        {
            int songId = GameManager.Instance.m_DefaultSong;
            BigNumber goldClaim = new BigNumber(0);
            SongProfile song = ProfileManager.GetSongProfiles(songId);

            if (song.m_FinishFirst == 0)
            {
                goldClaim = GameData.Instance.GetSongConfig(songId).m_1stReward;
                goldClaim *= m_RewardPercent;
                Helper.DebugLog("1st win: " + goldClaim);
            }
            else
            {
                if (StatsSystem.Instance.score > song.m_HighScore)
                {
                    goldClaim = GameData.Instance.GetSongConfig(songId).m_BreakScoreReward;
                }
                else
                {
                    goldClaim = GameData.Instance.GetSongConfig(songId).m_ReplayReward;
                }
                goldClaim *= m_RewardPercent;
                Helper.DebugLog("2nd win: " + goldClaim);
            }

            return goldClaim;
        }

        public BigNumber CalGoldStory()
        {
            // WeekProfile weekProfile = ProfileManager.GetWeekProfiles(GameManager.Instance.m_WeekNo);

            // BigNumber totalReward = new BigNumber(0);

            // totalReward = weekProfile.CalReward((int)GameManager.Instance.m_StoryLevel);

            // if (weekProfile.m_RewardLevel < (int)GameManager.Instance.m_StoryLevel)
            // {
            //     weekProfile.SetRewardLevel((int)GameManager.Instance.m_StoryLevel);
            // }

            SongWeekProfile songWeekProfile = ProfileManager.GetSongWeekProfiles(GameManager.Instance.m_StorySongID);

            BigNumber totalReward = new BigNumber(0);

            totalReward = songWeekProfile.CalReward(GameManager.Instance.m_StoryLevel, StatsSystem.Instance.score);

            songWeekProfile.SetScoreByLevel(GameManager.Instance.m_StoryLevel, StatsSystem.Instance.score);

            // if (songWeekProfile.m_RewardLevel < (int)GameManager.Instance.m_StoryLevel)
            // {
            //     songWeekProfile.SetRewardLevel((int)GameManager.Instance.m_StoryLevel);
            // }

            return totalReward;
        }

        public void X3Claim()
        {
            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                ProfileManager.AddGold(CalGoldStory() * 3);
                SetWeekProfile();
                // GameManager.Instance.NextWeekSong();
                // txt_Mode.text = "Story";
                if (GameManager.Instance.IsStoryWeekEnd())
                {
                    if (ProfileManager.GetWeek() < 6)
                    {
                        ProfileManager.SetWeek(ProfileManager.GetWeek() + 1);
                    }
                    Mode();
                }
                else
                {
                    GameManager.Instance.NextWeekSong();
                    StatsSystem.Instance.score = 0;
                    StatsSystem.Instance.combo = 0;
                }

                FadeOut();
                return;
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                ProfileManager.AddGold(CalGoldWin() * 3);
                SetSongProfile();
            }

            btn_X3Claim.interactable = false;
            btn_Claim.interactable = false;
            txt_TotalGold.text = ProfileManager.GetGold();

            btn_X3Claim.gameObject.SetActive(false);
            btn_Claim.gameObject.SetActive(false);
            btn_Mode.gameObject.SetActive(true);
            btn_Home.gameObject.SetActive(true);

            StatsSystem.Instance.score = 0;
            StatsSystem.Instance.combo = 0;

            // FadeOut();
            // GameManager.Instance.Home();
        }

        public void Claim()
        {
            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                ProfileManager.AddGold(CalGoldStory());
                SetWeekProfile();
                GameManager.Instance.NextWeekSong();
                FadeOut();

                // StatsSystem.Instance.score = 0;
                // StatsSystem.Instance.combo = 0;
                // StatsSystem.Instance.missed = 0;

                // StatsSystem.Instance.UpdateScoreDisplay();
                // ComboSystem.Instance.UpdateComboDisplay();

                return;
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                ProfileManager.AddGold(CalGoldWin());
                SetSongProfile();
            }

            btn_X3Claim.interactable = false;
            btn_Claim.interactable = false;
            txt_TotalGold.text = ProfileManager.GetGold();

            btn_X3Claim.gameObject.SetActive(false);
            btn_Claim.gameObject.SetActive(false);
            btn_Mode.gameObject.SetActive(true);
            btn_Home.gameObject.SetActive(true);

            StatsSystem.Instance.score = 0;
            StatsSystem.Instance.combo = 0;

            // FadeOut();
            // GameManager.Instance.Home();
        }

        public void NextZone()
        {
            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                ProfileManager.AddGold(CalGoldStory());
                SetWeekProfile();
                btn_X3Claim.interactable = false;
                btn_Claim.interactable = false;
                txt_TotalGold.text = ProfileManager.GetGold();

                txt_Mode.text = "Story";
                if (GameManager.Instance.IsStoryWeekEnd())
                {
                    if (ProfileManager.GetWeek() < 6)
                    {
                        ProfileManager.SetWeek(ProfileManager.GetWeek() + 1);
                    }
                    Mode();
                }
                else
                {
                    GameManager.Instance.NextWeekSong();
                    StatsSystem.Instance.score = 0;
                    StatsSystem.Instance.combo = 0;
                }
            }
        }

        public void Mode()
        {
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

        public void Home()
        {
            FadeOut();
            Note.m_ReturnHome = true;
            GameManager.Instance.Home();
        }

        public void SetSongProfile()
        {
            int songId = GameManager.Instance.m_DefaultSong;
            SongProfile song = ProfileManager.GetSongProfiles(songId);

            if (song.m_FinishFirst == 0)
            {
                song.Set1stFinish();
            }

            if (StatsSystem.Instance.score >= song.m_HighScore)
            {
                song.SetHighScore(StatsSystem.Instance.score);
            }
        }

        public void SetWeekProfile()
        {
            WeekProfile weekProfile = ProfileManager.GetWeekProfiles(GameManager.Instance.m_WeekNo);

            if (StatsSystem.Instance.score >= weekProfile.m_HighScore)
            {
                Helper.DebugLog("Score = " + StatsSystem.Instance.score);
                Helper.DebugLog("Week Score = " + weekProfile.m_HighScore);
                weekProfile.SetHighScore(StatsSystem.Instance.score);
            }
        }

        IEnumerator DelayClaim()
        {
            yield return Yielders.Get(1.5f);
            btn_Claim.gameObject.SetActive(true);
        }

        IEnumerator DelayNextZone()
        {
            yield return Yielders.Get(1.5f);
            btn_NextZone.gameObject.SetActive(true);
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