#import <Foundation/Foundation.h>
#import <PlayOnSDK/PlayOnManager.h>
 
@interface PlayOnManager (Adapter)
- (void)setAdapter:(NSString *)name withVersion:(NSString *)version;
- (NSString *)getAdapterName;
- (NSString *)getAdapterVersion;
- (BOOL)isUsedAdapter;
@end
