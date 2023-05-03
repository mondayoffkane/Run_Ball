using System;
using UnityEngine;

namespace AmazonAds.Android {
    public class DTBCallback : AndroidJavaProxy {
        public enum ErrorCode { NO_ERROR, NETWORK_ERROR, NETWORK_TIMEOUT, NO_FILL, INTERNAL_ERROR, REQUEST_ERROR }

        public DTBCallback () : base ("com.amazon.device.ads.DTBAdCallback") { }

        public Amazon.OnSuccessDelegate onSuccessCallback;
        public Amazon.OnFailureDelegate onFailureCallback;
        public Amazon.OnFailureWithErrorDelegate onFailureWithErrorCallback;

        private void onSuccess (AndroidJavaObject paramDTBAdResponse) {
            AndroidAdResponse response = new AndroidAdResponse (paramDTBAdResponse);
            if (paramDTBAdResponse != null) {
                AndroidJavaObject refreshRequest = paramDTBAdResponse.Call<AndroidJavaObject> ("getAdLoader");
                AdRequest refreshLoader = new AdRequest(new Android.DTBAdRequest(refreshRequest));
                response.SetAdLoader(refreshLoader);
            }
            onSuccessCallback (response);
        }

        private void onFailure (AndroidJavaObject paramAdError) {
            AndroidJavaObject codeObj = paramAdError.Call<AndroidJavaObject> ("getCode");
            int codeInt = codeObj.Call<int> ("ordinal");
            ErrorCode code = (ErrorCode) codeInt;
            string message = paramAdError.Call<String> ("getMessage");

            if (onFailureCallback != null) {
                onFailureCallback (code + ":" + message);
            } 
            
            if (onFailureWithErrorCallback != null) {
                AdError adError = new AdError(codeInt, message);
                AndroidJavaObject refreshRequest = paramAdError.Call<AndroidJavaObject> ("getAdLoader");
                AdRequest refreshLoader = new AdRequest(new Android.DTBAdRequest(refreshRequest));
                adError.SetAdLoader(refreshLoader);
                adError.SetAdError(paramAdError);
                onFailureWithErrorCallback(adError);
            }
        }
    }
}