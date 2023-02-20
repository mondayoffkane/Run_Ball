#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>
#import <CoreTelephony/CTTelephonyNetworkInfo.h>
#import <CoreTelephony/CTCarrier.h>
#import <sys/utsname.h>

@interface ClientMetadata : NSObject

+ (ClientMetadata *)sharedManager;
- (NSString *)getManufecturer;
- (NSString *)getModel;
- (NSString *)getPlatform;
- (NSString *)getOSVersion;
- (NSString *)getLanguage;
- (NSArray *)getInputLanguages;
- (NSArray *)getAudioInputs;
- (NSArray *)getAudioOutputs;
- (NSString *)getVolumeLevel;
- (NSString *)getBatteryLevel;
- (NSString *)getBatteryState;
- (NSString *)getOrientation;
- (NSString *)getNetworkType;
@end
