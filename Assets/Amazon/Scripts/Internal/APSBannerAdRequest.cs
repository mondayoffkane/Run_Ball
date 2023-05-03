namespace AmazonAds {
    public class APSBannerAdRequest : AdRequest {

        public APSBannerAdRequest () : base() {  
             Amazon.OnApplicationPause += OnApplicationPause;
        }

        public APSBannerAdRequest (string slotGroupName) : base() {
            Amazon.OnApplicationPause += OnApplicationPause;
            client.SetSlotGroup (slotGroupName);
        }

        public APSBannerAdRequest (int width, int height, string uid) : base() {
            Amazon.OnApplicationPause += OnApplicationPause;
            AdSize size = new AdSize (width, height, uid);
            client.SetSizes (size.GetInstance ());
        }

        public APSBannerAdRequest (AdSize size) {
            Amazon.OnApplicationPause += OnApplicationPause;
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

        public void SetAutoRefreshAdMob (bool flag, bool isSmartBanner = false) {
            client.SetAutoRefreshAdMob (flag, isSmartBanner);
        }

        public void SetAutoRefreshMoPub (bool flag, int refreshTime) {
            client.SetAutoRefreshMoPub (flag, refreshTime);
        }

        public void DisposeAd () {
            client.DisposeAd ();
        }

        public void IsAutoRefreshAdMob () {
            client.IsAutoRefreshAdMob ();
        }

        public void IsAutoRefreshMoPub () {
            client.IsAutoRefreshMoPub ();
        }
        public string AutoRefreshID () {
            return client.AutoRefreshID ();
        }

        public void CreateFetchManager (bool isSmartBanner = false) {
            client.CreateFetchManager (isSmartBanner);
        }

        public void DestroyFetchManager () {
            client.DestroyFetchManager ();
        }

        public void OnApplicationPause (bool isPaused) {
            if (isPaused) {
                if( client.IsAutoRefreshAdMob() ){
                    client.StopFetchManager();
                }
            } else {
                if( client.IsAutoRefreshAdMob() ){
                    client.StartFetchManager();
                }
            }
        }
    }
}