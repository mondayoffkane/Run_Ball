#import <PlayOnSDK/PlayOnSDK.h>
#import "PlayOnAdListener.h"
typedef void (*PlayOnVoidDelegateNative) ();

@interface PlayOnManagerListener: NSObject <PlayOnManagerDelegate> {
    PlayOnNoArgsDelegateNative _onInitializationCallback;
    POTypeCallbackClientRef* _clientRef;
}

-(instancetype) initWithListenersOnInitialization:(POTypeCallbackClientRef* )client with:(PlayOnNoArgsDelegateNative)onInitializationRef;

- (void)onInitialization;

@end
