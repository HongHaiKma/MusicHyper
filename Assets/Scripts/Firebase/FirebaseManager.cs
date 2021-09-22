using Firebase.Analytics;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using RhythmGameStarter;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private const float DEFAULT_LOADING_TIME = 2;
    // private static FirebaseManager m_Instance = null;
    private bool IsLoaded = false;
    //private FirebaseDB m_firebaseDb;
    // public static FirebaseManager Instance
    // {
    //     get
    //     {
    //         return m_Instance;
    //     }
    // }
    private FirebaseAnalyticsManager m_FirebaseAnalyticsManager;
    // private FirebaseRemoteConfigManager m_FirebaseRemoteConfigManager;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    [Header("Remote Config")]
    FirebaseRemoteConfig remoteConfig = FirebaseRemoteConfig.DefaultInstance;

    private void Start()
    {
        // m_Instance = this;
        // DontDestroyOnLoad(gameObject);
        // remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        Init();
    }
    public void Init()
    {
        if (m_FirebaseAnalyticsManager == null)
        {
            m_FirebaseAnalyticsManager = new FirebaseAnalyticsManager();
        }
        // if (m_FirebaseRemoteConfigManager == null)
        // {
        //     m_FirebaseRemoteConfigManager = new FirebaseRemoteConfigManager();
        // }
        Debug.Log("Start Config");
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(continuationAction: task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        IsLoaded = true;
        System.Collections.Generic.Dictionary<string, object> defaults =
            new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add("inter_cd_time", 30);
        remoteConfig.SetDefaultsAsync(defaults).ContinueWith(task =>
        {
            // it's now safe to Fetch
            // ConfigValue config = remoteConfig.GetValue("inter_cd_time");
            // Helper.DebugLog("Config value: " + config.ToString());
            // maxInterTime = (int)config.DoubleValue;
            // interCd.SetMaxTime(maxInterTime);
            remoteConfig.FetchAndActivateAsync().ContinueWith(task =>
            {
                // handle completion
                ConfigValue config = remoteConfig.GetValue("inter_cd_time");
                Helper.DebugLog("Config value: " + (config.DoubleValue).ToString());
            });
        });
        // m_FirebaseRemoteConfigManager.SetupDefaultConfigs();
        // FetchData(() =>
        // {
        //     EventManager.TriggerEvent("UpdateRemoteConfigs");
        // });
    }
    public bool IsFirebaseReady()
    {
        return IsLoaded;
    }
    public void LogAnalyticsEvent(string eventName, string eventParamete, double eventValue)
    {
        if (IsFirebaseReady())
        {
            m_FirebaseAnalyticsManager.LogEvent(eventName, eventParamete, eventValue);
        }
    }
    public void LogAnalyticsEvent(string eventName, Parameter[] paramss)
    {
        if (IsFirebaseReady())
        {
            m_FirebaseAnalyticsManager.LogEvent(eventName, paramss);
        }
    }
    public void LogAnalyticsEvent(string eventName)
    {
        if (IsFirebaseReady())
        {
            m_FirebaseAnalyticsManager.LogEvent(eventName);
        }
    }
    public void SetUserProperty(string propertyName, string property)
    {
        if (IsFirebaseReady())
        {
            m_FirebaseAnalyticsManager.SetUserProperty(propertyName, property);
        }
    }
}
