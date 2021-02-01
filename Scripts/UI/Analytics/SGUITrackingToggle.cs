using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SGUITrackingToggle : MonoBehaviour
{
    public string parameter = "";

    private Toggle toggle = null;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();

        // set callbacks
        toggle.onValueChanged.AddListener(delegate { OnClick(); });
    }

    void OnClick() => SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.OnClick, "ToggleName", toggle.name, "Parameter", parameter, "Value", toggle.isOn?"True":"False");
}
