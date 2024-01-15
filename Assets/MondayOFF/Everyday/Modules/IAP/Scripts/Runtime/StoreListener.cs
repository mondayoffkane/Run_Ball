#if UNITY_PURCHASING
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace MondayOFF
{
    internal class StoreListener

     : IStoreListener
    {
        internal static event Action OnStoreListenerInitialized = default;
        internal static event Action OnBeforePurchase = default;
        [Obsolete("Please use OnAfterPurchaseWithProduct(PurchaseProcessStatus, string)")]
        internal static event Action<bool> OnAfterPurchase = default;
        internal static event Action<PurchaseProcessStatus, string> OnAfterPurchaseWithProductId = default;
        internal static event Action<Product, PurchaseFailureReason> OnPurchaseFailedEvent = default;
        internal bool isInitialized = false;
        internal bool IsInitialized => isInitialized;

        private IStoreController storeController = default;
        private IExtensionProvider extensionProvider = default;
        private Dictionary<string, ProductData> products = default;

        private readonly byte[] googleTangleData = null;
        private readonly byte[] appleTangleData = null;
        private ValidationStatus validationStatus = default;
        private enum ValidationStatus : int
        {
            NoValidation = 0,
            ReceiptValidation = 1,
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            EverydayLogger.Info("Initialized Store Listener");

            storeController = controller;
            extensionProvider = extensions;

            isInitialized = true;

            OnStoreListenerInitialized?.Invoke();
            OnStoreListenerInitialized = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            EverydayLogger.Info($"Failed to initialize Store Listener: {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            EverydayLogger.Info($"Failed to initialize Store Listener: {error}\nMessage: {message}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            EverydayLogger.Info($"Failed purchasing : {product.definition.id}");
            OnPurchaseFailedEvent?.Invoke(product, failureReason);
            OnAfterPurchase?.Invoke(false);
            OnAfterPurchaseWithProductId?.Invoke(PurchaseProcessStatus.PURCHASE_FAILED, product.definition.id);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var purchasedProductID = purchaseEvent.purchasedProduct.definition.id;

            Debug.Assert(products.ContainsKey(purchasedProductID), $"[EVERYDAY] Why is {purchasedProductID} not found in the catalogue??");

            bool validPurchase = true;
            if (validationStatus == ValidationStatus.ReceiptValidation)
            {

                var validator = new CrossPlatformValidator(googleTangleData, appleTangleData, Application.identifier);

                try
                {
                    var result = validator.Validate(purchaseEvent.purchasedProduct.receipt);
                }
                catch (IAPSecurityException e)
                {
                    EverydayLogger.Error("Failed to validate receipt");
                    Debug.LogException(e);
                    validPurchase = false;
                }
            }

            if (validPurchase)
            {
                // Unlock the appropriate content here.
                products[purchasedProductID].CompletePurchase();

                try
                {
                    SingularSDK.InAppPurchase(purchaseEvent.purchasedProduct, null);
                }
                catch (System.Exception e)
                {
                    EverydayLogger.Error("Failed to send purchase data to Singular");
                    Debug.LogException(e);
                }
            }

            OnAfterPurchase?.Invoke(validPurchase);
            OnAfterPurchaseWithProductId?.Invoke(validPurchase ? PurchaseProcessStatus.VALID : PurchaseProcessStatus.INVALID_RECEIPT, purchaseEvent.purchasedProduct.definition.id);
            return PurchaseProcessingResult.Complete;
        }

        internal (string, string) GetLocalizedPrice(in string productID)
        {
            if (!products.TryGetValue(productID, out var productData))
            {
                EverydayLogger.Warn($"{productID} does not exist in EverydayProducts");
                return ("", "");
            }

            Product p = storeController.products.WithID(productID);
            if (p != null && p.availableToPurchase)
            {
                return (p.metadata.localizedPriceString, p.metadata.isoCurrencyCode);
            }

            EverydayLogger.Warn("GetLocalizedPrice: FAIL. Either is not found or is not available for purchase");

            return ("", "");
        }

        internal IAPStatus RegisterProducts(in string productID, in Action onPurchase)
        {
            if (!products.TryGetValue(productID, out var productData))
            {
                EverydayLogger.Warn($"{productID} does not exist in EverydayProducts");
                return IAPStatus.ProductDoesNotExist;
            }

            if (productData.isRegistered)
            {
                EverydayLogger.Warn($"{productID} is already registered with rewarding callback! Do you really want to replace reward of this product??");
            }

            EverydayLogger.Info($"{productID} is registered");
            productData.onPurchase = onPurchase;
            return IAPStatus.Success;
        }

        internal IAPStatus PurchaseProduct(in string productID)
        {
            if (!products.TryGetValue(productID, out var productData))
            {
                EverydayLogger.Warn($"{productID} does not exist in EverydayProducts");
                return IAPStatus.ProductDoesNotExist;
            }

            if (!isInitialized)
            {
                EverydayLogger.Warn("Store listener is not initialized yet. Purchase will be processed after initialization");
                return IAPStatus.StoreListenerNotInitialized;
            }

            if (!productData.isRegistered)
            {
                EverydayLogger.Warn($"{productData.productID} is not registered with any rewarding callback. It is NOT safe to process purchase.");
                return IAPStatus.ProductNotRegistered;
            }

            Product p = storeController.products.WithID(productID);
            if (p != null && p.availableToPurchase)
            {
                OnBeforePurchase?.Invoke();
                EverydayLogger.Info($"Purchasing product : '{p.definition.id}'");
                storeController.InitiatePurchase(p);
                return IAPStatus.Success;
            }

            EverydayLogger.Warn("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");

            return IAPStatus.PurchaseFailed;
        }

        internal void RestorePurchase()
        {
#if UNITY_IOS
            if (!isInitialized)
            {
                EverydayLogger.Info("RestorePurchase failed. Store listener is not initialized.");
                return;
            }
            EverydayLogger.Info("RestorePurchases started ...");

            var apple = extensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions(
                (result) => { EverydayLogger.Info("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore."); }
            );
#else
            EverydayLogger.Info("Calling Restore is not required on this platform");
#endif
        }

        internal StoreListener(in Dictionary<string, ProductData> productList)
        {
            this.products = productList;

            validationStatus = ValidationStatus.NoValidation;
            try
            {
                // Get Tangle Data if exists
                var tangleAssembly = Assembly.GetExecutingAssembly();

                var googleTangle = tangleAssembly.GetType("UnityEngine.Purchasing.Security.GooglePlayTangle");
                if (googleTangle != null)
                {
                    var dataMethod = googleTangle.GetMethod("Data");
                    googleTangleData = (byte[])dataMethod.Invoke(null, null);
                }

                var appleTangle = tangleAssembly.GetType("UnityEngine.Purchasing.Security.AppleTangle");
                if (appleTangle != null)
                {
                    var dataMethod = appleTangle.GetMethod("Data");
                    appleTangleData = (byte[])dataMethod.Invoke(null, null);
                }

                // They should have both?
                if (googleTangle != null && appleTangle != null)
                {
                    validationStatus = ValidationStatus.ReceiptValidation;
                }
                else
                {
                    throw new Exception($"[EVERYDAY] GoogleTangle : {googleTangle != null}, AppleTangle : {appleTangle != null}");
                }
            }
            catch (System.Exception e)
            {
                EverydayLogger.Warn("No Tangle found, proceed without validation");
                EverydayLogger.Warn(e.Message);
                // Debug.LogException(e);
            }
        }

#if UNITY_EDITOR
        internal void OnEditorStop()
        {
            OnStoreListenerInitialized = null;
            OnBeforePurchase = null;
            OnAfterPurchase = null;
            OnAfterPurchaseWithProductId = null;
            OnPurchaseFailedEvent = null;
        }
#endif
    }
}
#endif