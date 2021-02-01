using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance = null;

    // Use this for initialization
    void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(_instance);
    }

    void OnEnable()
    {
        // set callbacks
        SGSound.MusicLevelChanged += SetBackgroundVolume;
        SGStatus.PauseChanged += SetPause;
    }

    void OnDisable()
    {
        // unset callbacks
        SGSound.MusicLevelChanged -= SetBackgroundVolume;
        SGStatus.PauseChanged -= SetPause;
    }

    private void SetBackgroundVolume()
    {
        AudioSource audio = _instance.GetComponent<AudioSource>();
        audio.volume = SGSound.MusicLevel;
    }

    private void SetPause()
    {
        AudioSource audio = _instance.GetComponent<AudioSource>();
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
