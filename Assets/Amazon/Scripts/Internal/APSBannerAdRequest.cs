using System;
using UnityEngine;

namespace AmazonAds {
    public class APSBannerAdRequest : AdRequest {

        public APSBannerAdRequest () : base() { 
        }

        public APSBannerAdRequest (string slotGroupName) : base() {
            client.SetSlotGroup (slotGroupName);
        }

        public APSBannerAdRequest (int width, int height, string uid) : base() {
            AdSize size = new AdSize (width, height, uid);
            client.SetSizes (size.GetInstance ());
        }

        public APSBannerAdRequest (AdSize size) {
            client.SetSizes (size.GetInstance ());
        }

        public void LoadSmartBanner () {
            if (onSuccess != null && onFailed != null) {
                client.LoadSmartBanner (onFailed, onSuccess);
            } else if (onSuccess != null && onFailedWithError != null) {
                client.LoadSmartBanner (onFailedWithError, onSuccess);
            }
        }

        public void SetSizes (int width, int height, string uid) {
            AdSize size = new AdSize (width, height, uid);
            SetSizes (size);
        }

        public void SetSizes (AdSize size) {
            client.SetSizes (size.GetInstance ());
        }

        public void SetSlotGroup (string slotGroupName) {
            client.SetSlotGroup (slotGroupName);
        }

        [Obsolete("This API has been deprecated", false)]
        public void SetAutoRefreshAdMob (bool flag, bool isSmartBanner = false) {
            Debug.LogError("This API has been deprecated");
        }

        [Obsolete("This API has been deprecated", false)]
        public void SetAutoRefreshMoPub (bool flag, int refreshTime) {
            Debug.LogError("This API has been deprecated");
        }

        [Obsolete("This API has been deprecated", false)]
        public void DisposeAd () {
            Debug.LogError("This API has been deprecated");
        }

        [Obsolete("This API has been deprecated", false)]
        public void IsAutoRefreshAdMob () {
            Debug.LogError("This API has been deprecated");
        }

        [Obsolete("This API has been deprecated", false)]
        public void IsAutoRefreshMoPub () {
            Debug.LogError("This API has been deprecated");
        }
        public string AutoRefreshID () {
            return client.AutoRefreshID ();
        }

        [Obsolete("This API has been deprecated", false)]
        public void CreateFetchManager (bool isSmartBanner = false) {
            Debug.LogError("This API has been deprecated");
        }

        [Obsolete("This API has been deprecated", false)]
        public void DestroyFetchManager () {
            Debug.LogError("The API has been deprecated");
        }

        [Obsolete("This API has been deprecated", false)]
        public void OnApplicationPause (bool isPaused) {
            Debug.LogError("This API has been deprecated");
        }
    }
}