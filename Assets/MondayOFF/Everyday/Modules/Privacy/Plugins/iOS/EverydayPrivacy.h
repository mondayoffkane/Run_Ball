//
//  EverydayPrivacy.h
//
//  Created by heejong cho on 2/5/21.
//

#import <Foundation/Foundation.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>

@interface EverydayPrivacy : NSObject
typedef void (*AuthorizationRequestCallback)(int);
+ (void)requestTrackingAuthorization:(AuthorizationRequestCallback)callback;
+ (NSString *)getLocale;
+ (void)openAppSettings;
@end
