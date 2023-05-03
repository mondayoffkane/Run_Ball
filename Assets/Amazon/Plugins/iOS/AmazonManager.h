#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#import <DTBiOSSDK/DTBiOSSDK.h>
#import <DTBiOSSDK/DTBAdCallback.h>
#import <DTBiOSSDK/DTBAdBannerDispatcher.h>
#import <DTBiOSSDK/DTBAdInterstitialDispatcher.h>

#import "AmazonUnityCallback.h"
#import "DTBBannerDelegate.h"
#import "DTBInterstitialDelegate.h"

@interface AmazonManager: NSObject { }
+ (AmazonManager*)sharedManager;
- (void)initialize:(NSString*)keywords;
- (BOOL)isInitialized;
- (void)setUseGeoLocation:(bool)flag;
- (BOOL)getUseGeoLocation;
- (void)setLogLevel:(int)logLevel;
- (int)getLogLevel;
- (void)setTestMode:(bool)flag;
- (BOOL)isTestModeEnabled;
- (DTBAdSize*)createBannerAdSize:(int)width height:(int)height uuid:(NSString*)uuid;
- (DTBAdSize*)createVideoAdSize:(int)width height:(int)height uuid:(NSString*)uuid;
- (DTBAdSize*)createInterstitialAdSize:(NSString*)uuid;
- (DTBAdLoader*)createAdLoader;
- (void)setSizes:(DTBAdLoader*)adLoader size:(DTBAdSize*)size;
- (void)loadAd:(DTBAdLoader*)adLoader callback:(AmazonUnityCallback*)callback;
- (void)loadSmartBanner:(DTBAdLoader*)adLoader callback:(AmazonUnityCallback*)callback;
- (void)setMRAIDPolicy:(DTBMRAIDPolicy)policy;
- (int)getMRAIDPolicy;
- (void)setMRAIDSupportedVersions:(NSArray<NSString *> *)versions;
- (NSString*)jsonFromDict:(NSDictionary *)dict;
- (AmazonUnityCallback*)createCallback;
- (DTBBannerDelegate*)createBannerDelegate;
- (DTBInterstitialDelegate*)createInterstitialDelegate;
- (void)createFetchManager:(DTBAdLoader*)adLoader isSmartBanner:(BOOL)isSmartBanner;
- (DTBFetchManager*)getFetchManager:(int)slotType isSmartBanner:(BOOL)isSmartBanner;
-(void)fetchManagerPop:(DTBFetchManager*)fetchManager;
-(void)putCustomTarget:(DTBAdLoader*)adLoader key:(NSString*)key value:(NSString*)value;
-(void)startFetchManager:(DTBFetchManager*)fetchManager;
-(void)stopFetchManager:(DTBFetchManager*)fetchManager;
-(BOOL)isEmptyFetchManager:(DTBFetchManager*)fetchManager;
-(void)destroyFetchManager:(int)slotType;
-(void)setSlotGroup:(DTBAdLoader*)adLoader  slotGtoupName:(NSString*)slotGtoupName;
-(DTBSlotGroup*)createSlotGroup:(NSString*)slotGroupName;
-(void)addSlot:(DTBSlotGroup*)slot size:(DTBAdSize*)size;
-(void)addSlotGroup:(DTBSlotGroup*)group;
-(NSString*)fetchMoPubKeywords:(DTBAdResponse*)response;
-(NSString*)fetchAmznSlots:(DTBAdResponse*)response;
-(int)fetchAdWidth:(DTBAdResponse*)response;
-(int)fetchAdHeight:(DTBAdResponse*)response;
-(NSString*)fetchMediationHints:(DTBAdResponse*)response isSmart:(BOOL)isSmart;
-(void)setCMPFlavor:(DTBCMPFlavor)cFlavor;
-(void)setConsentStatus:(DTBConsentStatus)consentStatus;
-(NSMutableArray*)createArray;
-(void)addToArray:(NSMutableArray*)dictionary item:(int)item;
-(void)setVendorList:(NSMutableArray*)dictionary;
-(void)setAutoRefresh:(DTBAdLoader*)adLoader;
-(void)setAutoRefresh:(DTBAdLoader*)adLoader secs:(int)secs;
-(void)pauseAutorefresh:(DTBAdLoader*)adLoader;
-(void)stopAutoRefresh:(DTBAdLoader*)adLoader;
-(void)resumeAutoRefresh:(DTBAdLoader*)adLoader;
-(void)setAPSPublisherExtendedIdFeatureEnabled:(BOOL)isEnabled;
-(void)addCustomAttribute:(NSString *)withKey value:(id)value;
-(void)removeCustomAttribute:(NSString *)forKey;
-(void)setAdNetworkInfo:(DTBAdNetworkInfo *)dtbAdNetworkInfo;
-(void)setLocalExtras:(NSString *)adUnitId localExtras:(NSDictionary *)localExtras;
-(NSDictionary *)getMediationHintsDict:(DTBAdResponse*)response isSmart:(BOOL)isSmart;
-(void)showInterstitialAd:(DTBAdInterstitialDispatcher*)dispatcher;
@end
