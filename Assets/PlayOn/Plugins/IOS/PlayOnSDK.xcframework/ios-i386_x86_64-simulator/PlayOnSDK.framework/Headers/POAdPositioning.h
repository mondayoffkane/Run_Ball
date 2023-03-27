#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, POAdPosition) {
    TopLeft = 0,
    TopCenter = 1,
    TopRight = 2,
    CenterLeft = 3,
    Centered = 4,
    CenterRight = 5,
    BottomLeft = 6,
    BottomCenter = 7,
    BottomRight = 8
};

@protocol POAdPositioning <NSObject>

@end
