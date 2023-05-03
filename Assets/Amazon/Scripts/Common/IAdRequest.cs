using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
namespace AmazonAds {
    public abstract class IAdRequest {
        protected int refreshTime = 60;
        protected string autoRefreshID = "0"; //FetchManageerUniqueID
        protected bool isAutoRefreshAdMob = false;
        protected bool isAutoRefreshMoPub = false;
        protected bool requestHasBeenUsed = false;
        protected IFetchManager fetchManager;

        public abstract void PutCustomTarget (string key, string value);
        public abstract void SetSizes (IAdSize sizes);
        public abstract void SetSizes (IInterstitialAdSize sizes);
        public abstract void SetSizes (IVideo sizes);
        public abstract void SetSlotGroup (string slotGroupName);
        public abstract void LoadAd (Amazon.OnFailureDelegate failure, Amazon.OnSuccessDelegate success);
        public abstract void LoadAd (Amazon.OnFailureWithErrorDelegate failure, Amazon.OnSuccessDelegate success);
        public abstract void LoadSmartBanner (Amazon.OnFailureDelegate failure, Amazon.OnSuccessDelegate success);
        public abstract void LoadSmartBanner (Amazon.OnFailureWithErrorDelegate failure, Amazon.OnSuccessDelegate success);
        public abstract void SetAutoRefreshAdMob (bool flag, bool isSmartBanner = false);
        public abstract void SetAutoRefreshMoPub (bool flag);
        public abstract void SetAutoRefreshMoPub (bool flag, int refreshTime);
        public abstract void SetAutoRefresh();
        public abstract void SetAutoRefresh(int secs);
        public abstract void ResumeAutoRefresh();
        public abstract void StopAutoRefresh();
        public abstract void PauseAutoRefresh();
        public bool IsAutoRefreshAdMob (){ return isAutoRefreshAdMob;}
        public bool IsAutoRefreshMoPub (){ return isAutoRefreshMoPub;}
        public string AutoRefreshID (){return autoRefreshID.ToString ();}
        public abstract void DisposeAd ();
        public abstract void CreateFetchManager (bool isSmartBanner = false);
        public abstract void DestroyFetchManager ();
        public abstract void StopFetchManager();
        public abstract void StartFetchManager();
        public abstract void SetRefreshFlag(bool flag);

        protected static class Schedule {
            private class Runner : MonoBehaviour { }
            private static Runner _backer;
            private static Runner Backer {
                get {
                    if (_backer == null) {
                        var go = new GameObject ("Scheduler");
                        GameObject.DontDestroyOnLoad (go);
                        _backer = go.AddComponent<Runner> ();
                    }
                    return _backer;
                }
            }
            private static float expiration = 5f;

            public static void WaitForAdResponce (IFetchManager fetchManager, Amazon.OnFailureDelegate failure, Amazon.OnSuccessDelegate success) {
                Schedule.Backer.StartCoroutine (WaitForAdResponceCoroutine (fetchManager, failure, success));
            }

            static IEnumerator WaitForAdResponceCoroutine (IFetchManager fetchManager, Amazon.OnFailureDelegate failure, Amazon.OnSuccessDelegate success) {
                float timerExp = 0;
                bool flagResp = true;
                while (fetchManager.isEmpty()) {
                    timerExp += Time.deltaTime;
                    if (timerExp > expiration) {
                        flagResp = false;
                        failure ("no ads from fetchManager");
                        break;
                    }
                    yield return null;
                }

                if (flagResp) {
                    success( fetchManager.peek () );
                }
                Schedule.Clear ();
            }

            public static void Clear () {
                GameObject.Destroy (Backer);
            }
        }
    }
}