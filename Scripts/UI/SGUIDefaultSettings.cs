using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SGUIDefaultSettings : MonoBehaviour
{
    private Button button = null;

    void Start()
    {
        button = GetComponent<Button>();

        // set callbacks
        button.onClick.AddListener(delegate { OnClick(); });
    }

    public void OnClick ()
    {
        // restore sounds
        SGSound.SoundLevel = 1;
        SGSound.MusicLevel = 1;

        // restore storages
        SGStorage.ClearGenericPrefs();

        // delete files
        SGSaveLoad.DeleteLocal();

        // load boot scene
        StartCoroutine(SGEnvironment.Quit());
    }
}
