using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds.Android {
    public class AdRegistration {
        private static readonly AndroidJavaClass adRegistrationClass = new AndroidJavaClass ("com.amazon.device.ads.AdRegistration");
        private AndroidJavaObject adRegistration = null;



        public void AddProvider (AndroidJavaObject provider) { // AdProvider params
            adRegistration.CallStatic ("AddProvider", provider);
        }

        public AndroidJavaObject GetEventDistributer () { // return EventDistributor
            return adRegistration.CallStatic<AndroidJavaObject> ("getEventDistributer");
        }

        public HashSet<AndroidJavaObject> GtAdProviders () { // return AdProvider
            return adRegistration.CallStatic<HashSet<AndroidJavaObject>> ("getProprietaryProviderKeys");
        }

        public List<String> GetProprietaryProviderKeys () {
            return adRegistration.CallStatic<List<String>> ("getProprietaryProviderKeys");
        }

        public String GetAppKey () {
            return adRegistration.CallStatic<String> ("getAppKey");
        }

        public bool IsInitialized () {
            return adRegistration.CallStatic<bool> ("isInitialized");
        }

        public AndroidJavaObject GetInstance (String appKey) {
            AndroidJavaClass playerClass = new AndroidJavaClass (AmazonConstants.unityPlayerClass);
            AndroidJavaObject currentActivityObject = playerClass.GetStatic<AndroidJavaObject> ("currentActivity");
            object[] adRegParams = new object[2];
            adRegParams[0] = appKey;
            adRegParams[1] = currentActivityObject;
            adRegistration = adRegistrationClass.CallStatic<AndroidJavaObject>
                ("getInstance", adRegParams);

            return adRegistration;
        }

        public void SetMRAIDPolicy (Amazon.MRAIDPolicy policy) {
            AndroidJavaClass mraidEnum = new AndroidJavaClass ("com.amazon.device.ads.MRAIDPolicy");
            AndroidJavaObject curMraid = mraidEnum.CallStatic<AndroidJavaObject> ("valueOf", policy.ToString ());
            adRegistration.CallStatic ("setMRAIDPolicy", curMraid);
        }

        public Amazon.MRAIDPolicy GetMRAIDPolicy () { // returns MRAIDPolicy
            AndroidJavaObject mraid = adRegistration.CallStatic<AndroidJavaObject> ("getMRAIDPolicy");
            int codeInt = mraid.Call<int> ("ordinal");
            Amazon.MRAIDPolicy code = (Amazon.MRAIDPolicy) codeInt;
            return code;
        }

        public void SetMRAIDSupportedVersions (String[] versions) {
            adRegistration.CallStatic ("setMRAIDSupportedVersions", versions);
        }

        public void EnableLogging (bool enable) {
            adRegistration.CallStatic ("enableLogging", enable);
        }

        public void EnableLogging (bool enable, AndroidJavaObject logLevel) { // DTBLogLevel logLevel
            adRegistration.CallStatic ("enableLogging", enable, logLevel);
        }

        public void EnableTesting (bool isTesting) {
            adRegistration.CallStatic ("enableTesting", isTesting);
        }

        public bool IsTestMode () {
            return adRegistration.CallStatic<bool> ("isTestMode");
        }

        public void UseGeoLocation (bool isLocationEnabled) {
            adRegistration.CallStatic ("useGeoLocation", isLocationEnabled);
        }

        public bool IsLocationEnabled () {
            return adRegistration.CallStatic<bool> ("isLocationEnabled");
        }

        public String GetVersion () {
            return adRegistration.CallStatic<String> ("getVersion");
        }

        public void SetServerlessMarkers (String[] markers) {
            adRegistration.CallStatic ("setServerlessMarkers", markers);

        }
        public void AddSlotGroup (SlotGroup sg) { // param SlotGroup sg
            adRegistration.CallStatic ("addSlotGroup", sg.dTBSlot);
        }

        public AndroidJavaObject GetSlotGroup (String name) { // return SlotGroup
            return adRegistration.CallStatic<AndroidJavaObject> ("getSlotGroup", name);
        }

        public bool IsConsentStatusUnknown () {
            return adRegistration.CallStatic<bool> ("isConsentStatusUnknown");
        }

        public void SetVendorList (List<int> vendorList) {
            adRegistration.CallStatic ("setVendorList", vendorList);
        }

        public void SetConsentStatus (Amazon.ConsentStatus consentStatus) {
            AndroidJavaClass consEnum = new AndroidJavaClass ("com.amazon.device.ads.AdRegistration$ConsentStatus");
            AndroidJavaObject curcons = consEnum.CallStatic<AndroidJavaObject> ("valueOf", consentStatus.ToString ());
            adRegistration.CallStatic ("setConsentStatus", curcons);
        }

        public void SetCMPFlavor (Amazon.CMPFlavor cFlavor) {
            AndroidJavaClass flavEnum = new AndroidJavaClass ("com.amazon.device.ads.AdRegistration$CMPFlavor");
            AndroidJavaObject curFlav = flavEnum.CallStatic<AndroidJavaObject> ("valueOf", cFlavor.ToString ());
            adRegistration.CallStatic ("setCMPFlavor", curFlav);
        }

        public void ResetNonIAB () {
            adRegistration.CallStatic ("resetNonIAB");
        }

        public class SlotGroup {
            private readonly AndroidJavaClass slotGroup = new AndroidJavaClass ("com.amazon.device.ads.AdRegistration$SlotGroup");
            public AndroidJavaObject dTBSlot = null;

            public SlotGroup () {
                dTBSlot = new AndroidJavaObject ("com.amazon.device.ads.AdRegistration$SlotGroup");
            }

            public SlotGroup (string name) {
                dTBSlot = new AndroidJavaObject ("com.amazon.device.ads.AdRegistration$SlotGroup", name);
            }

            public void AddSlot (IAdSize size) {
                dTBSlot.Call ("addSlot", ((DTBAdSize) size).GetInstance ());
            }
        } 

        public void AddCustomAttribute (string withKey, string value) {
            adRegistration.CallStatic ("addCustomAttribute", withKey, value);
        }

        public void RemoveCustomAttr (string forKey) {
            adRegistration.CallStatic ("removeCustomAttribute", forKey);
        }

        public void SetAdNetworkInfo (string adNetworkName) {
            AndroidJavaClass adNetworkEnum = new AndroidJavaClass (AmazonConstants.dtbAdNetworkClass);
            AndroidJavaObject adNetworkObj = adNetworkEnum.CallStatic<AndroidJavaObject> ("valueOf", adNetworkName);
            AndroidJavaObject dtbAdNetworkInfo = new AndroidJavaObject (AmazonConstants.dtbAdNetworkInfoClass, adNetworkObj);
            adRegistration.CallStatic ("setAdNetworkInfo", dtbAdNetworkInfo);
        }
    }
}