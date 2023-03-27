#import <Foundation/Foundation.h>
#import <PlayOnSDK/POAdCustomEvent.h>

@interface POAdUtilities : NSObject

+ (void)trackUrlAsync:(NSURL *)url;
+ (void)sendCustomEvent:(NSString *)event toURL:(NSURL *)url andPayload:(NSString *)payload;
+ (void)sendCustomEvent:(NSString *)event toURL:(NSURL *)url withCode:(POAdCustomEventCode)code andPayload:(NSString *)payload;
+ (void)sendCustomEvent:(NSString *)event toURL:(NSURL *)url withParameters:(id)firstParameter, ... NS_REQUIRES_NIL_TERMINATION;
+ (void)sendCustomEvent:(NSString *)event toURL:(NSURL *)url withCode:(POAdCustomEventCode)code andParameters:(id)firstParameter, ... NS_REQUIRES_NIL_TERMINATION;
+ (void)sendCustomEvent:(NSString *)event toURL:(NSURL *)url andPayload:(NSString *)payload withParameters:(id)firstParameter, ... NS_REQUIRES_NIL_TERMINATION;
+ (void)sendCustomEvent:(NSString *)event toURL:(NSURL *)url withCode:(POAdCustomEventCode)code andPayload:(NSString *)payload withParameters:(id)firstParameter, ... NS_REQUIRES_NIL_TERMINATION;
+ (NSError *)errorWithCode:(NSInteger)code
            andDescription:(NSString *)description
          andFailureReason:(NSString *)reason
     andRecoverySuggestion:(NSString *)recovery
        andUnderlyingError:(NSError *)error;
+ (NSError *)errorWithCode:(NSInteger)code
            andDescription:(NSString *)description;
+ (NSError *)errorWithCode:(NSInteger)code
            andDescription:(NSString *)description
        andUnderlyingError:(NSError *)error;
@end
