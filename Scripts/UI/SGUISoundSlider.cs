using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SGUISoundSlider : MonoBehaviour
{
    private Slider slider = null;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = SGSound.SoundLevel;
        // set callbacks
        slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    private void OnValueChanged()
    {
        SGSound.SoundLevel = slider.value;
    }
}
