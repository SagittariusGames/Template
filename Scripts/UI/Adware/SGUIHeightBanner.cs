using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SGUIHeightBanner : MonoBehaviour
{
    public string remoteSettingsAdsEnable = "ads_enable";
    public float adBottom = 8;
    private RectTransform rectTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        string enable = SGFirebase.RemoteSettings(remoteSettingsAdsEnable, "0");
        if (enable == "0")
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
        } else
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, adBottom);
        }
    }
}
