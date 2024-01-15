using UnityEngine;

namespace MondayOFF {
    [System.Serializable]
    public class AccessData {
        public string BundleId;
        public int Platform;
        public int InterstitialInterval;
        public string InterstitialAdUnitId;
        public string BannerAdUnitId;
        public string RewardedAdUnitId;
        public string OdeeoKey;
        public string StoreId;
        public bool AdvertySandboxMode;
        public string AdvertyKey;
        public string Extra;

        internal static AccessData TestSetup() {
            EverydayLogger.Info("TestSetup");
            return new AccessData() {
                BundleId = Application.identifier,
                Platform = 1,
                InterstitialInterval = 30,
                InterstitialAdUnitId = "InterstitialAdUnitID",
                BannerAdUnitId = "BannerAdUnitID",
                RewardedAdUnitId = "RewardedAdUnitID",
                OdeeoKey = "bc832bc4-aec6-4886-a791-8472f168107b",
                StoreId = "1485533179",
                AdvertySandboxMode = true,
                AdvertyKey = "NTA5NDYxNmEtN2VkZi00MDVjLWIwZGItMzdlMDJlMjZlNjJlJGh0dHBzOi8vYWRzZXJ2ZXIuYWR2ZXJ0eS5jb20=",
                Extra = ""
            };
        }

        internal static AccessData BackupSetup(RuntimePlatform platform) {
            EverydayLogger.Info($"BackupSetup for MW - {platform}");

            if (platform == RuntimePlatform.Android) {
                return new AccessData() {
                    BundleId = Application.identifier,
                    Platform = 1,
                    InterstitialInterval = 30,
                    InterstitialAdUnitId = "cdce0d43a45d4e0f",
                    RewardedAdUnitId = "169b6565cf67492a",
                    BannerAdUnitId = "6ec30fe410f45a65",
                    OdeeoKey = "",
                    StoreId = "",
                    AdvertySandboxMode = false,
                    AdvertyKey = "",
                    Extra = ""
                };

            } else if (platform == RuntimePlatform.IPhonePlayer) {
                return new AccessData() {
                    BundleId = Application.identifier,
                    Platform = 2,
                    InterstitialInterval = 30,
                    InterstitialAdUnitId = "7c4b2ec836e7c609",
                    RewardedAdUnitId = "01d263f549e9404e",
                    BannerAdUnitId = "8d1e53d880c25a99",
                    OdeeoKey = "",
                    StoreId = "",
                    AdvertySandboxMode = false,
                    AdvertyKey = "",
                    Extra = ""
                };
            }

            EverydayLogger.Info("BackupSetup failed");
            return TestSetup();
        }
    }
}