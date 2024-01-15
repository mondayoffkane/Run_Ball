#if !UNITY_PURCHASING
using System;

namespace MondayOFF {
    internal class StoreListener {
        internal static event Action OnStoreListenerInitialized = default;
        internal static event Action OnBeforePurchase = default;
        [Obsolete("Please use OnAfterPurchaseWithProduct(PurchaseProcessStatus, string)")]
        internal static event Action<bool> OnAfterPurchase = default;
        internal static event Action<PurchaseProcessStatus, string> OnAfterPurchaseWithProductId = default;

        internal void RestorePurchase() {
        }

        internal StoreListener() {
        }
    }
}
#endif