using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class SGUIAutoCanvasScaler : MonoBehaviour
{
    CanvasScaler canvasScaler;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        ChangeScaleRef();
    }

    /// <summary>
    /// Nunca usar nos objetos UI medidas e pixels pois elas variam de acordo com 
    /// a densidade de cada aparelho.
    /// No modo desenvolvimento (Editor) usar Picas
    /// Para Android usar Milimetros
    /// Para outros dispsitivos, testar
    /// </summary>
    private void ChangeScaleRef ()
    {
#if UNITY_EDITOR
        canvasScaler.physicalUnit = CanvasScaler.Unit.Picas;
#elif UNITY_ANDROID
        canvasScaler.physicalUnit = CanvasScaler.Unit.Millimeters;
#endif
    }
}
