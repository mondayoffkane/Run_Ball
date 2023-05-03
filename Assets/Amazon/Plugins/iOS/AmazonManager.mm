#import "AmazonManager.h"

@implementation AmazonManager

#pragma mark NSObject

+ (AmazonManager*)sharedManager
{
    static AmazonManager* sharedManager = nil;

    if (!sharedManager)
        sharedManager = [[AmazonManager alloc] init];

    return sharedManager;
}

- (void)initialize:(NSString*)keywords
{
    [[DTBAds sharedInstance] setAppKey: keywords];
}

- (BOOL)isInitialized
{
    return [[DTBAds sharedInstance] isReady];
}

- (void)setUseGeoLocation:(bool)flag
{
    [[DTBAds sharedInstance] setUseGeoLocation:flag];
}

- (BOOL)getUseGeoLocation
{
    return [[DTBAds sharedInstance] useGeoLocation];
}

- (void)setLogLevel:(int)logLevel
{
    DTBLogLevel level = (DTBLogLevel) logLevel;
    [[DTBAds sharedInstance] setLogLevel:level];
}

- (int)getLogLevel
{
    return 0;
}

- (void)setTestMode:(bool)flag
{
    [[DTBAds sharedInstance] setTestMode:flag];
}

- (BOOL)isTestModeEnabled
{
    return [[DTBAds sharedInstance] testMode];
}

- (DTBAdSize*)createBannerAdSize:(int)width height:(int)height uuid:(NSString*)uuid{
    return [[DTBAdSize alloc] initBannerAdSizeWithWidth:width height:height andSlotUUID:uuid];
}

- (DTBAdSize*)createVideoAdSize:(int)width height:(int)height uuid:(NSString*)uuid{
    return [[DTBAdSize alloc] initVideoAdSizeWithPlayerWidth:width height: height andSlotUUID: uuid];;
}

- (DTBAdSize*)createInterstitialAdSize:(NSString*)uuid{
    return [[DTBAdSize alloc] initInterstitialAdSizeWithSlotUUID:uuid];
}

- (DTBAdLoader*)createAdLoader{
    return [DTBAdLoader new];
}

- (void)setSizes:(DTBAdLoader*)adLoader size:(DTBAdSize*)size{
    [adLoader setSizes:size, nil];
}

- (void)loadAd:(DTBAdLoader*)adLoader callback:(AmazonUnityCallback*)callback{
    [adLoader loadAd:callback];
}

- (void)loadSmartBanner:(DTBAdLoader*)adLoader callback:(AmazonUnityCallback*)callback{
    [adLoader loadSmartBanner:callback];
}

- (void)setMRAIDPolicy:(DTBMRAIDPolicy)policy
{
    [DTBAds sharedInstance].mraidPolicy = policy;
}

- (int) getMRAIDPolicy{
    return [DTBAds sharedInstance].mraidPolicy;
}

- (void)setMRAIDSupportedVersions:(NSString* _Nullable)versions
{
    [DTBAds sharedInstance].mraidCustomVersions = nil;
}


- (NSString *)jsonFromDict:(NSDictionary *)dict {
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dict
                                                       options:0
                                                         error:&error];
    if (!jsonData) {
        NSLog(@"Error converting JSON from VCS response dict: %@", error);
        return @"";
    } else {
        return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }
}

- (AmazonUnityCallback*)createCallback{
    AmazonUnityCallback* newCallback = [[AmazonUnityCallback alloc] init];
    return newCallback;
}

- (DTBBannerDelegate*)createBannerDelegate{
    return [[DTBBannerDelegate alloc] init];
}

- (DTBInterstitialDelegate*)createInterstitialDelegate{
    return [[DTBInterstitialDelegate alloc] init];
}

- (void)createFetchManager:(DTBAdLoader*)adLoader isSmartBanner:(BOOL)isSmartBanner{
    NSError *error = [DTBFetchFactory.sharedInstance createFetchManagerForLoader:adLoader isSmartBanner:isSmartBanner];
    if(error == nil){
        NSLog(@"FetchManager created");
    } else {
        NSLog(@"failed with error = %@", [error localizedDescription]);
    }
}

- (DTBFetchManager*)getFetchManager:(int)slotType isSmartBanner:(BOOL)isSmartBanner{
    if( !isSmartBanner ){
        return [[DTBFetchFactory sharedInstance] fetchManagerBySlotType:(DTBSlotType)slotType];
    }else {
        return [[DTBFetchFactory sharedInstance] fetchManagerBySlotType:SLOT_SMART];
    }
}

-(void)fetchManagerPop:(DTBFetchManager*)fetchManager{
    [fetchManager pop];
}
 
-(void)putCustomTarget:(DTBAdLoader*)adLoader key:(NSString*)key value:(NSString*)value{
    [adLoader putCustomTarget:value withKey:key];
}

-(void)startFetchManager:(DTBFetchManager*)fetchManager{
    [fetchManager start];
}

-(void)stopFetchManager:(DTBFetchManager*)fetchManager{
    [fetchManager stop];
}

-(BOOL)isEmptyFetchManager:(DTBFetchManager*)fetchManager{
    return [fetchManager isEmpty];
}

-(void)destroyFetchManager:(int)slotType{
    [[DTBFetchFactory sharedInstance] removeFetchManagerForSlotType:(DTBSlotType)slotType];
}

-(void)setSlotGroup:(DTBAdLoader*)adLoader  slotGtoupName:(NSString*)slotGtoupName{
    [adLoader setSlotGroup:slotGtoupName];
}

-(DTBSlotGroup*)createSlotGroup:(NSString*)slotGroupName{
     DTBSlotGroup *group = [[DTBSlotGroup alloc] initWithName:slotGroupName];
    return group;
}

-(void)addSlot:(DTBSlotGroup*)slot size:(DTBAdSize*)size{
    [slot addSize:size];
}

-(void)addSlotGroup:(DTBSlotGroup*)group{
    [DTBAds.sharedInstance addSlotGroup:group];
}

-(NSString*)fetchMoPubKeywords:(DTBAdResponse*)response {
    return [response keywordsForMopub];
}

-(NSString*)fetchAmznSlots:(DTBAdResponse *)response {
    return [response amznSlots];
}

-(int)fetchAdWidth:(DTBAdResponse *)response {
    DTBAdSize *adSize = [response adSize];
    return adSize.width;
}

-(int)fetchAdHeight:(DTBAdResponse *)response {
    DTBAdSize *adSize = [response adSize];
    return adSize.height;
}

-(NSString*)fetchMediationHints:(DTBAdResponse*)response isSmart:(BOOL)isSmart{
    NSError * err;
    NSDictionary * hint = [response mediationHints:isSmart];
    NSMutableDictionary *mHint = [hint mutableCopy];
    NSDate* myDate = mHint[@"load_start"];
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"dd-MM-yyyy"];
    NSString *stringDate = [dateFormatter stringFromDate:myDate];
    [mHint setValue:stringDate forKey:@"load_start"];
    NSData * jsonData = [NSJSONSerialization  dataWithJSONObject:mHint options:0 error:&err];
    NSString * mediationHints = [[NSString alloc] initWithData:jsonData   encoding:NSUTF8StringEncoding];
    return mediationHints;
}

-(void)setCMPFlavor:(DTBCMPFlavor)cFlavor {
    [DTBAds.sharedInstance setCmpFlavor:cFlavor];
}

-(void)setConsentStatus:(DTBConsentStatus)consentStatus{
    [DTBAds.sharedInstance setConsentStatus:consentStatus];
}

-(NSMutableArray*)createArray{
    return [[NSMutableArray alloc] init];
}

-(void)addToArray:(NSMutableArray*)dictionary item:(int)item{
    NSNumber* num = [NSNumber numberWithInt:item];
    [dictionary addObject:num];
}

-(void)setVendorList:(NSMutableArray*)dictionary{
    [DTBAds.sharedInstance setVendorList:dictionary];
}

-(void)setAutoRefresh:(DTBAdLoader*)adLoader{
    [adLoader setAutoRefresh];
}

-(void)setAutoRefresh:(DTBAdLoader*)adLoader secs:(int)secs{
    [adLoader setAutoRefresh:secs];
}

-(void)pauseAutorefresh:(DTBAdLoader*)adLoader{
    [adLoader pauseAutorefresh];
}

-(void)stopAutoRefresh:(DTBAdLoader*)adLoader{
    [adLoader stop];
}

-(void)resumeAutoRefresh:(DTBAdLoader*)adLoader{
    [adLoader resumeAutorefresh];
}

-(void)setAPSPublisherExtendedIdFeatureEnabled:(BOOL)isEnabled {
    [DTBAds.sharedInstance setAPSPublisherExtendedIdFeatureEnabled:isEnabled];
}

-(void)addCustomAttribute:(NSString *)withKey value:(id)value {
    [DTBAds.sharedInstance addCustomAttribute:withKey value:value];
}

-(void)removeCustomAttribute:(NSString *)forKey {
    [DTBAds.sharedInstance removeCustomAttribute:forKey];
}

-(void)setAdNetworkInfo:(DTBAdNetworkInfo *)dtbAdNetworkInfo {
    [[DTBAds sharedInstance] setAdNetworkInfo:dtbAdNetworkInfo];
}

-(void)setLocalExtras:(NSString *)adUnitId localExtras:(NSDictionary *)localExtras {
    [DTBAds setLocalExtras:adUnitId localExtras:localExtras];
}

-(NSDictionary *)getMediationHintsDict:(DTBAdResponse*)response isSmart:(BOOL)isSmart{
    return [response mediationHints:isSmart];
}

-(void)showInterstitialAd:(DTBAdInterstitialDispatcher*)dispatcher {
    [dispatcher showFromController:[self unityRootViewController]];
}

-(UIViewController *)unityRootViewController {
    id<UIApplicationDelegate> appDelegate = [UIApplication sharedApplication].delegate;
    // @TODO Check whether the appDelegate implements rootViewController. Refer to CR-68240623 for discussions.
    if ([appDelegate respondsToSelector:@selector(rootViewController)]) {
        return [[[UIApplication sharedApplication].delegate window] rootViewController];
    }
    return nil;
}

@end
