using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;

public class AnalysticsManager : Singleton<AnalysticsManager>
{
    public static void LogPlayLevel(int level)
    {
        string eventName = "PLAY_NORMAL_LEVEL_";
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

    public static void LogWinLevel(int level)
    {
        string eventName = "WIN_NORMAL_LEVEL_";
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
