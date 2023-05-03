using System;
using AOT;
using System.Runtime.InteropServices;
using UnityEngine;
namespace AmazonAds.IOS {
    public class DTBAdInterstitialDispatcher {
        public delegate void OnAdLoadedDelegate (IntPtr callback);
        public delegate void OnAdFailedDelegate (IntPtr callback);
        public delegate void OnAdClickedDelegate (IntPtr callback);      
        public delegate void OnImpressionFiredDelegate (IntPtr callback);
        public delegate void OnAdOpenDelegate (IntPtr callback);      
        public delegate void OnAdClosedDelegate (IntPtr callback);      

        IntPtr clientPtr;
        IntPtr thisPtr;
        public APSAdDelegate adDelegate;

        public DTBAdInterstitialDispatcher (APSAdDelegate delegates) {
            thisPtr = (IntPtr)GCHandle.Alloc(this);
            clientPtr = Externs._createInterstitialDelegate();
            adDelegate = delegates;
            Externs._setInterstitialDelegate(thisPtr, clientPtr, 
                                            OnAdLoaded, OnAdFailed, 
                                            OnAdClicked, OnImpressionFired,
                                            OnAdOpen, OnAdClosed);
        }

        public IntPtr GetPtr(){
            return clientPtr;
        }

        private static DTBAdInterstitialDispatcher IntPtrToClient(IntPtr client)
        {
            GCHandle handle = (GCHandle)client;
            return handle.Target as DTBAdInterstitialDispatcher;
        }

        [MonoPInvokeCallback (typeof (OnAdLoadedDelegate))]
        public static void OnAdLoaded (IntPtr client) {
            DTBAdInterstitialDispatcher interstitialDispatcher = IntPtrToClient(client);
            interstitialDispatcher.adDelegate.onAdLoaded();
        }

        [MonoPInvokeCallback (typeof (OnAdFailedDelegate))]
        public static void OnAdFailed (IntPtr client) {
            DTBAdInterstitialDispatcher interstitialDispatcher = IntPtrToClient(client);
            interstitialDispatcher.adDelegate.onAdFailed();
        }

        [MonoPInvokeCallback (typeof (OnAdClickedDelegate))]
        public static void OnAdClicked (IntPtr client) {
            DTBAdInterstitialDispatcher interstitialDispatcher = IntPtrToClient(client);
            interstitialDispatcher.adDelegate.onAdClicked();
        }

        [MonoPInvokeCallback (typeof (OnImpressionFiredDelegate))]
        public static void OnImpressionFired (IntPtr client) {
            DTBAdInterstitialDispatcher interstitialDispatcher = IntPtrToClient(client);
            interstitialDispatcher.adDelegate.onImpressionFired();
        }
        
        [MonoPInvokeCallback (typeof (OnAdOpenDelegate))]
        public static void OnAdOpen (IntPtr client) {
            DTBAdInterstitialDispatcher interstitialDispatcher = IntPtrToClient(client);
            interstitialDispatcher.adDelegate.onAdOpen();
        }

        [MonoPInvokeCallback (typeof (OnAdClosedDelegate))]
        public static void OnAdClosed (IntPtr client) {
            DTBAdInterstitialDispatcher interstitialDispatcher = IntPtrToClient(client);
            interstitialDispatcher.adDelegate.onAdClosed();
        }
    }
}