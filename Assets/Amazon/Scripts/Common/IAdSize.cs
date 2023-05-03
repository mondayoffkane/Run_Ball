namespace AmazonAds {
   public interface IAdSize { 
      int GetWidth ();
      int GetHeight ();
      string GetSlotUUID ();
   }
   public interface IInterstitialAdSize { }
   public interface IVideo { }
}