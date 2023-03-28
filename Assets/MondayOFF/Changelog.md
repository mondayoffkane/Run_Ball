# v3.0.14
- [Ads] `Strip AdMob` now deletes `Google Ad Manager` adapter as well. Don't forget to install `Google Ad Manager` when installing `Google AdMob`.
- [Singular] Fixed exception on Unity Editor when targeting iOS.

# v3.0.13
- [MAX] Removed `Google Ad Manager` adapter temporarily due to crash on launch.

# v3.0.12
- [PlayOn] Prevent manual `ShowPlayOn` when auto-show is enabled.
- `AudienceNetwork.AdSettings` now throws exception when Facebook Adapter is not integrated.
- Fixed parameter error on `OnPostprocessAllAssets` for Unity 2020.

# v3.0.11
- [Adverty] Updated to 4.1.3
- [MAX] Added Google Ads Manager adapter
- Migration with Publishing package

# v3.0.10
- [FacebookSDK] Updated to 15.1.0
- [Ads] Fixed retry attempt for `FullscreenAdType`.
- [Ads] Default `interstitialInterval` is now 30 seconds.
- [IAP] When `In-App Purchasing` package is not installed, it will run placeholder scripts.

# v3.0.9
- [IAP] `{lastWordOfBundleID}_noads` is now default product ID for No Ads. `NoAds` disables interstitial and banner on purchase and saves product ID to `PlayerPref`. 
- [IAP] Disabling Ad type no longer checks IAPManager initialization.
- [IAP] Successful purchases made before reward registration (i.e. Auto restore on Android) will grant reward when they get registered.
- [Ads] `ShowPlayOn` now returns `true` if it is displayed.
- [Ads] Added `playOnEveryNthInterstitial`. 

# v3.0.8
- [Ads] Added Adverty
- [Ads] AdSettings no longer checks `hasXXX`. Empty ad unit ID will be considered inactive.
- (iOS) [Singular] `SKANEnabled` is set to true. It MUST be kept true.

# v3.0.7
- [PlayON] `PlayOnAnchor` component had been added to position logo ad based on RectTransform. See ReadMe for more details.
- [Ads] `AdSettings` are injected into `AdTypes`.

# v3.0.6
- [IAP] Removed some unused codes
- [IAP] Added `RegisterAndPurchaseProduct(string productID, Action onPurchase)`. It will overwrite any registered callbacks!
- [IAP] If `RegisterProduct` is called when `StoreListener` is `null`, it will try to re-register once `StoreListener` gets initialized.
- [IAP] Enum `IAPStatus` is added. 
- [IAP] `RegisterProduct`, `PurchaseProduct`, and `RegisterAndPurchaseProduct` now returns `IAPStatus`. Use it to handle exceptions if needed.
- (iOS) PostProcess now sets `ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES` to `NO`.

# v3.0.5
- [IAP] `OnAfterPurchase(bool)` is now invoked when purchase fails.
- [MAX] (iOS) `Localize User Tracking Usage Description` is now enabled.
- (iOS) Fixed typo `"GoogleServices-Info.plist"` to `"GoogleService-Info.plist"` in `EventTrackerSwitch.cs`
- (iOS) Sets `Enable Bitcode` to `NO` for project wide build settings

# v3.0.4
- Interstitial interval now reset AFTER ad is hidden.
- Addded `resetTimerOnRewarded` in settings. Set it to true if you want to reset interval timer after watching rewarded ad (Default is false).

# v3.0.3
- (iOS) iOS plugin for Verve(HyBid) 2.16.0+ now compatible with swift binding

# v3.0.2
- Throws exception if Facebook `Client Token` is empty 
- (Android) Throws exception if Facebook `Client Token` does not exist in `AndroidManifest.xml`. Open Facebook Settings and click **Regenerate Android Manifest**.

# v3.0.1
- Added `Menu > Everyday > !! STRIP ADMOB !!`. It removes Google AdMob adapter and library.
- (Android) Automatically adds `mainTemplate.gradle`, `launcherTemplate.gradle`, and `gradleTemplate.properties` to `Plugins/Android`.
- (Android) Automatically adds multidex support to `launcherTemplate.gradle`

# v3.0.0 
- Initial release of version 3
- Please fresh install this plugin if you have Everyday < 3.0.0 integrated in your project.
