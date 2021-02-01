using UnityEngine;
using UnityEngine.Diagnostics;
using System.IO;
using System;

public class SGDebug
{
    public delegate void EventChanges();
    public static event EventChanges LogTextChanged = null;
    public static event EventChanges DrawLineChanged = null;
    public static event EventChanges DisableAllEnemiesChanged = null;
    public static event EventChanges DisableGUIChanged = null;
    public static event EventChanges PlayerInvincibleChanged = null;
    public static event EventChanges UnlockAllItemsChanged = null;

    public static bool DebugMode { get; set; } = false;
    private static string sLogText = "** THIS IS A DEBUG BUILD ! **";
    private static string sKey;
    private static string sValue;
    private static bool drawLine = false;
    private static bool disableAllEnemies = false;
    private static bool disableGUI = false;
    private static bool playerInvincible = false;
    private static bool unlockAllItems = false;

    public static void Log (object message)
    {
        if (DebugMode)
        {
            SGFirebase.Log(message.ToString());
            Debug.Log(message);
            LogText = message.ToString();
        }
    }

    public static void LogWarning(object message)
    {
        if (DebugMode)
        {
            SGFirebase.Log("Warning: " + message.ToString());
            Debug.LogWarning(message);
            LogText = "[W]" + message.ToString();
        }
    }

    public static void LogError(object message)
    {
        if (DebugMode)
        {
            SGFirebase.Log("Error: " + message.ToString());
            Debug.LogError(message);
            LogText = "[E]" + message.ToString();
        }
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.0f, bool depthTest = true)
    {
        if (DebugMode && drawLine)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }
    }

    public static void SetCustomKey(string key, string value)
    {
        sKey = key;
        sValue = value;
        SGFirebase.SetCustomKey(key, value);
        Debug.Log("Key: " + key + " Value: " + value);
    }

    public static void SetUserId(string id)
    {
        SGFirebase.SetUserId(id);
        Debug.Log("SetUserId: " + id);
    }

    public static void Exception(System.Exception e)
    {
        SGFirebase.LogException(e);
        Debug.LogException(e);
    }

    public static void Exception(System.Exception e, UnityEngine.Object context)
    {
        Exception(e);
        Debug.LogException(e, context);
    }

    public static void ForceException()
    {
        SGFirebase.Log("FORCED EXCEPTION!");
        // Throw an exception implementation
        throw new System.Exception("FORCED EXCEPTION!");
#if UNITY_EDITOR
        // nothing
#else
        // Causes an error that will crash the app at the platform level (Android or iOS)
        throw new InvalidOperationException("FORCED EXCEPTION!");
        Utils.ForceCrash(ForcedCrashCategory.FatalError);
#endif
    }

    public static string LogText {
        get {
            return sLogText;
        }
        set {
            if (DebugMode)
            {
                string newValue;
                System.Diagnostics.StackFrame trace = new System.Diagnostics.StackFrame(2, true);
                if (trace.GetMethod() != null)
                {
                    newValue = "- " + value + "   <i>(at " + Path.GetFileName(trace.GetFileName()) + ":" + trace.GetFileLineNumber() + " on " + trace.GetMethod().Name + ")</i>";
                }
                else
                {
                    newValue = "- " + value + "   <i>(at " + Path.GetFileName(trace.GetFileName()) + ":" + trace.GetFileLineNumber();
                }
                sLogText = newValue + "\n" + sLogText;
                Debug.Log(newValue);
                // event
                LogTextChanged?.Invoke();
            }
        }
    }

    public static bool SetDrawLine {
        get {
            return drawLine;
        }
        set {
            drawLine = value;
            DrawLineChanged?.Invoke();
        }
    }

    public static bool DisableAllEnemies {
        get {
            return disableAllEnemies;
        }
        set {
            disableAllEnemies = value;
            DisableAllEnemiesChanged?.Invoke();
        }
    }

    public static bool DisableGUI {
        get {
            return disableGUI;
        }
        set {
            disableGUI = value;
            DisableGUIChanged?.Invoke();
        }
    }

    public static bool PlayerInvincible {
        get {
            return playerInvincible;
        }
        set {
            playerInvincible = value;
            PlayerInvincibleChanged?.Invoke();
        }
    }

    public static bool UnlockAllItems {
        get {
            return unlockAllItems;
        }
        set {
            unlockAllItems = value;
            UnlockAllItemsChanged?.Invoke();
        }
    }

}
