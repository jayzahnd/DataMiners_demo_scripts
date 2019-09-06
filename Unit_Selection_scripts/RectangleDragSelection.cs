using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectangleDragSelection      //  inspiration came from : https://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/
{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static Rect GetScreenRect(Camera cameraScreen, Vector3 worldPosition1, Vector3 worldPosition2)
    {
        // My version needs to convert from world to screen space coordinates.
        Vector3 screenPosition1 = cameraScreen.WorldToScreenPoint(worldPosition1);
        Vector3 screenPosition2 = cameraScreen.WorldToScreenPoint(worldPosition2);
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Corners
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Rectangle
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 v1, Vector3 v2)  // Need to use world space and not screen space as input, otherwise the rectangle will move with the camera scroll and not be anchored.
    {
        Vector3 v1VP = camera.WorldToViewportPoint(v1);
        Vector3 v2VP = camera.WorldToViewportPoint(v2);
        Vector3 min = Vector3.Min(v1VP, v2VP);
        Vector3 max = Vector3.Max(v1VP, v2VP);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;
        //min.z = 0.0f;
        //max.z = 1.0f;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        //Debug.Log(min + " "+ max);
        return bounds;
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        RectangleDragSelection.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        RectangleDragSelection.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        RectangleDragSelection.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        RectangleDragSelection.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

}
