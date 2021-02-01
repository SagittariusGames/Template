using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Dropdown))]
public class SGUIDropdownApps : MonoBehaviour, IPointerClickHandler
{
    public string defaultAndroidBundleID = "com.google.android.apps.translate";
    //public string defaultiOSBundleID = "com.google.android.apps.translate";
    public string keyApp = "";
    public GameObject loadingLoop = null;

    private List<string> apps = new List<string>();
    private string appID = "";
    private Dropdown dropdown = null;
    private bool alreadyPopulate = false;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.enabled = false;
        dropdown.ClearOptions();

        SetPreviousValue();
    }

    private void SetPreviousValue ()
    {
        //set previous value
#if UNITY_EDITOR
        appID = SGStorage.GetGenericPref(keyApp, defaultAndroidBundleID);
#elif UNITY_ANDROID
        appID = SGStorage.GetGenericPref(keyApp, defaultAndroidBundleID);
#endif
        apps.Insert(0, appID);
        dropdown.AddOptions(apps);
        dropdown.captionText.text = appID;
    }

    private void PopulateDate ()
    {
        if (alreadyPopulate)
        {
            dropdown.enabled = true;
            dropdown.Show();
            return;
        }
        
        // empty option
        apps.Add("");

        // add debug options
#if UNITY_EDITOR
        if (SGDebug.DebugMode)
        {
            for (int i = 0; i < 1000; i++)
            {
                apps.Add("AppTest " + i.ToString());
            }
        }
#endif
        
        // load installed apps
        foreach (string app in SGInstalledApps.InstalledApps())
        {
            if (app != null)
            {
                if (app.Trim().Length > 0)
                    apps.Add(app.Trim());
            }
        }
        // sort list
        apps.Sort();
        
        // populate
        dropdown.AddOptions(apps);

        // select first value
        dropdown.value = 0;
        dropdown.RefreshShownValue();
        /*
        int i = GetIndexByName(dropdown, appID);
        dropdown.value = i;
        dropdown.RefreshShownValue();
        */
        
        // set callbacks
        dropdown.onValueChanged.AddListener(delegate { OnValueChanged(); });

        alreadyPopulate = true;
        dropdown.enabled = true;
        dropdown.Show();
    }

    // Ref: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Button.OnPointerClick.html
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        StartCoroutine(ShowLoading());
    }

    private IEnumerator ShowLoading()
    {
        loadingLoop.SetActive(true);
        yield return new WaitForSeconds(1);
        PopulateDate();
        yield return null;
        loadingLoop.SetActive(false);
    }

    private void OnValueChanged ()
    {
        SGStorage.SetGenericPref(keyApp, dropdown.captionText.text);
        dropdown.enabled = false;
    }

    /*
    public void OnSearchValueChanged (string value)
    {
        int i = GetIndexByName(dropdown, value);
        if (i >= 0)
        {
            dropdown.value = i;
            dropdown.Set(i);
            dropdown.Select();
            dropdown.RefreshShownValue();
            Debug.LogWarning(i + " " + dropdown.options[i].text);

            //dropdown.transform.FindChild(value).GetComponent<Text>().text = dropdown.options[dropdown.value].text;
            //dropdown.transform.Find("Item 9: voce").GetComponent<Text>().text = dropdown.options[i].text;
        }
    }*/

    // ref: https://answers.unity.com/questions/1482361/get-dropdown-index-value-based-on-text-value-using.html
    private int GetIndexByName(Dropdown dropDown, string name)
    {
        if (dropDown == null) { return -1; } // or exception
        if (string.IsNullOrEmpty(name) == true) { return -1; }
        List<Dropdown.OptionData> list = dropDown.options;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].text.Equals(name)) { return i; }
        }
        return -1;
    }
}
