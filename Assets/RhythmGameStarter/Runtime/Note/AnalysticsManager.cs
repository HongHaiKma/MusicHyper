using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

namespace RhythmGameStarter
{
    public class AnalysticsManager : Singleton<AnalysticsManager>
    {
        public static void LogPlayFreeSong(string _song)
        {
            string eventName = "PLAY_FREE_" + _song;
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogWinFreeplaySong(string _song)
        {
            string eventName = "WIN_FREE_" + _song;
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogfailFreeplaySong(string _song)
        {
            string eventName = "FAIL_FREE_" + _song;
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogReplayFreeplaySong(string _song)
        {
            string eventName = "REPLAY_NORMAL_" + _song;
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogPlayZoneX(int _level)
        {
            string eventName = "PLAY_ZONE_";
            if (_level < 10)
            {
                eventName = eventName + "00" + _level;
            }
            else if ((_level >= 10) && (_level < 100))
            {
                eventName = eventName + "0" + _level;
            }
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogWinZoneX(int _level)
        {
            string eventName = "WIN_ZONE_";
            if (_level < 10)
            {
                eventName = eventName + "00" + _level;
            }
            else if ((_level >= 10) && (_level < 100))
            {
                eventName = eventName + "0" + _level;
            }
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogFailZoneX(int _level)
        {
            string eventName = "FAIL_ZONE_";
            if (_level < 10)
            {
                eventName = eventName + "00" + _level;
            }
            else if ((_level >= 10) && (_level < 100))
            {
                eventName = eventName + "0" + _level;
            }
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogReplayZone(int level)
        {
            string eventName = "REPLAY_ZONE_";
            if (level < 10)
            {
                eventName = eventName + "00" + level;
            }
            else if ((level >= 10) && (level < 100))
            {
                eventName = eventName + "0" + level;
            }
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogTrySong(string song)
        {
            string eventName = "TRY_TIME_" + song;

            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogX3Claim()
        {
            string eventName = "REWARD_ADS_GOLD_WIN";

            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogRevive()
        {
            string eventName = "REVIVE_ADS";

            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogInterAdsShow()
        {
            string eventName = "INTERSTITIAL_ADS";
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogRemoveAds()
        {
            string eventName = "PURCHASE_REMOVE_ADS";
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogUnlockChallengeSong()
        {
            string eventName = "UNLOCK_CHALLENGE_ADS";
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogUnlockFreeplaySong()
        {
            string eventName = "UNLOCK_FREE_ADS";
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }
    }
}