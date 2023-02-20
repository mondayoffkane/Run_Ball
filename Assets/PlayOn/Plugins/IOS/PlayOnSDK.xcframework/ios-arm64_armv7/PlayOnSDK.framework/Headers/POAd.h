#import <Foundation/Foundation.h>

@class POCompanionBanner;
@class POAdPrice;

typedef NS_ENUM(NSInteger, POAdType) {
    AudioBannerAd,
    AudioRewardedBannerAd,
    AudioLogoAd,
    AudioRewardedLogoAd
};

typedef NS_ENUM(NSInteger, PORewardType) {
    InLevel,
    EndLevel
};

@interface POAd : NSObject
@property (nonatomic, strong) NSArray *mediaImpressionURLs;
@property (nonatomic, strong) NSArray *internalMediaImpressionURLs;
@property (nonatomic, strong) NSMutableArray *trackingCreativeViewEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingStartEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingPauseEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingResumeEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingFirstQuartileEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingMidpointEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingThirdQuartileEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingCompleteEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingSkipEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingMuteEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingUnmuteEventUrls;
@property (nonatomic, strong) NSMutableArray *trackingCloseLinearEventUrls;
@property (nonatomic, strong) NSString *mediaMIMEType;
@property (nonatomic, strong) NSString *format;
@property (nonatomic, strong) NSURL    *mediaURL;
@property (nonatomic, strong) NSArray *companionBanners;
@property (nonatomic, assign) NSInteger videoWidth;
@property (nonatomic, assign) NSInteger videoHeight;
@property (nonatomic, strong) NSURL     *companionClickThroughURL;
@property (nonatomic, strong) NSArray   *clickTrackingURLs;
@property (nonatomic, strong) NSMutableArray *verificationScriptResources;
@property (nonatomic, strong) POAdPrice *price;
@property (nonatomic, assign) BOOL isDefault;
@property (nonatomic, strong) NSString *trackingEventPayload;
@property (nonatomic, strong) NSURL *trackingEventURL;

- (BOOL)containsCompanionBanners;
+ (NSString *)stringWithAdType:(POAdType)adType;
@end
