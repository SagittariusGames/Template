using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LoadingNextLevelPrefab : MonoBehaviour
{
    delegate void ProgressBarDelegate(float progress);

    public Text progressText = null;
    public Transform progressBar = null;
    
    
    // Start is called before the first frame update
    void Start()
    {
        progressText.text = "0%";
        progressBar.localScale = Vector3.zero;

        if (SGScenes.IndexNextLevel >= 0)
        {
            Resources.UnloadUnusedAssets();
            StartCoroutine(LoadLevelScene(SGScenes.IndexNextLevel, OnLoadLevelProgressUpdate));
        }
        SGScenes.IndexNextLevel = -1;
    }

    IEnumerator LoadLevelScene(int scene, ProgressBarDelegate progressDelegate)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (!async.isDone)
        {
            progressDelegate(async.progress);
            async.allowSceneActivation = async.progress > 0.80; // waiting until 80%
            yield return null;
        }
    }

    void OnLoadLevelProgressUpdate(float progress)
    {
        progressText.text = ((int) (progress * 100)).ToString() + "%";
        progressBar.localScale = new Vector3(progress, 1, 1);
    }
}
