using System;
using UnityEngine;

namespace AmazonAds.Android {
  public class DTBAdSize : IAdSize {
    private AndroidJavaObject client;

    public DTBAdSize (int width, int height, String slotUUID) {
      client = new AndroidJavaObject ("com.amazon.device.ads.DTBAdSize", width, height, slotUUID);
    }
    public int GetWidth () {
      return client.Call<int> ("getWidth");
    }

    public int GetHeight () {
      return client.Call<int> ("getHeight");
    }

    public string GetSlotUUID () {
      return client.Call<string> ("getSlotUUID");
    }

    public AndroidJavaObject GetInstance () {
      return client;
    }

    public class DTBInterstitialAdSize : IInterstitialAdSize {
      private AndroidJavaObject client;
      public DTBInterstitialAdSize (String slotUUID) {
        client = new AndroidJavaObject ("com.amazon.device.ads.DTBAdSize$DTBInterstitialAdSize", slotUUID);
      }

      public AndroidJavaObject GetInstance () {
        return client;
      }
    }

    public class DTBVideo : IVideo {
      public AndroidJavaObject client;
      public DTBVideo (int playerWidth, int playerHeight, String slotUUID) {
        client = new AndroidJavaObject ("com.amazon.device.ads.DTBAdSize$DTBVideo", playerWidth, playerHeight, slotUUID);
      }

      public AndroidJavaObject GetInstance () {
        return client;
      }
    }
  }
}