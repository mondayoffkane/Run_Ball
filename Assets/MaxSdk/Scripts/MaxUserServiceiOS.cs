using System.Runtime.InteropServices;

#if UNITY_IOS
public class MaxUserServiceiOS
{
    private static readonly MaxUserServiceiOS _instance = new MaxUserServiceiOS();

    public static MaxUserServiceiOS Instance
    {
        get { return _instance; }
    }

    [System.Obsolete("This API has been deprecated and will be removed in a future release.")]
    public void PreloadConsentDialog() {}

    [DllImport("__Internal")]
    private static extern void _MaxShowConsentDialog();

    [System.Obsolete("This API has been deprecated and will be removed in a future release.")]   
    public void ShowConsentDialog()
    {
        _MaxShowConsentDialog();
    }
}
#endif
