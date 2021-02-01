using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SGEnvironment
{
    public delegate void EventChanges();
    public static event EventChanges ClipboardChanged = null;
    public static event EventChanges DynamicLinkChanged = null;
    public static event EventChanges ExternalCallParamChanged = null;

    private static string dynamicLink = "https://your_subdomain.page.link/?link=loading&apn=package_name[&amv=minimum_version][&afl=fallback_link]";
    private static string externalCallParam = "";

    public static string GetUnityVersion()
    {
        return Application.unityVersion;
    }

    public static string GetVersion()
    {
        return Application.version;
    }

    public static string GetProdutcName()
    {
        return Application.productName;
    }

    public static void SetClipboard (string message)
    {
        // general
        GUIUtility.systemCopyBuffer = message;
        //event
        ClipboardChanged?.Invoke();

        //analytics
        SGAnalytics.AnalyticsTraking("SetClipboard", "message", message);
    }

    public static string GetClipboard ()
    {
        return GUIUtility.systemCopyBuffer;
    }

    public static void SetDynamicLink(string link)
    {
        dynamicLink = UnityWebRequest.UnEscapeURL(link);
        //event
        DynamicLinkChanged?.Invoke();

        //analytics
        SGAnalytics.AnalyticsTraking("SetDynamicLink", "link", link);
    }

    // ex: https://your_subdomain.page.link/?link=your_deep_link&apn=package_name[&amv=minimum_version][&afl=fallback_link]
    public static string GetDynamicLink(string param = "")
    {
        int startIndex = 0;
        int endIndex = 0;

        if (string.IsNullOrEmpty(dynamicLink))
            return "";

        if (string.IsNullOrEmpty(param))
        {
            return dynamicLink;
        } else
        {
            startIndex = dynamicLink.IndexOf(param + "=");
            if (startIndex > 0)
            {
                startIndex += (param + "=").Length;
                endIndex = dynamicLink.IndexOf("?", startIndex);
                if (endIndex <= 0)
                    endIndex = dynamicLink.IndexOf("&", startIndex);
                if (endIndex <= 0)
                    endIndex = dynamicLink.Length;
            }

            return dynamicLink.Substring(startIndex, endIndex - startIndex);
        }
    }

    public static void SetExternalCallParam(string param)
    {
        externalCallParam = param;
        //event
        ExternalCallParamChanged?.Invoke();

        //analytics
        SGAnalytics.AnalyticsTraking("SetExternalCallParam", "param", param);
    }

    public static string GetExternalCallParam()
    {
        return externalCallParam;
    }

    public static IEnumerator Quit()
    {
        yield return new WaitForSeconds(2);

        //analytics
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.QuitGame);

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
            EditorApplication.isPlaying = false;
#endif

#if UNITY_WSA
        Application.Unload ();
#endif
#if UNITY_IOS
        Application.Unload ();
#else
        Application.Quit();
#endif
    }
}
