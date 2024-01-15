using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
namespace AmazonAds.IOS {
    public class DTBAdRequest : IAdRequest {
        private IntPtr adLoader;
        private string _slotGroupName = null;
        private IAdSize _bannerAdSize = null;
        private IInterstitialAdSize _interstitialAdSize = null;
        private IVideo _videoAdSize = null;

        public DTBAdRequest () {
            adLoader = Externs._createAdLoader ();
        }

        public DTBAdRequest (IntPtr adRequest) {
            adLoader = adRequest;
        }

        public IntPtr GetInstance(){
            return adLoader;
        }

        public override void DisposeAd () {
            if (fetchManager != null) {
                fetchManager.dispense();
            } else {
                UnityEngine.Debug.LogWarning ("FetchManager not Init. Please turn on auto-refresh.");
            }
        }

        public override void LoadAd (Amazon.OnFailureDelegate failure, Amazon.OnSuccessDelegate success) {
            requestHasBeenUsed = true;
            if (IsAutoRefreshAdMob ()) {
                Schedule.WaitForAdResponce (fetchManager, failure, success);
                return;
            }
            DTBCallback callback = new DTBCallback (failure, success);
            Externs._loadAd (adLoader, callback.GetPtr ());
        }

        public override void LoadSmartBanner (Amazon.OnFailureDelegate failure, Amazon.OnSuccessDelegate success) {
            requestHasBeenUsed = true;
            if (IsAutoRefreshAdMob ()) {
                Schedule.WaitForAdResponce (fetchManager, failure, success);
                return;
            }
            DTBCallback callback = new DTBCallback (failure, success);
            Externs._loadSmartBanner (adLoader, callback.GetPtr ());
        }

        public override void LoadAd (Amazon.OnFailureWithErrorDelegate failureWithError, Amazon.OnSuccessDelegate success) {
            requestHasBeenUsed = true;
            DTBCallback callback = new DTBCallback (failureWithError, success);
            Externs._loadAd (adLoader, callback.GetPtr ());
        }

        public override void LoadSmartBanner (Amazon.OnFailureWithErrorDelegate failureWithError, Amazon.OnSuccessDelegate success) {
            requestHasBeenUsed = true;
            DTBCallback callback = new DTBCallback (failureWithError, success);
            Externs._loadSmartBanner (adLoader, callback.GetPtr ());
        }

        public override void PutCustomTarget (string key, string value) {
            Externs._putCustomTarget (adLoader, key, value);
        }

        public override void SetAutoRefresh() {
            Externs._setAutoRefreshNoArgs(adLoader);
        }

        public override void SetAutoRefresh(int secs) {
            Externs._setAutoRefresh(adLoader,secs);
        }

        public override void PauseAutoRefresh() {
            Externs._pauseAutoRefresh(adLoader);
        }

        public override void StopAutoRefresh() {
            Externs._stopAutoRefresh(adLoader);
        }

        public override void ResumeAutoRefresh() {
            Externs._resumeAutoRefresh(adLoader);
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
            Debug.LogError("The API has been deprecated");
        }

        public override void SetRefreshFlag (bool flag) {
            Externs._setRefreshFlag(adLoader, flag);
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
            if (fetchManager == null){
                fetchManager = new DTBFetchManager(this, autoRefreshID, isSmartBanner);
            }
        }

        public override void DestroyFetchManager () {
            if (fetchManager != null){
                ((AmazonAds.IOS.DTBFetchManager)fetchManager).destroy(autoRefreshID);
            }
        }

        public override void SetSizes (IAdSize sizes) {
            _bannerAdSize = sizes;
            int height = sizes.GetHeight ();
            int width = sizes.GetWidth ();
            string slotType = "SLOT_" + width + "_" + height;
            autoRefreshID = slotType;
            Externs._setSizes (adLoader, ((DTBAdSize) sizes).GetInstance ());
        }

        public override void SetSizes (IInterstitialAdSize sizes) {
            _interstitialAdSize = sizes;
            Externs._setSizes (adLoader, ((DTBAdSize.DTBInterstitialAdSize) sizes).GetInstance ());
        }

        public override void SetSizes (IVideo sizes) {
            _videoAdSize = sizes;
            Externs._setSizes (adLoader, ((DTBAdSize.DTBVideo) sizes).GetInstance ());
        }

        public override void SetSlotGroup (string slotGroupName) {
            _slotGroupName = slotGroupName;
            Externs._setSlotGroup (adLoader, slotGroupName);
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
    }
}