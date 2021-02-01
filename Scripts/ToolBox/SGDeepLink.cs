using UnityEngine;

public class SGDeepLink
{
    public static void ReceiveExternalCall(string param = "arguments")
    {
#if UNITY_EDITOR
        // do something
#elif UNITY_ANDROID
        ReceiveExternalCallAndroid(param);
#else
        // do something
#endif
    }

    private static void ReceiveExternalCallAndroid(string param = "arguments")
    {
        string arguments = "";
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intent = ca.Call<AndroidJavaObject>("getIntent");
        bool hasExtra = intent.Call<bool>("hasExtra", "arguments");

        if (hasExtra)
        {
            AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");
            arguments = extras.Call<string>("getString", param);

            SGDebug.Log("Received external call: " + arguments);

            SGEnvironment.SetExternalCallParam(arguments);
            
            // dispose
            extras.Dispose();
        }
        // dispose
        up.Dispose();
        ca.Dispose();
        intent.Dispose();
    }
}
