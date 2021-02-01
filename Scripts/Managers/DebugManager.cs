using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    [Header("Editor")]
    public bool startOnBoot = true;

    [Header("Settings")]
    public GameObject openButton = null;
    public GameObject overlayOptions = null;
    public GameObject textLogObject = null;
    public Text log = null;
    public Toggle textLog = null;
    public Toggle clearPref = null;
    public Toggle resetDataAnalytics = null;
    public Toggle drawLineLog = null;
    public Toggle disableAllEnemies = null;
    public Toggle disableGUI = null;
    public Toggle playerInvincible = null;
    public Toggle unlockAllItems = null;
    public Toggle forceCrashlytics = null;
    public InputField jumpScene = null;
    public Text textInfo = null;
    public Text textFPS = null;

    private static DebugManager _instance = null;
    private float fpsDeltaTime = 0.0f;

    void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(_instance);

        // set debug mode
        SGDebug.DebugMode = Debug.isDebugBuild;
    }

    void OnEnable()
    {
        // set callbacks
        SGDebug.LogTextChanged += TextLogChanged;
    }

    void OnDisable()
    {
        // unset callbacks
        SGDebug.LogTextChanged -= TextLogChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Debug.isDebugBuild)
        {
            openButton.SetActive(true);
            Setup();

            if (startOnBoot)
            {
                startOnBoot = false;
                if (SceneManager.GetActiveScene().buildIndex > 0)
                    SceneManager.LoadScene(0);
            }

        }
        else
        {
            openButton.SetActive(false);
        }
    }

    void Update()
    {
        int fps = 0;

        // FPS counter
        if (SGDebug.DebugMode)
        {
            fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;
            fps = (int) (1.0f / fpsDeltaTime);
            textFPS.text = fps.ToString() + " fps";
        }

    }

    private void Setup()
    {
        // set initial values
        textLog.isOn = textLogObject.activeSelf;
        TextLogChanged();
        drawLineLog.isOn = SGDebug.SetDrawLine;
        disableAllEnemies.isOn = SGDebug.DisableAllEnemies;
        disableGUI.isOn = SGDebug.DisableGUI;
        playerInvincible.isOn = SGDebug.PlayerInvincible;
        unlockAllItems.isOn = SGDebug.UnlockAllItems;
        clearPref.isOn = false;
        forceCrashlytics.isOn = false;
        resetDataAnalytics.isOn = false;

        // set callbacks
        textLog.onValueChanged.AddListener(delegate { OnValueChanged_TextLog(); });
        drawLineLog.onValueChanged.AddListener(delegate { OnValueChanged_DrawLineLog(); });
        disableAllEnemies.onValueChanged.AddListener(delegate { OnValueChanged_DisableAllEnemies(); });
        disableGUI.onValueChanged.AddListener(delegate { OnValueChanged_DisableGUI(); });
        playerInvincible.onValueChanged.AddListener(delegate { OnValueChanged_PlayerInvincible(); });
        unlockAllItems.onValueChanged.AddListener(delegate { OnValueChanged_UnlockAllItems(); });
        forceCrashlytics.onValueChanged.AddListener(delegate { OnValueChanged_ForceCrashlytics(); });
        resetDataAnalytics.onValueChanged.AddListener(delegate { OnValueChanged_ResetDataAnalytics(); });
        clearPref.onValueChanged.AddListener(delegate { OnValueChanged_ClearPref(); });
        jumpScene.onEndEdit.AddListener(delegate { OnValueChanged_JumpScene(); });
    }

    public void OnClick_OpenOptions()
    {
        overlayOptions.SetActive(true);
        // update initial
        TextInfo();
    }
    public void OnClick_CloseOptions()
    {
        overlayOptions.SetActive(false);
    }

    private void OnValueChanged_TextLog()
    {
        textLogObject.SetActive(textLog.isOn);
    }

    private void OnValueChanged_ClearPref()
    {
        if (clearPref.isOn)
        {
            SGStorage.ClearGenericPrefs();
            PlayerPrefs.DeleteAll();
            clearPref.isOn = false;
        }
    }

    private void OnValueChanged_DrawLineLog()
    {
        SGDebug.SetDrawLine = drawLineLog.isOn;
    }

    private void OnValueChanged_DisableAllEnemies()
    {
        SGDebug.DisableAllEnemies = disableAllEnemies.isOn;
    }

    private void OnValueChanged_DisableGUI()
    {
        SGDebug.DisableGUI = disableGUI.isOn;
    }

    private void OnValueChanged_PlayerInvincible()
    {
        SGDebug.PlayerInvincible = playerInvincible.isOn;
    }

    private void OnValueChanged_UnlockAllItems()
    {
        SGDebug.UnlockAllItems = unlockAllItems.isOn;
    }

    private void OnValueChanged_ForceCrashlytics()
    {
        if (forceCrashlytics.isOn)
        {
            forceCrashlytics.isOn = false;
            SGDebug.ForceException();
            StartCoroutine(SGEnvironment.Quit());
        }
    }

    private void OnValueChanged_ResetDataAnalytics()
    {
        if (resetDataAnalytics.isOn)
        {
            resetDataAnalytics.isOn = false;
            SGAnalytics.ResetData();
        }
    }

    private void OnValueChanged_JumpScene()
    {
        int scene = -1;
        if (int.TryParse(jumpScene.text, out scene))
        {
            if (scene >= 0 && scene < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(scene);

                jumpScene.text = "";
                OnClick_CloseOptions();
            }
        }
    }

    private void TextLogChanged()
    {
        log.text = SGDebug.LogText;
    }

    private void TextInfo()
    {
        textInfo.text = "INFORMATIONS";
        // time
        textInfo.text += "\nDate Time: " + System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");

        // user
        textInfo.text += "\nUser name: " + SGFirebase.userName;
        textInfo.text += "\nUser email: " + SGFirebase.userEmail;
        textInfo.text += "\nUser id: " + SGFirebase.userId;

        // screen
        textInfo.text += "\nScreen dpi: " + Screen.dpi;
        textInfo.text += "\nScreen orientation: " + Screen.orientation;
        textInfo.text += "\nScreen dimension pixels: " + Screen.width + "x" + Screen.height;

        // some player prefs
        foreach (string key in SGStorage.GetGenericPrefs())
        {
            textInfo.text += "\nGenericPrefs : " + key + "=" + SGStorage.GetGenericPref(key);
        }

        // environment
        textInfo.text += "\nProduct: " + SGEnvironment.GetProdutcName() + " " + SGEnvironment.GetVersion();
        textInfo.text += "\nProduct company name: " + Application.companyName;
        textInfo.text += "\nProduct absolute url: " + Application.absoluteURL;
        textInfo.text += "\nProduct build GUID: " + Application.buildGUID;
        textInfo.text += "\nProduct identifier: " + Application.identifier;
        textInfo.text += "\nProduct installer name: " + Application.installerName;
        textInfo.text += "\nProduct internet reachability: " + Application.internetReachability;
        textInfo.text += "\nProduct data path: " + Application.dataPath;
        textInfo.text += "\nProduct persistent data path: " + Application.persistentDataPath;
        textInfo.text += "\nProduct streaming assets path: " + Application.streamingAssetsPath;
        textInfo.text += "\nProduct temporary cache path: " + Application.temporaryCachePath;
        textInfo.text += "\nUnity: " + SGEnvironment.GetUnityVersion();
        textInfo.text += "\nDevice platform: " + Application.platform;
        textInfo.text += "\nDevice unique identifier: " + SystemInfo.deviceUniqueIdentifier;
        textInfo.text += "\nDevice name: " + SystemInfo.deviceName;
        textInfo.text += "\nDevice model: " + SystemInfo.deviceModel;
        textInfo.text += "\nDevice SO: " + SystemInfo.operatingSystem;
        textInfo.text += "\nDevice system language: " + Application.systemLanguage;
        textInfo.text += "\nDevice type: " + SystemInfo.deviceType;
        textInfo.text += "\nDevice memory size: " + SystemInfo.systemMemorySize.ToString();
        textInfo.text += "\nDevice processors: " + SystemInfo.processorCount.ToString();
        textInfo.text += "\nFirebase Ready: " + SGFirebase.SetupReady.ToString();
    }
}