using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class SGUIInputFiledPlayerPrefs : MonoBehaviour
{
    public string defaultURL = "https://translate.google.com/#view=home&op=translate&sl=en&tl=pt&text=";
    public string keyURL = "";

    private InputField inputField = null;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();

        //set previous value
        string url = "";
#if UNITY_EDITOR
        url = SGStorage.GetGenericPref(keyURL, defaultURL);
#elif UNITY_ANDROID
        url = SGStorage.GetGenericPref(keyURL, defaultURL);
#endif
        inputField.text = url;

        // set callbacks
        inputField.onEndEdit.AddListener(delegate { OnValueChanged(); });
    }

    private void OnValueChanged()
    {
        SGStorage.SetGenericPref(keyURL, inputField.text);
    }
}
