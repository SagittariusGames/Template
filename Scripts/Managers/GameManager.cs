using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [Header("Scenes")]
    public int indexHome = 0;
    public int indexAbout = -1;
    public int indexTermsPrivacy = -1;
    public int indexLoading = -1;
    public int indexLoadSave = -1;
    public int indexSettings = -1;
    public int indexGameOver = -1;
    public int indexEnding = -1;
    public int indexFirstLevel = -1;
    public int indexAuth = -1;
    public int indexHelp = -1;

    [Header("Mobile")]
    [Tooltip("In seconds: -1 => never sleep; otherwise time left")]
    public bool neverSleep = false;

    [Header("Firebase Settings")]
    public bool firebaseActivate = true;
    public string remoteSettingsAppUpgrade = "app_version";
    public int remoteSettingsIntervalUpdate = 30;
    public int debugRemoteSettingsIntervalUpdate = 10;
    public GameObject msgUpgrade = null;
    public GameObject msgUpgradeNow = null;
    public string dynamicLinkSceneParam = "link";
    public bool authAnonymous = true;

    [Header("External Settings")]
    public string deepLinkParam = "arguments";

    private static GameManager _instance = null;

    private bool rsFirstAttempt = true;
    private bool authFirstAttempt = true;

    void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(_instance);
    }

    void Start()
    {
        if (firebaseActivate)
        {
            SGFirebase.Setup();
            if (Debug.isDebugBuild)
                StartCoroutine(SGFirebase.RemoteSettingsCountDown(debugRemoteSettingsIntervalUpdate));
            else
                StartCoroutine(SGFirebase.RemoteSettingsCountDown(remoteSettingsIntervalUpdate));
        }
        //analytics
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.StartGame);
        //adversiting
        SGAdvertising.Setup();

        StartGame();

        // sync scenes
        SGScenes.IndexAbout = indexAbout;
        SGScenes.IndexTermsPrivacy = indexTermsPrivacy;
        SGScenes.IndexEnding = indexEnding;
        SGScenes.IndexFirstLevel = indexFirstLevel;
        SGScenes.IndexGameOver = indexGameOver;
        SGScenes.IndexHome = indexHome;
        SGScenes.IndexLoading = indexLoading;
        SGScenes.IndexLoadSave = indexLoadSave;
        SGScenes.IndexSettings = indexSettings;
        SGScenes.IndexAuth = indexAuth;
        SGScenes.IndexHelp = indexHelp;

        SGDeepLink.ReceiveExternalCall(deepLinkParam);
    }

    void OnEnable()
    {
        // set callbacks
        SGSound.MusicLevelChanged += SetBackgroundVolume;
        SGStatus.PauseChanged += SetPause;
        Application.lowMemory += OnLowMemory;
        SGEnvironment.DynamicLinkChanged += SetDynamicLink;
    }

    void OnDisable()
    {
        // unset callbacks
        SGSound.MusicLevelChanged -= SetBackgroundVolume;
        SGStatus.PauseChanged -= SetPause;
        Application.lowMemory -= OnLowMemory;
        SGEnvironment.DynamicLinkChanged -= SetDynamicLink;
    }

    void Update()
    {
        // try first remote settings
        if (rsFirstAttempt && SGFirebase.RemoveSettingsTryUpdate())
        {
            CheckUpgrade();
            rsFirstAttempt = false;
            Debug.Log("Remote Settings: Save fresh data to PlayerPrefab");
        }

        // try authenticate
        if (authAnonymous && authFirstAttempt && SGFirebase.SetupReady)
        {
            if (!SGFirebase.AuthGetPersistent())
                SGFirebase.Authentication();
            authFirstAttempt = false;
        }
    }

    void OnApplicationQuit()
    {
        if (firebaseActivate)
            SGFirebase.Dispose();
    }

    /// <summary>
    /// 
    /// </summary>
    private void StartGame()
    {
        // no sleep for mobile - setup
        if (neverSleep)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        else
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

        //language
        SGLanguage.fileXML = "lang";

        // back music
        SetBackgroundVolume();
    }

    private void OnLowMemory()
    {
        // release all cached resorces
        Resources.UnloadUnusedAssets();
        //analytics
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.LowMemory);
    }

    private void SetBackgroundVolume()
    {
        AudioSource audio = _instance.GetComponent<AudioSource>();
        audio.volume = SGSound.MusicLevel;
    }

    private void SetPause()
    {
        AudioSource audio = _instance.GetComponent<AudioSource>();
        if (audio.isActiveAndEnabled)
        {
            if (SGStatus.Pause)
            {
                if (audio.isPlaying)
                    audio.Pause();
            }
            else if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
    }

    private void SetDynamicLink ()
    {
        string sceneName = SGEnvironment.GetDynamicLink(dynamicLinkSceneParam);
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// Verificar via remote settings a necessita de atualização do App, verificando a versão
    /// https://console.firebase.google.com/project/learn-english-gamification/config
    /// </summary>
    private void CheckUpgrade()
    {
        string upgrade = SGFirebase.RemoteSettings(remoteSettingsAppUpgrade, SGEnvironment.GetVersion());
        upgrade = upgrade.Trim();

        // check
        if (string.IsNullOrEmpty(upgrade))
            return;
        // check
        if (upgrade == SGEnvironment.GetVersion())
            return;

        // split parts
        string[] newVersion = upgrade.Split('.');
        string[] version = SGEnvironment.GetVersion().Split('.');
        
        // check
        if (newVersion.Length < 2 || version.Length < 2)
            return;

        // upgrade immediately or later
        if (newVersion[0] != version[0])
        {
            Debug.LogWarning("Upgrade Immediately: Old " + version[1] + " - New " + newVersion[1]);
            msgUpgradeNow.SetActive(true);
        } else if (newVersion[1] != version[1])
        {
            Debug.LogWarning("Upgrade: Old " + version[1] + " - New " + newVersion[1]);
            msgUpgrade.SetActive(true);
        }
    }

    public void OnClick_Upgrade()
    {
        msgUpgrade.SetActive(false);
    }

    public void OnClick_UpgradeNow()
    {
        StartCoroutine(SGEnvironment.Quit());
    }
}
