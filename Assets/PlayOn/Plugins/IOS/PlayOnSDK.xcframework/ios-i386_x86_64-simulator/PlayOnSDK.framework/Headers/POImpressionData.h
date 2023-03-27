#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface POImpressionData : NSObject
@property (nonatomic, strong) NSString *sessionID;
@property (nonatomic, strong) NSString *adUnit;
@property (nonatomic, strong) NSString *placementID;
@property (nonatomic, strong) NSString *country;
@property (nonatomic, assign) double revenue;
@end

NS_ASSUME_NONNULL_END
