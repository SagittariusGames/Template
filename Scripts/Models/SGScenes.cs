using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SGScene
{
    public SGScene(int sceneIndex, string data, int index)
    {
        SceneIndex = sceneIndex;
        Data = data;
        Index = index;
    }

    public int SceneIndex { get; }
    public string Data { get; }
    public int Index { get; }
}

public class SGScenes
{
    private static List<SGScene> listScenes = null;

    public static int IndexHome { get; set; } = -1;
    public static int IndexAbout { get; set; } = -1;
    public static int IndexTermsPrivacy { get; set; } = -1;
    public static int IndexLoading { get; set; } = -1;
    public static int IndexLoadSave { get; set; } = -1;
    public static int IndexSettings { get; set; } = -1;
    public static int IndexGameOver { get; set; } = -1;
    public static int IndexEnding { get; set; } = -1;
    public static int IndexFirstLevel { get; set; } = -1;
    public static int IndexNextLevel { get; set; } = -1;
    public static int IndexAuth { get; set; } = -1;
    public static int IndexHelp { get; set; } = -1;
    public static int GetActiveScene { get => SceneManager.GetActiveScene().buildIndex; }
    public static string GetActiveSceneName { get => SceneManager.GetActiveScene().name; }

    public static void LoadScene(int scene, string data = "", int index = -1)
    {
        if (scene > 0)
        {
            AddQueue(scene, data, index);

            IndexNextLevel = scene;
            SceneManager.LoadScene(IndexLoading);
        } else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public static void ReLoadScene()
    {
        IndexNextLevel = SGScenes.GetActiveScene;
        SceneManager.LoadScene(IndexLoading);
    }

    public static void LoadScene(string scene, string data = "", int index = -1)
    {
        int sceneIndex = SceneManager.GetSceneByName(scene).buildIndex;
        if  (sceneIndex >= 0)
            LoadScene(SceneManager.GetSceneByName(scene).buildIndex, data, index);
    }

    public static bool LoadBackScene()
    {
        if (listScenes == null)
            return false;
        
        if (QueueCount() <= 1)
            return false;

        SGScene scene = listScenes[QueueCount()-2];
        RemoveQueue();
        SceneManager.LoadScene(scene.SceneIndex);
        return true;
    }

    public static int QueueCount()
    {
        if (listScenes == null)
            return 0;

        return listScenes.Count;
    }

    public static string CurrentData()
    {
        if (listScenes == null)
            return "";

        if (QueueCount() == 0)
            return "";

        SGScene scene = listScenes[QueueCount()-1];
        return scene.Data;
    }

    public static int CurrentIndex()
    {
        if (listScenes == null)
            return -1;

        if (QueueCount() == 0)
            return -1;

        SGScene scene = listScenes[QueueCount() - 1];
        return scene.Index;
    }


    private static void AddQueue(int scene, string data = "", int index = -1)
    {
        if (listScenes == null)
            listScenes = new List<SGScene>();
        
        SGScene currentScene = new SGScene(scene, data, index);
        listScenes.Add(currentScene);
    }

    private static void RemoveQueue()
    {
        if (listScenes == null)
            return;

        if (QueueCount() == 0)
            return;
        
        listScenes.RemoveAt(QueueCount()-1);
    }

}
