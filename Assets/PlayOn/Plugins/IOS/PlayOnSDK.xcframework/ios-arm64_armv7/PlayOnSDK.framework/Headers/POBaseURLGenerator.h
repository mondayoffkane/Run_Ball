#import <Foundation/Foundation.h>
#import <PlayOnSDK/POAd.h>

@interface POBaseURLGenerator : NSObject

- (instancetype)initURLString:(NSString*)handlerType;
+ (void)setEngineInfo:(NSString *)name withVersion:(NSString *)version;
- (void)appendApiKey;
- (void)appendAppInfo;
- (void)appendDeviceInfo;
- (void)appendSDKInfo;
- (void)appendEngineInfo;
- (void)appendPrivacyInfo;
- (void)appendLocationInfo;
- (void)appendFreshRequestID;
- (void)appendCustomTag:(NSString *)tag;
- (void)appendAdType:(POAdType)adType;
- (void)appendCustomAttributes;
+ (void)addCustomAttribute:(NSString *)key withValue:(NSString *)value;
+ (NSArray *)getCustomAttributes;
+ (NSArray *)getCustomAttributes:(NSString *)key;
+ (void)clearCustomAttributes;
+ (void)removeCustomAttributes:(NSString *)key;
- (NSString *)getFinalUrlString;
+ (NSString *)getCOPPASupport;
+ (void)setCOPPASupport:(BOOL)isChildDirected;
@end
