using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
public class SGUIGridLayoutGroupResponsive : MonoBehaviour
{
    [Tooltip("Quebras de colunas (largura) à cada polegada virtual")]
    public float breakPointWidthInch = 10f;
    [Tooltip("Somente no Editor")]
    public float breakPointWidthInchOnlyEditor = 20f;
    public int forceExpandCards = 0;
    public ScrollRect mainContentScrollRect = null; // hide scrolls

    private GridLayoutGroup gridLayoutGroup = null;
    private RectTransform rectTransform = null;
    private RectTransform rectTransformParent = null;
    private float cellSizeY = 0f;
    private float cellSizeX = 0f;
    private bool fullScreen = false;
    private RectOffset padding = null;

    void Start()
    {
#if UNITY_EDITOR
        breakPointWidthInch = breakPointWidthInchOnlyEditor;
#endif
        rectTransform = this.GetComponent<RectTransform>();
        gridLayoutGroup = this.GetComponent<GridLayoutGroup>();
        rectTransformParent = mainContentScrollRect.GetComponent<RectTransform>();

        cellSizeY = gridLayoutGroup.cellSize.y;
        cellSizeX = gridLayoutGroup.cellSize.x;

        padding = gridLayoutGroup.padding;

    }

    // Update is called once per frame
    void Update()
    {
        CheckFullScreenPrepared();
        Responsive();
    }

    private void Responsive()
    {
        float inch = SGScreen.PixelsToInch(Screen.width);
        bool orientationPortrait = SGScreen.PortraitOrientation();
        Transform child;
        
        if (forceExpandCards > 0)
        {
            if (orientationPortrait)
            {
                cellSizeY = (rectTransformParent.rect.height - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom + gridLayoutGroup.spacing.y)) / forceExpandCards;
            } else
            {
                cellSizeY = (rectTransformParent.rect.height - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom + gridLayoutGroup.spacing.y));
            }
        }

        if (fullScreen)
        {
            rectTransform.localPosition = Vector3.zero;
            // SGDebug.Log("1 coluna, fullscreen");
            gridLayoutGroup.cellSize = new Vector2(rectTransformParent.rect.width, rectTransformParent.rect.height);
        }
        else if (orientationPortrait)
        {
            // SGDebug.Log("1 coluna");
            gridLayoutGroup.cellSize = new Vector2(rectTransform.rect.width - (gridLayoutGroup.padding.left + gridLayoutGroup.spacing.x), cellSizeY);

        } else if (inch > breakPointWidthInch)
        {
            // SGDebug.Log("3 coluna, se possível");
            gridLayoutGroup.cellSize = new Vector2(rectTransform.rect.width / 3 - (gridLayoutGroup.padding.left + gridLayoutGroup.spacing.x), cellSizeY);
        } else
        {
            // SGDebug.Log("2 coluna, se possível");
            gridLayoutGroup.cellSize = new Vector2(rectTransform.rect.width / 2 - (gridLayoutGroup.padding.left + gridLayoutGroup.spacing.x), cellSizeY);
        }

        // ajusta a altura do content
        child = this.transform.GetChild(this.transform.childCount - 1); // ultimo filho
        rectTransform.sizeDelta = new Vector2(0, child.localPosition.y * - 1 + cellSizeY); // seta novo valor
    }

    private void CheckFullScreenPrepared()
    {
        int nVisibleChildren = 0;
        
        // contar filhos visiveis
        for (int n = 0; n < this.transform.childCount; n++)
        {
            if (this.transform.GetChild(n).gameObject.activeSelf)
                nVisibleChildren++;
        }
        if (nVisibleChildren > 1)
            fullScreen = false;
        else
            fullScreen = true;
        
        // exibir barra de rolagens
        mainContentScrollRect.horizontal = !fullScreen;
        mainContentScrollRect.vertical = !fullScreen;

        // espaçamentos
        if (fullScreen)
            gridLayoutGroup.padding = new RectOffset();
        else
            gridLayoutGroup.padding = padding;
    }
}
