using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SGUIBackMusicSlider : MonoBehaviour
{
    private Slider slider = null;
    
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = SGSound.MusicLevel;
        // set callbacks
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void ValueChangeCheck ()
    {
        SGSound.MusicLevel = slider.value;
    }
}
