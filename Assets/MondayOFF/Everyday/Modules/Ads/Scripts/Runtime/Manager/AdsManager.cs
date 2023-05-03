using UnityEngine;
using Adverty;

namespace MondayOFF {
    public static partial class AdsManager {
        public static event System.Action OnInitialized = default;
        public static event System.Action OnBeforeInterstitial {
            add { Interstitial.OnBeforeShow += value; }
            remove { Interstitial.OnBeforeShow -= value; }
        }
        public static event System.Action OnAfterInterstitial {
            add { Interstitial.OnAfterShow += value; }
            remove { Interstitial.OnAfterShow -= value; }
        }
        public static event System.Action OnBeforeRewarded {
            add { Rewarded.OnBeforeShow += value; }
            remove { Rewarded.OnBeforeShow -= value; }
        }
        public static event System.Action OnAfterRewarded {
            add { Rewarded.OnAfterShow += value; }
            remove { Rewarded.OnAfterShow -= value; }
        }

        public static event System.Action OnRewardedAdLoaded = default;
        public static string US_PRIVACY_STRING = "";
        public static bool HAS_USER_CONSENT = false;

        public static void Initialize(in AdType activeAdTypes = AdType.All) {
            if (_isInitializeRequested) {
                Debug.LogWarning("[EVERYDAY] AdsManager is already initialized!");
                return;
            }
            _isInitializeRequested = true;

            Debug.Log("[EVERYDAY] Initializing AdsManager..");
            _activeAdTypes = activeAdTypes;

            InitializeAdTypes();
        }

        public static void ShowBanner() {
            if (_banner == null) {
                if (_settings != null) {
                    _settings.showBannerOnLoad = true;
                }
                Debug.Log("[EVERYDAY] ShowBanner is called but banner ad is not created or has been destroyed");
                return;
            }

            _banner.Show();
        }

        public static void HideBanner() {
            if (_banner == null) {
                if (_settings != null) {
                    _settings.showBannerOnLoad = false;
                }
                Debug.Log("[EVERYDAY] HideBanner is called but banner ad is not created or has been destroyed");
                return;
            }

            _banner.Hide();
        }

        public static bool ShowInterstitial() {
            if (_interstitial == null) {
                Debug.Log("[EVERYDAY] ShowInterstitial is called but interstitial ad is not created or has been destroyed");
                return false;
            }

            return _interstitial.Show();
        }

        public static bool IsRewardedReady() {
            if (_rewarded == null) {
                Debug.Log("[EVERYDAY] IsRewardedReady is called but rewarded ad is not created or has been destroyed");
                return false;
            }

            return _rewarded.IsReady();
        }

        public static bool ShowRewarded(in System.Action rewardCallback) {
            if (_rewarded == null) {
                Debug.Log("[EVERYDAY] ShowRewarded is called but rewarded ad is not created or has been destroyed!");
                return false;
            }

            if (rewardCallback != null) {
                _rewarded.SetReward(rewardCallback);
                return _rewarded.Show();
            } else {
                Debug.Log("[EVERYDAY] Rewarding callback is not set properly!");
                return false;
            }
        }

        public static bool ShowPlayOn() {
            if (_playOn == null) {
                Debug.Log("[EVERYDAY] ShowPlayOn is called but PlayOn is not created or has been destroyed");
                return false;
            }

            return _playOn.IsReady() && _playOn.Show();
        }

        public static void HidePlayOn() {
            if (_playOn == null) {
                Debug.Log("[EVERYDAY] HidePlayOn is called but PlayOn is not created or has been destroyed");
                return;
            }

            if (_playOn.IsReady()) {
                _playOn.Hide();
            }
        }

        public static bool IsPlayOnReady() {
            if (_playOn == null) {
                Debug.Log("[EVERYDAY] LinkLogoToAnchor is called but PlayOn is not created or has been destroyed");
                return false;
            }

            if (!_playOn.IsReady()) {
                return false;
            }

            return true;
        }

        public static bool LinkLogoToRectTransform(in PlayOnSDK.Position position, in RectTransform rectTransform, in Canvas canvas) {
            if (_playOn == null) {
                return false;
            }

            _playOn.LinkLogoToRectTransform(position, rectTransform, canvas);
            return true;
        }

        // public static void LinkLogoToPrefab(in AdUnitAnchor adUnitAnchor) {
        //     if (CheckInitialization()) { return; }

        //     if (_playOn == null) {
        //         Debug.Log("[EVERYDAY] LinkLogoToAnchor is called but PlayOn is not created or has been destroyed");
        //         return;
        //     }

        //     if (_playOn.IsReady()) {
        //         _playOn.LinkLogoToPrefab(adUnitAnchor);
        //     }
        // }

        public static bool InitializeAdverty(in Camera mainCamera) {
            if (_settings.initializeAdvertyOnAwake) {
                Debug.LogWarning("[EVERYDAY] Adverty was initialized upon Awake! If you want to change camera, use \'ChangeAdvertyCamera()\'.");
                return false;
            }

            if (_adverty != null) {
                Debug.LogWarning("[EVERYDAY] Adverty was already initialized!");
                return false;
            }

            CreateAdverty(mainCamera);
            return true;
        }

        public static void ChangeAdvertyCamera(in Camera mainCamera) {
            if (mainCamera == null) {
                Debug.LogError("[EVERYDAY] Cannot set null as a Adverty main camera!");
                return;
            }
            Debug.Log($"[EVERYDAY] {mainCamera.name} is set to Adverty main camera");
            AdvertySettings.SetMainCamera(mainCamera);
        }

        public static void DisableAdType(in AdType adType) {
            if (adType.HasFlag(AdType.Banner)) {
                DisableBanner();
            }
            if (adType.HasFlag(AdType.Rewarded)) {
                DisableRewarded();
            }
            if (adType.HasFlag(AdType.Interstitial)) {
                DisableInterstitial();
            }
        }

        public static void DisableBanner() {
            if (_banner != null) {
                _banner.Dispose();
                _banner = null;
                _activeAdTypes &= ~AdType.Banner;
            }
        }

        public static void DisableInterstitial() {
            if (_interstitial != null) {
                _interstitial.Dispose();
                _interstitial = null;
                _activeAdTypes &= ~AdType.Interstitial;
            }
        }

        public static void DisableRewarded() {
            if (_rewarded != null) {
                _rewarded.Dispose();
                _rewarded = null;
                _activeAdTypes &= ~AdType.Rewarded;
                MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= AdsManager.InternalLoadedCallback;
            }
        }
    }
}