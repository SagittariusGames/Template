using UnityEngine;
using UnityEngine.UI;

public class DropdownOverlayPrefab : MonoBehaviour
{
    public GameObject content = null;

    private Toggle[] toogles = null;

    void Start()
    {
        toogles = content.GetComponentsInChildren<Toggle>();
    }

    public void OnSearchValueChanged(string value)
    {
        Text text;
        int findings = 0;
        int count = 0;
        int index = 0;
        
        //foreach (Transform item in content.transform)
        foreach (Toggle toogle in toogles)
        {
            text = toogle.GetComponentInChildren<Text>();
            if (text.text.Trim().ToLower().Contains(value.Trim().ToLower()))
            {
                findings++;
                index = count;
            }
            count++;
        }
        if (findings == 1)
        {
            toogles[index].isOn = false;
        }
    }

    public void OnCloseClick()
    {
        toogles[0].isOn = false;
    }
}
