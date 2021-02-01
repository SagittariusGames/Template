using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class SGUIContentSizeFitterChildren : MonoBehaviour
{
    public bool horizontalFit = true;
    public bool verticalFit = true;
    public float addHorizontal = 0;
    public float addVertical = 0;
    public float delayToUpdate = 1; //sec
    private float chronometer = 0.0f;

    RectTransform rec;

    void Start()
    {
        rec = GetComponent<RectTransform>();
        chronometer = delayToUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        if (SGDateTime.Chronometer(ref chronometer))
        {
            if (horizontalFit)
                HorizontalAdjust();
            if (verticalFit)
                VerticalAdjust();

            //restart
            chronometer = delayToUpdate;
        }
    }

    private void HorizontalAdjust()
    {
        RectTransform children = GetComponentInChildren<RectTransform>();
        float width = 0;
        float newWidth = 0;

        foreach (RectTransform child in children)
        {
            width = child.rect.x * -1;
            if (width > newWidth)
                newWidth = width;
        }

        if (newWidth > 0)
        {
            rec.sizeDelta = new Vector2(newWidth + addHorizontal, rec.sizeDelta.y);
        }
    }


    private void VerticalAdjust ()
    {
        RectTransform children = GetComponentInChildren<RectTransform>();
        float height = 0;
        float newHeight = 0;

        foreach (RectTransform child in children)
        {
            height = child.rect.y * -1;
            if (height > newHeight)
                newHeight = height;
        }

        if (newHeight > 0)
        {
            rec.sizeDelta = new Vector2(rec.sizeDelta.x, newHeight + addVertical);
        }
    }
}
