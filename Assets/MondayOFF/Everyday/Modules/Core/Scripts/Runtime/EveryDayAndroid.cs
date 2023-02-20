#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace MondayOFF {
    public static partial class EveryDay {
        private static void PrepareSettings(MaxSdkBase.SdkConfiguration sdkConfiguration) {
            // MAX
            MaxSdk.SetHasUserConsent(true);
            MaxSdk.SetDoNotSell(false);

            // FB
            AudienceNetwork.AdSettings.SetDataProcessingOptions(new string[] { });

            // Privacy string
            AdsManager.US_PRIVACY_STRING = "1---";
            AdsManager.HAS_USER_CONSENT = true;
        }
    }
}
#endif