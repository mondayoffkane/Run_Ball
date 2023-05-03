using System.Collections;
using UnityEngine;

namespace AmazonAds.Android {
    public class DTBAdRequest : IAdRequest {
        private AndroidJavaObject dTBAdRequest = null;
        private string _slotGroupName = null;
        private IAdSize _bannerAdSize = null;
        private IInterstitialAdSize _interstitialAdSize = null;
        private IVideo _videoAdSize = null;

        public DTBAdRequest () {
            UnityEngine.AndroidJavaClass playerClass = new UnityEngine.AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            UnityEngine.AndroidJavaObject currentActivityObject = playerClass.GetStatic<UnityEngine.AndroidJavaObject> ("currentActivity");
            dTBAdRequest = new AndroidJavaObject ("com.amazon.device.ads.DTBAdRequest", currentActivityObject);
        }

        public DTBAdRequest (AndroidJavaObject dtbRequest) { 
            dTBAdRequest = dtbRequest;
        }

        public override void PutCustomTarget (string key, string value) {
            dTBAdRequest.Call ("putCustomTarget", key, value);
        }

        public override void SetSizes (IAdSize sizes) {
            _bannerAdSize = sizes;
            DTBAdSize size = (DTBAdSize) sizes;
            autoRefreshID = "Banner_" + sizes.GetWidth () + "_" + sizes.GetHeight () + "_" + sizes.GetSlotUUID ();
            AndroidJavaClass arrayClass = new AndroidJavaClass( "java.lang.reflect.Array" );
            AndroidJavaObject arrayObject = arrayClass.CallStatic< AndroidJavaObject >( "newInstance", new AndroidJavaClass( "com.amazon.device.ads.DTBAdSize" ), 1 );
            arrayClass.CallStatic( "set", arrayObject, 0, ((DTBAdSize) sizes).GetInstance ()  );
            var ar = new object[] { arrayObject };
            dTBAdRequest.Call ("setSizes", ar);
        }

        public override void SetSizes (IInterstitialAdSize sizes) {
            _interstitialAdSize = sizes;
            AndroidJavaClass arrayClass = new AndroidJavaClass( "java.lang.reflect.Array" );
            AndroidJavaObject arrayObject = arrayClass.CallStatic< AndroidJavaObject >( "newInstance", new AndroidJavaClass( "com.amazon.device.ads.DTBAdSize" ), 1 );
            arrayClass.CallStatic( "set", arrayObject, 0, ((DTBAdSize.DTBInterstitialAdSize) sizes).GetInstance ()  );
            var ar = new object[] { arrayObject };
            dTBAdRequest.Call ("setSizes", ar);
        }

        public override void SetSizes (IVideo sizes) {
            _videoAdSize = sizes;
            AndroidJavaClass arrayClass = new AndroidJavaClass( "java.lang.reflect.Array" );
            AndroidJavaObject arrayObject = arrayClass.CallStatic< AndroidJavaObject >( "newInstance", new AndroidJavaClass( "com.amazon.device.ads.DTBAdSize" ), 1 );
            arrayClass.CallStatic( "set", arrayObject, 0, ((DTBAdSize.DTBVideo) sizes).GetInstance ()  );
            var ar = new object[] { arrayObject };
            dTBAdRequest.Call ("setSizes", ar);
        }

        public override void SetSlotGroup (string slotGroupName) {
            _slotGroupName = slotGroupName;
            autoRefreshID = "Banner_" + slotGroupName;
            dTBAdRequest.Call ("setSlotGroup", slotGroupName);
        }

        public IAdSize GetBannerAdSizes () {
            return _bannerAdSize;
        }

        public IInterstitialAdSize GetInterstitialSizes () {
            return _interstitialAdSize;
        }

        public IVideo GetVideoSizes () {
            return _videoAdSize;
        }

        public string GetSlotGroup () {
            return _slotGroupName;
        }

        public override void LoadAd (Amazon.OnFailureDelegate failure, Amazon.OnSuccessDelegate success) {
            if (IsAutoRefreshAdMob ()) {
                Schedule.WaitForAdResponce (fetchManager, failure, success);
                return;
            }

            DTBCallback callback = createLoadAdCallback(success);
            callback.onFailureCallback = failure;

            dTBAdRequest.Call ("loadAd", callback);
            requestHasBeenUsed = true;
        }

        public override void LoadSmartBanner (Amazon.OnFailureDelegate failure, Amazon.OnSuccessDelegate success) {
            if (IsAutoRefreshAdMob ()) {
                Schedule.WaitForAdResponce (fetchManager, failure, success);
                return;
            }

            DTBCallback callback = createLoadAdCallback(success);
            callback.onFailureCallback = failure;

            dTBAdRequest.Call ("loadSmartBanner", callback);
            requestHasBeenUsed = true;
        }

        public override void LoadAd (Amazon.OnFailureWithErrorDelegate failure, Amazon.OnSuccessDelegate success) {
            DTBCallback callback = createLoadAdCallback(success);
            callback.onFailureWithErrorCallback = failure;
            
            dTBAdRequest.Call ("loadAd", callback);
            requestHasBeenUsed = true;
        }

        public override void LoadSmartBanner (Amazon.OnFailureWithErrorDelegate failure, Amazon.OnSuccessDelegate success) {
            DTBCallback callback = createLoadAdCallback(success);
            callback.onFailureWithErrorCallback = failure;
            
            dTBAdRequest.Call ("loadSmartBanner", callback);
            requestHasBeenUsed = true;
        }

        private DTBCallback createLoadAdCallback(Amazon.OnSuccessDelegate success) {
            DTBCallback callback = new DTBCallback ();
            callback.onSuccessCallback = success;

            return callback;
        }

        public AndroidJavaObject GetClient () {
            return dTBAdRequest;
        }

        public override void SetAutoRefresh() {
            dTBAdRequest.Call("setAutoRefresh");
        }

        public override void SetAutoRefresh(int secs) {
            dTBAdRequest.Call("setAutoRefresh", secs);
        }

        public override void ResumeAutoRefresh() {
            dTBAdRequest.Call("resumeAutoRefresh");
        }

        public override void StopAutoRefresh() {
            dTBAdRequest.Call("stop");
        }

        public override void PauseAutoRefresh() {
            dTBAdRequest.Call("pauseAutoRefresh");
        }

        public override void SetAutoRefreshMoPub (bool flag){
            SetAutoRefreshMoPub(flag, refreshTime);
        }

        public override void SetAutoRefreshMoPub (bool flag, int refreshTime) {
            isAutoRefreshMoPub = flag;
            this.refreshTime = refreshTime;
            if(flag){
                if( requestHasBeenUsed ){
                    ResumeAutoRefresh();
                } else {
                    SetAutoRefresh(refreshTime);
                }
            } else {
                PauseAutoRefresh();
            }
        }

        public override void SetAutoRefreshAdMob (bool flag, bool isSmartBanner = false) {
            isAutoRefreshAdMob = flag;
            if (flag) {
                CreateFetchManager (isSmartBanner);
                StartFetchManager ();
            } else {
                StopFetchManager ();
            }
        }
        
        public override void SetRefreshFlag (bool flag) {
            dTBAdRequest.Call("setRefreshFlag", flag);
        }

        public override void DisposeAd () {
            if (fetchManager != null) {
                fetchManager.dispense ();
            } else {
                Debug.LogWarning ("FetchManager not Init. Please turn on auto-refresh.");
            }
        }

        public override void StartFetchManager () {
            if (fetchManager != null){
                fetchManager.start ();
            }
        }

        public override void StopFetchManager () {
            if (fetchManager != null){
                fetchManager.stop ();
            }
        }

        public override void CreateFetchManager (bool isSmartBanner = false) {
            if (fetchManager == null) {
                fetchManager = DTBFetchFactory.GetInstance ().createFetchManager (autoRefreshID, dTBAdRequest, isSmartBanner);
            }
        }

        public override void DestroyFetchManager () {
            if (fetchManager != null) {
                DTBFetchFactory.GetInstance ().removeFetchManager (autoRefreshID);
                fetchManager = null;
            }
        }
    }
}