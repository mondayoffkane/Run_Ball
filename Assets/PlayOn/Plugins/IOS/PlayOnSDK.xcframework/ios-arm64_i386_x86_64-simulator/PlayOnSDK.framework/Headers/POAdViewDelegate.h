@class POAdView;

@protocol POAdViewDelegate <NSObject>
@optional
- (void)adViewDidPresentAd:(POAdView *)adView;
- (void)adView:(POAdView *)adView didFailToPresentAdWithError:(NSError *)error;
- (void)adViewWillLeaveApplication:(POAdView *)adView;
@end
