using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SGUITrackingSlider : MonoBehaviour
{
    public string parameter = "";

    private Slider slider = null;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        // set callbacks
        slider.onValueChanged.AddListener(delegate { OnClick(); });
    }

    void OnClick() => SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.OnClick, "SliderName", slider.name, "Parameter", parameter, "Value", slider.value.ToString());
}
