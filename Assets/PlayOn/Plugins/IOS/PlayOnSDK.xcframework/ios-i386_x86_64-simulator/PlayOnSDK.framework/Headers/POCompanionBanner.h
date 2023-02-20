#import <Foundation/Foundation.h>

@interface POCompanionBanner : NSObject

/// The width of the banner
@property (nonatomic, assign) NSInteger width;

/// The height of the banner
@property (nonatomic, assign) NSInteger height;

/// The content URL of the banner
@property (nonatomic, strong) NSURL     *contentURL;

/// The HTML content of the banner
@property (nonatomic, strong) NSString  *contentHTML;

@end
