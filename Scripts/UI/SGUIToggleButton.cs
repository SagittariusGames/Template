using UnityEngine;

public class SGUIToggleButton : MonoBehaviour
{
    public GameObject Background = null;

    // Start is called before the first frame update
    public void UnSetBackground(bool value)
    {
        if (Background)
            Background.SetActive(!value);
    }
}
