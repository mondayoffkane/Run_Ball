namespace AmazonAds {
    public class APSVideoAdRequest : AdRequest {
        public APSVideoAdRequest (int width, int height, string uid) {
            AdSize.Video size = new AdSize.Video (width, height, uid);
            client.SetSizes (size.GetInstance ());
        }
    }
}