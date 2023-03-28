using UnityEngine;

namespace AudienceNetwork {
    public static class AdSettings {
#if UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetDataProcessingOptions(string[] dataProcessingOptions, int length);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetDetailedDataProcessingOptions(string[] dataProcessingOptions, int length, int country, int state);

        [System.Runtime.InteropServices.DllImport("__Internal")] 
        private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled) {
            FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }
#endif

        public static void SetDataProcessingOptions(string[] dataProcessingOptions) {
#if UNITY_ANDROID
            try {
                AndroidJavaClass adSettings = new AndroidJavaClass("com.facebook.ads.AdSettings");
                adSettings.CallStatic("setDataProcessingOptions", (object)dataProcessingOptions);
            } catch (System.Exception e) {
                Debug.Log("SetDataProcessingOptions: " + e.Message);
            }
#endif

#if UNITY_IOS
            FBAdSettingsBridgeSetDataProcessingOptions(dataProcessingOptions, dataProcessingOptions.Length);
#endif
        }

        public static void SetDataProcessingOptions(string[] dataProcessingOptions, int country, int state) {
#if UNITY_ANDROID
            try {
                AndroidJavaClass adSettings = new AndroidJavaClass("com.facebook.ads.AdSettings");
                adSettings.CallStatic("setDataProcessingOptions", (object)dataProcessingOptions, country, state);
            } catch (System.Exception e) {
                Debug.Log("SetDataProcessingOptions: " + e.Message);
            }
#endif

#if UNITY_IOS
            FBAdSettingsBridgeSetDetailedDataProcessingOptions(dataProcessingOptions, dataProcessingOptions.Length, country, state);
#endif
        }
    }
}