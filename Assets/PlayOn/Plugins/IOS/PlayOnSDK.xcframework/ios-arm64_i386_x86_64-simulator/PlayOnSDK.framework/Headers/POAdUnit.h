#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>
#import <PlayOnSDK/POAdUnitDelegate.h>
#import <PlayOnSDK/POAdPositioning.h>
#import <PlayOnSDK/POAd.h>

@interface POAdUnit : UIView

typedef NS_ENUM(NSInteger, ActionButtonType) {
    Mute,
    Close,
    None
};

@property (nonatomic, weak) id<POAdUnitDelegate> delegate;
@property (nonatomic, readonly) BOOL isAdAvailable;

- (instancetype)initWithType:(POAdType)adType;
- (void)setBannerWithPosition:(POAdPosition)position;
- (void)setRewardWithType:(PORewardType)type andAmount:(float)amount;
- (void)setPopupWithPosition:(POAdPosition)position xOffset:(int)xOffset yOffset:(int)yOffset;
- (void)setLogoWithPosition:(POAdPosition)position xOffset:(int)xOffset yOffset:(int)yOffset withSize:(int)size;
- (void)setVisualization:(UIColor *)background andTintColor:(UIColor *)tint;
- (void)setActionButtonType:(ActionButtonType)type andAppearanceDelay:(float)delay;
- (void)setProgressBarColor:(UIColor *)fillColor;
- (void)addCustomTag:(NSString *)tag;
- (void)trackRewardedOffer;
- (void)closeAd;
- (void)showAd;
- (void)onPause;
- (void)onResume;
@end
