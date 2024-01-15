using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using AmazonAds;

namespace MondayOFF
{
    public static partial class AdsManager
    {
        private static bool _isInitializeRequested = false;
        private static AdType _activeAdTypes = AdType.All;

        private static Interstitial _interstitial = default;
        private static Rewarded _rewarded = default;
        private static Banner _banner = default;
        private static PlayOn _playOn = default;
        private static Adverty _adverty = default;

#if UNITY_EDITOR
        private static CancellationTokenSource _source = default;
#endif

        internal static void PrepareManager()
        {
            EverydayLogger.Info($"Preparing Ads Manager");

            if (_isInitializeRequested)
            {
                InitializeAdTypes();
            }

#if UNITY_EDITOR
            Application.quitting -= OnEditorStop;
            Application.quitting += OnEditorStop;
#endif
        }

        internal static bool IsAdTypeActive(in AdType adType)
        {
            return (_activeAdTypes & adType) == adType;
        }

#if UNITY_EDITOR
        private static void OnEditorStop()
        {
            EverydayLogger.Info("Stop Playmode Ads Manager");
            _source?.Cancel();
            _source?.Dispose();
            _source = null;

            _interstitial?.Dispose();
            _interstitial = null;
            _rewarded?.Dispose();
            _rewarded = null;
            _banner?.Dispose();
            _banner = null;
            _playOn?.Dispose();
            _playOn = null;

            _isInitializeRequested = false;

            var methodInfo = typeof(MaxSdk).GetMethod("RemoveReadyAdUnit", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            methodInfo?.Invoke(null, new[] { EverydaySettings.AdSettings.bannerAdUnitId });
            methodInfo?.Invoke(null, new[] { EverydaySettings.AdSettings.interstitialAdUnitId });
            methodInfo?.Invoke(null, new[] { EverydaySettings.AdSettings.rewardedAdUnitId });
        }
#endif

        private static async void InitializeAdTypes()
        {
            if (EverydaySettings.AdSettings == null)
            {
                EverydayLogger.Info($"Ads Manager is not prepared. It will continue initialization after settings are loaded.");
                return;
            }

            if (EverydaySettings.AdSettings.HasAPSKey())
            {
                EverydayLogger.Info($"Initializing APS");
                Amazon.Initialize(EverydaySettings.AdSettings.apsAppId);
                Amazon.SetAdNetworkInfo(new AdNetworkInfo(DTBAdNetwork.MAX));
                Amazon.UseGeoLocation(true);
                // Amazon.SetConsentStatus(AdsManager.HAS_USER_CONSENT ? Amazon.ConsentStatus.EXPLICIT_YES : Amazon.ConsentStatus.EXPLICIT_NO);
#if UNITY_IOS
                Amazon.SetAPSPublisherExtendedIdFeatureEnabled(Privacy.HAS_ATT_CONSENT);
#endif
                // Amazon.EnableTesting(true);
                // Amazon.EnableLogging(true);
            }

            // Is this right place??
            if (EverydaySettings.AdSettings.IsNoAds())
            {
                _activeAdTypes = AdType.Rewarded;
            }

            System.TimeSpan delay = System.TimeSpan.FromSeconds(EverydaySettings.AdSettings.adInitializationDelay);
            System.Func<bool>[] adLoadingFuncs = new System.Func<bool>[3]{
                CreateBanner,
                CreateInterstitial,
                CreateRewarded
            };
            ushort loadingOrder = (ushort)EverydaySettings.AdSettings.adInitializationOrder;
            System.Func<bool> createAd = adLoadingFuncs[(loadingOrder >> 7) & 0b11];

#if UNITY_EDITOR
            _source = new CancellationTokenSource();
            CancellationToken token = _source.Token;
            try
            {
                if (createAd.Invoke())
                {
                    EverydayLogger.Info($"First Ad type: {createAd.Method.Name}");
                    await Task.Delay(delay, token);
                    token.ThrowIfCancellationRequested();
                }

                createAd = adLoadingFuncs[(loadingOrder >> 4) & 0b11];
                if (createAd.Invoke())
                {
                    EverydayLogger.Info($"Second Ad Type: {createAd.Method.Name}");
                    await Task.Delay(delay, token);
                    token.ThrowIfCancellationRequested();
                }

                createAd = adLoadingFuncs[(loadingOrder >> 1) & 0b11];
                if (createAd.Invoke())
                {
                    EverydayLogger.Info($"Third Ad Type: {createAd.Method.Name}");
                    await Task.Delay(delay, token);
                    token.ThrowIfCancellationRequested();
                }

                if (CreatePlayOn())
                {
                    EverydayLogger.Info($"Creating PlayOn");
                    await Task.Delay(delay, token);
                    token.ThrowIfCancellationRequested();
                }

                if (EverydaySettings.AdSettings.initializeAdvertyOnAwake)
                {
                    CreateAdverty(Camera.main);
                    EverydayLogger.Info($"Creating Adverty");
                }
            }
            catch (System.Exception)
            {
                EverydayLogger.Info("Application stopped, stop initializing ads");
            }
            finally
            {
                _source.Dispose();
                _source = null;
            }
#else
            if (createAd.Invoke()) {
                EverydayLogger.Info($"First Ad type: {createAd.Method.Name}");
                await Task.Delay(delay);
            }

            createAd = adLoadingFuncs[(loadingOrder >> 4) & 0b11];

            if (createAd.Invoke()) {
                EverydayLogger.Info($"Second Ad Type: {createAd.Method.Name}");
                await Task.Delay(delay);
            }

            createAd = adLoadingFuncs[(loadingOrder >> 1) & 0b11];

            if (createAd.Invoke()) {
                EverydayLogger.Info($"Third Ad Type: {createAd.Method.Name}");
                await Task.Delay(delay);
            }

            if (CreatePlayOn()) {
                EverydayLogger.Info($"Creating PlayOn");
                await Task.Delay(delay);
            }

            if (EverydaySettings.AdSettings.initializeAdvertyOnAwake) {
                CreateAdverty(Camera.main);
                EverydayLogger.Info($"Creating Adverty");
            }
#endif
            OnInitialized?.Invoke();
            OnInitialized = null;
        }

        private static bool CreateBanner()
        {
            if (_banner != null)
            {
                EverydayLogger.Warn("Banner is already initialized!");
                return false;
            }

            if (!_activeAdTypes.HasFlag(AdType.Banner) || !EverydaySettings.AdSettings.hasBanner)
            {
                EverydayLogger.Info("Skipping banner initialization");
                return false;
            }

            _banner = new Banner();
            return true;
        }

        private static bool CreateInterstitial()
        {
            if (_interstitial != null)
            {
                EverydayLogger.Warn("Interstitial is already initialized!!");
                return false;
            }

            if (!_activeAdTypes.HasFlag(AdType.Interstitial) || !EverydaySettings.AdSettings.hasInterstitial)
            {
                EverydayLogger.Info("Skipping intersitial initialization");
                return false;
            }

            _interstitial = new Interstitial();
            return true;
        }

        private static bool CreateRewarded()
        {
            if (_rewarded != null)
            {
                EverydayLogger.Warn("Rewarded is already initialized!");
                return false;
            }

            if (!_activeAdTypes.HasFlag(AdType.Rewarded) || !EverydaySettings.AdSettings.hasRewarded)
            {
                EverydayLogger.Info("Skipping rewarded initialization");
                return false;
            }

            _rewarded = new Rewarded();
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += AdsManager.InternalLoadedCallback;
            return true;
        }

        private static bool CreatePlayOn()
        {
            if (EverydaySettings.AdSettings.IsNoAds())
            {
                EverydayLogger.Info("No Ads, skipping PlayOn initialization");
                return false;
            }

            if (_playOn != null)
            {
                EverydayLogger.Warn("PlayOn is already initialized!");
                return false;
            }

            if (string.IsNullOrEmpty(EverydaySettings.AdSettings.playOnAPIKey))
            {
                EverydayLogger.Info("PlayOn Api Key is empty! Skipping PlayOn initialization");
                return false;
            }

            _playOn = new PlayOn();
            _playOn.Initialize();
            return true;
        }

        private static void CreateAdverty(in Camera mainCamera)
        {
            if (string.IsNullOrEmpty(EverydaySettings.AdSettings.advertyApiKey))
            {
                EverydayLogger.Info("Adverty Api Key is empty! Skipping Adverty initialization");
                return;
            }
            _adverty = new Adverty(mainCamera);
        }

        private static void InternalLoadedCallback(string _, MaxSdk.AdInfo __)
        {
            OnRewardedAdLoaded?.Invoke();
        }
    }
}