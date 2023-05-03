using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds.IOS {
    public class IOSAdView : IAdView {
        private IntPtr adDispatcher;

        public IOSAdView (AdSize adSize, APSAdDelegate delegates) {
            DTBAdBannerDispatcher adBannerDispatcher = new DTBAdBannerDispatcher(delegates);
            adDispatcher = Externs._createAdView(adSize.GetInstance().GetWidth(), adSize.GetInstance().GetHeight(), adBannerDispatcher.GetPtr());
        }

        public override void FetchAd (AdResponse adResponse) {
            if (adDispatcher != null) {
                Externs._fetchBannerAd(adDispatcher, adResponse.GetInstance());
            }
        }
    }
}