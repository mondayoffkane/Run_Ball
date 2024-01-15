#if UNITY_PURCHASING
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace MondayOFF
{
    public static class NoAds
    {
        public static System.Action OnNoAds = default;
        public static bool IsNoAds => PlayerPrefs.GetInt(NoAdsProductKey, 0) == 1;
        public static readonly string NoAdsProductKey = default;

        static NoAds()
        {
            var words = Application.identifier.Split('.');
            NoAdsProductKey = $"{words[words.Length - 1]}_noads";
            EverydayLogger.Info($"NoAds Key: {NoAdsProductKey}");
        }

        public static IAPStatus Purchase()
        {
            return IAPManager.PurchaseProduct(NoAdsProductKey);
        }

        internal static void OnPurchase()
        {
            // No Banner and Interstitial 
            AdsManager.DisableAdType(AdType.Banner | AdType.Interstitial);

            // Disable RV, PlayOn, Adverty too?
            OnNoAds?.Invoke();

            PlayerPrefs.SetInt(NoAdsProductKey, 1);
            PlayerPrefs.Save();
        }

        internal static void RegisterNoAds(in ConfigurationBuilder builder, in Dictionary<string, ProductData> productDict)
        {
            builder.AddProduct(NoAds.NoAdsProductKey, ProductType.NonConsumable, new IDs { { NoAds.NoAdsProductKey, AppleAppStore.Name }, { NoAds.NoAdsProductKey, GooglePlay.Name } });
            productDict.Add(NoAds.NoAdsProductKey, new ProductData() { onPurchase = NoAds.OnPurchase, productType = ProductType.NonConsumable });
        }
    }
}
#endif