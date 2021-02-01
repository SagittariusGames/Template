using UnityEngine;

public class SGUITextOverlay : MonoBehaviour
{
    public TextOverlayPrefab TextOverlayPrefab = null;
    public string title = "";
    [TextArea]
    public string text = "";

    public void OnClick_Open()
    {
        // instatiate new prefab
        TextOverlayPrefab newTextOverlay = Instantiate(TextOverlayPrefab);
        TextOverlayPrefab.gameObject.SetActive(true);

        // set title
        newTextOverlay.SetTitle(title);

        // set text
        newTextOverlay.LoadText(text);
    }
}
