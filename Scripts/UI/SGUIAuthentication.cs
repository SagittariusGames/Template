using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SGUIAuthentication : MonoBehaviour
{
    [Header("Login")]
    public GameObject Login = null;
    public Text errorLogin = null;
    public Text infoLogin = null;
    public InputField emailLogin = null;
    public InputField passLogin = null;

    [Header("Register")]
    public GameObject Register = null;
    public Text errorRegister = null;
    public InputField emailRegister = null;
    public InputField passRegister = null;

    [Header("Forgot")]
    public GameObject Forgot = null;
    public Text errorForgot = null;
    public InputField emailForgot = null;

    [Space(10)]
    public GameObject loadingPrefab = null;
    [Tooltip("in seconds")]
    public float timeout = 10f;

    private bool authAttemptLogin = false;
    private bool authAttemptRegister = false;
    private bool authAttemptForgot = false;

    private GameObject loading = null;

    private float timecount = 0f;

    void Start()
    {
        errorLogin.text = "";
        infoLogin.text = "";
        errorRegister.text = "";
        errorForgot.text = "";
    }

    void Update()
    {
        if (authAttemptLogin)
        {
            timecount += Time.deltaTime;

            if (SGFirebase.Auth == 0)
            {
                errorLogin.text = SGFirebase.LastError;
                authAttemptLogin = false;

                Loading(false);
            }
            else if (SGFirebase.Auth == 1)
            {
                errorLogin.text = "";
                errorRegister.text = "";
                authAttemptLogin = false;

                LoadHome();
                //Loading(false);
            }
        }

        if (authAttemptRegister)
        {
            timecount += Time.deltaTime;

            if (SGFirebase.Auth == 0)
            {
                errorRegister.text = SGFirebase.LastError;
                authAttemptRegister = false;

                Loading(false);
            }
            else if (SGFirebase.Auth == 1)
            {
                errorLogin.text = "";
                errorRegister.text = "";
                authAttemptRegister = false;

                infoLogin.text = "Registration successful. Please do not forget to validate your email before the first login.";
                OnClick_ActiveLogin();
                Loading(false);
            }
        }

        if (authAttemptForgot)
        {
            timecount += Time.deltaTime;

            if (SGFirebase.Auth == 0)
            {
                errorForgot.text = SGFirebase.LastError;
                authAttemptForgot = false;

                Loading(false);
            }
            else if (SGFirebase.Auth == 1)
            {
                errorLogin.text = "";
                errorForgot.text = "";
                authAttemptForgot = false;

                infoLogin.text = "Email successfully sent. Please follow the instructions on it before login.";
                OnClick_ActiveLogin();
                Loading(false);
            }
        }
        
        if (timecount >  timeout)
        {
            errorLogin.text = "The server response is taking too long... try again ?";
            errorRegister.text = "The server response is taking too long... try again ?";
            errorForgot.text = "The server response is taking too long... try again ?";
            authAttemptLogin = false;
            authAttemptRegister = false;
            authAttemptForgot = false;
            SGFirebase.AuthCancelAttempts();
            Loading(false);
            timecount = 0f;
        }
    }

    public void OnClick_Login()
    {
        Loading(true);
        SGFirebase.Authentication(emailLogin.text, passLogin.text);
        authAttemptLogin = true;
        timecount = 0f;
    }

    public void OnClick_Register()
    {
        Loading(true);
        SGFirebase.AuthCreateUser(emailRegister.text, passRegister.text);
        authAttemptRegister = true;
        timecount = 0f;
    }
    public void OnClick_ForgotPass()
    {
        Loading(true);
        SGFirebase.AuthResetPass(emailForgot.text);
        authAttemptForgot = true;
        timecount = 0f;
    }

    private void Loading (bool active)
    {
        if (active && loading == null)
        {
            loading = Instantiate(loadingPrefab);
            loading.SetActive(true);
        }
        else
        {
            Destroy(loading);
            loading = null;
        }
    }

    public void LoadHome ()
    {
        SceneManager.LoadScene(SGScenes.IndexHome);
    }

    public void OnClick_ActiveLogin ()
    {
        Login.SetActive(true);
        Register.SetActive(false);
        Forgot.SetActive(false);
    }

    public void OnClick_ActiveRegister()
    {
        Register.SetActive(true);
        Login.SetActive(false);
        Forgot.SetActive(false);
    }

    public void OnClick_ActiveForgot()
    {
        Forgot.SetActive(true);
        Register.SetActive(false);
        Login.SetActive(false);
    }

    public void OnClick_ActiveSignOut()
    {
        Forgot.SetActive(false);
        Register.SetActive(false);
        Login.SetActive(false);
    }
}
