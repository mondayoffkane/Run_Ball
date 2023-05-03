using UnityEngine;

namespace MondayOFF {
    internal sealed class Banner : AdTypeBase {
        private string _adUnitID => _settings.bannerAdUnitId;

        public override void Dispose() {
            Debug.Log("[EVERYDAY] Disposing Banner Ad");
            Hide();
            MaxSdk.DestroyBanner(_adUnitID);
        }

        internal override bool IsReady() {
            // There is no preload for banner
            return true;
        }

        internal override bool Show() {
            Debug.Log("[EVERYDAY] Show Banner");
            //show banner
            MaxSdk.ShowBanner(_adUnitID);

            return true;
        }

        internal void Hide() {
            Debug.Log("[EVERYDAY] Hide Banner");
            MaxSdk.HideBanner(_adUnitID);
        }

        internal Banner(in AdSettings settings) : base(settings) {
            Debug.Log("[EVERYDAY] Createing Banner Ad");
            if (_settings.HasAPSKey(AdType.Banner)) {
                LoadAPSBanner();
            } else {
                CreateMaxBannerAd();
            }
        }

        private void LoadAPSBanner() {
            Debug.Log("[EVERYDAY] Loading APS Banner");
            var apsBanner = new AmazonAds.APSBannerAdRequest(320, 50, _settings.apsBannerSlotId);
            apsBanner.onSuccess += (adResponse) => {
                MaxSdk.SetBannerLocalExtraParameter(_adUnitID, "amazon_ad_response", adResponse.GetResponse());
                CreateMaxBannerAd();
            };
            apsBanner.onFailedWithError += (adError) => {
                MaxSdk.SetBannerLocalExtraParameter(_adUnitID, "amazon_ad_error", adError.GetAdError());
                CreateMaxBannerAd();
            };

            apsBanner.LoadAd();
        }

        private void CreateMaxBannerAd() {
            MaxSdk.CreateBanner(_adUnitID, _settings.bannerPosition);

            if (_settings.showBannerOnLoad) {
                this.Show();
            }
        }
    }
}
