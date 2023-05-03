#import "AmazonManager.h"
#import "AmazonUnityCallback.h"
#import "DTBBannerDelegate.h"
#import "DTBInterstitialDelegate.h"


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Helpers

// Converts C style string to NSString
#define GetStringParam(_x_) ((_x_) != NULL ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""])
#define GetNullableStringParam(_x_) ((_x_) != NULL ? [NSString stringWithUTF8String:_x_] : nil)

static char* amazonStringCopy(NSString* input)
{
    const char* string = [input UTF8String];
    return string ? strdup(string) : NULL;
}

void _amazonInitialize(const char* appKey)
{
    [[AmazonManager sharedManager] initialize:GetStringParam(appKey)];
}

bool _amazonIsInitialized(){
    return [[AmazonManager sharedManager] isInitialized];
}

void _amazonSetUseGeoLocation(bool flag){
    [[AmazonManager sharedManager] setUseGeoLocation:flag];
}

bool _amazonGetUseGeoLocation(){
    return [[AmazonManager sharedManager] getUseGeoLocation];
}

void _amazonSetLogLevel(int logLevel){
    [[AmazonManager sharedManager] setLogLevel:logLevel];
}

bool _amazonGetLogLevel(){
    return [[AmazonManager sharedManager] getLogLevel];
}

void _amazonSetTestMode(bool flag){
    [[AmazonManager sharedManager] setTestMode:flag];
}

bool _amazonIsTestModeEnabled(){
    return [[AmazonManager sharedManager] isTestModeEnabled];
}

DTBAdSize* _createBannerAdSize(int width, int height, const char* uuid){
    return [[AmazonManager sharedManager] createBannerAdSize:width height:height uuid:GetStringParam(uuid)];
}
DTBAdSize* _createVideoAdSize(int width, int height, const char* uuid){
        return [[AmazonManager sharedManager] createVideoAdSize:width height:height uuid:GetStringParam(uuid)];
}

DTBAdSize* _createInterstitialAdSize(const char* uuid){
                return [[AmazonManager sharedManager] createInterstitialAdSize:GetStringParam(uuid)];
}
DTBAdLoader* _createAdLoader(){
    return [[AmazonManager sharedManager]createAdLoader];
}

void _setSizes(DTBAdLoader* adLoader, DTBAdSize* size){
    [[AmazonManager sharedManager]setSizes:adLoader size:size];
}

void _loadAd(DTBAdLoader* adLoader, AmazonUnityCallback* callback){
    [[AmazonManager sharedManager]loadAd:adLoader callback:callback];
}
void _loadSmartBanner(DTBAdLoader* adLoader, AmazonUnityCallback* callback){
    [[AmazonManager sharedManager]loadSmartBanner:adLoader callback:callback];
}
void _amazonSetMRAIDPolicy(int policy){
    [[AmazonManager sharedManager] setMRAIDPolicy:(DTBMRAIDPolicy)policy];
}

int _amazonGetMRAIDPolicy(){
    return [[AmazonManager sharedManager] getMRAIDPolicy];
}

void _amazonSetMRAIDSupportedVersions(const char* newVersion){
    [[AmazonManager sharedManager] setMRAIDSupportedVersions:GetStringParam(newVersion)];
}

void _amazonSetListeners(DTBAdCallbackClientRef* ptr, AmazonUnityCallback* callbackPtr, SuccessResponse onSuccessCallback, ErrorResponse onErrorCallback) {
    [callbackPtr setListeners:ptr success:onSuccessCallback errorCallback:onErrorCallback];
}

void _amazonSetListenersWithInfo(DTBAdCallbackClientRef* ptr, AmazonUnityCallback* callbackPtr, SuccessResponse onSuccessCallback, ErrorResponseWithInfo onErrorCallbackWithInfo) {
    [callbackPtr setListenersWithInfo:ptr success:onSuccessCallback errorCallbackWithInfo:onErrorCallbackWithInfo];
}

void _setBannerDelegate(DTBCallbackBannerRef* ptr, DTBBannerDelegate* callbackPtr, DTBAdDidLoadType adLoad, DTBAdFailedToLoadType adFailLoad, DTBBannerWillLeaveApplicationType leaveApp, DTBImpressionFiredType impFired) {
    [callbackPtr setDelegate:ptr adLoad:adLoad adFailLoad:adFailLoad leaveApp:leaveApp impFired:impFired];
}

void _setInterstitialDelegate(DTBCallbackInterstitialRef* ptr, DTBInterstitialDelegate* callbackPtr, DTBInterstitialDidLoadType adLoad, DTBDidFailToLoadAdWithErrorCodeType adFailLoad, DTBInterstitialWillLeaveApplicationType leaveApp, DTBInterstitialImpressionFiredType impFired, DTBInterstitialDidPresentScreenType didOpen, DTBInterstitialDidDismissScreenType didDismiss) {
    [callbackPtr setDelegate:ptr adLoad:adLoad adFailLoad:adFailLoad leaveApp:leaveApp impFired:impFired didOpen:didOpen didDismiss:didDismiss];
}

AmazonUnityCallback* _createCallback() {
    return [[AmazonManager sharedManager] createCallback];
}

DTBBannerDelegate* _createBannerDelegate() {
    return [[AmazonManager sharedManager] createBannerDelegate];
}

DTBInterstitialDelegate* _createInterstitialDelegate() {
    return [[AmazonManager sharedManager] createInterstitialDelegate];
}

DTBFetchManager* _getFetchManager(int autoRefreshID, bool isSmartBanner){
    return [[AmazonManager sharedManager] getFetchManager:autoRefreshID isSmartBanner:isSmartBanner];
}

void _fetchManagerPop(DTBFetchManager* fetchManager){
    [[AmazonManager sharedManager] fetchManagerPop:fetchManager];
}
 
void _putCustomTarget(DTBAdLoader* adLoader, const char* key, const char* value){
    [[AmazonManager sharedManager] putCustomTarget:adLoader key:GetStringParam(key) value:GetStringParam(value)];
}

void _createFetchManager(DTBAdLoader* adLoader, bool isSmartBanner){
        [[AmazonManager sharedManager] createFetchManager:adLoader isSmartBanner:isSmartBanner];
}

void _startFetchManager(DTBFetchManager* fetchManager){
    [[AmazonManager sharedManager] startFetchManager:fetchManager];
}

void _stopFetchManager(DTBFetchManager* fetchManager){
    [[AmazonManager sharedManager] stopFetchManager:fetchManager];
}

bool _isEmptyFetchManager(DTBFetchManager* fetchManager){
    return [[AmazonManager sharedManager] isEmptyFetchManager:fetchManager];
}

void _destroyFetchManager(int autoRefreshID){
    [[AmazonManager sharedManager] destroyFetchManager:autoRefreshID];
}

void _setSlotGroup(DTBAdLoader* adLoader, const char* slotGroupName){
    [[AmazonManager sharedManager] setSlotGroup:adLoader slotGtoupName:GetStringParam(slotGroupName)];
}

DTBSlotGroup* _createSlotGroup(const char* slotGroupName){
    return [[AmazonManager sharedManager] createSlotGroup:GetStringParam(slotGroupName)];
}

void _addSlot(DTBSlotGroup* slot, DTBAdSize* size){
    [[AmazonManager sharedManager] addSlot:slot size:size];
}

void _addSlotGroup(DTBSlotGroup* slot){
    [[AmazonManager sharedManager] addSlotGroup:slot];
}

const char* _fetchMoPubKeywords(DTBAdResponse* response){
    return amazonStringCopy([[AmazonManager sharedManager] fetchMoPubKeywords:response]);
}

const char* _fetchAmznSlots(DTBAdResponse* response){
    return amazonStringCopy([[AmazonManager sharedManager] fetchAmznSlots:response]);
}

int _fetchAdHeight(DTBAdResponse* response){
    return [[AmazonManager sharedManager] fetchAdHeight:response];
}

int _fetchAdWidth(DTBAdResponse* response){
    return [[AmazonManager sharedManager] fetchAdWidth:response];
}

const char* _fetchMediationHints(DTBAdResponse* response, bool isSmartBanner){
    NSString* str = [[AmazonManager sharedManager] fetchMediationHints:response isSmart:isSmartBanner];
    return amazonStringCopy(str);
}

void _setCMPFlavor (int cFlavor){
    [[AmazonManager sharedManager] setCMPFlavor:(DTBCMPFlavor)cFlavor];
}

void _setConsentStatus (int consentStatus){
    [[AmazonManager sharedManager] setConsentStatus:(DTBConsentStatus)consentStatus];
}

NSMutableArray* _createArray(){
    return [[AmazonManager sharedManager] createArray];
}

void _addToArray (NSMutableArray* dictionary, int item) {
    [[AmazonManager sharedManager] addToArray:dictionary item:item];
}

void _setVendorList(NSMutableArray* dictionary){
    [[AmazonManager sharedManager] setVendorList:dictionary];
}

void _setAutoRefreshNoArgs(DTBAdLoader* adLoader){
    [[AmazonManager sharedManager] setAutoRefresh:adLoader];
}

void _setAutoRefresh(DTBAdLoader* adLoader, int secs){
    [[AmazonManager sharedManager] setAutoRefresh:adLoader secs:secs];
}

void _pauseAutoRefresh(DTBAdLoader* adLoader){
    [[AmazonManager sharedManager] pauseAutorefresh:adLoader];
}

void _stopAutoRefresh(DTBAdLoader* adLoader){
    [[AmazonManager sharedManager] stopAutoRefresh:adLoader];
}

void _resumeAutoRefresh(DTBAdLoader* adLoader){
    [[AmazonManager sharedManager] resumeAutoRefresh:adLoader];
}

void _setAPSPublisherExtendedIdFeatureEnabled(bool isEnabled) {
  [[AmazonManager sharedManager] setAPSPublisherExtendedIdFeatureEnabled:isEnabled];
}

void _addCustomAttribute(const char *withKey, const void *value) {
  [[AmazonManager sharedManager] addCustomAttribute:GetStringParam(withKey) value:GetStringParam(value)];
}

void _removeCustomAttribute(const char* forKey) {
  [[AmazonManager sharedManager] removeCustomAttribute:GetStringParam(forKey)];
}

void _setAdNetworkInfo(int adNetworkId) {
    DTBAdNetworkInfo *dtbAdNetworkInfo = [[DTBAdNetworkInfo alloc]initWithNetworkName:(DTBAdNetwork)adNetworkId];
    [[AmazonManager sharedManager] setAdNetworkInfo:dtbAdNetworkInfo];
}

void _setLocalExtras(const char *adUnitId, NSDictionary *localExtras) {
    [DTBAds setLocalExtras:GetStringParam(adUnitId) localExtras:localExtras];
}

DTBAdBannerDispatcher* _createAdView(int width, int height, DTBBannerDelegate* delegate) {
    CGRect rect = CGRectMake(0.0f, 0.0f, (CGFloat)width, (CGFloat)height);
    return [[DTBAdBannerDispatcher alloc] initWithAdFrame:rect delegate:delegate];
}

DTBAdInterstitialDispatcher* _createAdInterstitial(DTBInterstitialDelegate* delegate) {
    return [[DTBAdInterstitialDispatcher alloc] initWithDelegate:delegate];
}

void _fetchBannerAd(DTBAdBannerDispatcher* dispatcher, DTBAdResponse* adResponse) {
    [dispatcher fetchBannerAdWithParameters:[adResponse mediationHints]];
}

void _fetchInterstitialAd(DTBAdInterstitialDispatcher* dispatcher, DTBAdResponse* adResponse) {
    [dispatcher fetchAdWithParameters:[adResponse mediationHints]];
}

void _showInterstitial(DTBAdInterstitialDispatcher* dispatcher) {
    [[AmazonManager sharedManager] showInterstitialAd:dispatcher];
}

NSDictionary* _getMediationHintsDict(DTBAdResponse* response, bool isSmartBanner){
    return [[AmazonManager sharedManager] getMediationHintsDict:response isSmart:isSmartBanner];
}

void _setRefreshFlag(DTBAdLoader* adLoader, bool flag) {
    [adLoader setRefreshFlag:flag];
}

DTBAdLoader* _getAdLoaderFromResponse(DTBAdResponse* adResponse) {
    return [adResponse getAdLoader];
}

DTBAdLoader* _getAdLoaderFromAdError(DTBAdErrorInfo* errorInfo) {
    return [errorInfo getAdLoader];
}

