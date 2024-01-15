#if UNITY_PURCHASING
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace MondayOFF
{
    public static class IAPManager
    {
        public static event Action OnBeforePurchase
        {
            add
            {
                StoreListener.OnBeforePurchase += value;
            }
            remove
            {
                StoreListener.OnBeforePurchase -= value;
            }
        }
        [Obsolete("Please use OnAfterPurchaseWithProduct(PurchaseProcessStatus, string)")]
        public static event Action<bool> OnAfterPurchase
        {
            add
            {
                StoreListener.OnAfterPurchase += value;
            }
            remove
            {
                StoreListener.OnAfterPurchase -= value;
            }
        }

        public static event Action<PurchaseProcessStatus, string> OnAfterPurchaseWithProductId
        {
            add
            {
                StoreListener.OnAfterPurchaseWithProductId += value;
            }
            remove
            {
                StoreListener.OnAfterPurchaseWithProductId -= value;
            }
        }

        public static event Action<Product, PurchaseFailureReason> OnPurchaseFailed
        {
            add
            {
                StoreListener.OnPurchaseFailedEvent += value;
            }
            remove
            {
                StoreListener.OnPurchaseFailedEvent -= value;
            }
        }

        public static bool IsInitialized => _storeListener != null && _storeListener.isInitialized;

        private static StoreListener _storeListener = default;

        public static (string isoCurrencyCode, string localizedPriceString) GetLocalizedPrice(in string productID)
        {
            if (_storeListener == null)
            {
                EverydayLogger.Info("Store listener is not created yet");
                return default;
            }

            return _storeListener.GetLocalizedPrice(productID);
        }

        public static IAPStatus RegisterProduct(in string productID, in Action onPurchase)
        {
            if (_storeListener == null)
            {
                EverydayLogger.Info($"Store listener is not created yet. {productID} will be registered when it gets initialized");
                var capturedProdID = productID;
                var capturedOnPurchase = onPurchase;
                StoreListener.OnStoreListenerInitialized += () => RegisterProduct(capturedProdID, capturedOnPurchase);
                return IAPStatus.StoreListenerNotInitialized;
            }

            _storeListener.RegisterProducts(productID, onPurchase);
            return IAPStatus.Success;
        }

        public static IAPStatus PurchaseProduct(in string productID)
        {
            if (_storeListener == null)
            {
                EverydayLogger.Info("Store listener is not created yet");
                return IAPStatus.StoreListenerNotInitialized;
            }
            return _storeListener.PurchaseProduct(productID);
        }

        public static IAPStatus RegisterAndPurchaseProduct(in string productID, in Action onPurchase)
        {
            var status = RegisterProduct(productID, onPurchase);
            if (status != IAPStatus.Success)
            {
                return status;
            }
            return PurchaseProduct(productID);
        }

        public static void RestorePurchase()
        {
            if (_storeListener == null)
            {
                EverydayLogger.Info("Store listener is not created yet");
                return;
            }
            _storeListener.RestorePurchase();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            // System.Threading.Tasks.Task.Run(() =>
            // {
            //     while (EverydaySettings.Instance == null)
            //     {
            //         System.Threading.Thread.Sleep(100);
            //     }
            //     EverydaySettings.AdSettings.IsNoAds = () => NoAds.IsNoAds;
            // });

            EverydaySettings.AdSettings.IsNoAds = () => NoAds.IsNoAds;

            Initialize();
        }

        private async static void Initialize()
        {
            if (!EveryDay.isInitialized)
            {
                EveryDay.OnEverydayInitialized += Initialize;
                return;
            }

            if (_storeListener != null)
            {
                EverydayLogger.Warn("Initialization of IAP Manager is already requested..");
                if (_storeListener.isInitialized)
                {
                    EverydayLogger.Warn("IAP Manager is already initialized");
                }
                return;
            }

            EverydayLogger.Info("UNITY_PURCHASING is defined.");
            EverydayLogger.Info("Initializing IAP Manager");

            EverydayProducts everydayProducts = default;
            var assets = Resources.LoadAll<EverydayProducts>("EverydayProducts");
            if (assets == null || assets.Length <= 0)
            {
                EverydayLogger.Info($"{typeof(EverydayProducts).Name} NOT found, search all");
                assets = Resources.LoadAll<EverydayProducts>("");
            }
            if (assets.Length != 1)
            {
                EverydayLogger.Error($"Found 0 or multiple {typeof(EverydayProducts).Name}s in Resources folder. There should only be one.");
            }
            else
            {
                everydayProducts = assets[0];
            }
            Debug.Assert(everydayProducts != null, "[EVERYDAY] EverydayProducts not found!");

            var module = StandardPurchasingModule.Instance();
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

            // Add product
            Dictionary<string, ProductData> productDict = new Dictionary<string, ProductData>();
            foreach (var item in everydayProducts.products)
            {
                if (productDict.TryAdd(item.productID, item))
                {
                    builder.AddProduct(item.productID, item.productType, new IDs { { item.productID, AppleAppStore.Name }, { item.productID, GooglePlay.Name } });
                }
                else
                {
                    EverydayLogger.Error($"Duplicate productID found: {item.productID}");
                }
            }

            // Default No Ads
            NoAds.RegisterNoAds(in builder, in productDict);

#if UNITY_EDITOR
            await System.Threading.Tasks.Task.Delay(2000);
#endif
            _storeListener = new StoreListener(productDict);

            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);

                UnityPurchasing.Initialize(_storeListener, builder);

            }
            catch (System.Exception exception)
            {
                EverydayLogger.Warn("Failed to initialize IAP Manager");
                Debug.LogException(exception);
            }

#if UNITY_EDITOR
            Application.quitting -= OnEditorStop;
            Application.quitting += OnEditorStop;
#endif
        }

#if UNITY_EDITOR
        private static void OnEditorStop()
        {
            EverydayLogger.Info("Stop Playmode IAP Manager");
            _storeListener.OnEditorStop();
            _storeListener = null;
        }
#endif
    }
}
#endif