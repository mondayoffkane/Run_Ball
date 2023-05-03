using System;
using UnityEngine;

namespace AmazonAds.Android {
    public class DTBAdInterstitialListener : AndroidJavaProxy {
        public DTBAdInterstitialListener () : base ("com.amazon.device.ads.DTBAdInterstitialListener") { }

        public APSAdDelegate adDelegate;

        private void onAdLoaded (AndroidJavaObject paramObject) {
            if (adDelegate != null) {
                adDelegate.onAdLoaded();
            }
        }

        private void onAdFailed (AndroidJavaObject paramObject) {
            if (adDelegate != null) {
                adDelegate.onAdFailed();
            }
        }
        
        private void onAdClicked (AndroidJavaObject paramObject) {
            if (adDelegate != null) {
                adDelegate.onAdClicked();
            }
        }

        private void onAdOpen (AndroidJavaObject paramObject) {
            if (adDelegate != null) {
                adDelegate.onAdOpen();
            }
        }

        private void onAdClosed (AndroidJavaObject paramObject) {
            if (adDelegate != null) {
                adDelegate.onAdClosed();
            }
        }

        private void onImpressionFired (AndroidJavaObject paramObject) {
            if (adDelegate != null) {
                adDelegate.onImpressionFired();
            }
        }

        private void onAdLeftApplication (AndroidJavaObject paramObject) {
            
        }

        private void onVideoCompleted (AndroidJavaObject paramObject) {
            if (adDelegate != null) {
                adDelegate.onVideoCompleted();
            }
        }
    }
}