using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance;

    [Header("Settings")]
    public string AndroidGameId = "6100048"; // Replace this
    public bool TestMode = true;

    [Header("Ad Units")]
    public string InterstitialAdId = "Interstitial_Android";
    public string RewardedAdId = "Rewarded_Android";

    private bool isInterstitialLoaded = false;
    private bool isRewardedLoaded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ✅ SAFE: prevent crash if GameId missing
        if (string.IsNullOrEmpty(AndroidGameId))
        {
            Debug.LogError("Unity Ads Game ID is missing!");
            return;
        }

        Advertisement.Initialize(AndroidGameId, TestMode, this);
    }

    // INIT
    public void OnInitializationComplete()
    {
        Debug.Log("Ads Initialized");

        // ✅ SAFE: check Ad IDs before loading
        if (!string.IsNullOrEmpty(InterstitialAdId))
            Advertisement.Load(InterstitialAdId, this);
        else
            Debug.LogWarning("InterstitialAdId is empty");

        if (!string.IsNullOrEmpty(RewardedAdId))
            Advertisement.Load(RewardedAdId, this);
        else
            Debug.LogWarning("RewardedAdId is empty");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError("Init Failed: " + error + " | " + message);
    }

    // LOAD
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId == InterstitialAdId)
            isInterstitialLoaded = true;

        if (adUnitId == RewardedAdId)
            isRewardedLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError("Load Failed: " + adUnitId + " | " + error + " | " + message);
    }

    // SHOW
    public void ShowInterstitialAd()
    {
        // ✅ SAFE: ensure ads initialized
        if (!Advertisement.isInitialized)
        {
            Debug.LogWarning("Ads not initialized yet");
            return;
        }

        if (isInterstitialLoaded)
        {
            Debug.Log("Showing Interstitial Ad");
            Advertisement.Show(InterstitialAdId, this);
            isInterstitialLoaded = false;
        }
        else
        {
            Debug.Log("Interstitial NOT ready yet");
        }
    }

    public void ShowRewardAd()
    {
        // ✅ SAFE: ensure ads initialized
        if (!Advertisement.isInitialized)
        {
            Debug.LogWarning("Ads not initialized yet");
            return;
        }

        if (isRewardedLoaded)
        {
            Advertisement.Show(RewardedAdId, this);
            isRewardedLoaded = false;
        }
        else
        {
            Debug.Log("Rewarded not ready");
        }
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState state)
    {
        if (adUnitId == RewardedAdId && state == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Reward Given");

       
                GameEvents.OnGiveAHintMethod();
   
        }

        // ✅ SAFE: reload only if valid id
        if (!string.IsNullOrEmpty(adUnitId))
            Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError("Show Failed: " + adUnitId + " | " + error + " | " + message);
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}