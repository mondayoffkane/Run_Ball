using UnityEngine;

namespace MondayOFF {
    internal sealed class PlayOn : AdTypeBase {
        private AdUnit _adUnit = new AdUnit(PlayOnSDK.AdUnitType.AudioLogoAd);
        private int _shownCount = 0;

        public override void Dispose() {
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= AutoShowPlayOn;
            _adUnit.Dispose();
            _adUnit = null;
        }

        internal override bool IsReady() {
            if (!PlayOnSDK.IsInitialized()) {
                Debug.Log("[EVERYDAY] PlayOn is not initialized");
                return false;
            }

            return true;
        }

        internal override bool Show() {
            if (_settings.playPlayonEveryNthInterstitial > 0) {
                Debug.LogWarning($"[EVERYDAY] PlayOn is set to show after {_settings.playPlayonEveryNthInterstitial}. Do you really want to manually show PlayOn?");
                return false;
            }

            if (!_adUnit.IsAdAvailable()) {
                Debug.Log("[EVERYDAY] PlayOn Ad Unit is not available yet");
                return false;
            }
            Debug.Log("[EVERYDAY] Show PlayOn");
            _adUnit.ShowAd();
            return true;
        }

        internal void Hide() {
            Debug.Log("[EVERYDAY] Hide PlayON");
            _adUnit.CloseAd();
        }

        internal void LinkLogoToRectTransform(PlayOnSDK.Position position, RectTransform rectTransform, Canvas canvas) {
            if (_settings.playOnPosition.useScreenPositioning) {
                Debug.Log("[EVERYDAY] Using legacy PlayOn positioning! Do not use PlayOnAnchor");
                return;
            }
            _adUnit.LinkLogoToRectTransform(position, rectTransform, canvas);
        }

        // internal void LinkLogoToPrefab(in AdUnitAnchor adUnitAnchor) {
        //     _adUnit.LinkLogoToPrefab(adUnitAnchor);
        // }

        internal PlayOn(in AdSettings settings) : base(settings) { }

        internal void Initialize() {
            if (_settings.playOnPosition.useScreenPositioning) {
                _adUnit.SetLogo(_settings.playOnPosition.playOnLogoAnchor, _settings.playOnPosition.playOnLogoOffset.x, _settings.playOnPosition.playOnLogoOffset.y, _settings.playOnPosition.playOnLogoSize);
            }

            PlayOnSDK.OnInitializationFinished += OnInitialized;

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
        }

        private void AutoShowPlayOn(string _, MaxSdk.AdInfo __) {
            if (_shownCount++ % _settings.playPlayonEveryNthInterstitial == 0) {
                if (!_adUnit.IsAdAvailable()) {
                    Debug.Log("[EVERYDAY] PlayOn Ad Unit is not available yet");
                    _shownCount--;
                    return;
                }
                _adUnit.ShowAd();
            }
        }

        private class ApplicationLifecycleTracker : MonoBehaviour {
            private void OnApplicationPause(bool pauseStatus) {
                Debug.Log($"[EVERYDAY] PlayON OnApplicationPause({pauseStatus})");
                PlayOnSDK.onApplicationPause(pauseStatus);
            }
        }
    }
}