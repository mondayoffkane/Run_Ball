#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <PlayOnSDK/PlayOnManagerDelegate.h>
#import <PlayOnSDK/POLogs.h>
#import <PlayOnSDK/POAdUnit.h>

typedef NS_ENUM(NSInteger, ConsentRegulationType) {
    ConsentTypeUndefined,
    ConsentTypeNone,
    ConsentTypeGdpr,
    ConsentTypeCcpa
};

@interface PlayOnManager: NSObject
@property (nonatomic, weak) id<PlayOnManagerDelegate> delegate;

+ (PlayOnManager *)sharedManager;
- (BOOL)isInitialized;
- (void)initialize:(NSString *)appKey withStoreId:(NSString *)storeId;
- (void)setEngineInfo:(NSString *)engineName withVersion:(NSString *)engineVersion;
- (void)setLogLevel:(POLogLevel)level;
- (void)setIsChildDirected:(BOOL)flag;
- (NSString *)getSDKVersion;
- (NSString *)getDeviceVolumeLevel;
- (void)requestTrackingAuthorization;
+ (UIViewController *)unityViewController;
- (void)addCustomAttribute:(NSString *)key withValue:(NSString *)value;
- (NSArray *)getCustomAttributes;
- (NSArray *)getCustomAttributes:(NSString *)key;
- (void)clearCustomAttributes;
- (void)removeCustomAttributes:(NSString *)key;
- (void)addAdUnit:(POAdUnit *)unit;
- (void)removeAdUnit:(POAdUnit *)unit;
- (void)onPause;
- (void)onResume;

- (NSString *)getConsentString;
- (BOOL)isGeneralConsentGiven;
- (void)clearConsentString;
- (void)setGdprConsent:(BOOL)flag;
- (void)setGdprConsent:(BOOL)flag withConsentString:(NSString *)consentString;
- (void)setDoNotSell:(BOOL)flag;
- (void)setDoNotSell:(BOOL)flag withConsentString:(NSString *)consentString;

- (void)forceRegulationType:(ConsentRegulationType)type;
- (void)clearForceRegulationType;
- (ConsentRegulationType)getRegulationType;

- (void)setConsentString:(NSString *)consentString;
@end
