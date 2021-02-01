using UnityEngine;
using System.Collections;

public class BootManager : MonoBehaviour
{
    public int waitForSeconds = 2;

    private bool showLogo = false;


    void Start()
    {
        StartCoroutine(WaitLogo());
    }

    void Update()
    {
        if (SGFirebase.SetupReady && showLogo)
        {
            SGScenes.LoadScene(SGScenes.IndexHome);
        }
    }

    IEnumerator WaitLogo()
    {
        yield return new WaitForSeconds(waitForSeconds);
        showLogo = true;
    }
}
