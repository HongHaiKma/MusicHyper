using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using GoogleMobileAds.Api;
using System;
using RhythmGameStarter;
// using GoogleMobileAdsMediationTestSuite.Api;
using Facebook.Unity;
// using AudienceNetwork;

[DefaultExecutionOrder(-92)]

#region MAX
public class AdsManager : Singleton<AdsManager>
{
    // private string m_APP_ID = "ca-app-pub-3940256099942544/3419835294";

    // private BannerView m_BannerView;
    // private string m_BannerId = "ca-app-pub-3940256099942544/6300978111";
    public bool m_BannerLoaded;

    // private InterstitialAd interstitial;
    // private string m_InterId = "ca-app-pub-3940256099942544/1033173712";
    public bool m_WatchInter;

    // private RewardedAd rewardedAd;
    // private string m_RewardId = "ca-app-pub-3940256099942544/5224354917";


    public bool openRwdAds;
    public bool openRwdAds2;
    private RewardType m_RewardType;

    public int m_TrySongId;


    [Header("MAX SDK")]
    // string adUnitId = "lBnu4FPbTxGUi8cqgwNENfRfqINTVk8B9NxC2Japp2DqKPThsIhPCF7zcC6wr9_IN8OLTcG9SR4dT4OJwQPTBf";
    // private string m_InterId = "04ed1093a54d8b5d";
    // private string m_RewardId = "18edca5329c5f208";
    // private string m_BannerId = "a29cb6e6fe5d0f40";

    string adUnitId = "lBnu4FPbTxGUi8cqgwNENfRfqINTVk8B9NxC2Japp2DqKPThsIhPCF7zcC6wr9_IN8OLTcG9SR4dT4OJwQPTBf";
    private string m_InterId = "d7ca787637c32b03";
    private string m_RewardId = "1e7e983078a113d1";
    private string m_BannerId = "be213b3cc42f51ca";

    int retryAttempt;

    public string m_Ads = "AdsAds";

    private void Awake()
    {
        // AudienceNetworkAds.Initialize();
        // FB.Init();

        if (!PlayerPrefs.HasKey(m_Ads))
        {
            PlayerPrefs.SetInt(m_Ads, 1);
        }

        m_BannerLoaded = false;
        m_WatchInter = true;
        openRwdAds2 = false;

        // MobileAds.Initialize(initStatus => { });
        // MobileAds.Initialize(m_APP_ID);

        // MobileAds.SetiOSAppPauseOnBackground(true);
        // // #if UNITY_EDITOR
        // //         // Initialize the Google Mobile Ads SDK.
        // //         MobileAds.Initialize(HandleInitCompleteAction);
        // //         AppLovin.Initialize();
        // // #else
        // MobileAds.Initialize((initStatus) =>
        // {
        //     Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
        //     foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
        //     {
        //         string className = keyValuePair.Key;
        //         AdapterStatus status = keyValuePair.Value;
        //         switch (status.InitializationState)
        //         {
        //             case AdapterState.NotReady:
        //                 // The adapter initialization did not complete.
        //                 MonoBehaviour.print("Adapter: " + className + " not ready.");
        //                 break;
        //             case AdapterState.Ready:
        //                 // The adapter was successfully initialized.
        //                 MonoBehaviour.print("Adapter: " + className + " is initialized.");
        //                 break;
        //         }
        //     }
        //     //MediationTestSuite.OnMediationTestSuiteDismissed += this.HandleMediationTestSuiteDismissed;
        //     this.RequestBanner();
        //     this.RequestInter();
        //     this.RequestRewardVideo();
        // });
        // MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[] { "6d97e445-c154-4dc5-878b-9b27b6bddacd" });

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                // AppLovin SDK is initialized, configure and start loading ads.
                Debug.Log("MAX SDK Initialized");
                // MaxSdk.ShowMediationDebugger();

                InitializeInterstitialAds();
                InitializeRewardedAds();
                InitializeBannerAds();
            };

        MaxSdk.SetSdkKey(adUnitId);
        MaxSdk.InitializeSdk();

        LoadBanner();
    }

    #region MAX SDK

    public void InitializeBannerAds()
    {
        if (PlayerPrefs.GetInt(m_Ads) == 1)
        {
            Helper.DebugLog("Ads Manager Check ads");
            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.

            MaxSdk.CreateBanner(m_BannerId, MaxSdkBase.BannerPosition.BottomCenter);
            MaxSdk.SetBannerExtraParameter(m_BannerId, "adaptive_banner", "true");
            // MaxSdk.CreateBanner(adUnitId, 0f, 25f);
            // MaxSdk.CreateBanner(adUnitId, 0f, 0f);

            // Set background or background color for banners to be fully functional.
            MaxSdk.SetBannerBackgroundColor(m_BannerId, Color.white);
        }
    }

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(m_RewardId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        Helper.DebugLog("OnRewardedAdLoadedEvent");
        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        Helper.DebugLog("OnRewardedAdLoadFailedEvent");
        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        Helper.DebugLog("OnRewardedAdFailedToDisplayEvent");
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        if (!openRwdAds)
        {
            StartCoroutine(IEProcessRewardVideoClosed());
        }

        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        if (openRwdAds)
        {
            openRwdAds = false;

            ProcessRewardVideo();
        }
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }

    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(m_InterId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }

    #endregion

    // private void Start()
    // {

    // }

    // public void _InitilizationOfSDF()
    // {
    //     MobileAds.Initialize(initStatus => { });

    //     this.RequestBanner();
    // }

    // public void Start()
    // {

    // }

    // private void OnEnable()
    // {
    //     StartListenToEvent();
    // }

    // private void OnDisable()
    // {
    //     StopListenToEvent();
    // }

    // private void OnDestroy()
    // {
    //     StopListenToEvent();
    // }

    public void StartListenToEvent()
    {
        // EventManager.AddListener(GameEvent.END_GAME, LoadBanner);
        // EventManager.AddListener(GameEvent.END_GAME, LoadInter);
    }

    public void StopListenToEvent()
    {
        // EventManager.RemoveListener(GameEvent.END_GAME, LoadBanner);
        // EventManager.RemoveListener(GameEvent.END_GAME, LoadInter);
    }

    // public void RequestBanner()
    // {
    //     // #if UNITY_ANDROID
    //     // #elif UNITY_IPHONE
    //     // #endif
    //     if (ProfileManager.CheckAds())
    //     {
    //         Helper.DebugLog("RequestBanner");
    //         AdSize adSize = new AdSize(320, 35);
    //         this.m_BannerView = new BannerView(m_BannerId, adSize, AdPosition.Bottom);
    //         // this.m_BannerView = new BannerView(m_BannerId, AdSize.Banner, AdPosition.Bottom);

    //         // Called when an ad request has successfully loaded.
    //         this.m_BannerView.OnAdLoaded += this.HandleOnAdLoaded;
    //         // Called when an ad request failed to load.
    //         this.m_BannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
    //         // Called when an ad is clicked.
    //         this.m_BannerView.OnAdOpening += this.HandleOnAdOpened;
    //         // Called when the user returned from the app after an ad click.
    //         this.m_BannerView.OnAdClosed += this.HandleOnAdClosed;
    //         // Called when the ad click caused the user to leave the application.
    //         // this.m_BannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

    //         AdRequest request = new AdRequest.Builder().Build();

    //         this.m_BannerView.LoadAd(request);
    //     }
    // }

    // public void RequestInter()
    // {
    //     // #if UNITY_ANDROID
    //     // #elif UNITY_IPHONE
    //     // #endif
    //     if (ProfileManager.CheckAds())
    //     {
    //         // Initialize an InterstitialAd.
    //         this.interstitial = new InterstitialAd(m_InterId);

    //         // Called when an ad request has successfully loaded.
    //         this.interstitial.OnAdLoaded += HandleOnAdLoaded;
    //         // Called when an ad request failed to load.
    //         this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
    //         // Called when an ad is shown.
    //         this.interstitial.OnAdOpening += HandleOnAdOpened;
    //         // Called when the ad is closed.
    //         this.interstitial.OnAdClosed += HandleOnAdClosed;
    //         // Called when the ad click caused the user to leave the application.
    //         // this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

    //         // Create an empty ad request.
    //         AdRequest request = new AdRequest.Builder().Build();
    //         // Load the interstitial with the request.
    //         this.interstitial.LoadAd(request);
    //     }
    // }

    // public void RequestRewardVideo()
    // {
    //     this.rewardedAd = new RewardedAd(m_RewardId);

    //     this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
    //     // this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
    //     // this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
    //     this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
    //     this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    //     this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

    //     // Create an empty ad request.
    //     AdRequest request = new AdRequest.Builder().Build();
    //     // Load the rewarded ad with the request.
    //     // this.rewardedAd.LoadAd(request);
    //     this.rewardedAd.LoadAd(request);
    // }

    public void LoadBanner()
    {
        // if (ProfileManager.CheckAds())
        // {
        //     // AdSize adSize = new AdSize(320, 35);
        //     // this.m_BannerView = new BannerView(m_BannerId, adSize, AdPosition.Bottom);
        //     // this.m_BannerView.Destroy();
        //     AdRequest request = new AdRequest.Builder().Build();
        //     this.m_BannerView.LoadAd(request);
        // }
        // this.m_BannerView.Hide();
        // this.m_BannerView.Destroy();

        // RequestBanner();

        // if (ProfileManager.CheckAds())
        // {
        //     // AdSize adSize = new AdSize(320, 35);
        //     // this.m_BannerView = new BannerView(m_BannerId, adSize, AdPosition.Bottom);
        //     // this.m_BannerView.Destroy();
        //     AdRequest request = new AdRequest.Builder().Build();
        //     this.m_BannerView.LoadAd(request);
        // }

        if (PlayerPrefs.GetInt(m_Ads) == 1)
        {
            Helper.DebugLog("Ads Manager Check ads");
            MaxSdk.ShowBanner(m_BannerId);
        }
    }

    public void DestroyBanner()
    {
        // this.m_BannerView.Hide();
        // this.m_BannerView.Destroy();
        MaxSdk.HideBanner(m_BannerId);
    }

    // public void LoadInter()
    // {
    //     if (ProfileManager.CheckAds())
    //     {
    //         // this.interstitial.Destroy();
    //         AdRequest request = new AdRequest.Builder().Build();
    //         this.interstitial.LoadAd(request);
    //     }
    // }

    // public void LoadRewardVideo()
    // {
    //     this.rewardedAd = new RewardedAd(m_RewardId);
    //     this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
    //     // this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
    //     // this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
    //     this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
    //     this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    //     this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    //     AdRequest request = new AdRequest.Builder().Build();
    //     this.rewardedAd.LoadAd(request);
    // }

    public void WatchInterstitial()
    {
        if (m_WatchInter && PlayerPrefs.GetInt(m_Ads) == 1 && ProfileManager.MyProfile.m_InterTime.GetCurrentValue() <= 0)
        {
            Helper.DebugLog("Ads Manager Check ads");
            // if (interstitial.IsLoaded())
            // {
            //     interstitial.Show();
            //     // AnalysticsManager.LogInterAdsShow();
            // }
            // else
            // {
            //     // RequestInter();
            //     LoadInter();
            // }
            if (MaxSdk.IsInterstitialReady(m_InterId))
            {
                AnalysticsManager.LogInterAdsShow();
                ProfileManager.MyProfile.m_InterTime.Reset();
                MaxSdk.ShowInterstitial(m_InterId);
            }
            else
            {
                LoadInterstitial();
            }
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");

        m_BannerLoaded = true;

        // if (Helper.NoAds())
        // {
        //     DestroyBanner();
        // }
    }

    // public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    // {
    //     // MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
    //     //                     + args.Message);

    //     m_BannerLoaded = false;
    //     // LoadBanner();

    //     // LoadInter();
    // }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");

        // if (Helper.NoAds())
        // {
        //     DestroyBanner();
        // }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    // public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    // {
    //     MonoBehaviour.print(
    //         "HandleRewardedAdFailedToLoad event received with message: "
    //                          + args.Message);
    //     // LoadRewardVideo();
    // }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    // public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    // {
    //     MonoBehaviour.print(
    //         "HandleRewardedAdFailedToShow event received with message: "
    //                          + args.Message);

    //     LoadRewardVideo();
    // }

    // public void HandleRewardedAdClosed(object sender, EventArgs args)
    // {
    //     MonoBehaviour.print("HandleRewardedAdClosed event received");
    //     if (!openRwdAds)
    //     {
    //         StartCoroutine(IEProcessRewardVideoClosed());
    //     }

    //     LoadRewardVideo();
    // }

    // public void HandleUserEarnedReward(object sender, Reward args)
    // {
    //     string type = args.Type;
    //     double amount = args.Amount;
    //     MonoBehaviour.print(
    //         "HandleRewardedAdRewarded event received for "
    //                     + amount.ToString() + " " + type);

    //     if (openRwdAds)
    //     {
    //         openRwdAds = false;

    //         ProcessRewardVideo();
    //     }
    // }

    IEnumerator IEProcessRewardVideoClosed()
    {
        // yield return Yielders.EndOfFrame;
        yield return null;

        openRwdAds2 = false;

        switch (m_RewardType)
        {
            // case RewardType.GOLD_1:
            //     EventManager.CallEvent(GameEvent.ADS_GOLD_1_ANIM);
            //     break;
            // case RewardType.GOLD_2:
            //     EventManager.CallEvent(GameEvent.ADS_GOLD_2_ANIM);
            //     break;
            // // case RewardType.CHARACTER_2:
            // //     EventManager.CallEvent(GameEvent.ADS_CHARACTER_2_ANIM);
            // //     break;
            // case RewardType.KEYS3_1:
            //     PopupCaller.OpenBonusRewardPopup(false, true);
            //     // EventManager.CallEvent(GameEvent.ADS_3KEYS_1_ANIM);
            //     break;
            // case RewardType.START_LONGER:
            //     PlaySceneManager.Instance.btn_StartLonger.gameObject.SetActive(false);
            //     EventManager.CallEvent(GameEvent.ADS_START_LONGER);
            //     AnalysticsManager.LogRewardLonger();
            //     break;
            // case RewardType.OUTFIT_PROGRESS:
            //     EventManager.CallEvent(GameEvent.ADS_OUTFIT_PROGRESS_ANIM);
            //     break;
            case RewardType.TRY_SONG:
                EventManager1<int>.CallEvent(GameEvent.TRY_SONG, m_TrySongId);
                break;
            case RewardType.X3_CLAIM:
                AdsManager.Instance.m_WatchInter = false;
                EventManager.CallEvent(GameEvent.X3_CLAIM);
                break;
            case RewardType.CONTINUE:
                EventManager.CallEvent(GameEvent.CONTINUE);
                break;
            default:
                break;
        }
    }

    public void ProcessRewardVideo()
    {
        StartCoroutine(IEProcessRewardVideo());
    }

    IEnumerator IEProcessRewardVideo()
    {
        // yield return Yielders.EndOfFrame;
        yield return null;

        switch (m_RewardType)
        {
            // case RewardType.CHARACTER:
            //     EventManager.CallEvent(GameEvent.ADS_CHARACTER_LOGIC);
            //     break;
            // // case RewardType.CHARACTER_2:
            // //     m_WatchInter = false;
            // //     EventManager.CallEvent(GameEvent.ADS_CHARACTER_2_LOGIC);
            // //     break;
            // case RewardType.GOLD_1:
            //     EventManager.CallEvent(GameEvent.ADS_GOLD_1_LOGIC);
            //     AnalysticsManager.LogRewardGoldOutfit();
            //     break;
            // case RewardType.GOLD_2:
            //     m_WatchInter = false;
            //     ProfileManager.AddGold(GameManager.Instance.m_GoldWin);
            //     ProfileManager.Instance.PassLevel();
            //     AnalysticsManager.LogRewardGoldWin();
            //     break;
            // case RewardType.KEYS3_1:
            //     EventManager.CallEvent(GameEvent.ADS_3KEYS_1_LOGIC);
            //     PopupCaller.GetBonusRewardPopup().m_OpenAgain = true;
            //     ProfileManager.AddKeys(3);
            //     AnalysticsManager.LogReward3Keys();
            //     break;
            // // case RewardType.START_LONGER:
            // //     // EventManager.CallEvent(GameEvent.ADS_3KEYS_1_LOGIC);
            // //     // ProfileManager.AddKeys(3);
            // //     break;
            // case RewardType.OUTFIT_PROGRESS:
            //     if (PopupOutfitReward.m_RandomEpicChar == -1)
            //     {
            //         ProfileManager.AddGold(10000);
            //         EventManager.CallEvent(GameEvent.UPDATE_GOLD);
            //     }
            //     else
            //     {
            //         ProfileManager.UnlockEpicNewCharacter(PopupOutfitReward.m_RandomEpicChar);
            //         ProfileManager.SetSelectedCharacter(PopupOutfitReward.m_RandomEpicChar);
            //     }
            //     AnalysticsManager.LogRewardCharProgress();
            //     break;
            // case RewardType.X3_CLAIM:
            //     EventManager.CallEvent(GameEvent.X3_CLAIM);
            //     break;
            // default:
            //     break;
        }
    }

    public void WatchRewardVideo(RewardType _rewardType)
    {
        Helper.DebugLog("Watch Reward Video");
        StartCoroutine(IEWatchRewardVideo(_rewardType));
    }

    public void WatchRewardVideo(RewardType _rewardType, int _songId)
    {
        m_TrySongId = _songId;
        StartCoroutine(IEWatchRewardVideo(_rewardType));
    }

    IEnumerator IEWatchRewardVideo(RewardType _rewardType)
    {
        // LoadRewardVideo();

        m_RewardType = _rewardType;
        openRwdAds = true;
        openRwdAds2 = true;

        // PlaySceneManager.Instance.g_LoadingAds.SetActive(true);
        GUIManager.Instance.SetLoadingPopup(true);

        yield return Yielders.Get(1f);
        yield return Yielders.EndOfFrame;

        // PlaySceneManager.Instance.g_LoadingAds.SetActive(false);
        GUIManager.Instance.SetLoadingPopup(false);

        // if (this.rewardedAd.IsLoaded())
        // {
        //     this.rewardedAd.Show();
        // }
        // else
        // {
        //     LoadRewardVideo();
        //     // GUIManager.Instance.GetPanelLoadingAds().g_NetworkError.SetActive(true);
        //     // yield return Yielders.Get(0.5f);
        //     // GUIManager.Instance.GetPanelLoadingAds().g_NetworkError.SetActive(false);
        // }

        if (MaxSdk.IsRewardedAdReady(m_RewardId))
        {
            MaxSdk.ShowRewardedAd(m_RewardId);
            openRwdAds2 = true;
            Helper.DebugLog("IsRewardedAdReady");
        }
        else
        {
            openRwdAds2 = false;
            LoadRewardedAd();
        }
    }

    // public void ShowMediationTestSuite()
    // {
    //     MediationTestSuite.Show();
    // }
}

#endregion
public enum RewardType
{
    CHARACTER,
    CHARACTER_2,
    GOLD_1,
    GOLD_2,
    KEYS3_1,
    START_LONGER,
    OUTFIT_PROGRESS,
    TRY_SONG,
    X3_CLAIM,
    CONTINUE,
}