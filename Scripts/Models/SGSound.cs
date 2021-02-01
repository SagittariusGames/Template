using UnityEngine;

public class SGSound
{
    public delegate void EventChanges();
    public static event EventChanges SoundLevelChanged = null;
    public static event EventChanges MusicLevelChanged = null;

    private static float soundLevel = 1f;
    private static float musicLevel = 1f;

    /// <summary>
    /// 
    /// </summary>
    public static float SoundLevel {
        get {
            if (soundLevel < 0)
                soundLevel = PlayerPrefs.GetFloat("Sound Level", 1f);
            return soundLevel;
        }
        set {
            soundLevel = value;
            if (soundLevel < 0)
                PlayerPrefs.DeleteKey("Sound Level");
            else
                PlayerPrefs.SetFloat("Sound Level", soundLevel);
            // event
            SoundLevelChanged?.Invoke();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static float MusicLevel {
        get {
            if (musicLevel < 0)
                musicLevel = PlayerPrefs.GetFloat("Music Level", 1f);
            return musicLevel;
        }
        set {
            musicLevel = value;
            if (musicLevel < 0)
                PlayerPrefs.DeleteKey("Music Level");
            else
                PlayerPrefs.SetFloat("Music Level", musicLevel);
            // event
            MusicLevelChanged?.Invoke();
        }
    }
}
