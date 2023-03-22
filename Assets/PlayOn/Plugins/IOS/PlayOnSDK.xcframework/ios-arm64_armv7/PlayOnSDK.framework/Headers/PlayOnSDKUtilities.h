#import <Foundation/Foundation.h>

@interface PlayOnSDKUtilities : NSObject

+ (void)downloadDataFromURL:(NSURL *)url withCompletionHandler:(void (^)(NSData *data, NSError *error))completionHandler;
@end
