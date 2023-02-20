#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

typedef NS_ENUM(NSInteger, POLogLevel) {
    POLogLevelNone = 0,
    POLogLevelInfo = 1,
    POLogLevelDebug = 2
};

@interface POLogs : NSObject
@property (nonatomic, assign) POLogLevel logLevel;

+ (instancetype)sharedInstance;
- (void)logMessage:(NSString *)message atLogLevel:(POLogLevel)level;
@end


