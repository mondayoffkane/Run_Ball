#import <DTBiOSSDK/DTBiOSSDK.h>
#import <DTBiOSSDK/DTBAdBannerDispatcher.h>

typedef const void *DTBCallbackBannerRef;

typedef void (*DTBAdDidLoadType) (DTBCallbackBannerRef* callback);
typedef void (*DTBAdFailedToLoadType) (DTBCallbackBannerRef* callback);
typedef void (*DTBBannerWillLeaveApplicationType) (DTBCallbackBannerRef* callback);
typedef void (*DTBImpressionFiredType) (DTBCallbackBannerRef* callback);

@interface DTBBannerDelegate : NSObject <DTBAdBannerDispatcherDelegate>
{
    DTBAdDidLoadType _adDidLoadDelegate;
    DTBAdFailedToLoadType _adFailedToLoadDelegate;
    DTBBannerWillLeaveApplicationType _bannerWillLeaveApplicationDelegate;
    DTBImpressionFiredType _impressionFiredDelegate;

    DTBCallbackBannerRef* _callbackClient;
}

- (void)setDelegate:(DTBCallbackBannerRef*)client 
             adLoad:(DTBAdDidLoadType)adLoad 
         adFailLoad:(DTBAdFailedToLoadType)adFailLoad 
           leaveApp:(DTBBannerWillLeaveApplicationType)leaveApp 
           impFired:(DTBImpressionFiredType)impFired;
@end
