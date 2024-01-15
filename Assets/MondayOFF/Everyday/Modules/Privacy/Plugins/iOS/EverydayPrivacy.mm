//
//  EverydayPrivacy.mm
//
//  Created by heejong cho on 2/5/21.
//

#import "EverydayPrivacy.h"

@implementation EverydayPrivacy
+(void)requestTrackingAuthorization:(AuthorizationRequestCallback)callbackPointer{
    if (@available(iOS 14, *)) {
        dispatch_after(dispatch_time(DISPATCH_TIME_NOW, 0.5 * NSEC_PER_SEC), dispatch_get_main_queue(), ^{
            [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
                callbackPointer(status);
            }];
        });
    }else{
        callbackPointer(3);
    }
}

+(NSString*)getLocale{
    NSString *countryCode = [[NSLocale currentLocale] objectForKey: NSLocaleCountryCode];
    return countryCode;
}
@end

char* convertNSStringToCString(const NSString* nsString){
    if (nsString == NULL)
        return NULL;

    const char* nsStringUtf8 = [nsString UTF8String];

    char* cString = (char*)malloc(strlen(nsStringUtf8) + 1);
    strcpy(cString, nsStringUtf8);

    return cString;
}

extern "C" {
    void _RequestTrackingAuthorization(AuthorizationRequestCallback callback){
        [EverydayPrivacy requestTrackingAuthorization: callback];
    }

    char* _GetLocale(){
        return convertNSStringToCString([EverydayPrivacy getLocale]);
    }

    void _OpenAppSettings(){
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString] options:@{} completionHandler:nil];
    }
}
