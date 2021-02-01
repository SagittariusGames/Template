using UnityEngine;

public class SGInstalledApps
{
    public static string[] InstalledApps()
    {
#if UNITY_EDITOR
        return new string[0];
#elif UNITY_ANDROID
        return InstalledAppsAndroid();
#else
        return new string[0];
#endif
    }

    // ref1: https://answers.unity.com/questions/1087891/check-app-is-installed-or-not-in-ios-device-with-b.html
    // ref2: https://forum.unity.com/threads/using-androidjavaclass-to-return-installed-apps.337296/
    private static string[] InstalledAppsAndroid()
    {
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject pm = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject appInfo = null;
        //take the list of all packages on the device
        AndroidJavaObject packages = pm.Call<AndroidJavaObject>("getInstalledPackages", 0);
        int count = packages.Call<int>("size");
        string[] names = new string[count];
        for (int i = 0; i < count; i++)
        {
            appInfo = packages.Call<AndroidJavaObject>("get", i);
            names[i] = appInfo.Get<string>("packageName");
        }

        // dispose
        up.Dispose();
        ca.Dispose();
        pm.Dispose();
        packages.Dispose();
        appInfo.Dispose();

        return names;
    }
    public bool CheckAppInstallation(string bundleId)
    {
        bool installed = false;
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject pm = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
            if (launchIntent == null)
                installed = false;

            else
                installed = true;
        }

        catch (System.Exception e)
        {
            SGDebug.Exception(e);
            installed = false;
        }

        // dispose
        up.Dispose();
        ca.Dispose();
        pm.Dispose();
        launchIntent.Dispose();

        return installed;
    }
}
