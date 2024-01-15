@class POImpressionData;

@protocol POAdUnitDelegate <NSObject>
- (void)onAvailabilityChanged:(BOOL)flag;
- (void)onShow;
- (void)onClose;
- (void)onClick;
- (void)onReward:(float)amount;
@optional
- (void)onUserClose;
- (void)onImpression:(POImpressionData *)impressionData;
@end

