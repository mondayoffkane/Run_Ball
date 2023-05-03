using System;

namespace AmazonAds {
    public class AdSize {
        IAdSize client;
        public AdSize (int width, int height, String slotUID) {
#if UNITY_ANDROID
            client = new Android.DTBAdSize (width, height, slotUID);
#elif UNITY_IOS
            client = new IOS.DTBAdSize (width, height, slotUID);
#endif
        }
        public IAdSize GetInstance () {
            return client;
        }

        public class InterstitialAdSize {
            IInterstitialAdSize client;
            public InterstitialAdSize (String slotUID) {
#if UNITY_ANDROID
                client = new Android.DTBAdSize.DTBInterstitialAdSize (slotUID);
#elif UNITY_IOS
                client = new IOS.DTBAdSize.DTBInterstitialAdSize (slotUID);
#endif
            }

            public IInterstitialAdSize GetInstance () {
                return client;
            }
        }

        public class Video {
            IVideo client;
            public Video (int playerWidth, int playerHeight, String slotUUID) {
#if UNITY_ANDROID
                client = new Android.DTBAdSize.DTBVideo (playerWidth, playerHeight, slotUUID);
#elif UNITY_IOS
                client = new IOS.DTBAdSize.DTBVideo (playerWidth, playerHeight, slotUUID);
#endif
            }

            public IVideo GetInstance () {
                return client;
            }
        }
    }
}