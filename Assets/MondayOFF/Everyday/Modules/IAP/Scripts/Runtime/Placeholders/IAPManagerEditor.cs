#if !UNITY_PURCHASING
using System;
using UnityEngine;

namespace MondayOFF {
    public static class IAPManager {
        public static event Action OnBeforePurchase {
            add {
                StoreListener.OnBeforePurchase += value;
            }
            remove {
                StoreListener.OnBeforePurchase -= value;
            }
        }
        public static event Action<bool> OnAfterPurchase {
            add {
                StoreListener.OnAfterPurchase += value;
            }
            remove {
                StoreListener.OnAfterPurchase -= value;
            }
        }

        private static StoreListener _storeListener = default;

        public static IAPStatus RegisterProduct(in string productID, in Action onPurchase) {
            Debug.LogWarning("[EVERYDAY] IAP is not enabled! Please add In-App Purchasing Package to the project.");
            return IAPStatus.StoreListenerNotInitialized;
        }

        public static IAPStatus PurchaseProduct(in string productID) {
            Debug.LogWarning("[EVERYDAY] IAP is not enabled! Please add In-App Purchasing Package to the project.");
            return IAPStatus.StoreListenerNotInitialized;
        }

        public static IAPStatus RegisterAndPurchaseProduct(in string productID, in Action onPurchase) {
            Debug.LogWarning("[EVERYDAY] IAP is not enabled! Please add In-App Purchasing Package to the project.");
            return IAPStatus.StoreListenerNotInitialized;
        }

        public static void RestorePurchase() {
            Debug.LogWarning("[EVERYDAY] IAP is not enabled! Please add In-App Purchasing Package to the project.");
        }

        public static void Initialze() {
            Debug.Log("[EVERYDAY] UNITY_PURCHASING is not defined.");
        }
    }
}
#endif