using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds.IOS {
    public class IOSAdInterstitial : IAdInterstitial {
        private IntPtr adDispatcher;

        public IOSAdInterstitial (APSAdDelegate delegates) {
            DTBAdInterstitialDispatcher adInterstitialDispatcher = new DTBAdInterstitialDispatcher(delegates);
            adDispatcher = Externs._createAdInterstitial(adInterstitialDispatcher.GetPtr());
        }

        public override void FetchAd (AdResponse adResponse) {
            if (adDispatcher != null) {
                Externs._fetchInterstitialAd(adDispatcher, adResponse.GetInstance());
            }
        }

        public override void Show () {
            if (adDispatcher != null) {
                Externs._showInterstitial(adDispatcher);
            }
        }
    }
}