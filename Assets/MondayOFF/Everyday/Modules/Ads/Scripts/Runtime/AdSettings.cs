using UnityEngine;

namespace MondayOFF {
    [System.Serializable]
    internal class AdSettings {
        [Header("Initialize Ads Manager on load")]
        [Tooltip("If you want to manually initialize Ads Manager (ex/ to check No Ads status before initialization), do NOT forget to initialize Ads Manager manually.")]
        [SerializeField] internal bool initializeOnLoad = true;

        [Header("Show Banner upon initialization")]
        [SerializeField] internal bool showBannerOnLoad = true;

        [Header("Ad initialization order")]
        [SerializeField] internal AdInitializationOrder adInitializationOrder = AdInitializationOrder.Banner_Inter_Reward;
        [SerializeField][Range(0f, 10f)] internal float delay = 2f;
        [Header("Interstitial Interval")]
        [Tooltip("Minimum delay between interstitial ads")]
        [SerializeField][Range(5f, 120f)] internal float interstitialInterval = 30f;
        [Tooltip("Should interstitial timer reset after rewarded ad?")]
        [SerializeField] internal bool resetTimerOnRewarded = true;
        [Header("Banner Position")]
        [SerializeField] internal MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;

#pragma warning disable CS0414
        [Header("====== iOS Ad Unit IDs ======")]
        // [SerializeField][LabelOverride("Interstitial (IS)")] bool iOS_has_IS = false;
        [SerializeField][LabelOverride("Interstitial")] string iOS_IS_AdUnitID = "";
        // [SerializeField][LabelOverride("Rewarded (RV)")] bool iOS_has_RV = false;
        [SerializeField][LabelOverride("Rewarded")] string iOS_RV_AdUnitID = "";
        // [SerializeField][LabelOverride("Banner (BN)")] bool iOS_has_BN = false;
        [SerializeField][LabelOverride("Banner")] string iOS_BN_AdUnitID = "";

        [Header("====== Android Ad Unit IDs ======")]
        // [SerializeField][LabelOverride("Interstitial (IS)")] bool AOS_has_IS = false;
        [SerializeField][LabelOverride("Interstitial")] string AOS_IS_AdUnitID = "";
        // [SerializeField][LabelOverride("Rewarded (RV)")] bool AOS_has_RV = false;
        [SerializeField][LabelOverride("Rewarded")] string AOS_RV_AdUnitID = "";
        // [SerializeField][LabelOverride("Banner (BN)")] bool AOS_has_BN = false;
        [SerializeField][LabelOverride("Banner")] string AOS_BN_AdUnitID = "";

        [Space(20)]
        [Header("====== PlayOn ======")]
        // [SerializeField][LabelOverride("PlayOn")] internal bool hasPlayOn = false;
        [SerializeField][LabelOverride("Android Api Key")] private string playOnAPIKey_Android = "";
        [SerializeField][LabelOverride("iOS Api Key")] private string playOnAPIKey_iOS = "";
        [SerializeField][LabelOverride("Apple AppStore ID")] internal string storeID = "";
        [SerializeField][Range(-1, 5)] internal int playPlayonEveryNthInterstitial = 2;
        [Header("Positioning")]
        [SerializeField] internal PlayOnPosition playOnPosition = default;
        [Space(20)]
        [Header("====== Adverty ======")]
        [SerializeField] internal bool initializeAdvertyOnAwake = true;
        [SerializeField][LabelOverride("Android Api Key")] private string advertyApiKey_Android = "";
        [SerializeField][LabelOverride("iOS Api Key")] private string advertyApiKey_iOS = "";

#pragma warning restore CS0414

#if UNITY_IOS
        internal string interstitialAdUnitId => iOS_IS_AdUnitID;
        internal string rewardedAdUnitId => iOS_RV_AdUnitID;
        internal string bannerAdUnitId => iOS_BN_AdUnitID;

        internal bool hasInterstitial => !string.IsNullOrEmpty(iOS_IS_AdUnitID);
        internal bool hasRewarded => !string.IsNullOrEmpty(iOS_RV_AdUnitID);
        internal bool hasBanner => !string.IsNullOrEmpty(iOS_BN_AdUnitID);

        internal string playOnAPIKey => playOnAPIKey_iOS;

        internal string advertyApiKey => advertyApiKey_iOS;
#else
        internal string interstitialAdUnitId => AOS_IS_AdUnitID;
        internal string rewardedAdUnitId => AOS_RV_AdUnitID;
        internal string bannerAdUnitId => AOS_BN_AdUnitID;

        
        internal bool hasInterstitial => !string.IsNullOrEmpty(AOS_IS_AdUnitID);
        internal bool hasRewarded => !string.IsNullOrEmpty(AOS_RV_AdUnitID);
        internal bool hasBanner => !string.IsNullOrEmpty(AOS_BN_AdUnitID);

        internal string playOnAPIKey => playOnAPIKey_Android;
        internal string advertyApiKey => advertyApiKey_Android;
#endif

        internal System.Func<bool> IsNoAds = () => false;

        internal bool HasInterstitialAdUnitID() {
            return hasInterstitial && !string.IsNullOrEmpty(interstitialAdUnitId);
        }

        internal bool HadRewardedAdUnitID() {
            return hasRewarded && !string.IsNullOrEmpty(rewardedAdUnitId);
        }

        internal bool HasBannerAdUnitID() {
            return hasBanner && !string.IsNullOrEmpty(bannerAdUnitId);
        }
    }

    [System.Serializable]
    internal class PlayOnPosition {
        [SerializeField] internal bool useScreenPositioning = false;
        [Tooltip("Anchor position of logo ad")]
        [SerializeField][DrawIf("useScreenPositioning")] internal PlayOnSDK.Position playOnLogoAnchor = PlayOnSDK.Position.TopLeft;
        [Tooltip("Offset from anchor")]
        [SerializeField][DrawIf("useScreenPositioning")] internal Vector2Int playOnLogoOffset = new Vector2Int(0, 150);
        [Tooltip("Logo ad size")]
        [SerializeField][DrawIf("useScreenPositioning")] internal int playOnLogoSize = 64;
    }
}