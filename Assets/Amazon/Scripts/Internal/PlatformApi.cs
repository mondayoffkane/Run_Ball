using System;
using System.Collections.Generic;

namespace AmazonAds {
    public abstract class PlatformApi {
        public abstract void Initialization (string key);
        public abstract bool IsInitialized ();
        public abstract bool IsTestMode ();
        public abstract void EnableTesting (bool flag);
        public abstract void EnableLogging (bool flag);
        public abstract void UseGeoLocation (bool isLocationEnabled);
        public abstract bool IsLocationEnabled ();
        public abstract void SetMRAIDPolicy (Amazon.MRAIDPolicy policy);
        public abstract Amazon.MRAIDPolicy GetMRAIDPolicy ();
        public abstract void SetMRAIDSupportedVersions (String[] versions);
        public abstract void AddSlotGroup(SlotGroup group);
        public abstract void SetCMPFlavor(Amazon.CMPFlavor cFlavor);
        public abstract void SetConsentStatus(Amazon.ConsentStatus consentStatus);
        public abstract void SetVendorList(List<int> vendorList);
        public abstract void AddCustomAttribute(string withKey, string value);
        public abstract void RemoveCustomAttr(String forKey);
        public abstract void SetAdNetworkInfo(AdNetworkInfo adNetworkInfo);
#if UNITY_IOS
        public abstract void SetAPSPublisherExtendedIdFeatureEnabled(bool isEnable);
        public abstract void SetLocalExtras(string adUnitId, AmazonAds.AdResponse adResponse);
#endif
    }
}