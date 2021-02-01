using UnityEngine;

public class SGUIDynamicLink : MonoBehaviour
{
    public string firebaseDynamicLink = "";

    public void OnClick()
    {
        if (!string.IsNullOrEmpty(firebaseDynamicLink))
            SGAppLauncher.LaunchURL(firebaseDynamicLink);
    }
}
