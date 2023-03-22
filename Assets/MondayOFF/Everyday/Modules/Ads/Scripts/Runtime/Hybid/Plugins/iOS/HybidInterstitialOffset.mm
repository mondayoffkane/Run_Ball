#if __has_include(<HyBid/HyBid-Swift.h>)
    #import <HyBid/HyBid-Swift.h>
#else
    #import "HyBid-Swift.h"
#endif

extern "C" {
  void SetHyBidOffsets(int skipOffset){
      [[HyBidRenderingConfig sharedConfig] setVideoSkipOffset:[[HyBidSkipOffset alloc] initWithOffset:@(skipOffset) isCustom:true]];
      [[HyBidRenderingConfig sharedConfig] setHtmlSkipOffset:[[HyBidSkipOffset alloc] initWithOffset:@(skipOffset) isCustom:true]];
  }
}