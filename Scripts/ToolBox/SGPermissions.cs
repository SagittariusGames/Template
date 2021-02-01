using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

// ref: https://docs.unity3d.com/Manual/android-RequestingPermissions.html
public class SGPermissions
{
    public static bool CameraPermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            Permission.RequestUserPermission(Permission.Camera);
#endif

#if PLATFORM_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.Camera);
#endif

        return true;
    }

    public static bool LocationPermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
            Permission.RequestUserPermission(Permission.CoarseLocation);
#endif

#if PLATFORM_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.CoarseLocation);
#endif

        return true;
    }

    public static bool FineLocationPermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            Permission.RequestUserPermission(Permission.FineLocation);
#endif

#if PLATFORM_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.FineLocation);
#endif

        return true;
    }

    public static bool MicrofonePermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);
#endif

#if PLATFORM_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.Microphone);
#endif

        return true;
    }

    public static bool ExternalStorageReadPermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
#endif

#if PLATFORM_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead);
#endif

        return true;
    }

    public static bool ExternalStorageWritePermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
#endif

#if PLATFORM_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite);
#endif

        return true;
    }

    public static bool PhonePermission()
    {
        return true;
    }

    public static bool CalendarPermission()
    {
        return true;
    }

    public static bool ContactsPermission()
    {
        return true;
    }
}
