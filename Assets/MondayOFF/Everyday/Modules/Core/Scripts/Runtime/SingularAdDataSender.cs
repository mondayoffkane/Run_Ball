namespace MondayOFF {
    public static class SingularAdDataSender {
#if !UNITY_EDITOR
        private static int IS_COUNT = 0;
        private static int RV_COUNT = 0;

        internal static void SendAdData(string adUnitID, MaxSdk.AdInfo adInfo) {
            SingularAdData data = new SingularAdData("AppLovin", "USD", adInfo.Revenue);

            data.WithAdUnitId(adInfo.AdUnitIdentifier).
                WithNetworkName(adInfo.NetworkName).
                WithAdPlacmentName(adInfo.Placement);

            SingularSDK.AdRevenue(data);

            // Track Rewarded
            if (adInfo.AdFormat.Equals("REWARDED")) {
                var currentCount = ++RV_COUNT;
                switch (currentCount) {
                    case 5:
                    case 10:
                        SingularSDK.Event($"RV_{currentCount}");
                        break;
                }
            }
            // Track Interstitial
            else if (adInfo.AdFormat.Equals("INTER")) {
                var currentCount = ++IS_COUNT;
                switch (currentCount) {
                    case 5:
                    case 10:
                        SingularSDK.Event($"IS_{currentCount}");
                        break;
                }
            }
        }
#else
        internal static void SendAdData(string adUnitID, MaxSdk.AdInfo adInfo) {
            EverydayLogger.Info("Send Ad Data to Singular");
        }
#endif
    }
}