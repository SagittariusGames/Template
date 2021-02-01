using System;
using UnityEngine;
using GoogleMobileAds.Api;

// ref: https://developers.google.com/admob/unity/banner
public class SGUIAdBanner : MonoBehaviour
{
    public string androidAdID = "ca-app-pub-6379533088148188/6615357501";
    public string iosAdID = "ca-app-pub-6379533088148188/6615357501";
    public bool dontShowUp = false;
    public string remoteSettingsAdsEnable = "ads_enable";

    private static SGUIAdBanner _instance = null;
    private static bool showBanner = false;
    private BannerView bannerView = null;
    private static bool firstTime = false;
    private static bool isLoaded = false;
    private string enable = "0";

    void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(_instance);

        enable = SGFirebase.RemoteSettings(remoteSettingsAdsEnable, "0");

        if (dontShowUp) {
            _instance.DestroyBanner();
        }
        else if (enable == "0")
        {
            _instance.DestroyBanner();
        }
        else {
            _instance.RequestBanner();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (enable != "0")
        {
            SGAdvertising.Setup();
            if (SGAdvertising.SetupReady)
                RequestBanner();
        }
    }

    void Update()
    {
        if (!firstTime)
        {
            Request();
        }
    }

    void OnDestroy()
    {
        DestroyBanner();
    }

    public void RequestBanner()
    {
        if (showBanner)
            return;

#if UNITY_EDITOR
        string adId = SGAdvertising.testBanner;
#elif UNITY_ANDROID
        string adId = androidAdID;
#elif UNITY_IPHONE
        string adId = iosAdID;
#else
        string adId = SGAdvertising.testBanner;
#endif
        // test mode
        if (SGDebug.DebugMode)
        {
            adId = SGAdvertising.testBanner;
        }

        //ref: https://developers.google.com/admob/unity/banner#banner_sizes
        //AdSize adSize = new AdSize(320, 50);
        //BANNER = 320x50
        bannerView = new BannerView(adId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);

        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }

    public void Request()
    {
        if (isLoaded)
        {
            firstTime = true;
            bannerView.Show();
            SGDebug.Log("AdBanner: Show");
            showBanner = true;
        }
    }

    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            showBanner = false;
            SGDebug.Log("AdBanner: Destroy");
        }
    }

    private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        SGDebug.Log("AdBanner: HandleAdLoaded event received");
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.AdStart, "type", "Banner");
        isLoaded = true;
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        SGDebug.LogError("AdBanner: HandleFailedToReceiveAd event received with message: " + args.Message);
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.AdFailed, "type", "Banner");
    }

    private void HandleOnAdOpened(object sender, EventArgs args)
    {
        SGDebug.Log("AdBanner: HandleAdOpened event received");
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.AdCompleted, "type", "Banner");
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        SGDebug.Log("AdBanner: HandleAdClosed event received");
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.AdClose, "type", "Banner");
    }

    private void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        SGDebug.Log("AdBanner: HandleAdLeavingApplication event received");
    }
}
