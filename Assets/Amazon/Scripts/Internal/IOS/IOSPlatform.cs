using System.Collections.Generic;
using AOT;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace AmazonAds.IOS {
    public class IOSPlatform : PlatformApi {

        public override void Initialization (string key) {
            Externs._amazonInitialize (key);
        }

        public override bool IsInitialized () {
            return Externs._amazonIsInitialized ();
        }

        public override bool IsTestMode () {
            return Externs._amazonIsTestModeEnabled ();
        }

        public override void EnableTesting (bool flag) {
            Externs._amazonSetTestMode (flag);
        }
        public override void EnableLogging (bool flag) {
            Externs._amazonSetLogLevel (flag ? 1 : 0);
        }

        public override void UseGeoLocation (bool isLocationEnabled) {
            Externs._amazonSetUseGeoLocation (isLocationEnabled);
        }

        public override bool IsLocationEnabled () {
            return Externs._amazonGetUseGeoLocation ();
        }

        public override void SetMRAIDPolicy (Amazon.MRAIDPolicy policy) {
            Externs._amazonSetMRAIDPolicy ((int) policy);
        }

        public override Amazon.MRAIDPolicy GetMRAIDPolicy () {
            return (Amazon.MRAIDPolicy) Externs._amazonGetMRAIDPolicy ();
        }

        public override void SetMRAIDSupportedVersions (string[] versions) {
            Externs._amazonSetMRAIDSupportedVersions (versions.ToString());
        }

        public override void AddSlotGroup(SlotGroup group)
        {
            DTBSlotGroup ptr = (DTBSlotGroup)group.GetInstance();
            Externs._addSlotGroup(ptr.GetInstance());
        }

        public override void SetCMPFlavor(Amazon.CMPFlavor cFlavor)
        {
            int fla = 0;
            switch (cFlavor)
            {
                case Amazon.CMPFlavor.CMP_NOT_DEFINED:
                fla = -1;
                break;
                case Amazon.CMPFlavor.GOOGLE_CMP:
                fla = 1;
                break;
                case Amazon.CMPFlavor.MOPUB_CMP:
                fla = 2;
                break;
            }
            Externs._setCMPFlavor(fla);
        }

        public override void SetConsentStatus(Amazon.ConsentStatus consentStatus)
        {
            int cons = 0;
            switch (consentStatus)
            {
                case Amazon.ConsentStatus.CONSENT_NOT_DEFINED:
                cons = -1;
                break;
                case Amazon.ConsentStatus.EXPLICIT_YES:
                cons = 7;
                break;
                case Amazon.ConsentStatus.EXPLICIT_NO:
                cons = -0;
                break;
                case Amazon.ConsentStatus.UNKNOWN:
                cons = 1;
                break;
            }
            Externs._setConsentStatus(cons);
        }

        public override void SetVendorList(List<int> vendorList)
        {
            IntPtr dictionary = Externs._createArray();
            foreach (var item in vendorList)
            {
                Externs._addToArray(dictionary, item);
            }
            Externs._setVendorList(dictionary);
        }

        public override void AddCustomAttribute(string withKey, string value)
        {
            Externs._addCustomAttribute(withKey, value);
        }

        public override void RemoveCustomAttr(string forKey)
        {
            Externs._removeCustomAttribute(forKey);
        }

        public override void SetAdNetworkInfo(AdNetworkInfo adNetworkInfo) 
        {
            DTBAdNetwork dtbAdNetwork = adNetworkInfo.getAdNetwork();
            Externs._setAdNetworkInfo((int)dtbAdNetwork);
        }

#if UNITY_IOS
        public override void SetAPSPublisherExtendedIdFeatureEnabled(bool isEnabled)
        {
            Externs._setAPSPublisherExtendedIdFeatureEnabled(isEnabled);
        }

        public override void SetLocalExtras(string adUnitId, AmazonAds.AdResponse adResponse) {
            Externs._setLocalExtras(adUnitId, Externs._getMediationHintsDict(adResponse.GetInstance(), false));
        }
#endif
    }
}