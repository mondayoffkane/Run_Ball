#import "DTBBannerDelegate.h"

@implementation DTBBannerDelegate
- (void)setDelegate:(DTBCallbackBannerRef*)client 
             adLoad:(DTBAdDidLoadType)adLoad 
         adFailLoad:(DTBAdFailedToLoadType)adFailLoad 
           leaveApp:(DTBBannerWillLeaveApplicationType)leaveApp 
           impFired:(DTBImpressionFiredType)impFired
{
    _callbackClient = client;
    _adDidLoadDelegate = adLoad;
    _adFailedToLoadDelegate = adFailLoad;
    _bannerWillLeaveApplicationDelegate = leaveApp;
    _impressionFiredDelegate = impFired;

}


#pragma mark - DTBBannerDelegate

- (void)adDidLoad:(UIView * _Nonnull)adView {
    if (_adDidLoadDelegate != nil) {
        _adDidLoadDelegate(_callbackClient);
    }
}

- (void)adFailedToLoad:(UIView * _Nullable)banner errorCode:(NSInteger)errorCode {
    if (_adFailedToLoadDelegate != nil) {
        _adFailedToLoadDelegate(_callbackClient);
    }
}

- (void)bannerWillLeaveApplication:(UIView *)adView {
    if (_bannerWillLeaveApplicationDelegate != nil) {
        _bannerWillLeaveApplicationDelegate(_callbackClient);
    }
}

- (void)impressionFired {
    if (_impressionFiredDelegate != nil) {
        _impressionFiredDelegate(_callbackClient);
    }
}

@end
