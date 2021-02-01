using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SGUILoginButton : MonoBehaviour
{
    private Button bt = null;

    // Start is called before the first frame update
    void Start()
    {
        bt = GetComponent<Button>();
    }

    void Update()
    {
        bt.interactable = SGFirebase.SetupReady;
    }

    public void OnClick_Login()
    {
        SceneManager.LoadScene(SGScenes.IndexAuth);
    }

    public void OnClick_Logout()
    {
        SGFirebase.AuthSignOut();
    }

    public void OnClick_Delete()
    {
        SGFirebase.AuthDeleteUser();
    }
}
