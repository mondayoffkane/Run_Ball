#import <Foundation/Foundation.h>
#import <PlayOnSDK/PlayOnSDK.h>
#import <PlayOnSDK/PlayOnSDK-Swift.h>

@interface PersonalInfo : NSObject
@property (nonatomic, assign) BOOL wasShownPopupInCurrentSession;

+ (PersonalInfo *)sharedManager;
- (NSString *)getCountry;
- (NSString *)getSessionID;
- (NSString *)getApiKey;
- (NSString *)getAppID;
- (NSString *)getIDFA;
- (NSString *)getIDFV;
- (NSString *)getPlayOnID;
- (NSString *)getIsChildDirected;
- (void)setIsChildDirected:(BOOL)isDirected;
- (void)setAppID:(NSString *)appID;
- (void)setApiKey:(NSString *)apiKey;
- (void)setSessionID:(NSString *)sessionID;
- (void)setCountry:(NSString *)country;
- (void)requestTrackingAuthorizationWithCompletionHandler:(void (^)(NSUInteger status))completion;
- (Class)appTrackingManager;

- (NSString *)getConsentString;
- (BOOL)isGeneralConsentGiven;
- (void)clearConsentString;
- (void)setGdprConsent:(BOOL)flag;
- (void)setGdprConsent:(BOOL)flag withConsentString:(NSString *)consentString;
- (void)setDoNotSell:(BOOL)flag;
- (void)setDoNotSell:(BOOL)flag withConsentString:(NSString *)consentString;

- (BOOL)isGdprApplied;
- (BOOL)isCcpaApplied;
- (void)setIsGdprApplied:(BOOL)flag;
- (void)setIsCcpaApplied:(BOOL)flag;
- (void)forceRegulationType:(ConsentRegulationType)type;
- (void)clearForceRegulationType;
- (ConsentRegulationType)getRegulationType;
- (NSString *)getRegulationTypeString;

- (void)setConsentString:(NSString *)consentString;
@end
