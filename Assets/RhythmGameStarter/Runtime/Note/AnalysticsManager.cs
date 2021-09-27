using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

namespace RhythmGameStarter
{
    public class AnalysticsManager : Singleton<AnalysticsManager>
    {
        public static void LogPlayBOOPEEBO()
        {
            string eventName = "PLAY_FREE_BOOPEEBO";
            // if (level < 10)
            // {
            //     eventName = eventName + "00" + level;
            // }
            // else if ((level >= 10) && (level < 100))
            // {
            //     eventName = eventName + "0" + level;
            // }
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogWinBOOPEEBO()
        {
            string eventName = "WIN_FREE_BOOPEEBO";
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogFailBOOPEEBO()
        {
            string eventName = "FAIL_FREE_BOOPEEBO";
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogReplayBOOPEEBO()
        {
            string eventName = "REPLAY_NORMAL_BOOPEEBO";
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

        public static void LogLoseLevel(int level)
        {
            string eventName = "FAIL_NORMAL_LEVEL_";
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

        public static void LogRetryLevel(int level)
        {
            string eventName = "REPLAY_NORMAL_LEVEL_";
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

        public static void LogInterAdsShow()
        {
            string eventName = "INTERSTITIAL_ADS";
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogGetShopGold1()
        {
            string eventName = "SHOP_FREE_COIN";
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }

        public static void LogUnlockCharacter(int _index, string _name)
        {
            string eventName = "CHARACTER_UNLOCK_" + _name + "_" + _index;
            FirebaseManager.Instance.LogAnalyticsEvent(eventName);
        }
    }
}