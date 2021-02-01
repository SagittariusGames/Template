using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar))]
[RequireComponent(typeof(Image))]
public class SGUIScrollbarAutoHide : MonoBehaviour
{
    public GameObject child = null;
    public float waitingSeconds = 1;

    private Image image = null;
    private Scrollbar scrollBar = null;

    private float secondsremaining = 0;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        scrollBar = this.GetComponent<Scrollbar>();

        scrollBar.onValueChanged.AddListener(delegate { Show(); });
    }

    // Update is called once per frame
    void Update()
    {
        secondsremaining += Time.deltaTime;

        if (secondsremaining >= waitingSeconds)
        {
            Hide();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Show()
    {
        image.enabled = true;
        child.SetActive(true);
        secondsremaining = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Hide()
    {
        image.enabled = false;
        child.SetActive(false);
    }
}
