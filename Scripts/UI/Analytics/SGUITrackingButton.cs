using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SGUITrackingButton : MonoBehaviour
{
    public string parameter = "";

    private Button button = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        // set callbacks
        button.onClick.AddListener(delegate { OnClick(); });
    }

    void OnClick() => SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.OnClick, "ButtonName", button.name, "Parameter", parameter);
}
