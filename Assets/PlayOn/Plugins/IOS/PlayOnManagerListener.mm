#import "PlayOnManagerListener.h"

@implementation PlayOnManagerListener

-(instancetype) initWithListenersOnInitialization:(POTypeCallbackClientRef* )client with:(PlayOnNoArgsDelegateNative)onInitializationFinishedRef and:(PlayOnNoArgsDelegateNative)onInitializationFailedRef

{
    self = [super init];
    _clientRef = client;
    _onInitializationFinishedCallback = onInitializationFinishedRef;
    _onInitializationFailedCallback = onInitializationFailedRef;
    return self;
}

#pragma mark - PlayOnManagerListener

-(void) onInitializationFinished{
    _onInitializationFinishedCallback(_clientRef);
}

- (void)onInitializationFailed:(int)code description:(NSString *)description {
    _onInitializationFailedCallback(_clientRef);
}

@end
