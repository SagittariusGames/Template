using UnityEngine.UI;
using UnityEngine;

public class AlertDialogPrefab : MonoBehaviour
{
    public Text title = null;
    public Text alert = null;
    public Text ok = null;
    public Text cancel = null;
    public Button btOk = null;
    public Button btCancel = null;

    public void SetUp(string newTitle, string newAlert, string newOK = "OK", string newCancel = "Cancel")
    {
        title.text = newTitle;
        alert.text = newAlert;
        ok.text = newOK;
        cancel.text = newCancel;
    }
}
