//
//  EverydayAppTracking.h
//
//  Created by heejong cho on 2/5/21.
//

#import <Foundation/Foundation.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>

@interface EverydayAppTracking : NSObject
typedef void (*AuthorizationRequestCallback)(bool);
-(void)requestTrackingAuthorization:(AuthorizationRequestCallback)callback;
@end
