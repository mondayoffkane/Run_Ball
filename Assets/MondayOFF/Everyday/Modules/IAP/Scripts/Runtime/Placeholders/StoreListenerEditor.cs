#if !UNITY_PURCHASING
using System;

namespace MondayOFF {
    internal class StoreListener {
        internal static event Action OnStoreListenerInitialized = default;
        internal static event Action OnBeforePurchase = default;
        internal static event Action<bool> OnAfterPurchase = default;

        internal void RestorePurchase() {
        }

        internal StoreListener() {
        }
    }
}
#endif