using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AmazonAds {
    public abstract class IAdView {
        public abstract void FetchAd (AdResponse adResponse);
    }
}