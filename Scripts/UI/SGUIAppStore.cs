using UnityEngine;

// Ref: https://forum.unity.com/threads/rate-my-app-how-to-create-it.128921/
public class SGUIAppStore : MonoBehaviour
{
    private const string androidRatingURI = "http://play.google.com/store/apps/details?id={0}";
    private const string iOSRatingURI = "itms://itunes.apple.com/us/app/apple-store/{0}?mt=8";

    [Tooltip("iOS App ID (number), example: 1122334455")]
    public string iOSAppID = "";

    private string url;

    void Start()
    {
#if UNITY_IOS
        if (!string.IsNullOrEmpty (iOSAppID)) {
            url = iOSRatingURI.Replace("{0}",iOSAppID);
        }
        else {
            Debug.LogWarning ("Please set iOSAppID variable");
        }
#elif UNITY_ANDROID
        url = androidRatingURI.Replace("{0}", Application.identifier);
#endif
    }

    public void OnClick_Rate()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("Unable to open URL, invalid OS");
        }
    }
}