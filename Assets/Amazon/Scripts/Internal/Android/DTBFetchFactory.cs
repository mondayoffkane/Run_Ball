using System;
using UnityEngine;
namespace AmazonAds.Android {
    public class DTBFetchFactory {
        private static readonly AndroidJavaClass dTBFetchFactoryClass = new AndroidJavaClass ("com.amazon.device.ads.DTBFetchFactory");
        private AndroidJavaObject dTBFetchFactory = null;

        public DTBFetchFactory (AndroidJavaObject obj) {
            dTBFetchFactory = obj;
        }

        public static DTBFetchFactory GetInstance () {
            return new DTBFetchFactory (dTBFetchFactoryClass.CallStatic<AndroidJavaObject> ("getInstance"));
        }

        public DTBFetchManager GetFetchManager (String label) {
            return new DTBFetchManager (dTBFetchFactory.Call<AndroidJavaObject> ("getFetchManager", label));
        }

        public DTBFetchManager createFetchManager (String label, AndroidJavaObject loader) { //DTBAdLoader
            return new DTBFetchManager (dTBFetchFactory.Call<AndroidJavaObject> ("createFetchManager", label, loader));
        }

        public DTBFetchManager createFetchManager (String label, AndroidJavaObject loader, bool isSmartBanner) { //DTBAdLoader
            return new DTBFetchManager (dTBFetchFactory.Call<AndroidJavaObject> ("createFetchManager", label, loader, isSmartBanner));
        }

        public void removeFetchManager (String label) {
            dTBFetchFactory.Call ("removeFetchManager", label);
        }
    }
}