using UnityEngine;

public class MaxUserServiceUnityEditor
{
    private static readonly MaxUserServiceUnityEditor _instance = new MaxUserServiceUnityEditor();

    public static MaxUserServiceUnityEditor Instance
    {
        get { return _instance; }
    }

    [System.Obsolete("This API has been deprecated and will be removed in a future release.")]
    public void PreloadConsentDialog()
    {
        MaxSdkLogger.UserWarning("The consent dialog cannot be pre-loaded in the Unity Editor. Please export the project to Android first.");
    }

    [System.Obsolete("This API has been deprecated and will be removed in a future release.")]
    public void ShowConsentDialog()
    {
        MaxSdkLogger.UserWarning("The consent dialog cannot be shown in the Unity Editor. Please export the project to Android first.");
    }
}
