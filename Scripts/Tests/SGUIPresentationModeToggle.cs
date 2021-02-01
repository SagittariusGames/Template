using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SGUIPresentationModeToggle : MonoBehaviour
{
    public string remoteSettings = "presentation_mode";
    public float timeToClick = 1.0f;
    
    private Toggle toggle = null;

    // Start is called before the first frame update
    void Start()
    {
        string activate = SGFirebase.RemoteSettings(remoteSettings, "0");

        toggle = GetComponent<Toggle>();
        if (activate == "1")
            StartCoroutine(Click());
    }

    public IEnumerator Click()
    {
        yield return new WaitForSeconds(timeToClick);
        toggle.isOn = true;
    }
}
