using UnityEngine;

public class SGUILoadingCard : MonoBehaviour
{
    public GameObject active = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Loading();
    }

    public void Loading() => active.SetActive(true);

    public void UnLoading() => active.SetActive(false);
}
