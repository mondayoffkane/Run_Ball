using UnityEngine;

namespace MondayOFF {
    internal sealed class Interstitial : FullscreenAdType {
        private float _interval => _settings.interstitialInterval;
        private string _adUnitID => _settings.interstitialAdUnitId;
        private float _lastInterstitialTimestamp = 0f;

        public override void Dispose() {
            Debug.Log("[EVERYDAY] Disposing Interstitial Ad");
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= ResetTimer;

            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= ResetTimer;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= ResetTimerForRewarded;
        }

        internal override bool IsReady() {
            return MaxSdk.IsInterstitialReady(_adUnitID);
        }

        internal override bool Show() {
            if (IsReady()) {
                if (Time.realtimeSinceStartup >= (_lastInterstitialTimestamp + _interval)) {
                    CallOnBeforeShow();
                    Debug.Log("[EVERYDAY] Show Interstitial");
                    MaxSdk.ShowInterstitial(_adUnitID);
                    return true;
                } else {
                    Debug.Log($"[EVERYDAY] You are trying to show interstitial ad too frequently.\nPlease wait {_lastInterstitialTimestamp + _interval - Time.realtimeSinceStartup} seconds before showing another Interstitial");
                    return false;
                }
            }
            Debug.Log("[EVERYDAY] Interstitial ad is not loaded yet");

            LoadInterstitialAd();
            return false;
        }

        internal Interstitial(in AdSettings settings) : base(settings) {
            Debug.Log("[EVERYDAY] Createing Interstitial Ad");

            // Attach callbacks
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += ResetTimer;

            if (_settings.resetTimerOnRewarded) {
                MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += ResetTimer;
                // Temporal fix for rewarded ad not resetting interstitial timer
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += ResetTimerForRewarded;
            }

            LoadInterstitialAd();
        }

        private void LoadInterstitialAd() {
            if (!IsReady()) {
                MaxSdk.LoadInterstitial(_adUnitID);
            }
        }

        private async void TryLoadingAfterDelay(System.TimeSpan delay) {
            await System.Threading.Tasks.Task.Delay(delay);
            LoadInterstitialAd();
        }

        private void OnAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
            _retryAttempt = 0;
        }

        private void OnAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
            // Interstitial ad failed to load. We recommend retrying with exponentially higher delays.
            _retryAttempt = Mathf.Min(_retryAttempt + 1, MaxRetryCount);
            int retryDelay = _retryAttempt * RetryInterval;
            TryLoadingAfterDelay(System.TimeSpan.FromSeconds(retryDelay));
        }

        private void OnAdDisplayFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) {
            CallOnAfterShow();
            // Interstitial ad failed to display. We recommend loading the next ad
            LoadInterstitialAd();
        }

        private void OnAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            CallOnAfterShow();
            // Interstitial ad is hidden. Pre-load the next ad
            LoadInterstitialAd();
        }

        private void ResetTimer(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            _lastInterstitialTimestamp = Time.realtimeSinceStartup;
        }

        private void ResetTimerForRewarded(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo) {
            _lastInterstitialTimestamp = Time.realtimeSinceStartup;
        }
    }
}