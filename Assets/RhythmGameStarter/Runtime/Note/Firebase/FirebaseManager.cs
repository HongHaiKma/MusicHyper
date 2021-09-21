using Firebase.Analytics;
// using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.Events;

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
    private void Awake()
    {
        // m_Instance = this;
        // DontDestroyOnLoad(gameObject);
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
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
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
    // public void FetchData(UnityAction successCallback)
    // {
    //     m_FirebaseRemoteConfigManager.FetchData(successCallback);
    // }
    // public ConfigValue GetConfigValue(string key)
    // {
    //     return m_FirebaseRemoteConfigManager.GetValues(key);
    // }
}
