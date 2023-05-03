using System;
using AOT;
using System.Runtime.InteropServices;
using UnityEngine;
namespace AmazonAds.IOS {
    public class DTBAdBannerDispatcher {
        public delegate void OnAdLoadedDelegate (IntPtr callback);
        public delegate void OnAdFailedDelegate (IntPtr callback);
        public delegate void OnAdClickedDelegate (IntPtr callback);      
        public delegate void OnImpressionFiredDelegate (IntPtr callback);

        IntPtr clientPtr;
        IntPtr thisPtr;
        public APSAdDelegate adDelegate;

        public DTBAdBannerDispatcher (APSAdDelegate delegates) {
            thisPtr = (IntPtr)GCHandle.Alloc(this);
            clientPtr = Externs._createBannerDelegate();
            adDelegate = delegates;
            Externs._setBannerDelegate(thisPtr, clientPtr, 
                                    OnAdLoaded, OnAdFailed, 
                                    OnAdClicked, OnImpressionFired);
        }

        public IntPtr GetPtr()
        {
            return clientPtr;
        }

        private static DTBAdBannerDispatcher IntPtrToClient(IntPtr client)
        {
            GCHandle handle = (GCHandle)client;
            return handle.Target as DTBAdBannerDispatcher;
        }

        [MonoPInvokeCallback (typeof (OnAdLoadedDelegate))]
        public static void OnAdLoaded (IntPtr client) {
            DTBAdBannerDispatcher bannerDispatcher = IntPtrToClient(client);
            bannerDispatcher.adDelegate.onAdLoaded();
        }

        [MonoPInvokeCallback (typeof (OnAdFailedDelegate))]
        public static void OnAdFailed (IntPtr client) {
            DTBAdBannerDispatcher bannerDispatcher = IntPtrToClient(client);
            bannerDispatcher.adDelegate.onAdFailed();
        }

        [MonoPInvokeCallback (typeof (OnAdClickedDelegate))]
        public static void OnAdClicked (IntPtr client) {
            DTBAdBannerDispatcher bannerDispatcher = IntPtrToClient(client);
            bannerDispatcher.adDelegate.onAdClicked();
        }

        [MonoPInvokeCallback (typeof (OnImpressionFiredDelegate))]
        public static void OnImpressionFired (IntPtr client) {
            DTBAdBannerDispatcher bannerDispatcher = IntPtrToClient(client);
            bannerDispatcher.adDelegate.onImpressionFired();
        }
    }

}