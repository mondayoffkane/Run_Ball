using UnityEngine;
using Facebook.Unity;

namespace MondayOFF {
    internal static class FacebookInitializer {
        internal static void Initialize() {
            if (!FB.IsInitialized) {
                try {
                    FB.Init(OnFBInitialization);
                } catch (System.Exception e) {
                    Debug.LogException(e);
                    Debug.LogWarning("[EVERYDAY] Failed to initialize Facebook SDK");
                }
            } else {
                FB.ActivateApp();
            }
        }

        private static void OnFBInitialization() {
            if (FB.IsInitialized) {
                FB.ActivateApp();
            } else {
                Debug.LogWarning("[EVERYDAY] Failed to Initialize the Facebook SDK");
            }
        }
    }
}