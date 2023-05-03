#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface POCompanionBanner : NSObject
@property (nonatomic, assign) NSInteger width;
@property (nonatomic, assign) NSInteger height;
@property (nonatomic, strong) NSString *contentHTML;
@property (nonatomic, strong) NSString *contentStatic;
@property (nonatomic, strong) NSString *contentIFrame;
@end

NS_ASSUME_NONNULL_END
