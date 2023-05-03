#import <DTBiOSSDK/DTBiOSSDK.h>
#import <DTBiOSSDK/DTBAdInterstitialDispatcher.h>

typedef const void *DTBCallbackInterstitialRef;

typedef void (*DTBInterstitialDidLoadType) (DTBCallbackInterstitialRef* callback);
typedef void (*DTBDidFailToLoadAdWithErrorCodeType) (DTBCallbackInterstitialRef* callback);
typedef void (*DTBInterstitialDidPresentScreenType) (DTBCallbackInterstitialRef* callback);
typedef void (*DTBInterstitialDidDismissScreenType) (DTBCallbackInterstitialRef* callback);
typedef void (*DTBInterstitialWillLeaveApplicationType) (DTBCallbackInterstitialRef* callback);
typedef void (*DTBInterstitialImpressionFiredType) (DTBCallbackInterstitialRef* callback);

@interface DTBInterstitialDelegate : NSObject <DTBAdInterstitialDispatcherDelegate> {
    DTBInterstitialDidLoadType _didLoadDelegate;
    DTBDidFailToLoadAdWithErrorCodeType _didFailToLoadDelegate;
    DTBInterstitialDidPresentScreenType _didPresentScreenDelegate;
    DTBInterstitialDidDismissScreenType _didDismissScreenDelegate;
    DTBInterstitialWillLeaveApplicationType _leaveAppDelegate;
    DTBInterstitialImpressionFiredType _impFiredDelegate;

    DTBCallbackInterstitialRef* _callbackClient;
}

- (void)setDelegate:(DTBCallbackInterstitialRef*)client 
             adLoad:(DTBInterstitialDidLoadType)adLoad 
         adFailLoad:(DTBDidFailToLoadAdWithErrorCodeType)adFailLoad 
           leaveApp:(DTBInterstitialWillLeaveApplicationType)leaveApp 
           impFired:(DTBInterstitialImpressionFiredType)impFired 
            didOpen:(DTBInterstitialDidPresentScreenType)didOpen 
         didDismiss:(DTBInterstitialDidDismissScreenType)didDismiss;
@end
