using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class SGUITrackingInputField : MonoBehaviour
{
    public string parameter = "";

    private InputField inputField = null;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();

        // set callbacks
        inputField.onEndEdit.AddListener(delegate { OnClick(); });
    }

    void OnClick() => SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.OnClick, "ToggleName", inputField.name, "Parameter", parameter, "Value", inputField.text);
}
