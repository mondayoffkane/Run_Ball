using System;

namespace AmazonAds {
    public enum DTBAdNetwork { 
        GOOGLE_AD_MANAGER,
        MOPUB_AD_SERVER,
        ADMOB,
        AD_GENERATION,
        IRON_SOURCE,
        MAX,
        NIMBUS,
        OTHER
    }
    
    public class AdNetworkInfo {
        private DTBAdNetwork adNetwork;

        public AdNetworkInfo(DTBAdNetwork dtbAdNetwork) {
            adNetwork = dtbAdNetwork;
        }

        public String getAdNetworkName() {
            return adNetwork.ToString();
        }

        internal DTBAdNetwork getAdNetwork() {
            return adNetwork;
        }
    }
}