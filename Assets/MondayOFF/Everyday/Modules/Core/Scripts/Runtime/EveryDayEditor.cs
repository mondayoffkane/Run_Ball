#if UNITY_EDITOR || UNITY_STANDALONE

namespace MondayOFF {
    public static partial class EveryDay {
        private static void PrepareSettings(MaxSdkBase.SdkConfiguration sdkConfiguration) {
            AdsManager.US_PRIVACY_STRING = "1---";
            AdsManager.HAS_USER_CONSENT = true;
        }
    }
}
#endif