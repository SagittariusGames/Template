using UnityEngine;
using UnityEngine.Events;

public class SGUIAlertDialog : MonoBehaviour
{
    public AlertDialogPrefab AlertDialogPrefab = null;
    public string title = "";
    public string alert = "";
    public string ok = "OK";
    public string cancel = "Cancel";
    private AlertDialogPrefab newAlertDialog = null;

    [Header("Event buttons")]
    // user callbacks
    public UnityEvent OnClickOK = null;
    public UnityEvent OnClickCancel = null;

    public void OnClick_Open()
    {
        // instatiate new prefab
        newAlertDialog = Instantiate(AlertDialogPrefab);
        newAlertDialog.gameObject.SetActive(true);
        // setup
        newAlertDialog.SetUp(title, alert, ok, cancel);
        // set callbacks
        newAlertDialog.btOk.onClick.AddListener(delegate { ClickOK(); });
        newAlertDialog.btCancel.onClick.AddListener(delegate { ClickCancel(); });
    }

    private void ClickOK()
    {
        if (OnClickOK != null)
            OnClickOK.Invoke();
        Destroy(newAlertDialog.gameObject);
    }

    private void ClickCancel()
    {
        if (OnClickCancel != null)
            OnClickCancel.Invoke();
        Destroy(newAlertDialog.gameObject);
    }
}
