#import "PlayOnManagerListener.h"

@implementation PlayOnManagerListener 

-(instancetype) initWithListenersOnInitialization:(POTypeCallbackClientRef* )client with:(PlayOnNoArgsDelegateNative)onInitializationRef
{
    self = [super init];
    _clientRef = client;
    _onInitializationCallback = onInitializationRef;
    return self;
}

#pragma mark - PlayOnManagerListener

-(void) onInitialization{
    NSLog(@"onInitialization");
    _onInitializationCallback(_clientRef);
}

@end
