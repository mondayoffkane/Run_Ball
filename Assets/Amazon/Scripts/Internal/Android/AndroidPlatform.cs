using System.Collections.Generic;
using System;

namespace AmazonAds.Android {
    public class AndroidPlatform : PlatformApi {
        public AdRegistration adRegistration;
        public override void Initialization (string key) {
            adRegistration = new AdRegistration ();
            adRegistration.GetInstance (key);
        }

        public override bool IsInitialized () {
            return adRegistration.IsInitialized ();
        }

        public override bool IsTestMode () {
            return adRegistration.IsTestMode ();
        }

        public override void EnableTesting (bool flag) {
            adRegistration.EnableTesting (flag);
        }

        public override void AddSlotGroup(SlotGroup group){
            adRegistration.AddSlotGroup (((DTBSlotGroup)group.GetInstance()).GetInstance());
        }

        public override void SetMRAIDPolicy (Amazon.MRAIDPolicy policy) {
            adRegistration.SetMRAIDPolicy (policy);
        }

        public override Amazon.MRAIDPolicy GetMRAIDPolicy () {
            return adRegistration.GetMRAIDPolicy ();
        }

        public override void SetMRAIDSupportedVersions (string[] versions) {
            adRegistration.SetMRAIDSupportedVersions (versions);
        }

        public override void UseGeoLocation (bool isLocationEnabled) {
            adRegistration.UseGeoLocation (isLocationEnabled);
        }

        public override bool IsLocationEnabled () {
            return adRegistration.IsLocationEnabled ();
        }

        public override void EnableLogging (bool flag) {
            adRegistration.EnableLogging (flag);
        }

        public override void SetCMPFlavor(Amazon.CMPFlavor cFlavor)
        {
            adRegistration.SetCMPFlavor(cFlavor);
        }

        public override void SetConsentStatus(Amazon.ConsentStatus consentStatus)
        {
            adRegistration.SetConsentStatus(consentStatus);
        }

        public override void SetVendorList(List<int> vendorList)
        {
            adRegistration.SetVendorList(vendorList);
        }

        public override void AddCustomAttribute(string withKey, string value)
        {
            adRegistration.AddCustomAttribute(withKey, value);
        }

        public override void RemoveCustomAttr(string forKey)
        {
            adRegistration.RemoveCustomAttr(forKey);
        }

        public override void SetAdNetworkInfo(AdNetworkInfo adNetworkInfo) 
        {
            adRegistration.SetAdNetworkInfo(adNetworkInfo.getAdNetworkName());
        }

#if UNITY_IOS
        public override void SetAPSPublisherExtendedIdFeatureEnabled(bool isEnabled)
        {
            throw new System.NotImplementedException();
        }

        public override void SetLocalExtras(string adUnitId, AmazonAds.AdResponse adResponse)
        {
            throw new System.NotImplementedException();
        }
#endif
    }
}