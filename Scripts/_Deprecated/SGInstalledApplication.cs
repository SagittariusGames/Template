using UnityEngine;

public class SGInstalledApplication : MonoBehaviour
{
    //ref: https://forum.unity.com/threads/using-androidjavaclass-to-return-installed-apps.337296/
    public static void getInstalledApp()
    {
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        int flag = new AndroidJavaClass("android.content.pm.PackageManager").GetStatic<int>("GET_META_DATA");
        AndroidJavaObject pm = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject packages = pm.Call<AndroidJavaObject>("getInstalledApplications", flag);

        int count = packages.Call<int>("size");

        AndroidJavaObject[] links = new AndroidJavaObject[count];
        string[] names = new string[count];
        int ii = 0;

        for (int i = 0; ii < count;)
        {
            //get the object
            AndroidJavaObject currentObject = packages.Call<AndroidJavaObject>("get", ii);
            try
            {
                //try to add the variables to the next entry
                links[i] = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", currentObject.Get<AndroidJavaObject>("processName"));
                names[i] = pm.Call<string>("getApplicationLabel", currentObject);
                Debug.Log("(" + ii + ") " + i + " " + names[i]);
                //go to the next app and entry
                i++;
                ii++;
            }
            catch
            {
                //if it fails, just go to the next app and try to add to that same entry.
                Debug.Log("skipped " + ii);
                ii++;
            }
        }
    }
}