using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtension
{
    public static Rect GetScreenRect(RectTransform rectTransform, Canvas canvas)
    {
        Vector3[] corners = new Vector3[4];
        Vector3[] screenCorners = new Vector3[2];
        rectTransform.GetWorldCorners(corners);
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
        {
            screenCorners[0] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[1]);
            screenCorners[1] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[3]);
        }
        else
        {
            screenCorners[0] = RectTransformUtility.WorldToScreenPoint(null, corners[1]);
            screenCorners[1] = RectTransformUtility.WorldToScreenPoint(null, corners[3]);
        }
        screenCorners[0].y = screenCorners[0].y - Screen.safeArea.y;
        screenCorners[1].y = screenCorners[1].y - Screen.safeArea.y;
        
        int navigationBarHeight = 0;
        if (!Screen.fullScreen) {
#if UNITY_ANDROID && !UNITY_EDITOR // Support Navigation Bar offset in Android
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject resources = activity.Call<AndroidJavaObject>("getResources");
            int resourceId = resources.Call<int>("getIdentifier", "navigation_bar_height", "dimen", "android");
            if (resourceId > 0) {
                navigationBarHeight = resources.Call<int>("getDimensionPixelSize", resourceId);
            }
#endif
        }

        Rect rect = new Rect(screenCorners[0], screenCorners[0] - screenCorners[1]);
        rect = new Rect(new Vector2(rect.xMin, rect.yMin - rect.size.y + navigationBarHeight), new Vector2(-rect.size.x, rect.size.y));
        return rect;
    }

    public static bool IsRectContainsRect(Rect ad, Rect parentRect)
    {
        float equalFactorRemover = .1f; // making sure that points are in different dimensions
        bool topLeft = parentRect.Contains(new Vector2(ad.xMin + equalFactorRemover, ad.yMax - equalFactorRemover));
        bool rightBot = parentRect.Contains(new Vector2(ad.xMax - equalFactorRemover, ad.yMin + equalFactorRemover));
        bool topRight = parentRect.Contains(new Vector2(ad.xMax - equalFactorRemover, ad.yMax - equalFactorRemover));
        bool leftBot = parentRect.Contains(new Vector2(ad.xMin + equalFactorRemover, ad.yMin + equalFactorRemover));
        return topLeft && rightBot && topRight && leftBot;
    }

    public static Vector2 ConvertRectToPosition(Rect rect, PlayOnSDK.Position position, int size)
    {
        Vector2 result = Vector2.zero;
        float halfSize = (float)size / 2f;
        switch (position)
        {
            case PlayOnSDK.Position.Centered:
                result.x = rect.center.x - halfSize;
                result.y = rect.center.y - halfSize;
                break;
            case PlayOnSDK.Position.BottomLeft:
                result.x = rect.xMin;
                result.y = rect.yMin;
                break;
            case PlayOnSDK.Position.BottomRight:
                result.x = rect.xMax - size;
                result.y = rect.yMin;
                break;
            case PlayOnSDK.Position.TopLeft:
                result.x = rect.xMin;
                result.y = rect.yMax - size;
                break;
            case PlayOnSDK.Position.TopRight:
                result.x = rect.xMax - size;
                result.y = rect.yMax - size;
                break;
            case PlayOnSDK.Position.CenterLeft:
                result.x = rect.xMin;
                result.y = rect.center.y - halfSize;
                break;
            case PlayOnSDK.Position.CenterRight:
                result.x = rect.xMax - size;
                result.y = rect.center.y - halfSize;
                break;
            case PlayOnSDK.Position.BottomCenter:
                result.x = rect.center.x - halfSize;
                result.y = rect.yMin;
                break;
            case PlayOnSDK.Position.TopCenter:
                result.x = rect.center.x - halfSize;
                result.y = rect.yMax - size;
                break;
        }
        return result;
    }

    public static Vector2 PixelPositionToDP(Vector2 pos)
    {
        return pos / PlayOnSDK.GetDeviceScale();
    }
}