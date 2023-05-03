#import <PlayOnSDK/PlayOnSDK.h>
#import "PlayOnAdListener.h"
#import "PlayOnManagerListener.h"
///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Helpers

// Converts C style string to NSString
#define GetStringParam(_x_) ((_x_) != NULL ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""])
#define GetNullableStringParam(_x_) ((_x_) != NULL ? [NSString stringWithUTF8String:_x_] : nil)
#define UIColorFromRGB(rgbValue) [UIColor colorWithRed:((float)((rgbValue & 0xFF0000) >> 16))/255.0 green:((float)((rgbValue & 0xFF00) >> 8))/255.0 blue:((float)(rgbValue & 0xFF))/255.0 alpha:1.0]
static char* plsyOnStringCopy(NSString* input){
    const char* string = [input UTF8String];
    return string ? strdup(string) : NULL;
}


void _playOnInitialize(const char* appKey, const char* storeId){
    [PlayOnManager.sharedManager initialize:GetStringParam(appKey) withStoreId:GetStringParam(storeId)];
}

const char* _playOnGetConsentString(){
    return plsyOnStringCopy([PlayOnManager.sharedManager getConsentString]);
}

bool _playOnIsGeneralConsentGiven(){
    return [PlayOnManager.sharedManager isGeneralConsentGiven];
}

void _playOnClearConsentString(){
    [PlayOnManager.sharedManager clearConsentString];
}

void _playOnSetConsentString(const char* consentString){
    [PlayOnManager.sharedManager setConsentString:GetStringParam(consentString)];
}

void _playOnSetGdprConsent(bool consent){
    [PlayOnManager.sharedManager setGdprConsent:consent];
}

void _playOnSetGdprConsentWithString(bool consent, const char* consentString){
    [PlayOnManager.sharedManager setGdprConsent:consent withConsentString:GetStringParam(consentString)];
}

void _playOnSetDoNotSell(bool isApplied){
    [PlayOnManager.sharedManager setDoNotSell:isApplied];
}

void _playOnSetDoNotSellWithString(bool isApplied, const char* consentString){
    [PlayOnManager.sharedManager setDoNotSell:isApplied withConsentString:GetStringParam(consentString)];
}

void _playOnForceRegulationType(int type){
    [PlayOnManager.sharedManager forceRegulationType:type];
}

void _playOnClearForceRegulationType(){
    [PlayOnManager.sharedManager clearForceRegulationType];
}

int _playOnGetRegulationType(){
    return (int)[PlayOnManager.sharedManager getRegulationType];
}

CFTypeRef _playOnCreateAudioAdUnit(int adType){
    POAdUnit* adUnit = [[POAdUnit alloc] initWithType:adType];
    [[PlayOnManager.unityViewController view] addSubview:adUnit];
    return CFBridgingRetain(adUnit);
}


void _playOnDestroyBridgeReference(CFTypeRef adUnit){
    CFRelease( adUnit );
}

PlayOnManagerListener* _playOnSetOnInitializationListener(POTypeCallbackClientRef * listener, PlayOnVoidDelegateNative onInitializationFinished, PlayOnNoArgsDelegateNative onInitializationFail){
    PlayOnManagerListener* newDelegate = [[PlayOnManagerListener alloc] initWithListenersOnInitialization:listener with:onInitializationFinished and:onInitializationFail];
    CFBridgingRetain(newDelegate);
    [[PlayOnManager sharedManager] setDelegate:newDelegate];
    return newDelegate;
}


bool _playOnIsInitialized(){
    return [PlayOnManager.sharedManager isInitialized];
}

void _playOnSetPopup(POAdUnit* ad, int position, int xOffset, int yOffset){
    [ad setPopupWithPosition:position xOffset:xOffset yOffset:yOffset];
}

void _playOnSetReward(POAdUnit* ad, int type, float value){
    [ad setRewardWithType:(PORewardType)type andAmount:value];
}

bool _playOnIsAdAvailable(POAdUnit* ad){
    return [ad isAdAvailable];
}

void _playOnClose(POAdUnit* ad){
    [ad closeAd];
}

void _playOnShow(POAdUnit* ad){
  [ad removeFromSuperview];
  [[PlayOnManager.unityViewController view] addSubview:ad];
  [ad showAd];
}

void _playOnSetLogo(POAdUnit* ad, int position, int xOffset, int yOffset, int size){
    [ad setLogoWithPosition:position xOffset:xOffset yOffset:yOffset withSize:size];
}

PlayOnAdListener* _playOnSetListeners(POAdUnit* ad, POTypeCallbackClientRef * adListener, PlayOnNoArgsDelegateNative onShow, PlayOnNoArgsDelegateNative onClose, PlayOnNoArgsDelegateNative onClick, PlayOnStateDelegateNative onAvailabilityChange, PlayOnFloatDelegateNative onReward, PlayOnDataDelegateNative onImpression){
    PlayOnAdListener* newDelegate = [[PlayOnAdListener alloc] initWithListeners:adListener onShow:onShow onClose:onClose onClick:onClick onAvailabilityChange:onAvailabilityChange onReward:onReward onImpression:onImpression];
    CFBridgingRetain(newDelegate);
    [ad setDelegate:newDelegate];
    return newDelegate;
}

void _playOnSetBanner(POAdUnit* ad, int position){
    [ad setBannerWithPosition:position];
}

void _playOnSetVisualization(POAdUnit* ad, const char* tint, const char* background){
    unsigned result = 0;
    NSScanner *scannerTint = [NSScanner scannerWithString:GetStringParam(tint)];
    NSScanner *scannerBackground = [NSScanner scannerWithString:GetStringParam(background)];
    [scannerTint setScanLocation:1]; // bypass '#' character
    [scannerBackground setScanLocation:1]; // bypass '#' character

    [scannerTint scanHexInt:&result];
    UIColor* tintColor = UIColorFromRGB(result);

    [scannerBackground scanHexInt:&result];
    UIColor* backgroundColor = UIColorFromRGB(result);

    [ad setVisualization:backgroundColor andTintColor:tintColor];
}

void _playOnSetProgressBar(POAdUnit* ad, const char* tint){
    unsigned result = 0;
    NSScanner *scannerTint = [NSScanner scannerWithString:GetStringParam(tint)];
    [scannerTint setScanLocation:1]; // bypass '#' character

    [scannerTint scanHexInt:&result];
    UIColor* tintColor = UIColorFromRGB(result);

    [ad setProgressBarColor:tintColor];
}

void _playOnSetActionButton(POAdUnit* ad, int actionType, float deltaTime){
    [ad setActionButtonType:actionType andAppearanceDelay:deltaTime];
}

void _playOnTrackRewardedOffer(POAdUnit* ad){
    [ad trackRewardedOffer];
}

void _playOnSetLogLevel(int level){
    [[PlayOnManager sharedManager] setLogLevel:level];
}

void _playOnAddToMutableArray(NSMutableArray* ar, int item){
    NSNumber* num = [NSNumber numberWithInt:item];
    [ar addObject:num];
}

NSMutableArray* _playOnCreateMutableArray(){
    return [[NSMutableArray alloc] init];
}

void _playOnSetEngineInfo(const char* engineName, const char* engineVersion){
    [PlayOnManager.sharedManager setEngineInfo:GetStringParam(engineName) withVersion:GetStringParam(engineVersion)];
}

void _playOnRequestTrackingAuthorization(){
    [PlayOnManager.sharedManager requestTrackingAuthorization];
}

void _playOnAddCustomAttribute(const char* key, const char* value){
    [PlayOnManager.sharedManager addCustomAttribute:GetStringParam(key) withValue:GetStringParam(value)];
}

void _playOnClearCustomAttributes(){
    [PlayOnManager.sharedManager clearCustomAttributes];
}

void _playOnRemoveCustomAttribute(const char*  key){
    [PlayOnManager.sharedManager removeCustomAttributes:GetStringParam(key)];
}

const char* _playOnGetCustomAttributes(){
    NSError* emptyError = [NSError alloc];
    NSArray* cAttrs = [PlayOnManager.sharedManager getCustomAttributes];
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:cAttrs options:NSJSONWritingPrettyPrinted error:&emptyError];
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    return plsyOnStringCopy(jsonString);
}

const char* _playOnGetCustomAttributesWithKey(const char* key){
    NSError* emptyError = [NSError alloc];
    NSArray* cAttrs = [PlayOnManager.sharedManager getCustomAttributes:GetStringParam(key)];
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:cAttrs options:NSJSONWritingPrettyPrinted error:&emptyError];
    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    return plsyOnStringCopy(jsonString);
}

void _playOnSetIsChildDirected(bool flag){
    [PlayOnManager.sharedManager setIsChildDirected:flag];
}

const char* _playOnImpressionGetPlacementID(POImpressionData *data){
    return plsyOnStringCopy(data.placementID);
}
 
const char* _playOnImpressionGetSessionID(POImpressionData *data){
    return plsyOnStringCopy(data.sessionID);
}
 
int _playOnImpressionGetAdType(POImpressionData *data){
    return plsyOnStringCopy(data.adUnit);
}
 
const char* _playOnImpressionGetCountry(POImpressionData *data){
    return plsyOnStringCopy(data.country);
}
 
double _playOnGetRevenue(POImpressionData *data){
    return data.revenue;
}

float _playOnGetDeviceVolumeLevel(){
    return [[PlayOnManager.sharedManager getDeviceVolumeLevel] floatValue];
}

void _playOnPause(){
    [PlayOnManager.sharedManager onPause];
}

void _playOnResume(){
    [PlayOnManager.sharedManager onResume];
}

float _playOnGetDeviceScale(){
    return [UIScreen mainScreen].nativeScale;
}

void _playOnAddAdUnitToRootView(POAdUnit* ad){
    [[PlayOnManager.unityViewController view] addSubview:ad];
}

void _playOnRemoveAdUnitFromSuperView(POAdUnit* ad){
    [ad removeFromSuperview];
}

const char* _playOnGetPlayerID(){
    return plsyOnStringCopy([PlayOnManager.sharedManager getPlayerID]);
}

void _playOnSetPlayerID(const char*  id){
    [PlayOnManager.sharedManager setPlayerID:GetStringParam(id)];
}