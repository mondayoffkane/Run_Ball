using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
namespace AmazonAds.IOS {
    public class DTBFetchManager: IFetchManager {
        private IntPtr fetchManager;
        protected enum DTBSlotType {
            SLOT_320_50 = 0,
            SLOT_300_250 = 1,
            SLOT_728_90 = 2,
            SLOT_SMART = 3
        }
        public DTBFetchManager (DTBAdRequest adLoader, string autoRefreshID, bool isSmartBanner) { 
            Externs._createFetchManager (adLoader.GetInstance(), isSmartBanner);
            fetchManager = Externs._getFetchManager ( (int) Enum.Parse(typeof(DTBSlotType), autoRefreshID), isSmartBanner);
        }

        public void dispense () {
            Externs._fetchManagerPop (fetchManager);
        }

        public void start () {
            Externs._startFetchManager (fetchManager);
        }

        public void stop () {
            Externs._stopFetchManager (fetchManager);
        }

        public bool isEmpty () {
            return Externs._isEmptyFetchManager(fetchManager);
        }

        public AmazonAds.AdResponse peek () {
            return new IOSAdResponse();
        }

        public void destroy(string autoRefreshID){
            Externs._destroyFetchManager ((int) Enum.Parse(typeof(DTBSlotType), autoRefreshID) );
            fetchManager = IntPtr.Zero;
        }
    }
}