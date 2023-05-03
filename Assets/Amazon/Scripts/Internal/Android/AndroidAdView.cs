using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds.Android {
    public class AndroidAdView : IAdView {
        private AndroidJavaObject dtbAdView = null;
        private static readonly AndroidJavaClass SDKUtilitiesClass = new AndroidJavaClass(AmazonConstants.sdkUtilitiesClass);

        public AndroidAdView (APSAdDelegate delegates) {
            UnityEngine.AndroidJavaClass playerClass = new UnityEngine.AndroidJavaClass(AmazonConstants.unityPlayerClass);
            UnityEngine.AndroidJavaObject currentActivityObject = playerClass.GetStatic<UnityEngine.AndroidJavaObject> ("currentActivity");

            DTBAdBannerListener adBannerListener = new DTBAdBannerListener();
            adBannerListener.adDelegate = delegates;
            dtbAdView = new AndroidJavaObject(AmazonConstants.dtbAdViewClass, currentActivityObject, adBannerListener);
        }

        public override void FetchAd (AdResponse adResponse) {
            if (dtbAdView != null) {
                AndroidJavaObject response = adResponse.GetAndroidResponseObject();
                string bidInfo = SDKUtilitiesClass.CallStatic<string>("getBidInfo", response);
                dtbAdView.Call("fetchAd", bidInfo);
            }
        }
    }
}