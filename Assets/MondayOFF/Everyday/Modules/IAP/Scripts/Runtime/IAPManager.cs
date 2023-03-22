#if UNITY_PURCHASING
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

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
            if (_storeListener == null) {
                Debug.Log($"[EVERYDAY] Store listener is not created yet. {productID} will be registered when it gets initialized");
                var capturedProdID = productID;
                var capturedOnPurchase = onPurchase;
                StoreListener.OnStoreListenerInitialized += () => RegisterProduct(capturedProdID, capturedOnPurchase);
                return IAPStatus.StoreListenerNotInitialized;
            }

            _storeListener.RegisterProducts(productID, onPurchase);
            return IAPStatus.Success;
        }

        public static IAPStatus PurchaseProduct(in string productID) {
            if (_storeListener == null) {
                Debug.Log("[EVERYDAY] Store listener is not created yet");
                return IAPStatus.StoreListenerNotInitialized;
            }
            return _storeListener.PurchaseProduct(productID);
        }

        public static IAPStatus RegisterAndPurchaseProduct(in string productID, in Action onPurchase) {
            var status = RegisterProduct(productID, onPurchase);
            if (status != IAPStatus.Success) {
                return status;
            }
            return PurchaseProduct(productID);
        }

        public static void RestorePurchase() {
            if (_storeListener == null) {
                Debug.Log("[EVERYDAY] Store listener is not created yet");
                return;
            }
            _storeListener.RestorePurchase();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void AfterAssembliesLoaded() {
            System.Threading.Tasks.Task.Run(() => {
                while (EveryDay.settings == null) {
                    System.Threading.Thread.Sleep(100);
                }
                EveryDay.settings.adSettings.IsNoAds = () => NoAds.IsNoAds;
            });
            Initialize();
        }

        private async static void Initialize() {
            if(!EveryDay.isInitialized){
                EveryDay.onEverydayInitialized += Initialize;
                return;
            }

            if (_storeListener != null) {
                Debug.LogWarning("[EVERYDAY] Initialization of IAP Manager is already requested..");
                if (_storeListener.isInitialized) {
                    Debug.LogWarning("[EVERYDAY] IAP Manager is already initialized");
                }
                return;
            }

            Debug.Log("[EVERYDAY] UNITY_PURCHASING is defined.");
            Debug.Log("[EVERYDAY] Initializing IAP Manager");

            EverydayProducts everydayProducts = default;
            var assets = Resources.LoadAll<EverydayProducts>("EverydayProducts");
            if (assets == null || assets.Length <= 0) {
                Debug.Log($"[EVERYDAY] {typeof(EverydayProducts).Name} NOT found, search all");
                assets = Resources.LoadAll<EverydayProducts>("");
            }
            if (assets.Length != 1) {
                Debug.LogError($"[EVERYDAY] Found 0 or multiple {typeof(EverydayProducts).Name}s in Resources folder. There should only be one.");
            } else {
                everydayProducts = assets[0];
            }
            Debug.Assert(everydayProducts != null, "[EVERYDAY] EverydayProducts not found!");

            var module = StandardPurchasingModule.Instance();
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

            // Add product
            Dictionary<string, ProductData> productDict = new Dictionary<string, ProductData>();
            foreach (var item in everydayProducts.products) {
                builder.AddProduct(item.productID, item.productType, new IDs { { item.productID, AppleAppStore.Name }, { item.productID, GooglePlay.Name } });
                productDict.Add(item.productID, item);
            }

            // Default No Ads
            NoAds.RegisterNoAds(in builder, in productDict);

#if UNITY_EDITOR
            await System.Threading.Tasks.Task.Delay(2000);
#endif
            _storeListener = new StoreListener(productDict);

            try {
                var options = new InitializationOptions()
                    .SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);

                UnityPurchasing.Initialize(_storeListener, builder);

            } catch (System.Exception exception) {
                Debug.LogWarning("[EVERYDAY] Failed to initialize IAP Manager");
                Debug.Log(exception.StackTrace);
            }

#if UNITY_EDITOR
            Application.quitting -= OnEditorStop;
            Application.quitting += OnEditorStop;
#endif
        }

#if UNITY_EDITOR
        private static void OnEditorStop() {
            Debug.Log("[EVERYDAY] Stop Playmode IAP Manager");
            _storeListener.OnEditorStop();
            _storeListener = null;
        }
#endif
    }
}
#endif