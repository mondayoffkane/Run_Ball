#if UNITY_PURCHASING
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace MondayOFF {
    internal class StoreListener : IStoreListener {
        internal static event Action OnStoreListenerInitialized = default;
        internal static event Action OnBeforePurchase = default;
        internal static event Action<bool> OnAfterPurchase = default;
        internal bool isInitialized = false;

        private IStoreController storeController = default;
        private IExtensionProvider extensionProvider = default;
        private Dictionary<string, ProductData> products = default;

        private readonly byte[] googleTangleData = null;
        private readonly byte[] appleTangleData = null;
        private ValidationStatus validationStatus = default;
        private enum ValidationStatus : int {
            NoValidation = 0,
            ReceiptValidation = 1,
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
            Debug.Log("[EVERYDAY] Initialized Store Listener");

            storeController = controller;
            extensionProvider = extensions;

            isInitialized = true;

            OnStoreListenerInitialized?.Invoke();
            OnStoreListenerInitialized = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error) {
            Debug.Log($"[EVERYDAY] Failed to initialize Store Listener: {error.ToString()}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message) {
            Debug.Log($"[EVERYDAY] Failed to initialize Store Listener: {error.ToString()}\nMessage: {message}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
            Debug.Log($"[EVERYDAY] Failed purchasing : {product.definition.id}");
            OnAfterPurchase?.Invoke(false);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent) {
            var purchasedProductID = purchaseEvent.purchasedProduct.definition.id;

            Debug.Assert(products.ContainsKey(purchasedProductID), $"[EVERYDAY] Why is {purchasedProductID} not found in the catalogue??");

            bool validPurchase = true;
            if (validationStatus == ValidationStatus.ReceiptValidation) {

                var validator = new CrossPlatformValidator(googleTangleData, appleTangleData, Application.identifier);

                try {
                    var result = validator.Validate(purchaseEvent.purchasedProduct.receipt);
                } catch (IAPSecurityException e) {
                    Debug.LogError("[EVERYDAY] Failed to validate receipt");
                    Debug.Log(e.ToString());
                    validPurchase = false;
                }
            }

            if (validPurchase) {
                // Unlock the appropriate content here.
                products[purchasedProductID].CompletePurchase();

                try {
                    SingularSDK.InAppPurchase(purchaseEvent.purchasedProduct, null);
                } catch (System.Exception e) {
                    Debug.LogError("[EVERYDAY] Failed to send purchase data to Singular");
                    Debug.Log(e.ToString());
                }
            }

            OnAfterPurchase?.Invoke(validPurchase);
            return PurchaseProcessingResult.Complete;
        }

        internal IAPStatus RegisterProducts(in string productID, in Action onPurchase) {
            if (!products.TryGetValue(productID, out var productData)) {
                Debug.LogWarning($"[EVERYDAY] {productID} does not exist in EverydayProducts");
                return IAPStatus.ProductDoesNotExist;
            }

            if (productData.isRegistered) {
                Debug.LogWarning($"[EVERYDAY] {productID} is already registered with rewarding callback! Do you really want to replace reward of this product??");
            }

            Debug.Log($"[EVERYDAY] {productID} is registered");
            productData.onPurchase = onPurchase;
            return IAPStatus.Success;
        }

        internal IAPStatus PurchaseProduct(in string productID) {
            if (!products.TryGetValue(productID, out var productData)) {
                Debug.LogWarning($"[EVERYDAY] {productID} does not exist in EverydayProducts");
                return IAPStatus.ProductDoesNotExist;
            }

            if (!isInitialized) {
                Debug.LogWarning("[EVERYDAY] Store listener is not initialized yet. Purchase will be processed after initialization");
                return IAPStatus.StoreListenerNotInitialized;
            }

            if (!productData.isRegistered) {
                Debug.LogWarning($"[EVERYDAY] {productData.productID} is not registered with any rewarding callback. It is NOT safe to process purchase.");
                return IAPStatus.ProductNotRegistered;
            }

            Product p = storeController.products.WithID(productID);
            if (p != null && p.availableToPurchase) {
                OnBeforePurchase?.Invoke();
                Debug.Log(string.Format("[EVERYDAY] Purchasing product : '{0}'", p.definition.id));
                storeController.InitiatePurchase(p);
                return IAPStatus.Success;
            }

            Debug.Log("[EVERYDAY] BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");

            return IAPStatus.PurchaseFailed;
        }

        internal void RestorePurchase() {
#if UNITY_IOS
            if (!isInitialized) {
                Debug.Log("[EVERYDAY] RestorePurchase failed. Store listener is not initialized.");
                return;
            }
            Debug.Log("[EVERYDAY] RestorePurchases started ...");

            var apple = extensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions(
                (result) => { Debug.Log("[EVERYDAY] RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore."); }
            );
#else
            Debug.Log("[EVERYDAY] Calling Restore is not required on this platform");
#endif
        }

        internal StoreListener(in Dictionary<string, ProductData> productList) {
            this.products = productList;

            validationStatus = ValidationStatus.NoValidation;
            try {
                // Get Tangle Data if exists
                var tangleAssembly = Assembly.GetExecutingAssembly();

                var googleTangle = tangleAssembly.GetType("UnityEngine.Purchasing.Security.GooglePlayTangle");
                if (googleTangle != null) {
                    var dataMethod = googleTangle.GetMethod("Data");
                    googleTangleData = (byte[])dataMethod.Invoke(null, null);
                }

                var appleTangle = tangleAssembly.GetType("UnityEngine.Purchasing.Security.AppleTangle");
                if (appleTangle != null) {
                    var dataMethod = appleTangle.GetMethod("Data");
                    appleTangleData = (byte[])dataMethod.Invoke(null, null);
                }

                // They should have both?
                if (googleTangle != null && appleTangle != null) {
                    validationStatus = ValidationStatus.ReceiptValidation;
                } else {
                    throw new Exception($"[EVERYDAY] GoogleTangle : {googleTangle != null}, AppleTangle : {appleTangle != null}");
                }
            } catch (System.Exception e) {
                Debug.LogWarning("[EVERYDAY] No Tangle found, proceed without validation");
                Debug.Log(e.ToString());
            }
        }

#if UNITY_EDITOR
        internal void OnEditorStop() {
            OnStoreListenerInitialized = null;
            OnBeforePurchase = null;
            OnAfterPurchase = null;
        }
#endif
    }
}
#endif