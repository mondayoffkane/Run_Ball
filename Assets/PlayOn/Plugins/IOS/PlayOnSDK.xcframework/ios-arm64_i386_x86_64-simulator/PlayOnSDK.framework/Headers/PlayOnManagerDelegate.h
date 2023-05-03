@protocol PlayOnManagerDelegate
- (void)onInitializationFinished;
- (void)onInitializationFailed:(int)code description:(NSString *)description;
@end
