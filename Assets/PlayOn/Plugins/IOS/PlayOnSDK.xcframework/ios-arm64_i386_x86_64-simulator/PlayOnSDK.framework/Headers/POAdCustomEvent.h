#import <Foundation/Foundation.h>

#ifndef POAdCustomEvent_h
#define POAdCustomEvent_h

typedef NS_ENUM(NSInteger, POAdCustomEventCode) {
    ApplicationMoveToBackground = 200,
    ApplicationMoveToForeground = 201,
    RewardedPopupAppear = 202,
    RewardedPopupDisappear = 203,
    AudioSessionInterruptionBegan = 204,
    AudioSessionInterruptionEnded = 205,
    AudioSessionPausedByClick = 206,
    HeadphonesDisconnected = 207,
    HeadphonesConnected = 208,
    AudioSessionResumedAfterClick = 209,
    UserChangeVolumeByButton = 210,
    DeveloperClosedAdByMethod = 211,
    UserClosedAdByButton = 212,
    AdCoverageBegan = 213,
    AdCoverageEnded = 214
};


#endif /* POAdCustomEvent_h */
