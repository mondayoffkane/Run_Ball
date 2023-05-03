using System;
using System.Collections.Generic;

namespace AmazonAds {
    public class APSAdDelegate {
        public delegate void OnAdLoaded ();
        public delegate void OnAdFailed ();
        public delegate void OnAdClicked ();
        public delegate void OnAdOpen ();
        public delegate void OnAdClosed ();
        public delegate void OnImpressionFired ();
        public delegate void OnVideoCompleted ();

        public OnAdLoaded onAdLoaded = OnAdLoadedImpl;
        public OnAdFailed onAdFailed = OnAdFailedImpl;
        public OnAdClicked onAdClicked = OnAdClickedImpl;
        public OnAdOpen onAdOpen = OnAdOpenImpl;
        public OnAdClosed onAdClosed = OnAdClosedImpl;
        public OnImpressionFired onImpressionFired = OnImpressionFiredImpl;
        public OnVideoCompleted onVideoCompleted = OnVideoCompletedImpl;

        public APSAdDelegate () {

        }

        private static void OnAdLoadedImpl () { }
        private static void OnAdFailedImpl () { }
        private static void OnAdClickedImpl () { }
        private static void OnAdOpenImpl () { }
        private static void OnAdClosedImpl () { }
        private static void OnImpressionFiredImpl () { }
        private static void OnVideoCompletedImpl () { }
    }
}
