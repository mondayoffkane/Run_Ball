using UnityEngine;
using UnityEngine.UI;
using MondayOFF;
using AudienceNetwork;

public class EverydayTestScript : MonoBehaviour
{
    [SerializeField] GameObject _bigBadCanvas = default;
    [SerializeField] RectTransform _playonAdUnitAnchor = default;
    [Space(10)]
    [SerializeField] Button _initializeAdsManagerButton = default;
    [SerializeField] Button _showInterstitialButton = default;
    [SerializeField] Button _showRewardedButton = default;
    [SerializeField] Button _showBannerButton = default;
    [SerializeField] Button _hideBannerButton = default;


    private void Start()
    {
        AdsManager.ShowBanner();
        AdsManager.OnInitialized -= HideInitializeAdsManagerButton;
        AdsManager.OnInitialized += HideInitializeAdsManagerButton;

        AdsManager.OnRewardedAdLoaded -= OnRewarededReady;
        AdsManager.OnRewardedAdLoaded += OnRewarededReady;

        AdsManager.OnBeforeInterstitial -= ShowBadCanvas;
        AdsManager.OnBeforeInterstitial += ShowBadCanvas;

        AdsManager.OnAfterInterstitial -= HideBadCanvasAfterAd;
        AdsManager.OnAfterInterstitial += HideBadCanvasAfterAd;

        AdsManager.OnBeforeRewarded -= ShowBadCanvas;
        AdsManager.OnBeforeRewarded += ShowBadCanvas;

        AdsManager.OnAfterRewarded -= HideBadCanvasAfterAd;
        AdsManager.OnAfterRewarded += HideBadCanvasAfterAd;

        IAPManager.OnBeforePurchase -= ShowBadCanvas;
        IAPManager.OnBeforePurchase += ShowBadCanvas;

        // IAPManager.OnAfterPurchase -= HideBadCanvas;
        // IAPManager.OnAfterPurchase += HideBadCanvas;

        IAPManager.OnAfterPurchaseWithProductId -= HideBadCanvasAfterIap;
        IAPManager.OnAfterPurchaseWithProductId += HideBadCanvasAfterIap;

        Adverty.AdvertySettings.SetMainCamera(Camera.main);
    }

    private void Update()
    {
        var currentLogLevel = EverydaySettings.GetLogLevel();
        EverydaySettings.SetLogLevel(LogLevel.Warning);
        var timeUntilNextInterstitial = AdsManager.GetTimeUntilNextInterstitial();

        if (AdsManager.IsInterstitialReady())
        {
            if (timeUntilNextInterstitial > 0)
            {
                _showInterstitialButton.GetComponentInChildren<Text>().text = $"Waiting for {timeUntilNextInterstitial:0.00} seconds..";
                _showInterstitialButton.interactable = false;
            }
            else
            {
                _showInterstitialButton.GetComponentInChildren<Text>().text = "Show Interstitial";
                _showInterstitialButton.interactable = true;
            }
        }
        else
        {
            _showInterstitialButton.GetComponentInChildren<Text>().text = "Loading Interstitial..";
            _showInterstitialButton.interactable = false;
        }

        if (AdsManager.IsRewardedReady())
        {
            _showRewardedButton.GetComponentInChildren<Text>().text = "Show Rewarded";
            _showRewardedButton.interactable = true;
        }
        else
        {
            _showRewardedButton.gameObject.GetComponentInChildren<Text>().text = "Loading Rewarded..";
            _showRewardedButton.interactable = false;
        }

        if (AdsManager.IsBannerReady())
        {
            bool isBannerDisplayed = AdsManager.IsBannerDisplayed();
            _showBannerButton.interactable = !isBannerDisplayed;
            _hideBannerButton.interactable = isBannerDisplayed;
        }
        else
        {
            _showBannerButton.interactable = _hideBannerButton.interactable = false;
        }
        EverydaySettings.SetLogLevel(currentLogLevel);
    }

    private void HideInitializeAdsManagerButton()
    {
        _initializeAdsManagerButton.interactable = false;
        _initializeAdsManagerButton.GetComponentInChildren<Text>().text = "Ads Manager Initialized";
    }

    private void ShowBadCanvas()
    {
        EverydayLogger.Info("SHOW LOADING CANVAS");
        _bigBadCanvas.SetActive(true);
    }

    private void HideBadCanvasAfterAd()
    {
        EverydayLogger.Info("HIDE LOADING CANVAS AFTER AD");
        _bigBadCanvas.SetActive(false);
    }

    private void HideBadCanvasAfterIap(PurchaseProcessStatus processStatus, string productId)
    {
        EverydayLogger.Info("HIDE LOADING CANVAS AFTER PURCHASE");
        _bigBadCanvas.SetActive(false);
    }

    private void OnRewarededReady()
    {
        EverydayLogger.Info("REWARED IS READY");
    }

    public void Ads_InitializeAdsManager()
    {
        AdsManager.Initialize();
    }

    public void Ads_ShowIS()
    {
        AdsManager.ShowInterstitial();
    }

    public void Ads_ShowRV()
    {
        AdsManager.ShowRewarded(() => EverydayLogger.Info("-- Your Reward --"));
    }

    public void Ads_ShowBN()
    {
        AdsManager.ShowBanner();
    }

    public void Ads_HideBN()
    {
        AdsManager.HideBanner();
    }

    public void Ads_SetPlayOnPos()
    {
        AdsManager.LinkLogoToRectTransform(PlayOnSDK.Position.Centered, _playonAdUnitAnchor, _playonAdUnitAnchor.GetComponentInParent<Canvas>());
    }

    public void Ads_ShowPlayOn()
    {
        AdsManager.ShowPlayOn();
    }

    public void Ads_HidePlayOn()
    {
        AdsManager.HidePlayOn();
    }

    public void Ads_DisableIS()
    {
        // Either method works. DisableAdType(AdType) is useful when disabling multiple ad types.
        // AdsManager.DisableIS();
        AdsManager.DisableAdType(AdType.Interstitial);
    }

    public void Ads_DisableRV()
    {
        // AdsManager.DisableRV();
        AdsManager.DisableAdType(AdType.Rewarded);
    }

    public void Ads_DisableBN()
    {
        // AdsManager.DisableBN();
        AdsManager.DisableAdType(AdType.Banner);
    }

    public void IAP_InitializeIAPManager()
    {
        // IAPManager.Initialize();
    }

    public void IAP_RegisterCallbackConsumable()
    {
        IAPManager.RegisterProduct("Consumable", () =>
        {
            EverydayLogger.Info("Purchased Consumable");
        });
    }

    public void IAP_PurchaseConsumable()
    {
        IAPManager.PurchaseProduct("Consumable");
    }

    public void IAP_RegisterCallbackNonConsumable()
    {
        IAPManager.RegisterProduct("Non Consumable", () =>
        {
            EverydayLogger.Info("Purchased Non Consumable");
            PlayerPrefs.SetInt("Non Consumable", 1);
            PlayerPrefs.Save();
        });
    }

    public void IAP_PurchaseNonConsumable()
    {
        if (PlayerPrefs.GetInt("Non Consumable", 0) > 0)
        {
            EverydayLogger.Info("Non Consumable is already purchased! Do you really want to re-purchase it?");
            return;
        }

        IAPManager.PurchaseProduct("Non Consumable");
    }

    public void IAP_PurchaseNoAds()
    {
        if (NoAds.IsNoAds)
        {
            EverydayLogger.Info("No Ads is already purchased!");
            return;
        }

        NoAds.OnNoAds += () => EverydayLogger.Info("Bought No Ads");

        // They are the same
        // IAPManager.PurchaseProduct(NoAds.NoAdsProductKey); 
        NoAds.Purchase();
    }

    public void IAP_RestorePurchase()
    {
        IAPManager.RestorePurchase();
    }

    public void Events_TryStage(int stageNum)
    {
        EventTracker.TryStage(stageNum);
    }

    public void Events_ClearStage(int stageNum)
    {
        EventTracker.ClearStage(stageNum);
    }

    public void Show_MediationDebugger()
    {
        MaxSdk.ShowMediationDebugger();
    }

    public void Show_Review()
    {
        Review.RequestReview();
    }

    public void OpenAppSettings()
    {
        Privacy.OpenAppSettings();
    }
}
