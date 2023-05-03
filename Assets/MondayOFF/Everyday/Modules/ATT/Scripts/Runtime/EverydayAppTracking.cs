
using UnityEngine;
using System.Runtime.InteropServices;

namespace MondayOFF {
    /************************************************************************************************
        MondayOFFAppTracking.RequestTrackingAuthorization(callbackFunction(bool)) 만 호출 하시면 됩니다.
        유저가 ok를 누른 경우에만 callbackFunction에 true가 들어갑니다.
    ************************************************************************************************/
    public static class EverydayAppTracking {
#if UNITY_IOS && !UNITY_EDITOR
        private static System.Action<bool> OnAppTrackingAllow = default;

        /// <summary>Requests App Tracking Authorization to a user.</summary>
        /// <param name="onAllowCallback">Delegate to be called on authorization. True only if the user allows app tracking.</param>
        public static void RequestTrackingAuthorization(System.Action<bool> onAllowCallback) {
            OnAppTrackingAllow = onAllowCallback;
            _RequestTrackingAuthorization(OnCompleteCallback);
        }

        [DllImport("__Internal")]
        private static extern void _RequestTrackingAuthorization(System.Action<bool> onAllowCallback);

        [AOT.MonoPInvokeCallback(typeof(System.Action<bool>))]
        private static void OnCompleteCallback(bool hasAllowed) {
            OnAppTrackingAllow?.Invoke(hasAllowed);
        }

#else
    public static void RequestTrackingAuthorization(System.Action<bool> onAllowCallback) {
        // No action required for Android
        onAllowCallback?.Invoke(true);
    }
#endif
    }
}