using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SGUIScalerMesh : MonoBehaviour
{
    public RectTransform parentRectTransform = null;

    private RectTransform meshRectTransform = null;
    private Vector3 currentScale;
    private float originalRatioX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        meshRectTransform = GetComponent<RectTransform>();
        currentScale = meshRectTransform.localScale;
        originalRatioX = currentScale.x / currentScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Responsive();
    }
 
    /// <summary>
    /// Faz o cálculo de aspect ratios em X, para verificar qual melhor dimensão sem encaixa 
    /// Depois aplica a fórmula para alterar a escala X ou Y
    /// </summary>
    private void Responsive ()
    {
        // get new scale
        Vector3 newScale = new Vector3(parentRectTransform.rect.width, parentRectTransform.rect.height, currentScale.z);
        float newRatioX = newScale.x / newScale.y;

        // extend while maintaining the proportion
        if (originalRatioX > newRatioX)
            newScale.y = newScale.x / originalRatioX;
        else
            newScale.x = newScale.y * originalRatioX;
        meshRectTransform.localScale = newScale;
    }
}
