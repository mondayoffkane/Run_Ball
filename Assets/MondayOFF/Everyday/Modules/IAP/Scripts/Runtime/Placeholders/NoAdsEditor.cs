#if !UNITY_PURCHASING
using UnityEngine;

namespace MondayOFF {
    public static class NoAds {
        public static System.Action OnNoAds = default;
        public static bool IsNoAds => false;
        public static readonly string NoAdsProductKey = default;

        static NoAds() {
            // Debug.LogWarning("[EVERYDAY] IAP is not enabled! Please add In-App Purchasing Package to the project.");
        }

        public static IAPStatus Purchase() {
            Debug.LogWarning("[EVERYDAY] IAP is not enabled! Please add In-App Purchasing Package to the project.");
            return IAPManager.PurchaseProduct(NoAdsProductKey);
        }

        internal static void OnPurchase() {
            Debug.LogWarning("[EVERYDAY] IAP is not enabled! Please add In-App Purchasing Package to the project.");
        }
    }
}
#endif