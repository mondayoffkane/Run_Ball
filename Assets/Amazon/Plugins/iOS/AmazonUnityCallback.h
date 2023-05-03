#import <DTBiOSSDK/DTBiOSSDK.h>
#import <DTBiOSSDK/DTBAdCallback.h>

typedef const void *DTBAdCallbackClientRef;

typedef void (*SuccessResponse) (DTBAdCallbackClientRef* callback, DTBAdResponse* dataPtr);
typedef void (*ErrorResponse) (DTBAdCallbackClientRef* callback, int errorCode);
typedef void (*ErrorResponseWithInfo) (DTBAdCallbackClientRef* callback, int errorCode, DTBAdErrorInfo* adErrorInfoPtr);

@interface AmazonUnityCallback : NSObject <DTBAdCallback> {
    SuccessResponse _successCallback;
    ErrorResponse _errorCallback;
    ErrorResponseWithInfo _errorCallbackWithInfo;

    DTBAdCallbackClientRef* _callbackClient;
}

- (void)setListeners:(DTBAdCallbackClientRef*)client success:(SuccessResponse)success errorCallback:(ErrorResponse)error;
- (void)setListenersWithInfo:(DTBAdCallbackClientRef*)client success:(SuccessResponse)success errorCallbackWithInfo:(ErrorResponseWithInfo)errorCallbackWithInfo;
@end
