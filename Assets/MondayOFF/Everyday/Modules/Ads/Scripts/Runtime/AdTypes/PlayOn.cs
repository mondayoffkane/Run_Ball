using UnityEngine;

namespace MondayOFF {
    internal sealed class PlayOn : AdTypeBase {
        const string AD_PLATFORM = "odeeo";
        const string AD_CURRENCY = "USD";
        private AdUnit _adUnit = default;
        private int _shownCount = 0;

        public override void Dispose() {
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= AutoShowPlayOn;
            _adUnit.Dispose();
            _adUnit = null;
        }

        internal override bool IsReady() {
            if (!PlayOnSDK.IsInitialized()) {
                Debug.Log("[Everyday] PlayOn is not initialized");
                return false;
            }

            return true;
        }

        internal override bool Show() {
            if (_settings.playPlayonEveryNthInterstitial > 0) {
                Debug.LogWarning($"[Everyday] PlayOn is set to show after {_settings.playPlayonEveryNthInterstitial}. Do you really want to manually show PlayOn?");
                return false;
            }

            if (!_adUnit.IsAdAvailable()) {
                Debug.Log("[Everyday] PlayOn Ad Unit is not available yet");
                return false;
            }
            Debug.Log("[Everyday] Show PlayOn");
            _adUnit.ShowAd();
            return true;
        }

        internal void Hide() {
            Debug.Log("[Everyday] Hide PlayON");
            _adUnit.CloseAd();
        }

        internal void LinkLogoToRectTransform(PlayOnSDK.Position position, RectTransform rectTransform, Canvas canvas) {
            if (_settings.playOnPosition.useScreenPositioning) {
                Debug.Log("[Everyday] Using legacy PlayOn positioning! Do not use PlayOnAnchor");
                return;
            }
            _adUnit.LinkLogoToRectTransform(position, rectTransform, canvas);
        }

        internal PlayOn(in AdSettings settings) : base(settings) { }

        internal void Initialize() {
            _adUnit = new AdUnit(PlayOnSDK.AdUnitType.AudioLogoAd);

            // _adUnit.SetProgressBar(Color.black);
            // _adUnit.SetVisualization(Color.black, Color.red);

            if (_settings.playOnPosition.useScreenPositioning) {
                _adUnit.SetLogo(_settings.playOnPosition.playOnLogoAnchor, _settings.playOnPosition.playOnLogoOffset.x, _settings.playOnPosition.playOnLogoOffset.y, _settings.playOnPosition.playOnLogoSize);
            }

            PlayOnSDK.OnInitializationFinished += OnInitialized;
            PlayOnSDK.OnInitializationFailed += (errorCode, errorMessage) => {
                Debug.LogError($"[Everyday] PlayOn initialization failed with error code {errorCode} and message {errorMessage}");
            };

            if (_settings.playPlayonEveryNthInterstitial > 0) {
                MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += AutoShowPlayOn;
            }

            PlayOnSDK.Initialize(_settings.playOnAPIKey, _settings.storeID);
        }

        private void OnInitialized() {
            PlayOnSDK.OnInitializationFinished -= OnInitialized;

            var mainThreadDispatcherInstance = UnityMainThreadDispatcher.Instance();
            mainThreadDispatcherInstance.gameObject.AddComponent<ApplicationLifecycleTracker>();

            PlayOnSDK.SetGdprConsent(AdsManager.HAS_USER_CONSENT);
            PlayOnSDK.SetDoNotSell(!AdsManager.HAS_USER_CONSENT, AdsManager.US_PRIVACY_STRING);
            PlayOnSDK.SetIsChildDirected(false);

            _adUnit.AdCallbacks.OnImpression += OnAdImpression;
        }

        private void AutoShowPlayOn(string _, MaxSdk.AdInfo __) {
            if (_shownCount++ % _settings.playPlayonEveryNthInterstitial == 0) {
                if (!_adUnit.IsAdAvailable()) {
                    Debug.Log("[Everyday] PlayOn Ad Unit is not available yet");
                    _shownCount--;
                    return;
                }
                _adUnit.ShowAd();
            }
        }

        private void OnAdImpression(AdUnit.ImpressionData impressionData) {
            PlayOnSDK.AdUnitType adType = impressionData.GetAdType();
            double revenue = impressionData.GetRevenue();
            string placementID = impressionData.GetPlacementID();

            // Initialize the SingularAdData object with the relevant data
            SingularAdData singularAdData =
                new SingularAdData(AD_PLATFORM, AD_CURRENCY, revenue / 1000d)
                    .WithPlacementId(placementID)
                    .WithAdType(adType.ToString());

            // Report the data to Singular
            SingularSDK.AdRevenue(singularAdData);
        }

        private class ApplicationLifecycleTracker : MonoBehaviour {
            private void OnApplicationPause(bool pauseStatus) {
                Debug.Log($"[Everyday] PlayOn OnApplicationPause({pauseStatus})");
                PlayOnSDK.onApplicationPause(pauseStatus);
            }
        }
    }
}