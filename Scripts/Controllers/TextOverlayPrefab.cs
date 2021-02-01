using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextOverlayPrefab : MonoBehaviour
{
    public Text title = null;
    public TextMeshProUGUI text = null;

    public void OnClick_Close()
    {
        Destroy(gameObject);
    }

    public void SetTitle (string newTitle)
    {
        title.text = newTitle;
    }

    public void LoadText (string newText)
    {
        text.text = newText;
    }

    /*
    public void LoadTextFromFile (string file)
    {

    }
    */
}
