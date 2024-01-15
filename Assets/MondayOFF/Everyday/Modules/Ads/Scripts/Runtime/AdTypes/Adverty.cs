using System;
using UnityEngine;
using Adverty;

namespace MondayOFF
{
    internal class Adverty : IDisposable
    {
        internal Adverty(in Camera mainCamera)
        {
            EverydayLogger.Info("Initializing Adverty");
            UserData userData = new UserData(Privacy.TCString, Privacy.IS_GDPR_APPLICABLE, Privacy.CCPA_STRING);
            AdvertySDK.Init(EverydaySettings.AdSettings.advertyApiKey, AdvertySettings.Mode.Mobile, !Privacy.HAS_ATT_CONSENT, userData);
            AdvertySettings.SetMainCamera(mainCamera);
        }

        public void Dispose()
        {
            EverydayLogger.Info("Terminating Adverty");
            AdvertySDK.Terminate();
        }
    }
}