using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class SGUITrackingDropdown : MonoBehaviour
{
    public string parameter = "";

    private Dropdown dropdown = null;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<Dropdown>();

        // set callbacks
        dropdown.onValueChanged.AddListener(delegate { OnClick(); });
    }

    void OnClick() => SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.OnClick, "ToggleName", dropdown.name, "Parameter", parameter, "Value", dropdown.captionText.text);
}
