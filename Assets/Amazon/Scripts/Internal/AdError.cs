using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds {
    public class AdError {
        private int errorCode;
        private String errorMessage;
        private AdRequest adLoader;
        private AndroidJavaObject adError;
        private IntPtr adErrorPtr;

        public AdError(int code, String message) {
            errorCode = code;
            errorMessage = message;
        }

        public int GetCode () {
            return errorCode;
        }

        public String GetMessage() {
            return errorMessage;
        }

        public AdRequest GetAdLoader() {
            return adLoader;
        }

#if UNITY_ANDROID
        public AndroidJavaObject GetAdError() {
            return adError;
        }
#else
        public IntPtr GetAdError()
        {
            return adErrorPtr;
        }
#endif

        public IntPtr GetInstance() {
            return adErrorPtr;
        }

        internal void SetAdLoader(AdRequest adRequest) {
            adLoader = adRequest;
        }

        internal void SetAdError(AndroidJavaObject error) {
            adError = error;
        }

        internal void SetInstance(IntPtr inPtr) {
            adErrorPtr = inPtr;
        }
    }
}