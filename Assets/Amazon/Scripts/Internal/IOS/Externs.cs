using System.Runtime.InteropServices;
using System;

namespace AmazonAds.IOS
{
    public class Externs
    {
#if ENABLE_IL2CPP && UNITY_ANDROID
        public static void _amazonInitialize(string appKey) {} 
        public static bool _amazonIsInitialized() { return false; }
        public static void _amazonSetUseGeoLocation(bool flag) {}
        public static bool _amazonGetUseGeoLocation() { return false; }
        public static void _amazonSetLogLevel(int logLevel) {}
        public static bool _amazonGetLogLevel() { return false; }
        public static void _amazonSetTestMode(bool flag) {}
        public static bool _amazonIsTestModeEnabled() { return false; }
        public static IntPtr _createBannerAdSize(int width, int height, string uuid) { return IntPtr.Zero; }
        public static IntPtr _createVideoAdSize(int width, int height, string uuid){ return IntPtr.Zero; }
        public static IntPtr _createInterstitialAdSize(string uuid) { return IntPtr.Zero; }
        public static IntPtr _createAdLoader() { return IntPtr.Zero; }
        public static void _setSizes(IntPtr adLoader, IntPtr size) {}
        public static void _loadAd(IntPtr adLoader, IntPtr callback) {}
        public static void _loadSmartBanner(IntPtr adLoader, IntPtr callback) {}
        public static void _amazonSetListeners(IntPtr unityRef, IntPtr callback, DTBCallback.OnSuccessDelegate onSuccessCallback, DTBCallback.OnFailureDelegate onErrorCallback) {}
        public static void _amazonSetListenersWithInfo(IntPtr unityRef, IntPtr callback, DTBCallback.OnSuccessDelegate onSuccessCallback, DTBCallback.OnFailureWithErrorDelegate onErrorCallbackWithInfo) {}
        public static void _setBannerDelegate(IntPtr unityRef, IntPtr callback, DTBAdBannerDispatcher.OnAdLoadedDelegate onAdLoadedDelegate, DTBAdBannerDispatcher.OnAdFailedDelegate onAdFailedDelegate, 
        DTBAdBannerDispatcher.OnAdClickedDelegate onAdClickedDelegate, DTBAdBannerDispatcher.OnImpressionFiredDelegate onImpressionFiredDelegate) {}
        public static void _setInterstitialDelegate(IntPtr unityRef, IntPtr callback, DTBAdInterstitialDispatcher.OnAdLoadedDelegate onAdLoadedDelegate, DTBAdInterstitialDispatcher.OnAdFailedDelegate onAdFailedDelegate, 
        DTBAdInterstitialDispatcher.OnAdClickedDelegate onAdClickedDelegate, DTBAdInterstitialDispatcher.OnImpressionFiredDelegate onImpressionFiredDelegate, DTBAdInterstitialDispatcher.OnAdOpenDelegate onAdOpenDelegate, DTBAdInterstitialDispatcher.OnAdClosedDelegate onAdClosedDelegate) {}
        public static void _amazonSetMRAIDPolicy(int policy) {}
        public static int _amazonGetMRAIDPolicy() {return -1;}
        public static void _amazonSetMRAIDSupportedVersions(string versions) {}
        public static IntPtr _createCallback() { return IntPtr.Zero; }
        public static IntPtr _createBannerDelegate() { return IntPtr.Zero; }
        public static IntPtr _createInterstitialDelegate() { return IntPtr.Zero; }
        public static IntPtr _getFetchManager(int autoRefreshID, bool isSmartBanner) { return IntPtr.Zero; }
        public static void _fetchManagerPop(IntPtr fetchManager) {}
        public static void _putCustomTarget(IntPtr adLoader, string key, string value) {}
        public static void _createFetchManager(IntPtr adLoader, bool isSmartBanner) {}
        public static void _startFetchManager(IntPtr fetchManager) {}
        public static void _stopFetchManager(IntPtr fetchManager) {}
        public static bool _isEmptyFetchManager(IntPtr fetchManager) {return false; }
        public static void _destroyFetchManager(int autoRefreshID) { }
        public static void _setSlotGroup(IntPtr adLoader, string slotGroupName) {}
        public static IntPtr _createSlotGroup(string slotGroupName) { return IntPtr.Zero; }
        public static void _addSlot(IntPtr slot, IntPtr size) {}
        public static void _addSlotGroup(IntPtr group) {}
        public static string _fetchMediationHints(IntPtr resp, bool isSmartBanner) { return null; }       
        public static string _fetchAmznSlots(IntPtr resp) { return null; }
        public static string _fetchMoPubKeywords(IntPtr resp) { return null; }
        public static void _setCMPFlavor(int cFlavor) {}
        public static void _setConsentStatus(int consentStatus) {}
        public static IntPtr _createArray() { return IntPtr.Zero; }
        public static void _addToArray(IntPtr dictionary, int item) {}
        public static void _setVendorList(IntPtr dictionary) {}
        public static void _setAutoRefreshNoArgs(IntPtr adLoader) {}
        public static void _setAutoRefresh(IntPtr adLoader, int secs) {}
        public static void _pauseAutoRefresh(IntPtr adLoader) {}
        public static void _stopAutoRefresh(IntPtr adLoader) {}
        public static void _resumeAutoRefresh(IntPtr adLoader) {}
        public static void _setAPSFrequencyCappingIdFeatureEnabled(bool frequencyCappingIdFeatureEnabled) {}
        public static void _addCustomAttribute(string withKey, string value) {}
        public static void _removeCustomAttribute(string forKey) {}
        public static void _setAdNetworkInfo(int adNetwork) {}
        public static void _setLocalExtras(string adUnitId, IntPtr localExtras) {}
        public static IntPtr _createAdView(int width, int height, IntPtr dispatcher) { return IntPtr.Zero; }
        public static IntPtr _createAdInterstitial(IntPtr dispatcher) { return IntPtr.Zero; }
        public static void _fetchBannerAd(IntPtr adDispatcher, IntPtr adResponse) {}
        public static void _fetchInterstitialAd(IntPtr adDispatcher, IntPtr adResponse) {}
        public static void _showInterstitial(IntPtr adDispatcher) {}
        public static void _setRefreshFlag(IntPtr adLoader, bool flag) {}
        public static IntPtr _getAdLoaderFromResponse(IntPtr response) { return IntPtr.Zero; }
        public static IntPtr _getAdLoaderFromAdError(IntPtr adErrorInfo) { return IntPtr.Zero; }
        public static int _fetchAdWidth(IntPtr resp) { return -1; }
        public static int _fetchAdHeight(IntPtr resp) { return -1; }
#else
        [DllImport("__Internal")]
        public static extern void _amazonInitialize(string appKey);
        [DllImport("__Internal")]
        public static extern bool _amazonIsInitialized();
        [DllImport("__Internal")]
        public static extern void _amazonSetUseGeoLocation(bool flag);
        [DllImport("__Internal")]
        public static extern bool _amazonGetUseGeoLocation();
        [DllImport("__Internal")]
        public static extern void _amazonSetLogLevel(int logLevel);
        [DllImport("__Internal")]
        public static extern bool _amazonGetLogLevel();
        [DllImport("__Internal")]
        public static extern void _amazonSetTestMode(bool flag);
        [DllImport("__Internal")]
        public static extern bool _amazonIsTestModeEnabled();
        [DllImport("__Internal")]
        public static extern IntPtr _createBannerAdSize(int width, int height, string uuid);
        [DllImport("__Internal")]
        public static extern IntPtr _createVideoAdSize(int width, int height, string uuid);
        [DllImport("__Internal")]
        public static extern IntPtr _createInterstitialAdSize(string uuid);
        [DllImport("__Internal")]
        public static extern IntPtr _createAdLoader();
        [DllImport("__Internal")]
        public static extern void _setSizes(IntPtr adLoader, IntPtr size);
        [DllImport("__Internal")]
        public static extern void _loadAd(IntPtr adLoader, IntPtr callback);
        [DllImport("__Internal")]
        public static extern void _loadSmartBanner(IntPtr adLoader, IntPtr callback);
        [DllImport("__Internal")]
        public static extern void _amazonSetListeners(IntPtr unityRef, IntPtr callback, DTBCallback.OnSuccessDelegate onSuccessCallback, DTBCallback.OnFailureDelegate onErrorCallback);
        [DllImport("__Internal")]
        public static extern void _amazonSetListenersWithInfo(IntPtr unityRef, IntPtr callback, DTBCallback.OnSuccessDelegate onSuccessCallback, DTBCallback.OnFailureWithErrorDelegate onErrorCallbackWithInfo);
        [DllImport("__Internal")]
        public static extern void _setBannerDelegate(IntPtr unityRef, IntPtr callback, 
        DTBAdBannerDispatcher.OnAdLoadedDelegate onAdLoadedDelegate, DTBAdBannerDispatcher.OnAdFailedDelegate onAdFailedDelegate, 
        DTBAdBannerDispatcher.OnAdClickedDelegate onAdClickedDelegate, DTBAdBannerDispatcher.OnImpressionFiredDelegate onImpressionFiredDelegate);
        [DllImport("__Internal")]
        public static extern void _setInterstitialDelegate(IntPtr unityRef, IntPtr callback, 
        DTBAdInterstitialDispatcher.OnAdLoadedDelegate onAdLoadedDelegate, DTBAdInterstitialDispatcher.OnAdFailedDelegate onAdFailedDelegate, 
        DTBAdInterstitialDispatcher.OnAdClickedDelegate onAdClickedDelegate, DTBAdInterstitialDispatcher.OnImpressionFiredDelegate onImpressionFiredDelegate, 
        DTBAdInterstitialDispatcher.OnAdOpenDelegate onAdOpenDelegate, DTBAdInterstitialDispatcher.OnAdClosedDelegate onAdClosedDelegate);
        [DllImport("__Internal")]
        public static extern void _amazonSetMRAIDPolicy(int policy);
        [DllImport("__Internal")]
        public static extern int _amazonGetMRAIDPolicy();
        [DllImport("__Internal")]
        public static extern void _amazonSetMRAIDSupportedVersions(string versions);
        [DllImport("__Internal")]
        public static extern IntPtr _createCallback();
        [DllImport("__Internal")]
        public static extern IntPtr _createBannerDelegate();
        [DllImport("__Internal")]
        public static extern IntPtr _createInterstitialDelegate();
        [DllImport("__Internal")]
        public static extern IntPtr _getFetchManager(int autoRefreshID, bool isSmartBanner);
        [DllImport("__Internal")]
        public static extern void _fetchManagerPop(IntPtr fetchManager);
        [DllImport("__Internal")]
        public static extern void _putCustomTarget(IntPtr adLoader, string key, string value);
        [DllImport("__Internal")]
        public static extern void _createFetchManager(IntPtr adLoader, bool isSmartBanner);
        [DllImport("__Internal")]
        public static extern void _startFetchManager(IntPtr fetchManager);
        [DllImport("__Internal")]
        public static extern void _stopFetchManager(IntPtr fetchManager);
        [DllImport("__Internal")]
        public static extern bool _isEmptyFetchManager(IntPtr fetchManager);
        [DllImport("__Internal")]
        public static extern void _destroyFetchManager(int autoRefreshID);
        [DllImport("__Internal")]
        public static extern void _setSlotGroup(IntPtr adLoader, string slotGroupName);
        [DllImport("__Internal")]
        public static extern IntPtr _createSlotGroup(string slotGroupName);
        [DllImport("__Internal")]
        public static extern void _addSlot(IntPtr slot, IntPtr size);
        [DllImport("__Internal")]
        public static extern void _addSlotGroup(IntPtr group);
        [DllImport("__Internal")]
        public static extern string _fetchMediationHints(IntPtr resp, bool isSmartBanner);        
        [DllImport("__Internal")]
        public static extern string _fetchAmznSlots(IntPtr resp);
        [DllImport("__Internal")]
        public static extern int _fetchAdWidth(IntPtr resp);
        [DllImport("__Internal")]
        public static extern int _fetchAdHeight(IntPtr resp);
        [DllImport("__Internal")]
        public static extern void _setCMPFlavor(int cFlavor);
        [DllImport("__Internal")]
        public static extern void _setConsentStatus(int consentStatus);
        [DllImport("__Internal")]
        public static extern IntPtr _createArray();
        [DllImport("__Internal")]
        public static extern void _addToArray(IntPtr dictionary, int item);
        [DllImport("__Internal")]
        public static extern void _setVendorList(IntPtr dictionary);
        [DllImport("__Internal")]
        public static extern void _setAutoRefreshNoArgs(IntPtr adLoader);
        [DllImport("__Internal")]
        public static extern void _setAutoRefresh(IntPtr adLoader, int secs);
        [DllImport("__Internal")]
        public static extern void _pauseAutoRefresh(IntPtr adLoader);
        [DllImport("__Internal")]
        public static extern void _stopAutoRefresh(IntPtr adLoader);
        [DllImport("__Internal")]
        public static extern void _resumeAutoRefresh(IntPtr adLoader);
        [DllImport("__Internal")]
        public static extern void _addCustomAttribute(string withKey, string value);
        [DllImport("__Internal")]
        public static extern void _removeCustomAttribute(string forKey);
        [DllImport("__Internal")]
        public static extern void _setAdNetworkInfo(int adNetwork);
        [DllImport("__Internal")]
        public static extern IntPtr _createAdView(int width, int height, IntPtr dispatcher);
        [DllImport("__Internal")]
        public static extern IntPtr _createAdInterstitial(IntPtr dispatcher);
        [DllImport("__Internal")]
        public static extern void _fetchBannerAd(IntPtr adDispatcher, IntPtr adResponse);
        [DllImport("__Internal")]
        public static extern void _fetchInterstitialAd(IntPtr adDispatcher, IntPtr adResponse);
        [DllImport("__Internal")]
        public static extern void _showInterstitial(IntPtr adDispatcher);
        [DllImport("__Internal")]
        public static extern void _setRefreshFlag(IntPtr adLoader, bool flag);
        [DllImport("__Internal")]
        public static extern IntPtr _getAdLoaderFromResponse(IntPtr response);
        [DllImport("__Internal")]
        public static extern IntPtr _getAdLoaderFromAdError(IntPtr adErrorInfo);
#if UNITY_IOS
        [DllImport("__Internal")]
        public static extern void _setAPSPublisherExtendedIdFeatureEnabled(bool isEnabled);
        [DllImport("__Internal")]
        public static extern void _setLocalExtras(string adUnitId, IntPtr localExtras);
        [DllImport("__Internal")]
        public static extern IntPtr _getMediationHintsDict(IntPtr resp, bool isSmartBanner);
#endif
#endif
    }
}
