using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
namespace AmazonAds.Android {
    public class DTBFetchManager: IFetchManager {
        private static readonly AndroidJavaClass dTBFetchManagerClass = new AndroidJavaClass ("com.amazon.device.ads.DTBFetchManager");
        private AndroidJavaObject dTBFetchManager = null;

        public DTBFetchManager (AndroidJavaObject client) {
            dTBFetchManager = client;
        }

        public void dispense () {
            AndroidJavaObject dTBAdResponse = dTBFetchManager.Call<AndroidJavaObject> ("dispense");
        }

        public void start () {
            dTBFetchManager.Call ("start");
        }

        public void stop () {
            dTBFetchManager.Call ("stop");
        }

        public bool isEmpty () {
            return dTBFetchManager.Call<bool> ("isEmpty");
        }

        public AmazonAds.AdResponse peek () {
            AndroidJavaObject dTBAdResponse = dTBFetchManager.Call<AndroidJavaObject> ("peek");
            return new AndroidAdResponse(dTBAdResponse);
        }
    }
}