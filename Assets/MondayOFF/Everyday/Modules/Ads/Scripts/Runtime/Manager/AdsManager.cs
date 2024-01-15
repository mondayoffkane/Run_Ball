using UnityEngine;
using Adverty;

namespace MondayOFF
{
    public static partial class AdsManager
    {
        public static event System.Action OnInitialized = default;
        public static event System.Action OnBeforeInterstitial
        {
            add { Interstitial.OnBeforeShow += value; }
            remove { Interstitial.OnBeforeShow -= value; }
        }
        public static event System.Action OnAfterInterstitial
        {
            add { Interstitial.OnAfterShow += value; }
            remove { Interstitial.OnAfterShow -= value; }
        }
        public static event System.Action OnBeforeRewarded
        {
            add { Rewarded.OnBeforeShow += value; }
            remove { Rewarded.OnBeforeShow -= value; }
        }
        public static event System.Action OnAfterRewarded
        {
            add { Rewarded.OnAfterShow += value; }
            remove { Rewarded.OnAfterShow -= value; }
        }

        public static event System.Action OnRewardedAdLoaded = default;
        public static string US_PRIVACY_STRING = "";
        public static bool HAS_USER_CONSENT = false;

        public static void Initialize(in AdType activeAdTypes = AdType.All)
        {
            if (_isInitializeRequested)
            {
                EverydayLogger.Warn("AdsManager is already initializing or initialized!");
                return;
            }
            _isInitializeRequested = true;

            _activeAdTypes = activeAdTypes;

            if (!MaxSdk.IsInitialized())
            {
                EverydayLogger.Info("AdsManager will be initialized after MaxSdk is initialized..");
                return;
            }

            EverydayLogger.Info("Initializing AdsManager..");
            InitializeAdTypes();
        }

        public static bool IsInterstitialReady()
        {
            if (_interstitial == null)
            {
                EverydayLogger.Info($"IsInterstitialReady is called but interstitial ad is not created or has been destroyed - AD UNIT ID: {EverydaySettings.AdSettings.interstitialAdUnitId}");
                return false;
            }

            return _interstitial.IsReady();
        }

        public static float GetTimeUntilNextInterstitial()
        {
            if (_interstitial == null)
            {
                EverydayLogger.Info($"GetTimeUntilNextInterstitial is called but interstitial ad is not created or has been destroyed - AD UNIT ID: {EverydaySettings.AdSettings.interstitialAdUnitId}");
                return 0f;
            }

            return _interstitial.GetTimeUntilNextInterstitial();
        }

        public static bool ShowInterstitial()
        {
            if (_interstitial == null)
            {
                EverydayLogger.Info($"ShowInterstitial is called but interstitial ad is not created or has been destroyed - AD UNIT ID: {EverydaySettings.AdSettings.interstitialAdUnitId}");
                return false;
            }

            return _interstitial.Show();
        }

        public static bool IsRewardedReady()
        {
            if (_rewarded == null)
            {
                EverydayLogger.Info($"IsRewardedReady is called but rewarded ad is not created or has been destroyed - AD UNIT ID: {EverydaySettings.AdSettings.rewardedAdUnitId}");
                return false;
            }

            return _rewarded.IsReady();
        }

        public static bool ShowRewarded(in System.Action rewardCallback)
        {
            if (_rewarded == null)
            {
                EverydayLogger.Info($"ShowRewarded is called but rewarded ad is not created or has been destroyed! - AD UNIT ID: {EverydaySettings.AdSettings.rewardedAdUnitId}");
                return false;
            }

            if (rewardCallback != null)
            {
                _rewarded.SetReward(rewardCallback);
                return _rewarded.Show();
            }
            else
            {
                EverydayLogger.Warn("Rewarding callback is not set properly!");
                return false;
            }
        }

        public static bool IsBannerDisplayed()
        {
            if (_banner == null)
            {
                EverydayLogger.Info($"IsBannerDisplayed is called but banner ad is not created or has been destroyed - AD UNIT ID: {EverydaySettings.AdSettings.bannerAdUnitId}");
                return false;
            }

            return _banner.IsDisplayed();
        }

        public static bool IsBannerReady()
        {
            if (_banner == null)
            {
                EverydayLogger.Info($"IsBannerReady is called but banner ad is not created or has been destroyed - AD UNIT ID: {EverydaySettings.AdSettings.bannerAdUnitId}");
                return false;
            }

            return _banner.IsReady();
        }

        public static void ShowBanner()
        {
            if (_banner == null)
            {
                if (EverydaySettings.AdSettings != null)
                {
                    EverydaySettings.AdSettings.showBannerOnLoad = true;
                }
                EverydayLogger.Info($"ShowBanner is called but banner ad is not created or has been destroyed - AD UNIT ID: {EverydaySettings.AdSettings.bannerAdUnitId}");
                return;
            }

            _banner.Show();
        }

        public static void HideBanner()
        {
            if (_banner == null)
            {
                if (EverydaySettings.AdSettings != null)
                {
                    EverydaySettings.AdSettings.showBannerOnLoad = false;
                }
                EverydayLogger.Info($"HideBanner is called but banner ad is not created or has been destroyed - AD UNIT ID: {EverydaySettings.AdSettings.bannerAdUnitId}");
                return;
            }

            _banner.Hide();
        }

        public static bool ShowPlayOn()
        {
            if (_playOn == null)
            {
                EverydayLogger.Info($"ShowPlayOn is called but PlayOn is not created or has been destroyed - PlayOn API Key: {EverydaySettings.AdSettings.playOnAPIKey}");
                return false;
            }

            return _playOn.IsReady() && _playOn.Show();
        }

        public static void HidePlayOn()
        {
            if (_playOn == null)
            {
                EverydayLogger.Info($"HidePlayOn is called but PlayOn is not created or has been destroyed - PlayOn API Key: {EverydaySettings.AdSettings.playOnAPIKey}");
                return;
            }

            if (_playOn.IsReady())
            {
                _playOn.Hide();
            }
        }

        public static bool IsPlayOnReady()
        {
            if (_playOn == null)
            {
                EverydayLogger.Info($"IsPlayOnReady is called but PlayOn is not created or has been destroyed - PlayOn API Key: {EverydaySettings.AdSettings.playOnAPIKey}");
                return false;
            }

            if (!_playOn.IsReady())
            {
                return false;
            }

            return true;
        }

        public static bool LinkLogoToRectTransform(in PlayOnSDK.Position position, in RectTransform rectTransform, in Canvas canvas)
        {
            if (_playOn == null)
            {
                return false;
            }

            if (EverydaySettings.AdSettings.playOnPosition.useScreenPositioning)
            {
                EverydayLogger.Warn("PlayOn is using screen positioning. LinkLogoToRectTransform will not work.");
                return false;
            }

            return _playOn.LinkLogoToRectTransform(position, rectTransform, canvas);
        }

        // public static void LinkLogoToPrefab(in AdUnitAnchor adUnitAnchor) {
        //     if (CheckInitialization()) { return; }

        //     if (_playOn == null) {
        //         EverydayLogger.Info("LinkLogoToAnchor is called but PlayOn is not created or has been destroyed");
        //         return;
        //     }

        //     if (_playOn.IsReady()) {
        //         _playOn.LinkLogoToPrefab(adUnitAnchor);
        //     }
        // }

        public static bool InitializeAdverty(in Camera mainCamera)
        {
            if (EverydaySettings.AdSettings.initializeAdvertyOnAwake)
            {
                EverydayLogger.Warn("Adverty was initialized upon Awake! If you want to change camera, use \'ChangeAdvertyCamera()\'.");
                return false;
            }

            if (_adverty != null)
            {
                EverydayLogger.Warn("Adverty was already initialized!");
                return false;
            }

            CreateAdverty(mainCamera);
            return true;
        }

        public static void ChangeAdvertyCamera(in Camera mainCamera)
        {
            if (mainCamera == null)
            {
                EverydayLogger.Error("Cannot set null as a Adverty main camera!");
                return;
            }
            EverydayLogger.Info($"{mainCamera.name} is set to Adverty main camera");
            AdvertySettings.SetMainCamera(mainCamera);
        }

        public static void DisableAdType(in AdType adType)
        {
            if (adType.HasFlag(AdType.Banner))
            {
                DisableBanner();
            }
            if (adType.HasFlag(AdType.Rewarded))
            {
                DisableRewarded();
            }
            if (adType.HasFlag(AdType.Interstitial))
            {
                DisableInterstitial();
            }
        }

        public static void DisableBanner()
        {
            if (_banner != null)
            {
                _banner.Dispose();
                _banner = null;
                _activeAdTypes &= ~AdType.Banner;
            }
        }

        public static void DisableInterstitial()
        {
            if (_interstitial != null)
            {
                _interstitial.Dispose();
                _interstitial = null;
                _activeAdTypes &= ~AdType.Interstitial;
            }
        }

        public static void DisableRewarded()
        {
            if (_rewarded != null)
            {
                _rewarded.Dispose();
                _rewarded = null;
                _activeAdTypes &= ~AdType.Rewarded;
                MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= AdsManager.InternalLoadedCallback;
            }
        }
    }
}