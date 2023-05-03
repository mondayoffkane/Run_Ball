using System;
using System.Collections.Generic;

namespace AmazonAds {
    public class AdInterstitial {
        internal IAdInterstitial adInterstital;

        public AdInterstitial (APSAdDelegate delegates) {
            #if UNITY_ANDROID
                adInterstital = new Android.AndroidAdInterstitial(delegates);
            #elif UNITY_IOS
                adInterstital = new IOS.IOSAdInterstitial(delegates);
            #else
                //Other platforms not supported
            #endif
        }

        public void FetchAd (AdResponse adResponse) {
            adInterstital.FetchAd(adResponse);
        }

        public void Show () {
            adInterstital.Show();
        }
    }
}