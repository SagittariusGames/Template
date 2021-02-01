using System;
using UnityEngine;
using GoogleMobileAds.Api;

// ref: https://developers.google.com/admob/unity/interstitial
public class SGUIAdInterstitial : MonoBehaviour
{
    public string androidAdID = "ca-app-pub-6379533088148188/1057361762";
    public string iosAdID = "ca-app-pub-6379533088148188/1057361762";
    public string remoteSettingsAdsEnable = "ads_enable";

    private static bool firstTime = false;
    private InterstitialAd interstitialAd = null;
    private string enable = "0";

    void Start()
    {
        enable = SGFirebase.RemoteSettings(remoteSettingsAdsEnable, "0");

        if (enable != "0")
        {
            SGAdvertising.Setup();
            if (SGAdvertising.SetupReady)
                RequestInterstitial();
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
        DestroyInterstitial();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Request();
        }
    }
    
    private void RequestInterstitial()
    {
#if UNITY_EDITOR
        string adId = SGAdvertising.testInterstitial;
#elif UNITY_ANDROID
        string adId = androidAdID;
#elif UNITY_IPHONE
        string adId = iosAdID;
#else
        string adId = "unexpected_platform";
#endif
        // test mode
        if (SGDebug.DebugMode)
        {
            adId = SGAdvertising.testInterstitial;
        }
        
        // Interstitial
        interstitialAd = new InterstitialAd(adId);

        // Called when an ad request has successfully loaded.
        interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        interstitialAd.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitialAd.LoadAd(request);
    }

    public void Request()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded() && enable != "0")
        {
            firstTime = true;
            interstitialAd.Show();
            SGDebug.Log("AdInterstitial: Show");
        }
    }

    private void DestroyInterstitial()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            SGDebug.Log("AdInterstitial: Destroy");
        }
    }

    private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        SGDebug.Log("AdInterstitial: HandleAdLoaded event received");
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.AdStart, "type", "Interstitial");
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        SGDebug.LogError("AdInterstitial: HandleFailedToReceiveAd event received with message: " + args.Message);
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.AdFailed, "type", "Interstitial");
    }

    private void HandleOnAdOpened(object sender, EventArgs args)
    {
        SGDebug.Log("AdInterstitial: HandleAdOpened event received");
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.AdCompleted, "type", "Interstitial");
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        SGDebug.Log("AdInterstitial: HandleAdClosed event received");
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.AdClose, "type", "Interstitial");
    }

    private void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        SGDebug.Log("AdInterstitial: HandleAdLeavingApplication event received");
    }

}
