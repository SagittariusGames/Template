using GoogleMobileAds.Api;

public class SGAdvertising
{
    // ref: https://developers.google.com/admob/android/test-ads
    public const string testBanner = "ca-app-pub-3940256099942544/6300978111";
    public const string testInterstitial = "ca-app-pub-3940256099942544/1033173712";
    public const string testInterstitialVideo = "ca-app-pub-3940256099942544/8691691433";
    public const string testRewardedVideo = "ca-app-pub-3940256099942544/5224354917";
    public const string testNativeAdvanced = "ca-app-pub-3940256099942544/2247696110";
    public const string testNativeAdvancedVideo = "ca-app-pub-3940256099942544/1044960115";

    private static bool setupReady = false;

    // ref: https://developers.google.com/admob/unity/start
    public static void Setup ()
    {
        if (setupReady)
            return;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        //MobileAds.Initialize("ca-app-pub-6379533088148188~6266431503");
        setupReady = true;

        //anaytics
        //SGFirebase.AnalyticsTraking();
    }

    public static bool SetupReady {
        get {
            return setupReady;
        }
    }
}
