using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds {
    public class Amazon {
        private static PlatformApi api;

        public delegate void OnFailureDelegate (string errorMsg);
        public delegate void OnFailureWithErrorDelegate (AdError adError);
        public delegate void OnSuccessDelegate (AdResponse response);
        public delegate void OnApplicationPauseDelegate(bool pauseStatus);
        public static OnApplicationPauseDelegate OnApplicationPause = (pauseStatus) => { };

        public enum MRAIDPolicy {
            AUTO_DETECT,
            MOPUB,
            DFP,
            CUSTOM,
            NONE
        }
        public enum ConsentStatus {
            CONSENT_NOT_DEFINED,
            EXPLICIT_YES,
            EXPLICIT_NO,
            UNKNOWN
        }

        public enum CMPFlavor {
            CMP_NOT_DEFINED,
            GOOGLE_CMP,
            MOPUB_CMP
        }

        public static void Initialize (string key) {
#if UNITY_IOS
            api = new AmazonAds.IOS.IOSPlatform ();
#elif UNITY_ANDROID
            api = new AmazonAds.Android.AndroidPlatform ();
#endif
            api.Initialization (key);
        }

        public static void SetMRAIDPolicy (Amazon.MRAIDPolicy policy) {
            api.SetMRAIDPolicy (policy);
        }

        public static void SetCMPFlavor(Amazon.CMPFlavor cFlavor){
            api.SetCMPFlavor(cFlavor);
        }

        public static void SetConsentStatus(Amazon.ConsentStatus consentStatus){
            api.SetConsentStatus(consentStatus);
        }

        public static void SetVendorList(List<int> vendorList){
            api.SetVendorList(vendorList);
        }

        public static void AddSlotGroup (SlotGroup group) {
            api.AddSlotGroup (group);
        }

        public static MRAIDPolicy GetMRAIDPolicy () {
            return api.GetMRAIDPolicy ();
        }

        public static void SetMRAIDSupportedVersions (String[] versions) {
            api.SetMRAIDSupportedVersions (versions);
        }

        public static void EnableLogging (bool flag) {
            api.EnableLogging (flag);
        }

        public static void UseGeoLocation (bool isLocationEnabled) {
            api.UseGeoLocation (isLocationEnabled);
        }

        public static bool IsLocationEnabled () {
            return api.IsLocationEnabled ();
        }

        public static bool IsInitialized () {
            return api.IsInitialized ();
        }

        public static bool IsTestMode () {
            return api.IsTestMode ();
        }

        public static void EnableTesting (bool flag) {
            api.EnableTesting (flag);
        }

        public static void AddCustomAttribute(string withKey, string value)
        {
            api.AddCustomAttribute(withKey, value);
        }

        public static void RemoveCustomAttribute(string forKey)
        {
            api.RemoveCustomAttr(forKey);
        }

        public static void SetAdNetworkInfo(AdNetworkInfo adNetworkInfo)
        {
            api.SetAdNetworkInfo(adNetworkInfo);
        }

#if UNITY_IOS
        public static void SetAPSPublisherExtendedIdFeatureEnabled(bool isEnabled)
        {
            api.SetAPSPublisherExtendedIdFeatureEnabled(isEnabled);
        }

        public static void SetMediationLocalExtras(string adUnitId, AdResponse adResponse)
        {
            api.SetLocalExtras(adUnitId, adResponse);
        }
#endif
    }
}