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

        public TextMeshProUGUI txt_StartingIn;
        public float m_Time;
        public bool m_NextZone;

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
            m_Time = 3f;
            txt_StartingIn.text = "NEXT ZONE IN 3";
            btn_NextZone.interactable = true;
            btn_NextZone.gameObject.SetActive(true);
            m_NextZone = false;

            btn_X3Claim.gameObject.SetActive(false);
            btn_Mode.gameObject.SetActive(false);
            btn_Home.gameObject.SetActive(false);
            btn_Claim.gameObject.SetActive(false);

            Helper.DebugLog("PopuWin Enableeeeeeeeeeeeeeeeee");

            SoundManager.Instance.PlayBGM(0);

            FadeIn();

            AddListener();

            // btn_Mode.gameObject.SetActive(false);
            // btn_Home.gameObject.SetActive(false);

            txt_Combo.text = "COMBO: " + StatsSystem.Instance.maxCombo.ToString();
            // txt_Combo.text += System.Environment.NewLine;
            // txt_Combo.text += StatsSystem.Instance.combo.ToString();

            txt_Miss.text = "MISS: " + StatsSystem.Instance.missed.ToString(); ;
            // txt_Miss.text += System.Environment.NewLine;
            // txt_Miss.text += StatsSystem.Instance.missed.ToString();

            // btn_Claim.gameObject.SetActive(false);
            // btn_NextZone.gameObject.SetActive(false);

            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                txt_Mode.text = "STORY";
                if (GameManager.Instance.IsStoryWeekEnd())
                {
                    StartCoroutine(DelayNextZone());
                    // btn_Claim.gameObject.SetActive(false);
                }
                else
                {
                    // btn_NextZone.gameObject.SetActive(false);
                    StartCoroutine(DelayClaim());
                }
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                int songID = GameManager.Instance.m_DefaultSong;
                SongConfig songs = GameData.Instance.GetSongConfig(songID);
                AnalysticsManager.LogWinFreeplaySong(songs.m_Name);

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
                // txt_GoldClaim.text = totalReward.ToString();
                txt_X3GoldClaim.text = (totalReward * 3).ToString();
                txt_TotalGold.text = ProfileManager.GetGold();

                // btn_X3Claim.gameObject.SetActive(totalReward > 0 ? true : false);
                // btn_X3Claim.gameObject.SetActive(false);
                txt_GoldClaim.gameObject.SetActive(false);

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

            // txt_GoldClaim.text = CalGoldWin().ToString();
            txt_X3GoldClaim.text = (CalGoldWin() * 3).ToString();

            txt_TotalGold.text = ProfileManager.GetGold();
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void OnDestroy()
        {
            RemoveListener();
        }

        private void Update()
        {
            m_Time -= Time.deltaTime;
            txt_StartingIn.text = "NEXT ZONE IN " + (int)m_Time;

            if (btn_NextZone.interactable)
            {
                if (m_Time <= 0.5f)
                {
                    btn_NextZone.interactable = false;
                }
            }

            if (m_Time <= 0f)
            {
                m_Time = 0f;
                txt_StartingIn.text = "NEXT ZONE IN 0";
                if (!m_NextZone)
                {
                    FadeOut();
                    NextZone();
                    m_NextZone = true;
                }
            }
        }

        public void AddListener()
        {
            Helper.DebugLog("Win Popup AddListener");
            EventManager.AddListener(GameEvent.X3_CLAIM, X3ClaimLogic);
        }

        public void RemoveListener()
        {
            Helper.DebugLog("Win Popup RemoveListener");
            EventManager.RemoveListener(GameEvent.X3_CLAIM, X3ClaimLogic);
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
            // if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            // {
            //     ProfileManager.AddGold(CalGoldStory() * 3);
            //     SetWeekProfile();
            //     // GameManager.Instance.NextWeekSong();
            //     // txt_Mode.text = "Story";
            //     if (GameManager.Instance.IsStoryWeekEnd())
            //     {
            //         if (ProfileManager.GetWeek() < 6)
            //         {
            //             ProfileManager.SetWeek(ProfileManager.GetWeek() + 1);
            //         }
            //         Mode();
            //     }
            //     else
            //     {
            //         GameManager.Instance.NextWeekSong();
            //         StatsSystem.Instance.score = 0;
            //         StatsSystem.Instance.combo = 0;
            //     }

            //     FadeOut();
            //     return;
            // }
            // else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            // {
            //     ProfileManager.AddGold(CalGoldWin() * 3);
            //     SetSongProfile();
            // }

            // btn_X3Claim.interactable = false;
            // btn_Claim.interactable = false;
            // txt_TotalGold.text = ProfileManager.GetGold();

            // btn_X3Claim.gameObject.SetActive(false);
            // btn_Claim.gameObject.SetActive(false);
            // btn_Mode.gameObject.SetActive(true);
            // btn_Home.gameObject.SetActive(true);

            // StatsSystem.Instance.score = 0;
            // StatsSystem.Instance.combo = 0;

            // FadeOut();
            // GameManager.Instance.Home();

            // X3ClaimLogic();

            AdsManager.Instance.WatchRewardVideo(RewardType.X3_CLAIM);
        }

        public void X3ClaimLogic()
        {
            // Helper.DebugLog("X3ClaimLogic");
            // AnalysticsManager.LogX3Claim();
            // if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            // {
            //     ProfileManager.AddGold(CalGoldStory() * 3);
            //     SetWeekProfile();
            //     Helper.DebugLog("X3 claim logic Story");
            //     // GameManager.Instance.NextWeekSong();
            //     // txt_Mode.text = "Story";
            //     if (GameManager.Instance.IsStoryWeekEnd())
            //     {
            //         if (ProfileManager.GetWeek() < 8)
            //         {
            //             ProfileManager.SetWeek(ProfileManager.GetWeek() + 1);
            //         }
            //         AnalysticsManager.LogWinZoneX(GameManager.Instance.m_WeekNo);
            //         Mode();
            //     }
            //     else
            //     {
            //         StatsSystem.Instance.score = 0;
            //         StatsSystem.Instance.combo = 0;
            //         StatsSystem.Instance.missed = 0;
            //         StatsSystem.Instance.UpdateScoreDisplay();
            //         ComboSystem.Instance.UpdateComboDisplay();
            //         GameManager.Instance.txt_Miss.text = "MISS:" + StatsSystem.Instance.missed.ToString();

            //         GameManager.Instance.NextWeekSong();
            //     }

            //     FadeOut();
            //     return;
            // }
            // else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            // {
            //     ProfileManager.AddGold(CalGoldWin() * 3);
            //     SetSongProfile();
            // }

            ProfileManager.UnlockWeek(ProfileManager.GetWeek());

            GameManager.Instance.m_RenderCam.cullingMask = GameManager.Instance.m_InGame;

            AnalysticsManager.LogPlayZoneX(GameManager.Instance.m_WeekNo);

            GameManager.Instance.txt_Time.gameObject.SetActive(false);

            UIManager.Instance.g_StoryMenu.SetActive(false);
            List<WeekConfig> weekConfigs = GameData.Instance.GetWeekSong(ProfileManager.GetWeek());
            GameManager.Instance.m_StorysongNo = 0;
            GameManager.Instance.m_StorySongID = weekConfigs[0].m_Id;
            GameManager.Instance.m_WeekConfigs = weekConfigs;

            int count = weekConfigs.Count;
            for (int i = 0; i < count; i++)
            {
                GameManager.Instance.m_WeekSongs.Add(GameManager.Instance.m_Songs[weekConfigs[i].m_Id - 1]);
            }

            UIManager.Instance.OpenDialoguePopup(true);

            // btn_X3Claim.interactable = false;
            // btn_Claim.interactable = false;
            // txt_TotalGold.text = ProfileManager.GetGold();

            // btn_X3Claim.gameObject.SetActive(false);
            // btn_Claim.gameObject.SetActive(false);
            // btn_Mode.gameObject.SetActive(false);
            // btn_Home.gameObject.SetActive(true);
            btn_NextZone.gameObject.SetActive(true);

            StatsSystem.Instance.score = 0;
            StatsSystem.Instance.combo = 0;

            // FadeOut();
            // GameManager.Instance.Home();
        }

        public void Claim()
        {
            // if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            // {
            //     ProfileManager.AddGold(CalGoldStory());
            //     SetWeekProfile();
            //     GameManager.Instance.NextWeekSong();
            //     FadeOut();

            //     // StatsSystem.Instance.score = 0;
            //     // StatsSystem.Instance.combo = 0;
            //     // StatsSystem.Instance.missed = 0;

            //     // StatsSystem.Instance.UpdateScoreDisplay();
            //     // ComboSystem.Instance.UpdateComboDisplay();

            //     StatsSystem.Instance.score = 0;
            //     StatsSystem.Instance.combo = 0;
            //     StatsSystem.Instance.missed = 0;
            //     StatsSystem.Instance.UpdateScoreDisplay();
            //     ComboSystem.Instance.UpdateComboDisplay();
            //     GameManager.Instance.txt_Miss.text = "MISS:" + StatsSystem.Instance.missed.ToString();

            //     return;
            // }
            // else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            // {
            //     ProfileManager.AddGold(CalGoldWin());
            //     SetSongProfile();

            //     AdsManager.Instance.WatchInterstitial();
            // }

            ProfileManager.UnlockWeek(ProfileManager.GetWeek());

            GameManager.Instance.m_RenderCam.cullingMask = GameManager.Instance.m_InGame;

            AnalysticsManager.LogPlayZoneX(GameManager.Instance.m_WeekNo);

            GameManager.Instance.txt_Time.gameObject.SetActive(false);

            UIManager.Instance.g_StoryMenu.SetActive(false);
            List<WeekConfig> weekConfigs = GameData.Instance.GetWeekSong(ProfileManager.GetWeek());
            GameManager.Instance.m_StorysongNo = 0;
            GameManager.Instance.m_StorySongID = weekConfigs[0].m_Id;
            GameManager.Instance.m_WeekConfigs = weekConfigs;

            int count = weekConfigs.Count;
            for (int i = 0; i < count; i++)
            {
                GameManager.Instance.m_WeekSongs.Add(GameManager.Instance.m_Songs[weekConfigs[i].m_Id - 1]);
            }

            UIManager.Instance.OpenDialoguePopup(true);

            // btn_X3Claim.interactable = false;
            // btn_Claim.interactable = false;
            txt_TotalGold.text = ProfileManager.GetGold();

            // btn_X3Claim.gameObject.SetActive(false);
            // btn_Claim.gameObject.SetActive(false);
            // btn_Mode.gameObject.SetActive(false);
            // btn_Home.gameObject.SetActive(false);
            btn_NextZone.gameObject.SetActive(true);

            StatsSystem.Instance.score = 0;
            StatsSystem.Instance.combo = 0;

            // FadeOut();
            // GameManager.Instance.Home();
        }

        public void NextZone()
        {
            // if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            // {
            //     ProfileManager.AddGold(CalGoldStory());
            //     SetWeekProfile();
            //     btn_X3Claim.interactable = false;
            //     btn_Claim.interactable = false;
            //     txt_TotalGold.text = ProfileManager.GetGold();

            //     txt_Mode.text = "STORY";
            //     if (GameManager.Instance.IsStoryWeekEnd())
            //     {
            //         if (ProfileManager.GetWeek() < 8)
            //         {
            //             ProfileManager.SetWeek(ProfileManager.GetWeek() + 1);
            //         }
            //         Mode();
            //     }
            //     else
            //     {
            //         GameManager.Instance.NextWeekSong();
            //         StatsSystem.Instance.score = 0;
            //         StatsSystem.Instance.combo = 0;
            //     }
            // }

            // WeekProfile weekProfile = ProfileManager.GetWeekProfiles(GameManager.Instance.m_WeekNo);

            // if (StatsSystem.Instance.score >= weekProfile.m_HighScore)
            // {
            //     weekProfile.SetHighScore(StatsSystem.Instance.score);
            // }

            SongWeekProfile songWeekProfile = ProfileManager.GetSongWeekProfiles(GameManager.Instance.m_StorySongID);

            songWeekProfile.SetScoreByLevel(GameManager.Instance.m_StoryLevel, StatsSystem.Instance.score);

            if (ProfileManager.GetWeek() >= 8)
            {
                bool foundWeek = false;
                for (int i = 1; i <= 8; i++)
                {
                    WeekProfile profile = ProfileManager.GetWeekProfiles(i);
                    if (profile == null)
                    {
                        foundWeek = true;
                        ProfileManager.UnlockWeek(i);
                        ProfileManager.SetWeek(i);
                        // defaultSong = i;
                        // m_DefaultSong = defaultSong;
                        // SongManager.Instance.defaultSong = m_Songs[m_DefaultSong - 1];
                        // int songID = GameManager.Instance.m_DefaultSong;
                        // SongConfig songs = GameData.Instance.GetSongConfig(songID);
                        // AnalysticsManager.LogReplayFreeplaySong(songs.m_Name);
                        break;
                    }
                }

                if (!foundWeek)
                {
                    GUIManager.Instance.ChangeToPlayScene(true, () => UIManager.Instance.OpenStoryMenu());
                    return;
                }
            }

            GameManager.Instance.OnEnable();

            if (ProfileManager.GetWeek() < 8)
            {
                ProfileManager.SetWeek(ProfileManager.GetWeek() + 1);
            }

            ProfileManager.UnlockWeek(ProfileManager.GetWeek());

            Helper.DebugLog("Next zone unlock week: " + ProfileManager.GetWeek());

            GameManager.Instance.m_RenderCam.cullingMask = GameManager.Instance.m_InGame;

            AnalysticsManager.LogPlayZoneX(GameManager.Instance.m_WeekNo);

            GameManager.Instance.txt_Time.gameObject.SetActive(false);

            UIManager.Instance.g_StoryMenu.SetActive(false);
            List<WeekConfig> weekConfigs = GameData.Instance.GetWeekSong(ProfileManager.GetWeek());
            GameManager.Instance.m_StorysongNo = 0;
            GameManager.Instance.m_StorySongID = weekConfigs[0].m_Id;
            GameManager.Instance.m_WeekConfigs = weekConfigs;

            GameManager.Instance.m_WeekNo = ProfileManager.GetWeek();
            GameManager.Instance.m_WeekSongs.Clear();

            int count = weekConfigs.Count;
            for (int i = 0; i < count; i++)
            {
                GameManager.Instance.m_WeekSongs.Add(GameManager.Instance.m_Songs[weekConfigs[i].m_Id - 1]);
            }

            StatsSystem.Instance.missed = 0;
            StatsSystem.Instance.combo = 0;
            StatsSystem.Instance.score = 0;
            GameManager.Instance.txt_Miss.text = "MISS:" + StatsSystem.Instance.missed.ToString();
            ComboSystem.Instance.UpdateComboDisplay();
            StatsSystem.Instance.UpdateScoreDisplay();

            if (GameManager.Instance.m_Enemy != null)
            {
                GameManager.Instance.m_Enemy.SetAnimTrigger("Idle");
            }

            GameManager.Instance.img_Enemy.sprite = SpriteManager.Instance.m_EnemyIcons[GameManager.Instance.m_WeekNo - 1];

            GameManager.Instance.ResetVsBar();
            // StartCoroutine(GameManager.Instance.IEResetSong());
            // SongManager.Instance.delay = 4f;
            // SongManager.Instance.StopSong(true);
            // SongManager.Instance.PlaySong();

            GameManager.Instance.PauseSong();

            UIManager.Instance.OpenDialoguePopup(true);

            gameObject.SetActive(false);
        }

        public void Mode()
        {
            Helper.DebugLog("Call mode");
            FadeOut();
            Note.m_ReturnHome = true;
            if (GameManager.Instance.m_ModePlay == ModePlay.STORY)
            {
                GUIManager.Instance.ChangeToPlayScene(true, () =>
                {
                    AdsManager.Instance.WatchInterstitial();
                    UIManager.Instance.OpenStoryMenu();
                });
                // UIManager.Instance.OpenStoryMenu();
                // GameManager.Instance.StopSong();
            }
            else if (GameManager.Instance.m_ModePlay == ModePlay.FREEPLAY)
            {
                GUIManager.Instance.ChangeToPlayScene(true, () => UIManager.Instance.OpenFreeplayMenu());
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
            // btn_Claim.gameObject.SetActive(true);
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