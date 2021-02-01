using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(Text))]
public class SGUITextStatusSystem : MonoBehaviour
{
    public string placeholderStatus = "{status}";
    //public string optionSystemStatus = "";
    public enum Options { InternetAvailable, ProductName, CompanyName, PackageName, UnityVersion, Version }
    public Options optionSystemStatus;


    private Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

        switch (optionSystemStatus)
        {
            case Options.InternetAvailable:
                StartCoroutine(InternetAvailable());
                break;
            case Options.ProductName:
                ProductName();
                break;
            case Options.CompanyName:
                CompanyName();
                break;
            case Options.PackageName:
                PackageName();
                break;
            case Options.UnityVersion:
                UnityVersion();
                break;
            case Options.Version:
                Version();
                break;
            default:
                text.text = "";
                break;
        }
    }

    // Ref: https://docs.unity3d.com/Manual/UnityWebRequest-RetrievingTextBinaryData.html
    private IEnumerator InternetAvailable ()
    {
        bool isInternetAvaliable = false;

        UnityWebRequest www = UnityWebRequest.Get("http://www.google.com");
        www.timeout = 10;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            isInternetAvaliable = false;
        }
        else
        {
            // Show results as text
            if (www.downloadHandler.text.Length > 300)
                isInternetAvaliable = true;
        }

        SetPlaceholder(isInternetAvaliable.ToString());
    }

    private void ProductName() => SetPlaceholder(Application.productName);

    private void CompanyName() => SetPlaceholder(Application.companyName);

    private void PackageName() => SetPlaceholder(Application.identifier + " " + Application.installerName);

    private void UnityVersion() => SetPlaceholder(Application.unityVersion);
    private void Version() => SetPlaceholder(Application.version);

    private void SetPlaceholder(string newText) => text.text = placeholderStatus.Length > 0 ? text.text.Replace(placeholderStatus, newText) : newText;

}
