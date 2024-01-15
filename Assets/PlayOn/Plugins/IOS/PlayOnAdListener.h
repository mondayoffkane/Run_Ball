#import <PlayOnSDK/PlayOnSDK.h>

typedef const void *POTypeCallbackClientRef;
typedef const void *POTypeCallbackImpressionDataRef;

typedef void (*PlayOnNoArgsDelegateNative) (POTypeCallbackClientRef* callback);
typedef void (*PlayOnStateDelegateNative) (POTypeCallbackClientRef* callback, BOOL flag);
typedef void (*PlayOnFloatDelegateNative) (POTypeCallbackClientRef* callback, float value);
typedef void (*PlayOnDataDelegateNative) (POTypeCallbackClientRef* callback, CFTypeRef data);

@interface PlayOnAdListener: NSObject <POAdUnitDelegate> {
    PlayOnNoArgsDelegateNative _onShowCallback;
    PlayOnNoArgsDelegateNative _onCloseCallback;
    PlayOnNoArgsDelegateNative _onClickCallback;
    PlayOnStateDelegateNative _onAvailabilityChangedCallback;
    PlayOnFloatDelegateNative _onRewardCallback;
    PlayOnNoArgsDelegateNative _onUserCloseCallback;
    PlayOnDataDelegateNative _onImpressionCallback;
    
    POTypeCallbackClientRef* _clientRef;
    POTypeCallbackImpressionDataRef* _impressionDataRef;
}

    
-(instancetype) initWithListeners:(POTypeCallbackClientRef* )client
    onShow:(PlayOnNoArgsDelegateNative)onShowRef
    onClose:(PlayOnNoArgsDelegateNative)onCloseRef
    onClick:(PlayOnNoArgsDelegateNative)onClickRef
    onAvailabilityChange:(PlayOnStateDelegateNative)onAvailabilityChangeRef
    onReward:(PlayOnFloatDelegateNative)onRewardRef
    onUserClose:(PlayOnNoArgsDelegateNative)onUserCloseRef
    onImpression:(PlayOnDataDelegateNative)onImpressionRef;

- (void)onAvailabilityChanged:(BOOL)flag;

- (void)onClick;

- (void)onClose;

- (void)onReward:(float)amount;

- (void)onShow;

- (void)onUserClose;

- (void)onImpression:(POImpressionData *)impressionData;

@end
