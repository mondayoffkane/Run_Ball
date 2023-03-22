#import <PlayOnSDK/PlayOnSDK.h>
#import "PlayOnAdListener.h"
typedef void (*PlayOnVoidDelegateNative) ();

@interface PlayOnManagerListener: NSObject <PlayOnManagerDelegate> {
    PlayOnNoArgsDelegateNative _onInitializationFinishedCallback;
    PlayOnNoArgsDelegateNative _onInitializationFailedCallback;
    POTypeCallbackClientRef* _clientRef;
}

-(instancetype) initWithListenersOnInitialization:(POTypeCallbackClientRef* )client with:(PlayOnNoArgsDelegateNative)onInitializationFinishedRef and:(PlayOnNoArgsDelegateNative)onInitializationFailedRef;

- (void)onInitializationFinished;
- (void)onInitializationFailed:(int)code description:(NSString *)description;

@end
