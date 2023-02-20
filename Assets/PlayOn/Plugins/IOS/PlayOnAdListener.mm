#import "PlayOnAdListener.h"

@implementation PlayOnAdListener 

-(instancetype) initWithListeners:(POTypeCallbackClientRef* )client onShow:(PlayOnNoArgsDelegateNative)onShowRef onClose:(PlayOnNoArgsDelegateNative)onCloseRef onClick:(PlayOnNoArgsDelegateNative)onClickRef onAvailabilityChange:(PlayOnStateDelegateNative)onAvailabilityChangeRef onReward:(PlayOnFloatDelegateNative)onRewardRef onImpression:(PlayOnDataDelegateNative)onImpressionRef
{
    self = [super init];
    
    _clientRef = client;
    _onShowCallback = onShowRef;
    _onCloseCallback = onCloseRef;
    _onClickCallback = onClickRef;
    _onAvailabilityChangedCallback = onAvailabilityChangeRef;
    _onRewardCallback = onRewardRef;
    _onImpressionCallback = onImpressionRef;
    return self;
}

#pragma mark - AdListener

-(void) onAvailabilityChanged:(BOOL)flag{
    NSLog(@"onAvailabilityChanged %d", flag);
    _onAvailabilityChangedCallback(_clientRef, flag);
}

-(void) onShow{
    NSLog(@"onShow");
    _onShowCallback(_clientRef);
}

-(void) onClose {
    NSLog(@"onClose");
    _onCloseCallback(_clientRef);
}

-(void) onClick {
    NSLog(@"onClick");
    _onClickCallback(_clientRef);
}

- (void)onReward:(float)amount {
    NSLog(@"onReward");
    _onRewardCallback(_clientRef, amount);
}

- (void)onImpression:(POImpressionData *)impressionData {
    NSLog(@"onImpression");
    _onImpressionCallback(_clientRef, CFBridgingRetain(impressionData));
}

@end
