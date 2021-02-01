using System.Collections.Generic;
using UnityEngine;

public class SGStorage
{
    public delegate void EventChanges();
    public static event EventChanges StoragePrefChanged = null;

    private static Dictionary<string, string> genericPref = null;

    private static Dictionary<string, List<SGGenericList>> genericDictionary = null;

    private static void SetupGenericDictionary()
    {
        if (genericDictionary == null)
            genericDictionary = new Dictionary<string, List<SGGenericList>>();
    }

    public static void AddGenericList(string name, List<SGGenericList> newList)
    {
        SetupGenericDictionary();

        genericDictionary.Add(name, newList);
    }

    public static List<SGGenericList> GetGenericList(string name)
    {
        SetupGenericDictionary();

        if (genericDictionary.ContainsKey(name))
            return genericDictionary[name];
        else
            return NewGenericList(name);
    }

    public static List<SGGenericList> NewGenericList(string name)
    {
        List<SGGenericList> newList = new List<SGGenericList>();
        AddGenericList(name, newList);

        return newList;
    }

    // ref: https://stackoverflow.com/questions/3069431/listobject-removeall-how-to-create-an-appropriate-predicate
    public static bool RemoveAllGenericList(SGGenericList value)
    {
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public static string GetGenericPref(string key, string defaultValue = "") {
        string value;

        SetupGenericPref();
        
        if (genericPref.ContainsKey(key))
            return genericPref[key];
        else
        {
            value = PlayerPrefs.GetString(key, defaultValue);
            genericPref.Add(key, value);
            return value;
        }
    }

    public static void SetGenericPref(string key, string newValue)
    {
        SetupGenericPref();

        if (genericPref.ContainsKey(key))
            genericPref[key] = newValue;
        else
            genericPref.Add(key, newValue);

        PlayerPrefs.SetString(key, newValue);

        StoragePrefChanged?.Invoke();
    }

    private static void SetupGenericPref ()
    {
        if (genericPref == null)
            genericPref = new Dictionary<string, string>();
    }

    public static void ClearGenericPrefs ()
    {
        if (genericPref == null)
            return;

        foreach (string key in genericPref.Keys)
        {
            PlayerPrefs.DeleteKey(key);
        }
        genericPref.Clear();
        genericPref = null;
    }

    public static Dictionary<string, string>.KeyCollection GetGenericPrefs ()
    {
        if (genericPref == null)
            genericPref = new Dictionary<string, string>();

        return genericPref.Keys;
    }
}
