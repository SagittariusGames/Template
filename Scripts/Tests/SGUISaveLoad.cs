using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class test_GamePlayer : System.ICloneable
{
    // public test_GamePlayer current;
    public int scene;
    public string levelName;
    public string timeStemp;
    public int lives;
    public List<int> inventoryItens;
    public int score;
    public int energy;
    public float time;

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public void Reset()
    {
        //current = null;
        scene = 0;
        levelName = null;
        timeStemp = null;
        lives = 0;
        inventoryItens = null;
        score = 0;
        energy = 0;
        time = 0;
    }

    public float energyFloat {
        get {
            if (energy > 100)
                energy = 100;
            return (float)energy / 100;
        }
        set {
            if (value > 1)
                energy = 100;
            else
                energy = (int)value * 100;
        }
    }

}

[RequireComponent(typeof(Image))]
public class SGUISaveLoad : MonoBehaviour
{
    public static List<test_GamePlayer> tests_listGamesPlayer = new List<test_GamePlayer>();
    public static test_GamePlayer tests_currentGamePlayer = null;
    [Tooltip("in seconds")]
    public float timeout = 10f;

    private Image img = null;
    private bool trySaveRemote = false;
    private bool tryLoadRemote = false;
    private float timecount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        tests_currentGamePlayer = new test_GamePlayer
        {
            scene = 0,
            timeStemp = System.DateTime.Now.ToString(),
            levelName = "abacabb",
            lives = 2,
            score = 4546,
            energy = 50,
            time = 0.5f,
            inventoryItens = new List<int>(),
        };

        img = GetComponent<Image>();

    }

    void Update()
    {
        if (trySaveRemote)
        {
            timecount += Time.deltaTime;

            if (SGSaveLoad.remoteStatus == 0)
            {
                trySaveRemote = false;
            } else if (SGSaveLoad.remoteStatus == 1)
            {
                trySaveRemote = false;
                LoadRemote();
            }
        }

        if (tryLoadRemote)
        {
            timecount += Time.deltaTime;

            if (SGSaveLoad.remoteStatus == 0)
            {
                tryLoadRemote = false;
            }
            else if (SGSaveLoad.remoteStatus == 1)
            {
                img.color = Color.green;
                tryLoadRemote = false;
                GetValues(SGSaveLoad.snapshot);
            }
        }


        if (timecount > timeout)
        {
            trySaveRemote = false;
            tryLoadRemote = false;
            timecount = 0f;
        }

    }

    private bool AddSavedList()
    {
        if (tests_currentGamePlayer == null)
            return false;

        // update current date time
        tests_currentGamePlayer.timeStemp = SGDateTime.IntegerDateTime().ToString();
        //update active scene
        tests_currentGamePlayer.scene = SGScenes.GetActiveScene;
        tests_currentGamePlayer.levelName = "mamania";

        RemoveSavedList();
        tests_listGamesPlayer.Add(tests_currentGamePlayer);

        return true;
    }

    private void RemoveSavedList()
    {
        if (tests_currentGamePlayer == null)
            return;
        if (tests_listGamesPlayer == null)
            return;

        foreach (test_GamePlayer sg in tests_listGamesPlayer)
        {
            if (sg.scene == tests_currentGamePlayer.scene)
            {
                tests_listGamesPlayer.Remove(sg);
                sg.Reset();
                return;
            }
        }
    }

    private void ClearSavedList()
    {
        if (tests_listGamesPlayer == null)
            return;
        
        foreach (test_GamePlayer sg in tests_listGamesPlayer)
        {
            sg.Reset();
        }
        tests_listGamesPlayer.Clear();
    }

    public void OnClick_SaveLoadLocal()
    {
        img.color = Color.red;
        // binary, with list
        if (AddSavedList())
        {
            if (SGSaveLoad.SaveLocal(tests_listGamesPlayer))
            {
                Debug.LogWarning(tests_listGamesPlayer.Count);
                tests_listGamesPlayer.Clear();
                tests_listGamesPlayer = SGSaveLoad.LoadLocal<List<test_GamePlayer>>();
                if (tests_listGamesPlayer.Count > 0)
                {
                    Debug.LogWarning(tests_listGamesPlayer.Count);
                    SGDebug.LogText = "Save/Load successful!: " + SGSaveLoad.LocalFilePath;

                    img.color = Color.blue;
                }
            }
        }

        // xml
        string key = tests_currentGamePlayer.scene.ToString();
        if (SGSaveLoad.SaveLocalXML(tests_currentGamePlayer, key))
        {
            Debug.LogWarning(tests_currentGamePlayer.levelName);
            tests_currentGamePlayer = null;
            tests_currentGamePlayer = SGSaveLoad.LoadLocalXML<test_GamePlayer>(key);
            if (tests_currentGamePlayer.scene > 0)
            {
                Debug.LogWarning(tests_currentGamePlayer.levelName);
                SGDebug.LogText = "Save/Load successful!: " + SGSaveLoad.LocalFilePath;
                img.color = Color.blue;
            }
        }

        // remote
        SaveRemote();
    }

    private void SaveRemote()
    {
        SGSaveLoad.UploadXML(tests_currentGamePlayer, tests_currentGamePlayer.scene.ToString());
        //SGSaveLoad.RemoveXML(tests_currentGamePlayer.scene.ToString());
        trySaveRemote = true;
        timecount = 0f;
    }

    private void LoadRemote()
    {
        SGSaveLoad.DownloadXMLs();
        tryLoadRemote = true;
        timecount = 0f;
    }

    private void GetValues(DataSnapshot snapshot)
    {
        ClearSavedList();
        
        foreach (var childSnapshot in snapshot.Children)
        {
            SGSaveLoad.ConvertXMLToObject(tests_currentGamePlayer, childSnapshot.Value);
            AddSavedList();
        }
    }
}
