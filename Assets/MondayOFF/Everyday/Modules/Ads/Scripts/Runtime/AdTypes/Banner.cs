using UnityEngine;

namespace MondayOFF
{
    internal sealed class Banner : AdTypeBase
    {
        private string _adUnitID => EverydaySettings.AdSettings.bannerAdUnitId;
        private bool _isBannerDisplayed = false;
        private bool _isBannerCreated = false;

        public override void Dispose()
        {
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent -= OnAdLoadFailed;
            EverydayLogger.Info("Disposing Banner Ad");
            Hide();
            MaxSdk.DestroyBanner(_adUnitID);
        }

        internal override bool IsReady()
        {
            // There is no preload for banner
            return true;
        }

        internal override bool Show()
        {
            EverydayLogger.Info("Show Banner");
            if (!_isBannerCreated)
            {
                EverydayLogger.Info("Banner is not created yet");
                CreateMaxBannerAd();
            }
            //show banner
            MaxSdk.ShowBanner(_adUnitID);
            _isBannerDisplayed = true;
            return true;
        }

        internal bool IsDisplayed()
        {
            return _isBannerDisplayed;
        }

        internal void Hide()
        {
            EverydayLogger.Info("Hide Banner");
            MaxSdk.HideBanner(_adUnitID);
            _isBannerDisplayed = false;
        }

        internal Banner()
        {
            EverydayLogger.Info("Createing Banner Ad");

            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailed;

            if (EverydaySettings.AdSettings.HasAPSKey(AdType.Banner))
            {
                LoadAPSBanner();
            }
            else
            {
                CreateMaxBannerAd();
            }
        }

        private void LoadAPSBanner()
        {
            EverydayLogger.Info("Loading APS Banner");
            var apsBanner = new AmazonAds.APSBannerAdRequest(320, 50, EverydaySettings.AdSettings.apsBannerSlotId);
            apsBanner.onSuccess += (adResponse) =>
            {
                MaxSdk.SetBannerLocalExtraParameter(_adUnitID, "amazon_ad_response", adResponse.GetResponse());
                CreateMaxBannerAd();
            };
            apsBanner.onFailedWithError += (adError) =>
            {
                MaxSdk.SetBannerLocalExtraParameter(_adUnitID, "amazon_ad_error", adError.GetAdError());
                CreateMaxBannerAd();
            };

            apsBanner.LoadAd();
        }

        private void CreateMaxBannerAd()
        {
            if (AdsManager.IsAdTypeActive(AdType.Banner))
            {
                MaxSdk.CreateBanner(_adUnitID, EverydaySettings.AdSettings.bannerPosition);
                _isBannerCreated = true;
                if (EverydaySettings.AdSettings.showBannerOnLoad)
                {
                    this.Show();
                }
            }
        }

        private void OnAdLoadFailed(string adUnitId, MaxSdk.ErrorInfo errorInfo)
        {
            EverydayLogger.Info("Banner ad failed to load with error code: " + errorInfo.Code + ", and message: " + errorInfo.Message);
            CreateMaxBannerAd();
        }
    }
}
