using System.Collections.Generic;
using System;
using UnityEngine;

namespace AmazonAds.IOS {
    public class IOSAdResponse : AdResponse {
        IntPtr resp;
        private AdRequest adLoader;

        public IOSAdResponse(){ }
        public IOSAdResponse(IntPtr response){
            resp = response;
        }

        public override int GetHeight()
        {
            return Externs._fetchAdHeight(resp);
        }

        public override int GetWidth()
        {
            return Externs._fetchAdWidth(resp);
        }

        public override string GetMoPubKeywords () {
            return Externs._fetchMoPubKeywords(resp);
        }

        public override IntPtr GetIosResponseObject()
        {
            return resp;
        }

        public override AndroidJavaObject GetAndroidResponseObject()
        {
            throw new NotImplementedException();
        }

        public override string GetBidInfo()
        {
            throw new NotImplementedException();
        }

        public override string GetPricePoint()
        {
            return Externs._fetchAmznSlots(resp);
        }

        public override string GetMediationHints(bool isSmartBanner = false)
        {
            return Externs._fetchMediationHints(resp, isSmartBanner);
        }

        public override IntPtr GetInstance()
        {
            return resp;
        }

#if UNITY_ANDROID
        public override AndroidJavaObject GetResponse()
        {
            throw new NotImplementedException();
        }
#else
        public override IntPtr GetResponse()
        {
            return resp;
        }
#endif

        public override Dictionary<string, string> GetRendering (bool isSmartBanner = false, string fetchLabel = null) {
            Dictionary<string, string> rendering = new Dictionary<string, string>();
            if( resp != IntPtr.Zero){
                string mediationHints =  Externs._fetchMediationHints(resp, isSmartBanner);
                string amznSlots = Externs._fetchAmznSlots(resp);
                rendering.Add("mediationHints", mediationHints);
                rendering.Add("amznSlots", amznSlots);
            } else {
                rendering.Add("useFetchManager", "YES");
            }
            return rendering;
        }

        public override AdRequest GetAdLoader() {
            return adLoader;
        }

        internal override void SetAdLoader(AdRequest adRequest) {
            adLoader = adRequest;
        }
    }
}