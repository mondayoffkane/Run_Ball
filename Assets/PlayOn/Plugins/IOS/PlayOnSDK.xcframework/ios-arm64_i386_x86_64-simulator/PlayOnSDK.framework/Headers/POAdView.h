#import <UIKit/UIKit.h>
#import <PlayOnSDK/POAdViewDelegate.h>
#import <PlayOnSDK/POCompanionBanner.h>
#import <PlayOnSDK/POAdPositioning.h>

@interface POAdView : UIView
@property (nonatomic, weak) id<POAdViewDelegate> delegate;

- (instancetype)initWithPosition:(POAdPosition)position withAd:(POCompanionBanner *)banner resizeImage:(BOOL)shouldResizeImage;
- (instancetype)initWithPosition:(POAdPosition)position withAd:(POCompanionBanner *)banner resizeImage:(BOOL)shouldResizeImage frame:(CGRect)frame;
- (void)setFrameAndWebView:(CGRect)frame;
- (void)clear;
@end
