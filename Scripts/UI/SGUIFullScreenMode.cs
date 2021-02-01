using UnityEngine;

public class SGUIFullScreenMode : MonoBehaviour
{
    public GameObject mainHeader = null;// to hide
    public RectTransform mainContent = null;// to expand
    public GameObject mainFooter = null;// to hide
    public bool forceLandscapeMode = true;

    private Vector2 offsetMin;
    private Vector2 offsetMax;
    private bool fullScreen = false;

    private ScreenOrientation originalOrientation;

    void Start()
    {
        originalOrientation = Screen.orientation;

        offsetMin = mainContent.offsetMin;
        offsetMax = mainContent.offsetMax;
    }

    void OnDisable()
    {
        // back to original
        OnValueChanged(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private void FullScreen(bool value)
    {
        int nSibling;

        //force ?
        if (forceLandscapeMode)
        {
            if (value)
                Screen.orientation = ScreenOrientation.Landscape;
            else
                Screen.orientation = originalOrientation;
        }

        // exibir/ocultar tios
        if (mainHeader)
            mainHeader.SetActive(!value);
        if (mainFooter)
            mainFooter.SetActive(!value);
        // tamanho do grande pai
        if (value)
        {
            mainContent.offsetMin = new Vector2(0, 0);
            mainContent.offsetMax = new Vector2(0, 0);
        }
        else
        {
            mainContent.offsetMin = offsetMin;
            mainContent.offsetMax = offsetMax;
        }
        // exibir/ocultar os irmãos
        nSibling = this.transform.GetSiblingIndex();
        for (int n =0; n < this.transform.parent.childCount; n++)
        {
            if (n != nSibling)
                this.transform.parent.GetChild(n).gameObject.SetActive(!value);
        }
        // exibir/ocultar barra de botões do Android
        Screen.fullScreen = value;
        fullScreen = value;
    }

    public void OnClick()
    {
        FullScreen(!fullScreen);
    }

    public void OnValueChanged(bool value)
    {
        fullScreen = value;
        FullScreen(fullScreen);
    }
}
