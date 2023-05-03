#import "DTBInterstitialDelegate.h"

@implementation DTBInterstitialDelegate
- (void)setDelegate:(DTBCallbackInterstitialRef*)client 
             adLoad:(DTBInterstitialDidLoadType)adLoad 
         adFailLoad:(DTBDidFailToLoadAdWithErrorCodeType)adFailLoad 
           leaveApp:(DTBInterstitialWillLeaveApplicationType)leaveApp 
           impFired:(DTBInterstitialImpressionFiredType)impFired 
            didOpen:(DTBInterstitialDidPresentScreenType)didOpen 
         didDismiss:(DTBInterstitialDidDismissScreenType)didDismiss
{
    _callbackClient = client;
    _didLoadDelegate = adLoad;
    _didFailToLoadDelegate = adFailLoad;
    _leaveAppDelegate = leaveApp;
    _impFiredDelegate = impFired;
    _didPresentScreenDelegate = didOpen;
    _didDismissScreenDelegate = didDismiss;
}


#pragma mark - DTBInterstitialDelegate

- (void)interstitialDidLoad:(DTBAdInterstitialDispatcher * _Nullable )interstitial {
    if (_didLoadDelegate != nil) {
        _didLoadDelegate(_callbackClient);
    }
}

- (void)interstitial:(DTBAdInterstitialDispatcher * _Nullable )interstitial
    didFailToLoadAdWithErrorCode:(DTBAdErrorCode)errorCode {
    if (_didFailToLoadDelegate != nil) {
        _didFailToLoadDelegate(_callbackClient);
    }
}

- (void)interstitialWillPresentScreen:(DTBAdInterstitialDispatcher * _Nullable )interstitial {
}

- (void)interstitialDidPresentScreen:(DTBAdInterstitialDispatcher * _Nullable )interstitial {
    if (_didPresentScreenDelegate != nil) {
        _didPresentScreenDelegate(_callbackClient);
    }
}

- (void)interstitialWillDismissScreen:(DTBAdInterstitialDispatcher * _Nullable )interstitial {
}

- (void)interstitialDidDismissScreen:(DTBAdInterstitialDispatcher * _Nullable )interstitial {
    if (_didDismissScreenDelegate != nil) {
        _didDismissScreenDelegate(_callbackClient);
    }
}

- (void)interstitialWillLeaveApplication:(DTBAdInterstitialDispatcher * _Nullable )interstitial {
    if (_leaveAppDelegate != nil) {
        _leaveAppDelegate(_callbackClient);
    }
}

- (void)showFromRootViewController:(UIViewController *_Nonnull)controller {
}

- (void)impressionFired {
    if (_impFiredDelegate != nil) {
        _impFiredDelegate(_callbackClient);
    }
}
@end
