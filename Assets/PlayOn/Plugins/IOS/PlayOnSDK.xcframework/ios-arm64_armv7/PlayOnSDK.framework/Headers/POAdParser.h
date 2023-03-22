#import <Foundation/Foundation.h>
#import <PlayOnSDK/PlayOnSDKUtilities.h>

@class POAd;

@interface POAdParser : NSObject

typedef void(^CallbackBlock)(POAd *ad, NSError *error);

@property (nonatomic, copy) CallbackBlock callbackBlock;

- (instancetype)init;
- (void)parseFromRequestString:(NSString*)string completionBlock:(void (^)(POAd* ad, NSError *error)) completionBlock;
- (void)startParserWithData:(NSData*) data;
- (void)downloadDataFromURL:(NSURL *)url withCompletionHandler:(void (^)(NSData *data, NSError *error))completionHandler;
@end
