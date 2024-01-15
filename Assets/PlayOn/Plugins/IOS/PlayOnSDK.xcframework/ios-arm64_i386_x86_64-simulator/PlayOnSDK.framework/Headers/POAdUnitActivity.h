#import <UIKit/UIKit.h>
#import <PlayOnSDK/POAdUnit.h>
#import <PlayOnSDK/POAd.h>

@interface POAdUnitActivity : UIView
@property (nonatomic, assign) BOOL isPlaying;
@property (nonatomic, weak) id<POAdUnitDelegate> delegate;

- (instancetype)initWithAd:(POAd *)ad
              primaryColor:(UIColor *)primaryColor
            secondaryColor:(UIColor *)secondaryColor
      progressBarFillColor:(UIColor *)fillColor
               andDelegate:(id)delegate
                withAdType:(POAdType)adType
             andRewardType:(PORewardType)rewardType
          withRewardAmount:(float)rewardAmount
       andActionButtonType:(ActionButtonType)buttonType
       withAppearanceDelay:(float)delay;

- (instancetype)initWithAd:(POAd *)ad
              primaryColor:(UIColor *)primaryColor
            secondaryColor:(UIColor *)secondaryColor
      progressBarFillColor:(UIColor *)fillColor
               andDelegate:(id)delegate
                withAdType:(POAdType)adType
             andRewardType:(PORewardType)rewardType
          withRewardAmount:(float)rewardAmount
       andActionButtonType:(ActionButtonType)buttonType
       withAppearanceDelay:(float)delay
                     frame:(CGRect)frame;

- (void)showAd;
- (void)closeAd;
- (void)turnOnBannerWithPosition:(POAdPosition)position;
- (void)turnOnLogoWithPosition:(POAdPosition)position withXOffset:(int)xOffset withYOffset:(int)yOffset withSize:(int)size;
- (void)turnOnPopupWithPosition:(POAdPosition)position withXOffset:(int)xOffset withYOffset:(int)yOffset;
- (void)onPause;
- (void)onResume;
- (void)pauseAd;
- (void)resumeAd;
- (BOOL)isExpired;
@end
