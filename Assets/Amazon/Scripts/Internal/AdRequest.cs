using UnityEngine;
namespace AmazonAds {
    public class AdRequest {
        internal IAdRequest client;
        public Amazon.OnFailureDelegate onFailed;
        public Amazon.OnFailureWithErrorDelegate onFailedWithError;
        public Amazon.OnSuccessDelegate onSuccess;

        public AdRequest () {
#if UNITY_ANDROID
            client = new Android.DTBAdRequest ();
#elif UNITY_IOS
            client = new IOS.DTBAdRequest ();
#else
            //Other platforms not supported
#endif
        }

        public AdRequest (IAdRequest adRequest) {
            client = adRequest;
        }

        public void PutCustomTarget (string key, string value) {
            client.PutCustomTarget (key, value);
        }

        public void SetRefreshFlag (bool flag) {
            client.SetRefreshFlag(flag);
        }

        public void SetAutoRefresh() {
            client.SetAutoRefresh();
        }

        public void SetAutoRefresh(int secs) {
            client.SetAutoRefresh(secs);
        }

        public void ResumeAutoRefresh() {
            client.ResumeAutoRefresh();
        }

        public void StopAutoRefresh() {
            client.StopAutoRefresh();
        }

        public void PauseAutoRefresh() {
            client.PauseAutoRefresh();
        }

        public void LoadAd () {
            if (onSuccess != null && onFailed != null) {
                client.LoadAd (onFailed, onSuccess);
            } else if (onSuccess != null && onFailedWithError != null) {
                client.LoadAd (onFailedWithError, onSuccess);
            }
        }
    }
}