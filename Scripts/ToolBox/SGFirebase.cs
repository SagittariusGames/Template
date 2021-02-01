using System;
using System.Threading.Tasks;
using System.Collections;
using Firebase;
using Firebase.Extensions;
using Firebase.Crashlytics;
using Firebase.RemoteConfig;
using Firebase.DynamicLinks;
using Firebase.Database;
using Firebase.Analytics;
using UnityEngine;

public class SGFirebase
{
    public static string userName = "";
    public static string userEmail = "";
    public static string userId = "";

    private static bool setupReady = false;
    private static string lastError = "";
    private static string rsPreference = "_FRS_";
    private static string rsDebugPrefix = "_DBG_";
    private static bool rsActivateFetched = false;

    private static Firebase.Auth.FirebaseAuth auth = null;
    private static Firebase.Auth.Credential credential = null;
    private static int authPhases = 0;//1=authenticated; 0=none; -1=trying


    public static void Setup()
    {
        if (setupReady)
            return;
        
        FirebaseApp.LogLevel = LogLevel.Debug;
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                FirebaseApp app = FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                setupReady = true;
                // user default
                SetUserId(Application.buildGUID);

                //analytics
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));

                // listeners
                DynamicLinks.DynamicLinkReceived += OnDynamicLink;

            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        }
        );
    }

    public static bool SetupReady {
        get {
            return setupReady;
        }
    }

    //1=authenticated; 0=none; -1=trying
    public static int Auth {
        get {
            return authPhases;
        }
    }

    public static string LastError {
        get {
            return lastError;
        }
    }

    public static void Dispose()
    {
        // dispose
        setupReady = false;
        FirebaseApp.DefaultInstance.Dispose();

        AuthSignOut();
    }

    public static void Log(string message)
    {
        if (!setupReady)
            return;
        Crashlytics.Log(message);
    }

    public static void SetCustomKey(string key, string value)
    {
        if (!setupReady)
            return;
        Crashlytics.SetCustomKey(key, value);
        FirebaseAnalytics.SetUserProperty(key, value);
    }

    public static void SetUserId(string id)
    {
        if (!setupReady)
            return;
        userId = id;
        Crashlytics.SetUserId(id);
        FirebaseAnalytics.SetUserId(id);
    }

    public static void LogException(System.Exception e)
    {
        if (!setupReady)
            return;
        Crashlytics.LogException(e);
    }

    public static IEnumerator RemoteSettingsCountDown(int rsCountDown)
    {
        int count = rsCountDown;
        while (true)
        {
            yield return new WaitForSeconds(1);
            count--;
            if (count <= 0)
            {
                RemoteSettingsAsyncUpdate();
                RemoteSettingsSaveLate();
                count = rsCountDown;
            }
        }
    }

    public static bool RemoveSettingsTryUpdate()
    {
        RemoteSettingsAsyncUpdate();
        return RemoteSettingsSaveLate();
    }

    private static void RemoteSettingsAsyncUpdate()
    {
        if (!setupReady)
            return;

        // remote settings, async
        rsActivateFetched = false;
        FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero).ContinueWith(RemoteSettingsFetch);
        //Task fetchTask = FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
        //fetchTask.ContinueWith(RemoteSettingsFetch);
        Debug.Log("Remote Settings: Start async update.");
    }

    // ref: https://stackoverflow.com/questions/48521807/firebase-remote-config-in-unity-get-only-default-values-and-not-real-ones
    private static void RemoteSettingsFetch(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Remote Settings: Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Remote Settings: Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Remote Settings: Fetch completed!");

            var info = FirebaseRemoteConfig.Info;

            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    Debug.Log(string.Format("Remote Settings: Data loaded and ready (last fetch time {0}).", info.FetchTime));
                    if (FirebaseRemoteConfig.ActivateFetched())
                    {
                        rsActivateFetched = true;
                        SGDebug.Log(string.Format("Remote Settings: Activate Fetched and Data loaded and ready (last fetch time {0}).", info.FetchTime));
                    }
                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            Debug.LogError("Remote Settings: Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.LogError("Remote Settings: Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    Debug.LogWarning("Remote Settings: Latest Fetch call still pending.");
                    break;
                default:
                    Debug.LogWarning(string.Format("Remote Settings: Something get wrong. Data: {0}", info));
                    break;
            }
        }

    }

    private static bool RemoteSettingsSaveLate()
    {
        bool hasValues = false;
        
        if (!setupReady)
            return false;

        //save the latest remote settings
        var keys = FirebaseRemoteConfig.Keys;
        foreach (string key in keys)
        {
            string value = FirebaseRemoteConfig.GetValue(key).StringValue;
            SGStorage.SetGenericPref(rsPreference + key, value);
            Debug.Log("Remote Settings: Save data to PlayerPrefab: " + rsPreference + key + "=" + value);
        }

        hasValues = rsActivateFetched;

        return hasValues;
    }
    
    /// <summary>
    /// get the latest values for loaded remote settings
    /// </summary>
    public static string RemoteSettings(string propertyName, string defaultValue)
    {
        string newValue = "";
        
        if (SGDebug.DebugMode)
            newValue = SGStorage.GetGenericPref(rsPreference + rsDebugPrefix + propertyName, defaultValue);
        else
            newValue = SGStorage.GetGenericPref(rsPreference + propertyName, defaultValue);

        if (newValue == null)
            newValue = defaultValue;

        return newValue;
    }

    public static void OnDynamicLink(object sender, EventArgs args)
    {
        if (!setupReady)
            return;

        var dynamicLinkEventArgs = args as ReceivedDynamicLinkEventArgs;
        string link = dynamicLinkEventArgs.ReceivedDynamicLink.Url.OriginalString;

        SGDebug.Log("Received dynamic link: " + link);

        SGEnvironment.SetDynamicLink(link);
    }

    // ref: https://firebase.google.com/docs/database/unity/start?authuser=0
    // ref: https://firebase.google.com/docs/database/security/user-security?hl=pt-br
    // ref: https://github.com/firebase/quickstart-unity/blob/master/database/testapp/Assets/Firebase/Sample/Database/UIHandler.cs
    public static DatabaseReference RealTimeDatabase(string referenceKey = "")
    //public static DatabaseReference RealTimeDatabase(string realTimeDataseURL = "", string referenceKey = "")
    {
        if (!setupReady)
            return null;

        // Set up the Editor before calling into the realtime database.
        FirebaseApp app = FirebaseApp.DefaultInstance;
        //app.SetEditorDatabaseUrl(realTimeDataseURL);
        if (app.Options.DatabaseUrl == null)
            return null;

        if (string.IsNullOrEmpty(referenceKey))
        {
            return FirebaseDatabase.DefaultInstance.RootReference;
        }
        else
        {
            return FirebaseDatabase.DefaultInstance.GetReference(referenceKey);
        }
    }

    // true => if user has already authenticated
    public static bool AuthGetPersistent()
    {
        if (!setupReady)
            return false;
        if (authPhases == -1)
            return false;
        lastError = "";
        authPhases = -1;

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser == null)
            return false;
        if (string.IsNullOrEmpty(auth.CurrentUser.UserId))
            return false;

        AuthUpdateUser();
        authPhases = 1;

        return true;
    }

    // Anonymous
    // ref: https://firebase.google.com/docs/auth/unity/anonymous-auth?authuser=0
    public static void Authentication()
    {
        if (!setupReady)
            return;
        if (authPhases == -1)
            return;
        lastError = "";
        authPhases = -1;

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                lastError = task.Exception.ToString();
                authPhases = 0;
                return;
            }
            if (task.IsFaulted)
            {
                lastError = task.Exception.InnerExceptions[0].InnerException.Message;
                authPhases = 0;
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            AuthUpdateUser();
            // Debug.LogFormat("User signed in successfully: ({0})", newUser.UserId);
            authPhases = 1;
        });
    }

    // Email+pass
    // ref: https://firebase.google.com/docs/auth/unity/password-auth
    public static void Authentication(string email, string password)
    {
        if (!setupReady)
            return;
        if (authPhases == -1)
            return;
        lastError = "";
        authPhases = -1;

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                lastError = task.Exception.ToString();
                authPhases = 0;
                return;
            }

            if (task.IsFaulted)
            {
                lastError = task.Exception.InnerExceptions[0].InnerException.Message;
                authPhases = 0;
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;

            //email verified ?
            if (newUser.IsEmailVerified)
            {
                // Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
                AuthUpdateUser();
                // Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                authPhases = 1;
            } else
            {
                lastError = "You must verify your email to continue!";
                AuthSignOut();
                authPhases = 0;
            }
        });
    }

    // Email+pass+name
    // ref: https://firebase.google.com/docs/auth/unity/password-auth
    public static void AuthCreateUser(string email, string password)
    {
        if (!setupReady)
            return;
        if (authPhases == -1)
            return;
        lastError = "";
        authPhases = -1;

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                lastError = task.Exception.ToString();
                authPhases = 0;
                return;
            }

            if (task.IsFaulted)
            {
                lastError = task.Exception.InnerExceptions[0].InnerException.Message;
                authPhases = 0;
                //foreach (Exception ie in e.InnerExceptions)
                //    Console.WriteLine("{0}: {1}", ie.GetType().Name,
                //                      ie.Message);

                return;
            }

            Firebase.Auth.FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                user.SendEmailVerificationAsync().ContinueWith(task2 => {
                    if (task2.IsCanceled)
                    {
                        lastError = task2.Exception.ToString();
                        authPhases = 0;
                        return;
                    }
                    if (task2.IsFaulted)
                    {
                        lastError = task2.Exception.InnerExceptions[0].InnerException.Message;
                        authPhases = 0;
                        return;
                    }

                    authPhases = 1;
                    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSignUp);
                    AuthUpdateUser();
                    // Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                });
            }
        });
    }

    // Email
    // ref: https://firebase.google.com/docs/auth/unity/manage-users
    public static void AuthResetPass(string email)
    {
        if (!setupReady)
            return;
        if (authPhases == -1)
            return;
        lastError = "";
        authPhases = -1;

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
            if (task.IsCanceled)
            {
                lastError = task.Exception.ToString();
                authPhases = 0;
                return;
            }

            if (task.IsFaulted)
            {
                lastError = task.Exception.InnerExceptions[0].InnerException.Message;
                authPhases = 0;
                return;
            }

            authPhases = 1;
        });
    }

    // Phone/SMS
    // ref: https://firebase.google.com/docs/auth/unity/phone-auth
    public static void Authentication(string smsCode)
    {
        if (!setupReady)
            return;
        if (authPhases == -1)
            return;
        lastError = "";
        authPhases = -1;
    }

    private static void AuthUpdateUser()
    {
        if (!setupReady)
            return;
        if (auth == null)
            return;

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user == null)
            return;

        userName = user.DisplayName;
        userEmail = user.Email;
 
        SetUserId(user.UserId);

        FirebaseAnalytics.SetUserProperty("email", userEmail);
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
    }

    // ref: https://firebase.google.com/docs/auth/unity/manage-users
    public static void AuthDeleteUser()
    {
        if (!setupReady)
            return;
        if (auth == null)
            return;
        if (authPhases == -1)
            return;
        lastError = "";
        authPhases = 0;

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user == null)
            return;

        user.DeleteAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                lastError = task.Exception.ToString();
                AuthSignOut();
                return;
            }

            if (task.IsFaulted)
            {
                lastError = task.Exception.InnerExceptions[0].InnerException.Message;
                AuthSignOut();
                return;
            }
            
            AuthSignOut();
        });
    }

    public static void AuthCancelAttempts()
    {
        if (auth == null)
            authPhases = 0;
        else
            authPhases = 1;
    }

    public static void AuthSignOut()
    {
        userName = "";
        userEmail = "";
        userId = "";

        authPhases = 0;

        if (!setupReady)
            return;
        if (auth == null)
            return;

        auth.SignOut();
        auth.Dispose();
        auth = null;
        if (credential != null) {
            credential.Dispose();
            credential = null;
        }
    }
}