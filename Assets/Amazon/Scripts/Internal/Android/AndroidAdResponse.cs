using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds.Android {
    public class AndroidAdResponse : AdResponse {
        private AndroidJavaObject response;
        private AdRequest adLoader;

        private static readonly AndroidJavaClass sdkUtilitiesClass = new AndroidJavaClass("com.amazon.device.ads.SDKUtilities");

        public AndroidAdResponse( ) { }
        public AndroidAdResponse(AndroidJavaObject newResponse) {
            response = newResponse;
        }

        public override IntPtr GetInstance()
        {
            throw new NotImplementedException();
        }

        public override string GetBidInfo()
        {
            return sdkUtilitiesClass.CallStatic<string>("getBidInfo", response);
        }

        public override IntPtr GetIosResponseObject()
        {
            throw new NotImplementedException();
        }

        public override AndroidJavaObject GetAndroidResponseObject()
        {
            return response;
        }

        public override string GetPricePoint()
        {
            return sdkUtilitiesClass.CallStatic<string>("getPricePoint", response);
        }

        public override string GetMediationHints(bool isSmartBanner = false)
        {
            throw new NotImplementedException();
        }

        public override int GetHeight()
        {
            return sdkUtilitiesClass.CallStatic<int>("getHeight", response);
        }

        public override int GetWidth()
        {
            return sdkUtilitiesClass.CallStatic<int>("getWidth", response);
        }

#if UNITY_ANDROID
        public override AndroidJavaObject GetResponse()
        {
            return response;
        }
#else
        public override IntPtr GetResponse()
        {
            throw new NotImplementedException();
        }
#endif

        public override String GetMoPubKeywords () {
            return response.Call<String> ("getMoPubKeywords");
        }
        
        public override Dictionary<String, String> GetRendering (bool isSmartBanner = false, string fetchLabel = null) {
            Dictionary<String, String> map = new Dictionary<String, String> ();
            if( response != null){
                AndroidJavaObject bundle = response.Call<AndroidJavaObject> ("getRenderingBundle", isSmartBanner, fetchLabel);

                map.Add ("bid_html_template", bundle.Call<String> ("getString", "bid_html_template"));
                map.Add ("event_server_parameter", bundle.Call<String> ("getString", "event_server_parameter"));
                map.Add ("amazon_ad_info", bundle.Call<String> ("getString", "amazon_ad_info"));
                map.Add ("bid_identifier", bundle.Call<String> ("getString", "bid_identifier"));
                map.Add ("hostname_identifier", bundle.Call<String> ("getString", "hostname_identifier"));
                map.Add ("start_load_time", bundle.Call<long> ("getLong", "start_load_time").ToString ());
                if (isSmartBanner) {
                    map.Add ("expected_width", bundle.Call<int> ("getInt", "expected_width").ToString ());
                    map.Add ("expected_height", bundle.Call<int> ("getInt", "expected_height").ToString ());
                }
                if (bundle.Call<bool> ("containsKey", "amazon_request_queue"))
                    map.Add ("amazon_request_queue", bundle.Call<String> ("getString", "amazon_request_queue"));
            } else {
                map.Add("isAutoRefresh","1");
            }
            return map;
        }

        public override AdRequest GetAdLoader() {
            return adLoader;
        }

        internal override void SetAdLoader(AdRequest adRequest) {
            adLoader = adRequest;
        }
    }
}