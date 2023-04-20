using System;
using UnityEngine;
using Adverty;

namespace MondayOFF {

    internal class Adverty : IDisposable {
        private readonly AdSettings _settings = default;

        internal Adverty(in AdSettings settings, in Camera mainCamera) {
            Debug.Log("[EVERYDAY] Initializing Adverty");
            _settings = settings;
            UserData userData = new UserData(AgeSegment.Unknown, Gender.Unknown);
            AdvertySDK.Init(_settings.advertyApiKey, AdvertySettings.Mode.Mobile, !AdsManager.HAS_USER_CONSENT, userData);
            AdvertySettings.SetMainCamera(mainCamera);
            //AdvertySettings.SandboxMode = false;
        }

        public void Dispose() {
            Debug.Log("[EVERYDAY] Terminating Adverty");
            AdvertySDK.Terminate();
        }
    }
}