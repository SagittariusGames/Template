using Firebase.Analytics;

public class SGAnalytics
{
    public enum AnalyticsEvents { StartGame, QuitGame, GameOver, LevelCompleted, Search, LevelStart, LowMemory, LoadDefault, OnClick, AdStart, AdFailed, AdClose, AdCompleted };
    
    public static void AnalyticsTraking(AnalyticsEvents analyticsEvent, string parameterName = "", string parameterValue = "", string parameterName2 = "", string parameterValue2 = "", string parameterName3 = "", string parameterValue3 = "")
    {
        if (!SGFirebase.SetupReady)
            return;

        switch (analyticsEvent)
        {
            case AnalyticsEvents.StartGame:
                Parameter[] startGame = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                };
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen, startGame);
                FirebaseAnalytics.SetCurrentScreen(SGScenes.GetActiveSceneName, SGScenes.GetActiveSceneName);
                break;
            case AnalyticsEvents.QuitGame:
                Parameter[] quit = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                };
                FirebaseAnalytics.LogEvent("Quit", quit);
                break;
            case AnalyticsEvents.LevelStart:
                Parameter[] levelStart = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                };
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, levelStart);
                break;
            case AnalyticsEvents.Search:
                Parameter[] search = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                    new Parameter(parameterName, parameterValue),
                };
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSearch, search);
                break;
            case AnalyticsEvents.LevelCompleted:
                Parameter[] levelCompleted = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                };
                FirebaseAnalytics.LogEvent("LevelCompleted", levelCompleted);
                break;
            case AnalyticsEvents.LowMemory:
                Parameter[] lowMemory = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                };
                FirebaseAnalytics.LogEvent("LowMemory", lowMemory);
                break;
            case AnalyticsEvents.LoadDefault:
                Parameter[] loadDefault = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                };
                FirebaseAnalytics.LogEvent("LoadDefault", loadDefault);
                break;
            case AnalyticsEvents.GameOver:
                Parameter[] gameOver = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                };
                FirebaseAnalytics.LogEvent("GameOver", gameOver);
                break;
            case AnalyticsEvents.OnClick:
                Parameter[] onChangeData = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                    new Parameter(parameterName, parameterValue),
                    new Parameter(parameterName2, parameterValue2),
                    new Parameter(parameterName3, parameterValue3),
                };
                FirebaseAnalytics.LogEvent("OnChangeData", onChangeData);
                break;
            case AnalyticsEvents.AdStart:
                Parameter[] adStart = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                    new Parameter(parameterName, parameterValue),
                };
                FirebaseAnalytics.LogEvent("AdStart", adStart);
                break;
            case AnalyticsEvents.AdClose:
                Parameter[] adClose = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                    new Parameter(parameterName, parameterValue),
                };
                FirebaseAnalytics.LogEvent("AdClose", adClose);
                break;
            case AnalyticsEvents.AdCompleted:
                Parameter[] adCompleted = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                    new Parameter(parameterName, parameterValue),
                };
                FirebaseAnalytics.LogEvent("AdCompleted", adCompleted);
                break;
            case AnalyticsEvents.AdFailed:
                Parameter[] adFailed = {
                    new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                    new Parameter(parameterName, parameterValue),
                };
                FirebaseAnalytics.LogEvent("AdFailed", adFailed);
                break;
            default:
                break;
        }
    }

    public static void AnalyticsTraking(string name, string parameterName, int parameterValue)
    {
        if (SGFirebase.SetupReady)
        {
            Parameter[] analyticsTraking = {
                new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                new Parameter(parameterName, parameterValue),
            };
            FirebaseAnalytics.LogEvent(name, analyticsTraking);
        }
    }
    public static void AnalyticsTraking(string name, string parameterName, float parameterValue)
    {
        if (SGFirebase.SetupReady)
        {
            Parameter[] analyticsTraking = {
                new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                new Parameter(parameterName, parameterValue),
            };
            FirebaseAnalytics.LogEvent(name, analyticsTraking);
        }
    }
    public static void AnalyticsTraking(string name, string parameterName, string parameterValue)
    {
        if (SGFirebase.SetupReady)
        {
            Parameter[] analyticsTraking = {
                new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                new Parameter(parameterName, parameterValue),
            };
            FirebaseAnalytics.LogEvent(name, analyticsTraking);
        }
    }
    public static void AnalyticsTraking(string name, string parameterName, bool parameterValue)
    {
        if (SGFirebase.SetupReady)
        {
            Parameter[] analyticsTraking = {
                new Parameter(FirebaseAnalytics.ParameterLevelName, SGScenes.GetActiveSceneName),
                new Parameter(parameterName, parameterValue?1:0),
            };
            FirebaseAnalytics.LogEvent(name, analyticsTraking);
        }
    }

    public static void AnalyticsTrakingLifecycle(string parameterName, bool parameterValue = true)
    {
        AnalyticsTraking("Lifecycle", parameterName, parameterValue);
    }

    public static void ResetData()
    {
        if (SGFirebase.SetupReady)
        {
            FirebaseAnalytics.ResetAnalyticsData();
        }
    }
}
