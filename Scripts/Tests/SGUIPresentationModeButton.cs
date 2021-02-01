using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SGUIPresentationModeButton : MonoBehaviour
{
    public string remoteSettings = "presentation_mode";
    public float timeToClick = 1.0f;

    private Button button = null;

    // Start is called before the first frame update
    void Start()
    {
        string activate = SGFirebase.RemoteSettings(remoteSettings, "0");

        button = GetComponent<Button>();
        if (activate == "1")
            StartCoroutine(Click());
    }

    public IEnumerator Click()
    {
        yield return new WaitForSeconds(timeToClick);
        button.onClick.Invoke();
    }
}
