using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Image))]
public class SGUIImageThumbnail : MonoBehaviour
{
    public string imageURL = "";
    public int timeout = 2000;

    private Image image = null;

    private bool ready = false;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    void OnEnable()
    {
        if (!string.IsNullOrEmpty(imageURL))
            StartCoroutine(GetImage(imageURL));
        ready = true;
    }

    void OnDisable()
    {
        ready = false;
    }

    // ref: https://docs.unity3d.com/Manual/UnityWebRequest-CreatingDownloadHandlers.html
    private IEnumerator GetImage(string url)
    {
        UnityWebRequest webRequest = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        webRequest.downloadHandler = texDl;
        yield return webRequest.SendWebRequest();
        if (!(webRequest.isNetworkError || webRequest.isHttpError))
        {
            Texture2D t = texDl.texture;
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 1f);
            image.sprite = s;
        }
    }

    public void SetURL (string url)
    {
        imageURL = url;
        if (!string.IsNullOrEmpty(imageURL) && ready)
            StartCoroutine(GetImage(imageURL));
    }
}
