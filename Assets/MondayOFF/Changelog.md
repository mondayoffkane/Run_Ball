# v3.0.35

- [Adverty] Updated to 4.2.0
- [Singular] Updated to 4.2.0
- [Ads] Added Smaato Adpater

# v3.0.34

- [IAP] Added `OnPurchaseFailed(Product, PurchaseFailureReason)` event
- [IAP] Added `OnAfterPurchaseWithProductId(PurchaseProcessStatus, string)` event
- [IAP] `OnAfterPurchase` is now obsolete, use `OnAfterPurchaseWithProductId(PurchaseProcessStatus, string)`instead
- [Firebase] (Android only) Automatically regenerate `google-services-desktop.json` before build
- [Settings] Added `InitializationDelay`. Initialization delay is used only when `InitializeOnLaunch` is set to `true`. Goal is to show ATT dialog after splash screen.

# v3.0.33

- [IAP] Added `(string isoCurrencyCode, string localizedPrice) GetLocalizedPrice(string productID)` to `IAPManager`
- [Privacy] (iOS) Added `Privacy.OpenAppSettings()` to redirect user to current app's settings window
- [Privacy] (iOS) Added `UserTrackingUsageDescription`in the Everyday Settings
- [Ads] Added Ogury Adapter
- [Android] Added Firebase to proguard
- [Singular] Updated to 4.1.1

# v3.0.32

- [Adverty] Updated to 4.1.8
- [Facebook] Updated to 16.0.2
- [Settings] Added `InitializeOnLaunch` option
- [Everyday] Added `Everyday.Initialize()` for manual initialization
- [Adverty] Added URP fix for `AdSpriteLitShader.shader`

# v3.0.31

- [Singular] Updated to 4.0.19
- [IAP] Duplicate product ID no longer crashes IAP manager
- [Ads] TapJoy adapter has been removed

# v3.0.30

- [Firebase] Fixed a bug where logging events before initialization was causing app crash

# v3.0.29

- [Firebase] Firebase now tracks ad impression
- [Settings] Fixed typo
- [APS] Fixed test mode

# v3.0.28

- [Odeeo] Updated to 2.2.5
- [Singular] Updated to 4.0.18
- [Adverty] Updated to 4.1.7
- [Firebase] Updated to 11.2.0
- [Facebook] Updated to 16.0.1
<!-- - [Everyday] Delayed Everyday initialization for iOS 16.5+ -->
- [Ads] Fixed `OnBeforeXXX` and `OnAfterXXX` to full screen ad type specific.

# v3.0.27

- [Privacy] [IN TEST] Uses Google's UMP for GDPR applicable region
- [Settings] `Facebook App ID`, `Facebook Client Token`, `Bundle Id (Package Name)`, and `AdMob ID` are now editable in the settings window

# v3.0.26

- [Ads] Fixed a bug where `no_ads`was not working properly when restored
- [APS] Updated to 1.6.0

# v3.0.25

- [Adverty] Updated to 4.1.6
- [Singular] Updated to 4.0.16
- [CP] Fixed wrong AppStore url on some games
- [CP] Updated game list
- [Privacy] Added `GDPR.IsApplicable` to identify if the device's region is set to applicable country

# v3.0.24

- [Review] Added Review request

# v3.0.23

- [EDM4U] Updated to 1.2.176
- [iOS] APS dependency fix
- [Android] Fixed wrong variable name for Windows

# v3.0.22

- [Odeeo] Updated to 2.2.3
- [Settings] New Everyday Settings Window
- [Logger] Define `EVERYDAY_NO_LOG` to disable EverydayLogger

# v3.0.21

- [Singular] Updated to 4.0.15
- [Ads] Added HyprMX, MobileFuse

# v3.0.20

- [Ads] Added new ad network adapter Amazon Publisher Service.
- [Adverty] Updated to 4.1.5

# v3.0.19

- [Firebase] Updated to 10.6.0

# v3.0.18

- [IAP] Implemented `OnInitializeFailed(InitializationFailureReason error, string message)` for In-App Purchasing 4.6.0+
- [IAP] Fixed NoAds not hiding ads when restarting

# v3.0.17

- [Odeeo] Update to 2.2.2
- [Ads] When `Initialize` is called before the manager is prepared, it waits for the settings and continue initialization.
- [iOS] Max 11.8 no longer supports ATT dialogue. `EverydayAppTracking` has been added for ATT.

# v3.0.16

- [Odeeo] Update to 2.1.0
- [Singular] Plugins are relocated to `Assets/MondayOFF/Everyday/Plugins`
- [Singular] App crashes when Singular fails to initialize.
- [Ads] Fixed a bug where Rewarded was not resetting Interstitial interval correctly.
- [iOS] Target version requirement is now 12.0 or higher

# v3.0.15

- [Singular] Updated to 4.0.13
- [Adverty] Updated to 4.1.4
- [Odeeo] Added user level impression

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
