using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SGUIToggleTermsUse : MonoBehaviour
{
    private Toggle toggle = null;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = SGSecurity.TermsUse;
        ChangeColor(toggle.isOn);
        // set callbacks
        toggle.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    private void OnValueChanged()
    {
        SGSecurity.TermsUse = toggle.isOn;
        ChangeColor(toggle.isOn);

        /*
        if (!toggle.isOn)
            AGUIMisc.ShowToast("If you do not accept the terms, you will not be able to continue using this app!", AGUIMisc.ToastLength.Long);
        */
    }

    private void ChangeColor(bool isOn)
    {
        ColorBlock cb = toggle.colors;
        if (isOn)
        {
            cb.normalColor = Color.green;
            cb.selectedColor = Color.green;
        } else
        {
            cb.normalColor = Color.red;
            cb.selectedColor = Color.red;
        }
        toggle.colors = cb;
    }
}
