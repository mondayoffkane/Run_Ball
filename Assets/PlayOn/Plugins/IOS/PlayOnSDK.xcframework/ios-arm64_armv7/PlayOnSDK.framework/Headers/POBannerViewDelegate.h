@class POBannerView;

@protocol POBannerViewDelegate <NSObject>
@optional
- (void)bannerViewDidPresentAd:(POBannerView *)bannerView;
- (void)bannerView:(POBannerView *)bannerView didFailToPresentAdWithError:(NSError *)error;
- (void)bannerViewWillLeaveApplication:(POBannerView *)bannerView;
@end
