using UnityEngine;

public class SGScreen
{
    /// <summary>
    /// 
    /// </summary>
    public static bool PortraitOrientation ()
    {
        if (Screen.width > Screen.height)
            return false;
        else
            return true;
    }

    /// <summary>
    /// "Units" = unidade de medida do Unity, que varia pela distância do observador (câmera)
    /// The default scale is in meters
    /// </summary>
    public static float PixelsToUnits(float value, Camera cam)
    {
        float ortho = cam.orthographicSize;
        float pixel = cam.pixelHeight;
        float newValue = (value * ortho * 2) / pixel;

        return newValue;
    }

    /// <summary>
    /// inch is a physical measurement connected to actual screen size
    /// </summary>
    public static float PixelsToInch(float value)
    {
        float newValue = value / Screen.dpi;

        return newValue;
    }


    /// <summary>
    /// dp or dip means Density-independent Pixels
    /// dp = (width in pixels * 160) / screen density
    /// ref: https://material.io/design/layout/density-resolution.html#pixel-density-on-android
    /// </summary>
    public static float DensityIndependentPixels ()
    {
        float newValue = (Screen.width * 160) / Screen.dpi;

        return newValue;
    }

}
