using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extention
{
    public static float MaxY(this RectTransform rectTransform) {
        return rectTransform.GetCorners()[1].y;
    }

    public static float MinY(this RectTransform rectTransform) {
        return rectTransform.GetCorners()[0].y;
    }

    public static Vector3[] GetCorners(this RectTransform rectTransform) {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return corners;
    }
}
