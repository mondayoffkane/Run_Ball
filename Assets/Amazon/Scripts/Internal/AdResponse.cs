using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds {
    public abstract class AdResponse {
        public abstract String GetMoPubKeywords ();
        public abstract IntPtr GetInstance();
        public abstract Dictionary<String, String> GetRendering (bool isSmartBanner = false, string fetchLabel = null);
        public abstract AdRequest GetAdLoader();
        public abstract String GetBidInfo();
        public abstract String GetPricePoint();
        public abstract int GetWidth();
        public abstract int GetHeight();
        public abstract String GetMediationHints(bool isSmartBanner = false);
        internal abstract void SetAdLoader(AdRequest adRequest);

        public abstract IntPtr GetIosResponseObject();
        public abstract AndroidJavaObject GetAndroidResponseObject();

#if UNITY_ANDROID
        public abstract AndroidJavaObject GetResponse();
#else
        public abstract IntPtr GetResponse();
#endif

    }
}