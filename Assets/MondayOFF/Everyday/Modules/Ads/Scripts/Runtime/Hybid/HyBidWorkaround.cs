#if !UNITY_EDITOR
using UnityEngine;

namespace MondayOFF {
    internal static class HyBidWorkaround {
        const int INTERSTITIAL_SKIP_OFFSET = 5;
#if UNITY_ANDROID
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void SetOffset() {
            try {
                using (AndroidJavaClass HyBidSetting = new AndroidJavaClass("net.pubnative.lite.sdk.HyBid")) {
                    using (AndroidJavaObject offsetInteger = new AndroidJavaObject("java.lang.Integer", INTERSTITIAL_SKIP_OFFSET)) {
                        HyBidSetting.CallStatic("setHtmlInterstitialSkipOffset", offsetInteger);
                        HyBidSetting.CallStatic("setVideoInterstitialSkipOffset", offsetInteger);
                        Debug.Log($"[FIX] Setting HyBid skip offset set to {INTERSTITIAL_SKIP_OFFSET}s");
                    }
                }
            } catch (System.Exception e) {
                Debug.Log(e.ToString());
            }
        }
#elif UNITY_IOS
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void SetOffset() {
            try {
                SetHyBidOffsets(INTERSTITIAL_SKIP_OFFSET);
                Debug.Log($"[FIX] Setting HyBid skip offset set to {INTERSTITIAL_SKIP_OFFSET}s");
            } catch (System.Exception e) {
                Debug.LogWarning(e.ToString());
            }
        }

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetHyBidOffsets(int skipOffset);
#endif
    }
}
#endif