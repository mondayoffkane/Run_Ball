#if UNITY_PURCHASING
using UnityEngine;
using System.Collections.Generic;

namespace MondayOFF {
    // [CreateAssetMenu(menuName = "products", fileName = "Products", order = 10)]
    internal class EverydayProducts : ScriptableObject {
        [Header("Please enter Product IDs here")] public List<ProductData> products = default;
    }

    [System.Serializable]
    internal class ProductData {
        public string productID;
        public UnityEngine.Purchasing.ProductType productType;
        public bool isRegistered => _onPurchase != null && _onPurchase.GetInvocationList().Length > 0;
        public System.Action onPurchase {
            set {
                Debug.Assert(value != null, "[EVERYDAY] Cannot set null to onPurchase!");
                _onPurchase = value;
                if (_purchaseCount > 0) {
                    if (productType == UnityEngine.Purchasing.ProductType.NonConsumable) {
                        _purchaseCount = 1;
                    }
                    for (int i = 0; i < _purchaseCount; ++i) {
                        _onPurchase.Invoke();
                    }

                    _purchaseCount = 0;
                }
            }
        }

        private System.Action _onPurchase = default;
        private int _purchaseCount = 0;

        public void CompletePurchase() {
            if (isRegistered) {
                _onPurchase.Invoke();
            } else {
                Debug.Log($"[EVERYDAY] Purchase was completed but {productID} does not have reward set! Make sure this is the intended behaviour.");
                _purchaseCount++;
            }
        }
    }
}
#endif