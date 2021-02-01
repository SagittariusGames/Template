using UnityEngine;
using UnityEngine.Networking;

public class SGAppLauncher
{
    public static string defaultURL = "";
    public static string defaultMessage = "";

    public static void LaunchApp(string bundleId) => LaunchApp(bundleId, defaultURL);

    public static void LaunchApp(string bundleId, string alternativeURL)
    {
        bool launched = false;

        // open app in store
#if UNITY_EDITOR
        launched = false;
#elif UNITY_ANDROID
        launched = LaunchAndriodApp(bundleId);
#else
        launched = false;
#endif

        if (!launched)
        {
            LaunchURL(alternativeURL);

            //analytics
            SGAnalytics.AnalyticsTraking("LaunchApp", "alternativeURL", alternativeURL);
        } else
        {
            //analytics
            SGAnalytics.AnalyticsTraking("LaunchApp", "bundleId", bundleId);
        }
    }

    public static void LaunchURL(string url) => LaunchURL(url, defaultMessage);

    public static void LaunchURL(string url, string message)
    {
        Application.OpenURL(url + UnityWebRequest.EscapeURL(message));
    }

    private static bool LaunchAndriodApp(string bundleId, string param = "arguments")
    {
        bool fail = false;

        // get the current activity
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject pm = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject launchIntent = null;

        try
        {
            launchIntent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
            if (launchIntent == null)
                fail = true;
            else
                launchIntent.Call<AndroidJavaObject>("putExtra", param, defaultMessage);
        }
        catch (System.Exception e)
        {
            fail = true;
            SGDebug.Exception(e);
        }

        //open the app
        if (!fail)
        {
            ca.Call("startActivity", launchIntent);
        }

        up.Dispose();
        ca.Dispose();
        pm.Dispose();
        launchIntent.Dispose();

        return !fail;
    }
}
