using UnityEngine;
using MondayOFF;

public class EverydayAdTestScript : MonoBehaviour {
    [SerializeField] GameObject _bigBadCanvas = default;
    [SerializeField] RectTransform _playonAdUnitAnchor = default;

    private void Start() {
        AdsManager.OnRewardedAdLoaded -= OnRewarededReady;
        AdsManager.OnRewardedAdLoaded += OnRewarededReady;

        AdsManager.OnBeforeInterstitial -= ShowBadCanvas;
        AdsManager.OnBeforeInterstitial += ShowBadCanvas;

        AdsManager.OnAfterInterstitial -= HideBadCanvas;
        AdsManager.OnAfterInterstitial += HideBadCanvas;


        AdsManager.OnBeforeRewarded -= ShowBadCanvas;
        AdsManager.OnBeforeRewarded += ShowBadCanvas;

        AdsManager.OnAfterRewarded -= HideBadCanvas;
        AdsManager.OnAfterRewarded += HideBadCanvas;

        IAPManager.OnBeforePurchase -= ShowBadCanvas;
        IAPManager.OnBeforePurchase += ShowBadCanvas;

        IAPManager.OnAfterPurchase -= HideBadCanvas;
        IAPManager.OnAfterPurchase += HideBadCanvas;

        // UserData userData = new UserData(AgeSegment.Unknown, Gender.Unknown);
        // Adverty.AdvertySDK.Init(userData);

        Adverty.AdvertySettings.SetMainCamera(Camera.main);
    }

    private void ShowBadCanvas() {
        Debug.Log("[EVERYDAY] SHOW LOADING CANVAS");
        _bigBadCanvas.SetActive(true);
    }

    private void HideBadCanvas() {
        Debug.Log("[EVERYDAY] HIDE LOADING CANVAS");
        _bigBadCanvas.SetActive(false);
    }

    private void HideBadCanvas(bool isSuccess) {
        string message = default;
        if (isSuccess) {
            message = "blue>Successful";
        } else {
            message = "red>Failed";
        }
        Debug.Log($"[EVERYDAY] Purchase <color={message}</color>");
        _bigBadCanvas.SetActive(false);
    }

    private void OnRewarededReady() {
        Debug.Log("[EVERYDAY] REWARED IS READY");
    }

    public void Ads_InitializeAdsManager() {
        AdsManager.Initialize();
    }

    public void Ads_ShowIS() {
        AdsManager.ShowInterstitial();
    }

    public void Ads_ShowRV() {
        AdsManager.ShowRewarded(() => Debug.Log("-- Your Reward --"));
    }

    public void Ads_ShowBN() {
        AdsManager.ShowBanner();
    }

    public void Ads_HideBN() {
        AdsManager.HideBanner();
    }

    public void Ads_SetPlayOnPos() {
        AdsManager.LinkLogoToRectTransform(PlayOnSDK.Position.Centered, _playonAdUnitAnchor, _playonAdUnitAnchor.GetComponentInParent<Canvas>());
    }

    public void Ads_ShowPlayOn() {
        AdsManager.ShowPlayOn();
    }

    public void Ads_HidePlayOn() {
        AdsManager.HidePlayOn();
    }

    public void Ads_DisableIS() {
        // Either method works. DisableAdType(AdType) is useful when disabling multiple ad types.
        // AdsManager.DisableIS();
        AdsManager.DisableAdType(AdType.Interstitial);
    }

    public void Ads_DisableRV() {
        // AdsManager.DisableRV();
        AdsManager.DisableAdType(AdType.Rewarded);
    }

    public void Ads_DisableBN() {
        // AdsManager.DisableBN();
        AdsManager.DisableAdType(AdType.Banner);
    }

    public void IAP_InitializeIAPManager() {
        // IAPManager.Initialize();
    }

    public void IAP_RegisterCallbackConsumable() {
        IAPManager.RegisterProduct("Consumable", () => {
            Debug.Log("[EVERYDAY] Purchased Consumable");
        });
    }

    public void IAP_PurchaseConsumable() {
        IAPManager.PurchaseProduct("Consumable");
    }

    public void IAP_RegisterCallbackNonConsumable() {
        IAPManager.RegisterProduct("Non Consumable", () => {
            Debug.Log("[EVERYDAY] Purchased Non Consumable");
            PlayerPrefs.SetInt("Non Consumable", 1);
            PlayerPrefs.Save();
        });
    }

    public void IAP_PurchaseNonConsumable() {
        if (PlayerPrefs.GetInt("Non Consumable", 0) > 0) {
            Debug.Log("[EVERYDAY] Non Consumable is already purchased! Do you really want to re-purchase it?");
            return;
        }

        IAPManager.PurchaseProduct("Non Consumable");
    }

    public void IAP_PurchaseNoAds() {
        if (NoAds.IsNoAds) {
            Debug.Log("[EVERYDAY] No Ads is already purchased!");
            return;
        }

        NoAds.OnNoAds += ()=>Debug.Log("[EVERYDAY] Bought No Ads");
        
        // They are the same
        // IAPManager.PurchaseProduct(NoAds.NoAdsProductKey); 
        NoAds.Purchase();
    }

    public void IAP_RestorePurchase() {
        IAPManager.RestorePurchase();
    }

    public void Events_TryStage(int stageNum) {
        EventTracker.TryStage(stageNum);
    }

    public void Events_ClearStage(int stageNum) {
        EventTracker.ClearStage(stageNum);
    }
}
