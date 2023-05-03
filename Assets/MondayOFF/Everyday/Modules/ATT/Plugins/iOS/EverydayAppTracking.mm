//
//  EverydayAppTracking.mm
//
//  Created by heejong cho on 2/5/21.
//

#import "EverydayAppTracking.h"

@implementation EverydayAppTracking
-(void)requestTrackingAuthorization:(AuthorizationRequestCallback)callbackPointer{
    if (@available(iOS 14, *)) {
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
            callbackPointer(status==ATTrackingManagerAuthorizationStatusAuthorized);
        }];
    }
}
@end

extern "C" {
    void _RequestTrackingAuthorization(AuthorizationRequestCallback callback){
        [[[EverydayAppTracking alloc] init] requestTrackingAuthorization:callback];
    }
}
