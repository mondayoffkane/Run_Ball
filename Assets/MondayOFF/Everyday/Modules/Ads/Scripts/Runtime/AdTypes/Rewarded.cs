using UnityEngine;

namespace MondayOFF {
    internal sealed class Rewarded : FullscreenAdType {
        private string _adUnitID => _settings.rewardedAdUnitId;
        private System.Action _onRewarded = default;

        public override void Dispose() {
            Debug.Log("[EVERYDAY] Disposing Rewarded Ad");

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEvent;
        }

        internal void SetReward(System.Action reward) {
            _onRewarded = reward;
        }

        internal override bool IsReady() {
            return MaxSdk.IsRewardedAdReady(_adUnitID);
        }

        internal override bool Show() {
            if (IsReady()) {
                CallOnBeforeShow();
                Debug.Log("[EVERYDAY] Show Rewarded");
                MaxSdk.ShowRewardedAd(_adUnitID);
                return true;
            }
            Debug.Log("[EVERYDAY] Rewarded ad is not loaded yet");

            LoadRewardedAd();
            return false;
        }

        internal Rewarded(in AdSettings settings) : base(settings) {
            Debug.Log("[EVERYDAY] Createing Rewarded Ad");

            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnAdDisplayFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;

            if (_settings.HasAPSKey(AdType.Rewarded)) {
                LoadAPSRewarded();
            } else {
                LoadRewardedAd();
            }
        }

        private void LoadAPSRewarded() {
            Debug.Log("[EVERYDAY] Loading APS Rewarded Ad");
            var rewardedVideoAd = new AmazonAds.APSVideoAdRequest(320, 480, _settings.apsRewardedSlotId);
            rewardedVideoAd.onSuccess += (adResponse) => {
                MaxSdk.SetRewardedAdLocalExtraParameter(_settings.rewardedAdUnitId, "amazon_ad_response", adResponse.GetResponse());
                LoadRewardedAd();
            };
            rewardedVideoAd.onFailedWithError += (adError) => {
                MaxSdk.SetRewardedAdLocalExtraParameter(_settings.rewardedAdUnitId, "amazon_ad_error", adError.GetAdError());
                LoadRewardedAd();
            };

            rewardedVideoAd.LoadAd();
        }

        private void LoadRewardedAd() {
            if (!IsReady()) {
                MaxSdk.LoadRewardedAd(_adUnitID);
            }
        }

        private async void TryLoadingAfterDelay(System.TimeSpan delay) {
            await System.Threading.Tasks.Task.Delay(delay);
            LoadRewardedAd();
        }

        private void OnAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            // Reset retry attempt
            _retryAttempt = 0;
        }

        private void OnAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
            // Rewarded ad failed to load. We recommend retrying with exponentially higher delays.
            _retryAttempt = Mathf.Min(_retryAttempt + 1, MaxRetryCount);
            int retryDelay = _retryAttempt * RetryInterval;
            TryLoadingAfterDelay(System.TimeSpan.FromSeconds(retryDelay));
        }

        private void OnAdDisplayFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) {
            CallOnAfterShow();
            // Rewarded ad failed to display. We recommend loading the next ad
            LoadRewardedAd();
        }

        private void OnAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {

        }

        private void OnAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            CallOnAfterShow();
            // Rewarded ad is hidden. Pre-load the next ad
            LoadRewardedAd();
        }

        private void OnAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo) {
            // Rewarded ad was displayed and user should receive the reward
            _onRewarded?.Invoke();
            _onRewarded = null;
        }
    }
}