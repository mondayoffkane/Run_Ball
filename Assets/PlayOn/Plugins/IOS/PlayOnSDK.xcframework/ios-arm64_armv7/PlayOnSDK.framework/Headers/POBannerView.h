#import <UIKit/UIKit.h>
#import <PlayOnSDK/POBannerViewDelegate.h>
#import <PlayOnSDK/POCompanionBanner.h>
#import <PlayOnSDK/POAdPositioning.h>

@class POAdRequestURLBuilder;
@class POAd;

@interface POBannerView : UIView
@property (nonatomic, weak) id<POBannerViewDelegate> delegate;
@property (nonatomic, assign) BOOL shouldResizeImage;

- (instancetype)initWithPosition:(POAdPosition)pos withAd:(POCompanionBanner *)banner ;
- (void)setFrameAndWebView:(CGRect)frame;
- (void)clear;
@end
