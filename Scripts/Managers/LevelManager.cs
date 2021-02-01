using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("User")]
    public bool checkTermsUse = false;
    public bool checkGenuineApp = false;

    [Header("Events for status")]
    // user callbacks
    public UnityEvent OnPause = null;
    public UnityEvent OnPlay = null;

    [Header("Menus")]
    public GameObject Menu1 = null;
    public GameObject Menu2 = null;

    void Awake()
    {
        //analytics
        SGAnalytics.AnalyticsTrakingLifecycle("Awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        Resources.UnloadUnusedAssets();
        // check and close it if not accepted
        CheckTermsUse();
        CheckGenuineApp();

        //analytics
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.LevelStart);
        SGAnalytics.AnalyticsTrakingLifecycle("Start");
    }

    // Update is called once per frame
    void Update()
    {
        // saída de emergência
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Menu1 && Menu1.activeSelf)
                Menu1.SetActive(false);
            else if (Menu2 && Menu2.activeSelf)
                Menu2.SetActive(false);
            else
                LoadBackScene();
        }
    }

    void CheckTermsUse ()
    {
        if (checkTermsUse)
        {
            if (!SGSecurity.TermsUse)
            {
                if (SGScenes.IndexTermsPrivacy >= 0 && SGScenes.GetActiveScene != SGScenes.IndexTermsPrivacy)
                    LoadLevelScene(SGScenes.IndexTermsPrivacy);
            }
        }
    }

    void CheckGenuineApp()
    {
        if (checkGenuineApp)
        {
            if (!SGSecurity.CheckGenuine())
            {
                SGDebug.LogWarning("CheckGenuine=false");
                SGDebug.SetCustomKey("CheckGenuine", "False");
                SGDebug.ForceException();
                StartCoroutine(SGEnvironment.Quit());
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Pause ()
    {
        SGStatus.Pause = true;
        Time.timeScale = 0f;
        if (OnPause != null)
            OnPause.Invoke();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Resume()
    {
        SGStatus.Pause = false;
        Time.timeScale = 1f;
        if (OnPlay != null)
            OnPlay.Invoke();

        // check and close it if not accepted
        CheckTermsUse();
        CheckGenuineApp();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            Pause();
        else
            Resume();

        //analytics
        SGAnalytics.AnalyticsTrakingLifecycle("OnApplicationPause", pauseStatus);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            Pause();
        else
            Resume();

        //analytics
        SGAnalytics.AnalyticsTrakingLifecycle("OnApplicationFocus", hasFocus);
    }

    public void LevelCompleted ()
    {
        //analytics
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.LevelCompleted);
    }
    public void LevelGameOver()
    {
        //analytics
        SGAnalytics.AnalyticsTraking(SGAnalytics.AnalyticsEvents.GameOver);
    }

    public void LoadLevelScene(int scene)
    {
        SGScenes.LoadScene(scene);
    }

    /*
    public void LoadLevelScene(string scene)
    {
        SGScenes.LoadScene(scene);
    }
    */

    public void LoadLevelSceneProgress(int scene)
    {
        SGScenes.IndexNextLevel = scene;
        SGScenes.LoadScene(SGScenes.IndexLoading);
    }

    public void LoadNextLevelScene()
    {
        SGScenes.LoadScene(SGScenes.GetActiveScene + 1);
    }

    public void ReLoadLevelScene()
    {
        SGScenes.ReLoadScene();
    }

    public void LoadBackScene()
    {
        if (!SGScenes.LoadBackScene())
            StartCoroutine(SGEnvironment.Quit());
    }
    public void LoadLevelHome() => LoadLevelScene(SGScenes.IndexHome);
    public void LoadLevelAbout() => LoadLevelScene(SGScenes.IndexAbout);
    public void LoadLevelTermsPrivacy() => LoadLevelScene(SGScenes.IndexTermsPrivacy);
    public void LoadLevelSave() => LoadLevelScene(SGScenes.IndexLoadSave);
    public void LoadLevelSettings() => LoadLevelScene(SGScenes.IndexSettings);
    public void LoadLevelGameOver() => LoadLevelScene(SGScenes.IndexGameOver);
    public void LoadLevelEnding() => LoadLevelScene(SGScenes.IndexEnding);
    public void LoadLevelFirstLevel() => LoadLevelSceneProgress(SGScenes.IndexFirstLevel);
    public void LoadLevelHelp() => LoadLevelScene(SGScenes.IndexHelp);
}
