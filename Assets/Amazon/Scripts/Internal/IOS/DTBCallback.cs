using System;
using AOT;
using System.Runtime.InteropServices;
using UnityEngine;
namespace AmazonAds.IOS {
    public class DTBCallback {
        public delegate void OnFailureWithErrorDelegate (IntPtr callback, int errorMsg, IntPtr adError);
        public delegate void OnFailureDelegate (IntPtr callback, int errorMsg);
        public delegate void OnSuccessDelegate (IntPtr callback, IntPtr response);
        public enum ErrorCode { NO_ERROR, NETWORK_ERROR, NETWORK_TIMEOUT, NO_FILL, INTERNAL_ERROR, REQUEST_ERROR }
        IntPtr clientPtr;
        IntPtr thisPtr;

        public struct DTBAdResponceData{
            public string amznSlots;
            public string mediationHints;
            public string keywords;
        }

        public DTBCallback (Amazon.OnFailureDelegate failureDelegate, Amazon.OnSuccessDelegate successDelegate) { 
            thisPtr = (IntPtr)GCHandle.Alloc(this);
            clientPtr = Externs._createCallback();
            onSuccessCallback = successDelegate;
            onFailureCallback = failureDelegate;
            Externs._amazonSetListeners(thisPtr, clientPtr, OnSuccess, OnFailure);
        }

        public DTBCallback (Amazon.OnFailureWithErrorDelegate failureWithErrorDelegate, Amazon.OnSuccessDelegate successDelegate) { 
            thisPtr = (IntPtr)GCHandle.Alloc(this);
            clientPtr = Externs._createCallback();
            onSuccessCallback = successDelegate;
            onFailureWithErrorCallback = failureWithErrorDelegate;
            Externs._amazonSetListenersWithInfo(thisPtr, clientPtr, OnSuccess, OnFailureWithInfo);
        }

        public Amazon.OnSuccessDelegate onSuccessCallback;
        public Amazon.OnFailureDelegate onFailureCallback;
        public Amazon.OnFailureWithErrorDelegate onFailureWithErrorCallback;

        private AdRequest refreshAdLoader = null;

        public IntPtr GetPtr(){
            return clientPtr;
        }

        private static DTBCallback IntPtrToClient(IntPtr callbackClient)
        {
            GCHandle handle = (GCHandle)callbackClient;
            return handle.Target as DTBCallback;
        }

        [MonoPInvokeCallback (typeof (OnSuccessDelegate))]
        public static void OnSuccess (IntPtr callbackClient, IntPtr response) {
            DTBCallback client = IntPtrToClient(callbackClient);
            IOSAdResponse resp = new IOSAdResponse (response);
            IOS.DTBAdRequest dtbAdRequest = new IOS.DTBAdRequest(Externs._getAdLoaderFromResponse(response));
            resp.SetAdLoader(new AdRequest(dtbAdRequest));
            client.onSuccessCallback(resp);
        }

        [MonoPInvokeCallback (typeof (OnFailureDelegate))]
        public static void OnFailure (IntPtr callbackClient, int errorMsg) {
            DTBCallback client = IntPtrToClient(callbackClient);

            if (client.onFailureCallback != null) {
                client.onFailureCallback ("Code:" + errorMsg);
            }
        }

        [MonoPInvokeCallback (typeof (OnFailureWithErrorDelegate))]
        public static void OnFailureWithInfo (IntPtr callbackClient, int errorMsg, IntPtr errorInfo) {
            DTBCallback client = IntPtrToClient(callbackClient);

            if (client.onFailureWithErrorCallback != null) {
                AdError adError = new AdError(errorMsg, "");
                IOS.DTBAdRequest dtbAdRequest = new IOS.DTBAdRequest(Externs._getAdLoaderFromAdError(errorInfo));
                adError.SetAdLoader(new AdRequest(dtbAdRequest));
                adError.SetInstance(errorInfo);
                client.onFailureWithErrorCallback(adError);
            }
        }

    }
}