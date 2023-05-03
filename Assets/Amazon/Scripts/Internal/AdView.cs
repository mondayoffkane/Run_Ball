using System;
using System.Collections.Generic;

namespace AmazonAds {
    public class AdView {
        internal IAdView adView;

        public AdView (AdSize adSize, APSAdDelegate delegates) {
            #if UNITY_ANDROID
                adView = new Android.AndroidAdView(delegates);
            #elif UNITY_IOS
                adView = new IOS.IOSAdView(adSize, delegates);
            #else
                //Other platforms not supported
            #endif
        }

        public void fetchAd (AdResponse adResponse) {
            adView.FetchAd(adResponse);
        }
    }
}