#import <Foundation/Foundation.h>

extern NSString *const POErrorDomain;

typedef NS_ENUM(NSInteger, POAdErrorCode) {
    POErrorCodeInvalidRequest = 101,
    POErrorCodeUndefinedSize = 102,
    POErrorCodeNoInventory = 103,
    POErrorCodeInvalidAdURL = 104,
    POErrorCodeResponseParsingFailed = 105
};

@class POAd;
@class POBaseURLGenerator;

@interface POAdLoader : NSObject

- (void)loadAdWithStringRequest:(NSString*)request
       completionHandler:(void (^) (POAd *loadedAd, NSError *error))completionHandler;

- (void)loadAdWithBuilder:(POBaseURLGenerator*)builder
        completionHandler:(void (^) (POAd *loadedAd, NSError *error))completionHandler;

@end
