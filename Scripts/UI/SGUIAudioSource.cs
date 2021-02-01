using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SGUIAudioSource : MonoBehaviour
{
    public bool affectedByPause = true;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = SGSound.SoundLevel;
    }

    void OnEnable()
    {
        // set callbacks
        SGSound.SoundLevelChanged += SetVolume;
        if (affectedByPause)
            SGStatus.PauseChanged += SetPause;
    }

    void OnDisable()
    {
        // unset callbacks
        SGSound.SoundLevelChanged -= SetVolume;
        if (affectedByPause)
            SGStatus.PauseChanged -= SetPause;
    }

    private void SetVolume()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = SGSound.SoundLevel;
    }

    private void SetPause()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (SGStatus.Pause)
            audio.Pause();
        else
            audio.Play();
    }
}
