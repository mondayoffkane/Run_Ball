using System;
using UnityEngine;

namespace AmazonAds.IOS
{
    public class DTBAdSize : IAdSize
    {
        private IntPtr client;
        private int width = 0;
        private int height = 0;
        private String slotUUID= "";

        public DTBAdSize(int width, int height, String slotUUID)
        {
            this.slotUUID = slotUUID;
            this.width = width;
            this.height = height;
            client = Externs._createBannerAdSize(width, height, slotUUID);
        }

        public int GetHeight()
        {
            return height;
        }

        public IntPtr GetInstance()
        {
            return client;
        }

        public string GetSlotUUID()
        {
            return slotUUID;
        }

        public int GetWidth()
        {
            return width;
        }

        public class DTBInterstitialAdSize : IInterstitialAdSize
        {
            private IntPtr client;
            public DTBInterstitialAdSize(String slotUUID)
            {
                client = Externs._createInterstitialAdSize(slotUUID);
            }

            public IntPtr GetInstance()
            {
                return client;
            }
        }

        public class DTBVideo : IVideo
        {
            public IntPtr client;
            public DTBVideo(int playerWidth, int playerHeight, String slotUUID)
            {
                client = Externs._createVideoAdSize(playerWidth, playerHeight, slotUUID);
            }

            public IntPtr GetInstance()
            {
                return client;
            }
        }
    }
}