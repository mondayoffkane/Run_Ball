using UnityEngine;

namespace MondayOFF
{
    internal sealed class Interstitial : FullscreenAdType
    {
        internal static event System.Action OnBeforeShow = default;
        internal static event System.Action OnAfterShow = default;
        private float _interval => EverydaySettings.AdSettings.interstitialInterval;
        private string _adUnitID => EverydaySettings.AdSettings.interstitialAdUnitId;
        private float _lastInterstitialTimestamp = 0f;

        public override void Dispose()
        {
            EverydayLogger.Info("Disposing Interstitial Ad");
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= ResetTimer;

            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= ResetTimer;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= ResetTimerForRewarded;
        }

        internal override bool IsReady()
        {
            return MaxSdk.IsInterstitialReady(_adUnitID);
        }

        internal override bool Show()
        {
            if (IsReady())
            {
                if (Time.realtimeSinceStartup >= (_lastInterstitialTimestamp + _interval))
                {
                    OnBeforeShow?.Invoke();
                    EverydayLogger.Info("Show Interstitial");
                    MaxSdk.ShowInterstitial(_adUnitID);
                    return true;
                }
                else
                {
                    EverydayLogger.Info($"You are trying to show interstitial ad too frequently.\nPlease wait {_lastInterstitialTimestamp + _interval - Time.realtimeSinceStartup} seconds before showing another Interstitial");
                    return false;
                }
            }
            EverydayLogger.Info("Interstitial ad is not loaded yet");

            LoadInterstitialAd();
            return false;
        }

        internal float GetTimeUntilNextInterstitial()
        {
            return _lastInterstitialTimestamp + _interval - Time.realtimeSinceStartup;
        }

        internal Interstitial()
        {
            EverydayLogger.Info("Createing Interstitial Ad");

            // Attach callbacks
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += ResetTimer;

            if (EverydaySettings.AdSettings.resetTimerOnRewarded)
            {
                MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += ResetTimer;
                // Temporal fix for rewarded ad not resetting interstitial timer
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += ResetTimerForRewarded;
            }

            if (EverydaySettings.AdSettings.HasAPSKey(AdType.Interstitial))
            {
                LoadAPSInterstitial();
            }
            else
            {
                LoadInterstitialAd();
            }
        }

        private void LoadAPSInterstitial()
        {
            EverydayLogger.Info("Loading APS Interstitial");
            var interstitialVideoAd = new AmazonAds.APSVideoAdRequest(320, 480, EverydaySettings.AdSettings.apsInterstitialSlotId);
            interstitialVideoAd.onSuccess += (adResponse) =>
            {
                MaxSdk.SetInterstitialLocalExtraParameter(EverydaySettings.AdSettings.interstitialAdUnitId, "amazon_ad_response", adResponse.GetResponse());
                LoadInterstitialAd();
            };
            interstitialVideoAd.onFailedWithError += (adError) =>
            {
                MaxSdk.SetInterstitialLocalExtraParameter(EverydaySettings.AdSettings.interstitialAdUnitId, "amazon_ad_error", adError.GetAdError());
                LoadInterstitialAd();
            };

            interstitialVideoAd.LoadAd();
        }

        private void LoadInterstitialAd()
        {
            if (AdsManager.IsAdTypeActive(AdType.Interstitial))
            {
                if (!IsReady())
                {
                    MaxSdk.LoadInterstitial(_adUnitID);
                }
            }
        }

        private async void TryLoadingAfterDelay(System.TimeSpan delay)
        {
            await System.Threading.Tasks.Task.Delay(delay);
            LoadInterstitialAd();
        }

        private void OnAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
            _retryAttempt = 0;
            EverydayLogger.Debug("Interstitial ad loaded");
            EverydayLogger.Debug(adInfo.ToString());


        }

        private void OnAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load. We recommend retrying with exponentially higher delays.
            _retryAttempt = Mathf.Min(_retryAttempt + 1, MaxRetryCount);
            int retryDelay = _retryAttempt * RetryInterval;

            TryLoadingAfterDelay(System.TimeSpan.FromSeconds(retryDelay));
        }

        private void OnAdDisplayFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            OnAfterShow?.Invoke();
            // Interstitial ad failed to display. We recommend loading the next ad
            LoadInterstitialAd();
        }

        private void OnAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            OnAfterShow?.Invoke();
            // Interstitial ad is hidden. Pre-load the next ad
            LoadInterstitialAd();
        }

        private void ResetTimer(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _lastInterstitialTimestamp = Time.realtimeSinceStartup;
        }

        private void ResetTimerForRewarded(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            _lastInterstitialTimestamp = Time.realtimeSinceStartup;
        }
    }
}