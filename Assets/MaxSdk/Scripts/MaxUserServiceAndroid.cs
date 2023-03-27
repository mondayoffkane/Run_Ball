using UnityEngine;

public class MaxUserServiceAndroid
{
    private static readonly AndroidJavaClass _maxUnityPluginClass = new AndroidJavaClass("com.applovin.mediation.unity.MaxUnityPlugin");
    private static readonly MaxUserServiceAndroid _instance = new MaxUserServiceAndroid();

    public static MaxUserServiceAndroid Instance
    {
        get { return _instance; }
    }


    [System.Obsolete("This API has been deprecated and will be removed in a future release.")]
    public void PreloadConsentDialog()
    {
        _maxUnityPluginClass.CallStatic("preloadConsentDialog");
    }

    [System.Obsolete("This API has been deprecated and will be removed in a future release.")]
    public void ShowConsentDialog()
    {
        _maxUnityPluginClass.CallStatic("showConsentDialog");
    }
}
