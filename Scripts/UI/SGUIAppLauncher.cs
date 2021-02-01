using UnityEngine;
public class SGUIAppLauncher : MonoBehaviour
{
    public string defaultURL = "https://translate.google.com/#view=home&op=translate&sl=en&tl=pt&text=";
    public string defaultAndroidBundleID = "com.google.android.apps.translate";
    //public string defaultiOSBundleID = "com.google.android.apps.translate";
    public string keyURL = "";
    public string keyApp = "";

    public void TranslationURL_OnClick()
    {
        if (string.IsNullOrEmpty(SGAppLauncher.defaultMessage))
            return;

        string url = SGStorage.GetGenericPref(keyURL, defaultURL);
        SGAppLauncher.LaunchURL(url);
    }

    public void TranslationApp_OnClick()
    {
        if (string.IsNullOrEmpty(SGAppLauncher.defaultMessage))
            return;

        string url = SGStorage.GetGenericPref(keyURL, defaultURL);
        string appID = "";
#if UNITY_EDITOR
        appID = SGStorage.GetGenericPref(keyApp, defaultAndroidBundleID);
#elif UNITY_ANDROID
        appID = SGStorage.GetGenericPref(keyApp, defaultAndroidBundleID);
#endif
        SGAppLauncher.LaunchApp(appID, url);
    }
}
