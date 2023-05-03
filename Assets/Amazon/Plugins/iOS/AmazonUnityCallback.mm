#import "AmazonUnityCallback.h"
#import "AmazonManager.h"

@implementation AmazonUnityCallback
- (void)setListeners:(DTBAdCallbackClientRef*)client success:(SuccessResponse)success errorCallback:(ErrorResponse)error {
    _callbackClient = client;
    _successCallback = success;
    _errorCallback = error;
}

- (void)setListenersWithInfo:(DTBAdCallbackClientRef*)client success:(SuccessResponse)success errorCallbackWithInfo:(ErrorResponseWithInfo)errorCallbackWithInfo {
    _callbackClient = client;
    _successCallback = success;
    _errorCallbackWithInfo = errorCallbackWithInfo;
}


#pragma mark - AmazonUnityCallback

- (void)onFailure:(DTBAdError)error {
    if (_errorCallback != nil) {
        _errorCallback( _callbackClient, (int)error );
    }
}

- (void)onFailure:(DTBAdError)error
   dtbAdErrorInfo:(DTBAdErrorInfo *) dtbAdErrorInfo {
       if (_errorCallbackWithInfo != nil) {
           _errorCallbackWithInfo( _callbackClient, (int)error, dtbAdErrorInfo );
       }
   }

- (void)onSuccess:(DTBAdResponse *)adResponse {
    if (_successCallback != nil) {
        _successCallback( _callbackClient, adResponse );
    }
}
@end
