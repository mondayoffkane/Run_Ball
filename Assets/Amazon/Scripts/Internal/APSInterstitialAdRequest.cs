namespace AmazonAds {
    public class APSInterstitialAdRequest : AdRequest {
        public APSInterstitialAdRequest (string uid) {
            AdSize.InterstitialAdSize size = new AdSize.InterstitialAdSize (uid);
            client.SetSizes (size.GetInstance ());
        }
    }
}